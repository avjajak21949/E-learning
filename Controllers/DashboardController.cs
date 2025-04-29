using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Demo03.Data;
using Demo03.Models;
using System.Collections;

namespace Demo03.Controllers
{
    [Authorize (Roles = "Manager")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var today = DateTime.Today;
            
            // Get course categories for pie chart
            var courseCategories = await _context.Courses
                .Include(c => c.Category)
                .GroupBy(c => c.Category.Name)
                .Select(g => new { CategoryName = g.Key, Count = g.Count() })
                .ToListAsync();
            
            var viewModel = new DashboardViewModel
            {
                TotalCourses = await _context.Courses.CountAsync(),
                TotalClasses = await _context.Classes.CountAsync(),
                TodaySchedules = await _context.Schedules
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Course)
                    .Where(s => s.StartDate.Date == today)
                    .OrderBy(s => s.StartTime)
                    .ToListAsync(),
                UpcomingSchedules = await _context.Schedules
                    .Include(s => s.Class)
                        .ThenInclude(c => c.Course)
                    .Where(s => s.StartDate.Date > today)
                    .OrderBy(s => s.StartDate)
                    .ThenBy(s => s.StartTime)
                    .Take(5)
                    .ToListAsync(),
                CourseCategories = courseCategories
            };

            return View(viewModel);
        }
        
        // API endpoint for real-time course category data
        [HttpGet]
        public async Task<IActionResult> GetCourseCategories()
        {
            var courseCategories = await _context.Courses
                .Include(c => c.Category)
                .GroupBy(c => c.Category.Name)
                .Select(g => new { CategoryName = g.Key, Count = g.Count() })
                .ToListAsync();
                
            return Json(courseCategories);
        }
    }

    public class DashboardViewModel
    {
        public int TotalCourses { get; set; }
        public int TotalClasses { get; set; }
        public ICollection<Schedule> UpcomingSchedules { get; set; }
        public ICollection<Schedule> TodaySchedules { get; set; }
        public IEnumerable<dynamic> CourseCategories { get; set; }
    }
} 