using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Demo03.Data;
using Demo03.Models;

namespace Demo03.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AttendanceController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Attendance
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(user);

            IQueryable<Attendance> query = _context.Attendances
                .Include(a => a.Class)
                .Include(a => a.Student);

            if (userRoles.Contains("Teacher"))
            {
                var teacherId = user.Id;
                query = query.Where(a => a.Class.TeacherId == teacherId);
            }
            else if (userRoles.Contains("Student"))
            {
                var studentId = user.Id;
                query = query.Where(a => a.StudentID == studentId);
            }

            return View(await query.ToListAsync());
        }

        // GET: Attendance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Class)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendance/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name");
            ViewData["StudentID"] = new SelectList(_context.Students, "Id", "FullName");
            return View();
        }

        // POST: Attendance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("AttendanceID,ClassID,StudentID,Date,Status")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name", attendance.ClassID);
            ViewData["StudentID"] = new SelectList(_context.Students, "Id", "FullName", attendance.StudentID);
            return View(attendance);
        }

        // GET: Attendance/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name", attendance.ClassID);
            ViewData["StudentID"] = new SelectList(_context.Students, "Id", "FullName", attendance.StudentID);
            return View(attendance);
        }

        // POST: Attendance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("AttendanceID,ClassID,StudentID,Date,Status")] Attendance attendance)
        {
            if (id != attendance.AttendanceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.AttendanceID))
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
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name", attendance.ClassID);
            ViewData["StudentID"] = new SelectList(_context.Students, "Id", "FullName", attendance.StudentID);
            return View(attendance);
        }

        // GET: Attendance/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Class)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AttendanceID == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.AttendanceID == id);
        }
    }
} 