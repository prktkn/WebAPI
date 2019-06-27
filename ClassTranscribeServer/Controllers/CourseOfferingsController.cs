﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassTranscribeDatabase;
using ClassTranscribeDatabase.Models;

namespace ClassTranscribeServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseOfferingsController : ControllerBase
    {
        private readonly CTDbContext _context;

        public CourseOfferingsController(CTDbContext context)
        {
            _context = context;
        }

        // GET: api/CourseOfferings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseOffering>>> GetCourseOfferings()
        {
            return await _context.CourseOfferings.ToListAsync();
        }

        // GET: api/Courses/
        /// <summary>
        /// Gets all Offerings per Course per Instructor
        /// </summary>
        [HttpGet("ByInstructor/{userId}")]
        public async Task<ActionResult<IEnumerable<CourseOfferingDTO>>> GetCourseOfferingsByInstructor(string userId)
        {
            var courseOfferings = _context.UserOfferings
                .Where(uo => uo.IdentityRole.Name == "Instructor" && uo.ApplicationUserId == userId)
                .Select(u => u.Offering).SelectMany(o => o.CourseOfferings);

            return await courseOfferings.GroupBy(co => co.Course, co => co.Offering).Select(g => new CourseOfferingDTO
            {
                Course = g.Key,
                Offerings = g.ToList()
            }).ToListAsync();

        }

        // GET: api/CourseOfferings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseOffering>> GetCourseOffering(string id)
        {
            var courseOffering = await _context.CourseOfferings.FindAsync(id);

            if (courseOffering == null)
            {
                return NotFound();
            }

            return courseOffering;
        }

        // PUT: api/CourseOfferings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseOffering(string id, CourseOffering courseOffering)
        {
            if (id != courseOffering.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(courseOffering).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseOfferingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CourseOfferings
        [HttpPost]
        public async Task<ActionResult<CourseOffering>> PostCourseOffering(CourseOffering courseOffering)
        {
            _context.CourseOfferings.Add(courseOffering);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CourseOfferingExists(courseOffering.CourseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCourseOffering", new { id = courseOffering.CourseId }, courseOffering);
        }

        // DELETE: api/CourseOfferings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CourseOffering>> DeleteCourseOffering(string id)
        {
            var courseOffering = await _context.CourseOfferings.FindAsync(id);
            if (courseOffering == null)
            {
                return NotFound();
            }

            _context.CourseOfferings.Remove(courseOffering);
            await _context.SaveChangesAsync();

            return courseOffering;
        }

        private bool CourseOfferingExists(string id)
        {
            return _context.CourseOfferings.Any(e => e.CourseId == id);
        }
        public class CourseOfferingDTO
        {
            public Course Course { get; set; }
            public List<Offering> Offerings { get; set; }
        }
    }
}
