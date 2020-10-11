﻿using ClassTranscribeDatabase;
using ClassTranscribeDatabase.Models;
using CTCommons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static ClassTranscribeDatabase.CommonUtils;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TaskEngine.Tasks
{
    [SuppressMessage("Microsoft.Performance", "CA1812:MarkMembersAsStatic")] // This class is never directly instantiated
    class DatabaseMigrationTask : RabbitMQTask<string>
    {
        public DatabaseMigrationTask(RabbitMQConnection rabbitMQ,
            ILogger<DatabaseMigrationTask> logger)
            : base(rabbitMQ, TaskType.DatabaseMigration, logger)
        {
        }
        protected async override Task OnConsume(string example, TaskParameters taskParameters, ClientActiveTasks cleanup)
        {
            registerTask(cleanup, "DatabaseMigrationTask"); // may throw AlreadyInProgress exception
            _logger.LogInformation("DatabaseMigrationTask Starting");
            int captionCount = 0;
            int transcriptionCount = 0;

            using (var _context = CTDbContext.CreateDbContext())
            {
                CaptionQueries captionQueries = new CaptionQueries(_context);

                var transcriptions = await _context.Transcriptions.Take(30).ToListAsync();

                foreach (var transcription in transcriptions)
                {

                    var transcriptionId = transcription.Id;
                    var videoID = transcription.VideoId;
                    var captions = await captionQueries.GetCaptionsAsync(transcriptionId);

                    _logger.LogInformation($"{transcription.Id}: Caption count= {captions.Count}");
                    transcriptionCount++;
                }
            }

            _logger.LogInformation($"Example Task Done.  transcriptionCount={transcriptionCount} captionCount={captionCount}");
        }
    }
}
