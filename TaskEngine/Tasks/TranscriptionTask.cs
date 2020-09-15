﻿using ClassTranscribeDatabase;
using ClassTranscribeDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CTCommons.MSTranscription;
using static ClassTranscribeDatabase.CommonUtils;
using CTCommons;
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;


namespace TaskEngine.Tasks
{
    /// <summary>
    /// This task produces the transcriptions for a Video item.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1812:MarkMembersAsStatic")] // This class is never directly instantiated
    class TranscriptionTask : RabbitMQTask<string>
    {
        private readonly MSTranscriptionService _msTranscriptionService;
        private readonly GenerateVTTFileTask _generateVTTFileTask;
        private readonly SceneDetectionTask _sceneDetectionTask;
        private readonly CaptionQueries _captionQueries;
        private readonly CTDbContext _context;

        public TranscriptionTask(RabbitMQConnection rabbitMQ, MSTranscriptionService msTranscriptionService,
            GenerateVTTFileTask generateVTTFileTask, SceneDetectionTask sceneDetectionTask, ILogger<TranscriptionTask> logger, CaptionQueries captionQueries, CTDbContext context)
            : base(rabbitMQ, TaskType.Transcribe, logger)
        {
            _msTranscriptionService = msTranscriptionService;
            _generateVTTFileTask = generateVTTFileTask;
            _sceneDetectionTask = sceneDetectionTask;
            _captionQueries = captionQueries;
            _context = context;
        }

        /// <summary>
        /// [2020.7.7] For the purpose of resuming failed transcriptions, all the captions of the failed transcription would now be stored in the database.
        /// Before resuming, all the old captions would be queried out from the database and stored in a dictionary, which will be passed to the
        /// transcription function along with the resume time point.
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="taskParameters"></param>
        /// <returns></returns>
        protected async override Task OnConsume(string videoId, TaskParameters taskParameters)
        {
            Video video = await _context.Videos.Include(v => v.Video1).Where(v => v.Id == videoId).FirstAsync();
            Key key = TaskEngineGlobals.KeyProvider.GetKey(video.Id);

            // creat Dictionary and pass it to the recognition function
            Dictionary<string, List<Caption>> captions = new Dictionary<string, List<Caption>>();

            var languages = new List<string> { Languages.ENGLISH, Languages.SIMPLIFIED_CHINESE, Languages.KOREAN, Languages.SPANISH, Languages.FRENCH };
            foreach (string language in languages)
            {
                captions[language] = await _captionQueries.GetCaptionsAsync(video.Id, language);
            }

            var lastSuccessTime = TimeSpan.Zero;
            if (video.JsonMetadata != null && video.JsonMetadata["LastSuccessfulTime"] != null)
            {
                lastSuccessTime = TimeSpan.Parse(video.JsonMetadata["LastSuccessfulTime"].ToString());
            }

            var result = await _msTranscriptionService.RecognitionWithVideoStreamAsync(video.Video1, key, captions, lastSuccessTime);

            if (video.JsonMetadata == null)
            {
                video.JsonMetadata = new JObject();
            }

            video.JsonMetadata["LastSuccessfulTime"] = result.LastSuccessTime.ToString();

            await _context.SaveChangesAsync();
            TaskEngineGlobals.KeyProvider.ReleaseKey(key, video.Id);
            List<Transcription> transcriptions = new List<Transcription>();
            foreach (var language in result.Captions)
            {
                if (language.Value.Count > 0)
                {
                    transcriptions.Add(new Transcription
                    {
                        Language = language.Key,
                        VideoId = video.Id,
                        Captions = language.Value
                    });
                }
            }
            
            // TODO/TOREVIEW: Does this support the new restart functionality?
            if (video.Transcriptions != null && video.Transcriptions.Any())
            {
                var oldTranscriptions = video.Transcriptions;
                var oldCaptions = video.Transcriptions.SelectMany(t => t.Captions);
                _context.Captions.RemoveRange(oldCaptions);
                await _context.SaveChangesAsync();
                _context.Transcriptions.RemoveRange(oldTranscriptions);
                await _context.SaveChangesAsync();
            }

            // Add the latest transcriptions.
            await _context.Transcriptions.AddRangeAsync(transcriptions);
            await _context.SaveChangesAsync();
            
            // TODO/TOREVIEW: Refactor explicit task dependency
            transcriptions.ForEach(t => _generateVTTFileTask.Publish(t.Id));
            _sceneDetectionTask.Publish(video.Id);

            // TODO/TOREVIEW: Why leave the incrementing until the end?
            // TODO/TOREVIEW: Wrap everything with a try catch and rethrow?
            video.TranscriptionStatus = result.ErrorCode;
            video.TranscribingAttempts += 1;
            await _context.SaveChangesAsync();
        }
    }
}

