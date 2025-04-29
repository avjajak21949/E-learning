using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Demo03.Models;
using Demo03.Data;
using Microsoft.AspNetCore.Authorization;
using Demo03.Services;

namespace Demo03.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public TeacherController(
            UserManager<IdentityUser> userManager, 
            ApplicationDbContext context,
            IEmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
        }

        // GET: Teacher
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var isManager = await _userManager.IsInRoleAsync(user, "Manager");
            
            IQueryable<Teacher> teachersQuery = _context.Teachers
                .Include(t => t.Classes)
                .Include(t => t.Meetings)
                .Include(t => t.Documents);
            
            if (isManager)
            {
                teachersQuery = teachersQuery.Where(t => t.CreatedByEmployerId == user.Id);
            }
            else if (await _userManager.IsInRoleAsync(user, "Teacher"))
            {
                teachersQuery = teachersQuery.Where(t => t.Id == user.Id);
            }
            
            var teachers = await teachersQuery.ToListAsync();
            return View(teachers);
        }

        // GET: Teacher/Details/5
        [Authorize(Roles = "Manager,Teacher")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var isManager = await _userManager.IsInRoleAsync(user, "Manager");
            
            var teacher = await _context.Teachers
                .Include(t => t.Classes)
                .Include(t => t.Meetings)
                .Include(t => t.Documents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            if (isManager && teacher.CreatedByEmployerId != user.Id)
            {
                return Forbid();
            }

            return View(teacher);
        }

        // GET: Teacher/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teacher/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("FullName,Email,Password,Department,Specialization")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                teacher.CreatedByEmployerId = user.Id;
                teacher.UserName = teacher.Email;
                teacher.EmailConfirmed = true;
                
                var result = await _userManager.CreateAsync(teacher, teacher.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(teacher, "Teacher");
                    
                    // Send welcome email to the teacher
                    var subject = "Welcome to Our E-Learning Platform";
                    var body = $@"
                        <h2>Welcome {teacher.FullName}!</h2>
                        <p>Your teacher account has been created successfully.</p>
                        <p>Here are your login credentials:</p>
                        <ul>
                            <li>Email: {teacher.Email}</li>
                            <li>Password: {teacher.Password}</li>
                        </ul>
                        <p>Please login at <a href='https://localhost:5001'>https://localhost:5001</a> and change your password.</p>
                        <p>As a teacher, you can manage classes, create materials, and interact with students.</p>";

                    await _emailService.SendEmailAsync(teacher.Email, subject, body);
                    
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(teacher);
        }

        // GET: Teacher/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var teacher = await _context.Teachers.FindAsync(id);
            
            if (teacher == null)
            {
                return NotFound();
            }

            if (teacher.CreatedByEmployerId != user.Id)
            {
                return Forbid();
            }
            
            // Remove password from model state validation
            ModelState.Remove("Password");

            return View(teacher);
        }

        // POST: Teacher/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,Email,Department,Specialization")] Teacher teacherUpdate)
        {
            if (id != teacherUpdate.Id)
            {
                return NotFound();
            }

            // Remove password from model state validation
            ModelState.Remove("Password");

            var user = await _userManager.GetUserAsync(User);
            var existingTeacher = await _context.Teachers.FindAsync(id);
            
            if (existingTeacher == null)
            {
                return NotFound();
            }
            
            if (existingTeacher.CreatedByEmployerId != user.Id)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingTeacher.FullName = teacherUpdate.FullName;
                    existingTeacher.Email = teacherUpdate.Email;
                    existingTeacher.UserName = teacherUpdate.Email;
                    existingTeacher.Department = teacherUpdate.Department;
                    existingTeacher.Specialization = teacherUpdate.Specialization;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacherUpdate.Id))
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
            return View(teacherUpdate);
        }

        // GET: Teacher/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (teacher == null)
            {
                return NotFound();
            }

            if (teacher.CreatedByEmployerId != user.Id)
            {
                return Forbid();
            }

            return View(teacher);
        }

        // POST: Teacher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var teacher = await _context.Teachers.FindAsync(id);
            
            if (teacher == null)
            {
                return NotFound();
            }
            
            if (teacher.CreatedByEmployerId != user.Id)
            {
                return Forbid();
            }

            try
            {
                // Find any Comments related to this teacher
                var relatedComments = await _context.Comments.Where(c => c.UserID == id).ToListAsync();
                
                // Option 1: Delete all related comments
                _context.Comments.RemoveRange(relatedComments);
                
                // Option 2 (alternative): Set UserID to null for all related comments
                // foreach(var comment in relatedComments)
                // {
                //     comment.UserID = null;
                // }
                
                // Now delete the teacher
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the teacher: " + ex.Message);
                return View(teacher);
            }
        }

        private bool TeacherExists(string id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }
    }
} 