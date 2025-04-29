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
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;

        public StudentController(
            ApplicationDbContext context, 
            UserManager<IdentityUser> userManager,
            IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: Student
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var isManager = await _userManager.IsInRoleAsync(user, "Manager");
            
            IQueryable<Student> studentsQuery = _context.Students
                .Include(s => s.StudentClasses)
                    .ThenInclude(sc => sc.Class)
                        .ThenInclude(c => c.Course);

            if (isManager)
            {
                // Managers can see all students
                return View(await studentsQuery.ToListAsync());
            }
            else
            {
                // Students can only see their own profile
                studentsQuery = studentsQuery.Where(s => s.Id == user.Id);
                return View(await studentsQuery.ToListAsync());
            }
        }

        // GET: Student/Details/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var isManager = await _userManager.IsInRoleAsync(user, "Manager");

            var student = await _context.Students
                .Include(s => s.StudentClasses)
                    .ThenInclude(sc => sc.Class)
                        .ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            if (!isManager && student.Id != user.Id)
            {
                return Forbid();
            }

            return View(student);
        }

        // GET: Student/Create
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            Console.WriteLine($"Current user: {user?.UserName}, Roles: {string.Join(", ", roles)}");
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("FullName,Email,Password,StudentNumber,Department")] Student student)
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            Console.WriteLine($"Current user: {user?.UserName}, Roles: {string.Join(", ", roles)}");

            if (ModelState.IsValid)
            {
                student.UserName = student.Email;
                student.EmailConfirmed = true;
                
                var result = await _userManager.CreateAsync(student, student.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(student, "Student");

                    // Send welcome email to the student
                    var subject = "Welcome to Our E-Learning Platform";
                    var body = $@"
                        <h2>Welcome {student.FullName}!</h2>
                        <p>Your account has been created successfully.</p>
                        <p>Here are your login credentials:</p>
                        <ul>
                            <li>Email: {student.Email}</li>
                            <li>Password: {student.Password}</li>
                        </ul>
                        <p>Please login at <a href='https://localhost:5001'>https://localhost:5001</a> and change your password.</p>";

                    await _emailService.SendEmailAsync(student.Email, subject, body);

                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"Error creating student: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Model validation error: {error.ErrorMessage}");
                }
            }
            return View(student);
        }

        // GET: Student/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            
            // Remove password validation for edit
            ModelState.Remove("Password");
            
            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,Email,StudentNumber,Department")] Student studentUpdate)
        {
            if (id != studentUpdate.Id)
            {
                return NotFound();
            }
            
            // Remove password validation for edit
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingStudent = await _context.Students.FindAsync(id);
                    if (existingStudent == null)
                    {
                        return NotFound();
                    }

                    existingStudent.FullName = studentUpdate.FullName;
                    existingStudent.Email = studentUpdate.Email;
                    existingStudent.UserName = studentUpdate.Email;
                    existingStudent.StudentNumber = studentUpdate.StudentNumber;
                    existingStudent.Department = studentUpdate.Department;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(studentUpdate.Id))
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
                    // Log error
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the student: " + ex.Message);
                    return View(studentUpdate);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentUpdate);
        }

        // GET: Student/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentClasses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var student = await _context.Students.FindAsync(id);
                if (student == null)
                {
                    return NotFound();
                }

                // Find any Comments related to this student
                var relatedComments = await _context.Comments.Where(c => c.UserID == id).ToListAsync();
                
                // Delete all related comments first
                _context.Comments.RemoveRange(relatedComments);
                
                // Then delete the student
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the student: " + ex.Message);
                var student = await _context.Students
                    .Include(s => s.StudentClasses)
                    .FirstOrDefaultAsync(m => m.Id == id);
                return View(student);
            }
        }

        private bool StudentExists(string id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
} 