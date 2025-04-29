using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo03.Models;
using Demo03.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Demo03.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(UserManager<IdentityUser> userManager, ApplicationDbContext context, ILogger<CoursesController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var isManager = await _userManager.IsInRoleAsync(user, "Manager");
            IQueryable<Course> coursesQuery = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Classes);

            if (isManager)
            {
                // Managers can see all courses
                return View(await coursesQuery.ToListAsync());
            }
            else
            {
                // Teachers can only see courses they teach
                var teacher = await _context.Teachers.FindAsync(user.Id);
                if (teacher != null)
                {
                    coursesQuery = coursesQuery.Where(c => c.Classes.Any(cl => cl.TeacherId == teacher.Id));
                }
                return View(await coursesQuery.ToListAsync());
            }
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var isManager = await _userManager.IsInRoleAsync(user, "Manager");

            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Classes)
                .FirstOrDefaultAsync(m => m.CourseID == id);

            if (course == null)
            {
                return NotFound();
            }

            if (!isManager)
            {
                var teacher = await _context.Teachers.FindAsync(user.Id);
                if (teacher != null && !course.Classes.Any(cl => cl.TeacherId == teacher.Id))
                {
                    return Forbid();
                }
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("Name,CourseCode,Description,CategoryID,CreditHours,Place,Time,Price,Duration")] Course course)
        {
            // Log all model state errors for debugging
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogError($"Error in {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                        // Also add to model state to display in UI
                        ModelState.AddModelError("", $"Error in {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Set default values for required fields if not provided
                    if (string.IsNullOrEmpty(course.Duration))
                    {
                        course.Duration = "3 months"; // Default duration
                    }
                    
                    // Set the employer ID if needed
                    if (User.IsInRole("Manager"))
                    {
                        course.CreatedByEmployerId = _userManager.GetUserId(User);
                    }
                    
                    _context.Add(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating course");
                    ModelState.AddModelError("", $"An error occurred while creating the course: {ex.Message}");
                }
            }
            
            // If we got this far, something failed, redisplay form
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", course.CategoryID);
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", course.CategoryID);
            return View(course);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Name,CourseCode,Description,CategoryID,CreditHours,Place,Time,Price,Duration")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            // Log all model state errors for debugging
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        _logger.LogError($"Error in {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                        // Also add to model state to display in UI
                        ModelState.AddModelError("", $"Error in {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Make sure the employer ID is preserved
                    var existingCourse = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.CourseID == id);
                    if (existingCourse != null)
                    {
                        course.CreatedByEmployerId = existingCourse.CreatedByEmployerId;
                    }
                    
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating course");
                    ModelState.AddModelError("", $"An error occurred while updating the course: {ex.Message}");
                    ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", course.CategoryID);
                    return View(course);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Name", course.CategoryID);
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }
    }
}
