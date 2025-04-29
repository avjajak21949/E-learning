using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo03.Data;
using Demo03.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Demo03.Controllers
{
    [Authorize]
    public class SchedulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var schedules = await _context.Schedules
                .Include(s => s.Class)
                    .ThenInclude(c => c.Course)
                .ToListAsync();
            return View(schedules);
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Class)
                    .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(m => m.ScheduleID == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create(int? classId)
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name", classId);
            return View();
        }

        // POST: Schedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("ScheduleID,ClassID,DayOfWeek,StartDate,EndDate,DaysOfWeek,StartTime,EndTime,Location,AvailableSeats")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the class exists if ClassID is provided
                    if (schedule.ClassID > 0)
                    {
                        var classExists = await _context.Classes.AnyAsync(c => c.ClassID == schedule.ClassID);
                        if (!classExists)
                        {
                            ModelState.AddModelError("ClassID", "The selected class does not exist.");
                            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name", schedule.ClassID);
                            return View(schedule);
                        }
                    }

                    _context.Add(schedule);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating schedule: {ex.Message}");
                }
            }

            // If we get here, something went wrong - redisplay form
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name", schedule.ClassID);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["ClassID"] = new SelectList(_context.Classes.Include(c => c.Course), "ClassID", "Name", schedule.ClassID);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleID,ClassID,DayOfWeek,StartDate,EndDate,DaysOfWeek,StartTime,EndTime,Location,AvailableSeats")] Schedule schedule)
        {
            if (id != schedule.ScheduleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.ScheduleID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassID"] = new SelectList(_context.Classes.Include(c => c.Course), "ClassID", "Name", schedule.ClassID);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Class)
                    .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(m => m.ScheduleID == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Class)
                .FirstOrDefaultAsync(m => m.ScheduleID == id);
                
            if (schedule == null)
            {
                return NotFound();
            }
            
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Schedules/Calendar
        public async Task<IActionResult> Calendar()
        {
            var schedules = await _context.Schedules
                .Include(s => s.Class)
                    .ThenInclude(c => c.Course)
                .ToListAsync();

            var scheduleViewModels = schedules.Select(s => new
            {
                id = s.ScheduleID,
                title = $"{s.Class.Course.Name} - {s.Class.Name}",
                start = s.StartDate.ToString("yyyy-MM-dd"),
                end = s.EndDate.ToString("yyyy-MM-dd"),
                startTime = s.StartTime.ToString(@"hh\:mm"),
                endTime = s.EndTime.ToString(@"hh\:mm"),
                location = s.Location,
                className = s.Class.Name,
                courseName = s.Class.Course.Name,
                daysOfWeek = s.DaysOfWeek?.Split(',') ?? Array.Empty<string>()
            }).ToList();

            ViewBag.Schedules = JsonConvert.SerializeObject(scheduleViewModels);
            return View();
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.ScheduleID == id);
        }
    }
}