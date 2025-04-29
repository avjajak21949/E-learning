using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Demo03.Data;
using Demo03.Models;
using Microsoft.AspNetCore.Http;
using Demo03.Services;

namespace Demo03.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailService _emailService;

        public DocumentsController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment environment,
            IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
            _emailService = emailService;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var documents = await _context.Documents
                .Include(d => d.Class)
                .Include(d => d.UploadedBy)
                .Where(d => d.UploadedByUserId == user.Id)
                .ToListAsync();

            return View(documents);
        }

        // GET: Documents/Upload
        public IActionResult Upload()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name");
            return View();
        }

        // POST: Documents/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload([Bind("Title,Description,Type,ClassID")] Document document, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var user = await _userManager.GetUserAsync(User);
                document.UploadedByUserId = user.Id;
                document.FileName = file.FileName;
                document.UploadDate = DateTime.Now;

                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Save file
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                _context.Add(document);
                await _context.SaveChangesAsync();

                // Send email notifications to students in the class
                if (document.ClassID.HasValue)
                {
                    var studentsInClass = await _context.StudentClasses
                        .Include(sc => sc.Student)
                        .Where(sc => sc.ClassID == document.ClassID)
                        .Select(sc => sc.Student)
                        .ToListAsync();

                    var className = await _context.Classes
                        .Where(c => c.ClassID == document.ClassID)
                        .Select(c => c.Name)
                        .FirstOrDefaultAsync();

                    foreach (var student in studentsInClass)
                    {
                        var studentUser = await _userManager.FindByIdAsync(student.Id);
                        if (studentUser?.Email != null)
                        {
                            var subject = $"New Document Added to {className}";
                            var body = $@"
                                <h2>New Document Available</h2>
                                <p>A new document has been added to your class {className}:</p>
                                <p><strong>{document.Title}</strong></p>
                                <p>{document.Description}</p>
                                <p>You can view and download this document from your course materials.</p>";

                            await _emailService.SendEmailAsync(studentUser.Email, subject, body);
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name", document.ClassID);
            return View(document);
        }

        // GET: Documents/CourseDocuments
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CourseDocuments()
        {
            var user = await _userManager.GetUserAsync(User);
            
            // Get the classes the student is enrolled in
            var studentClasses = await _context.StudentClasses
                .Where(sc => sc.StudentId == user.Id)
                .Select(sc => sc.ClassID)
                .ToListAsync();

            // Get documents for those classes
            var documents = await _context.Documents
                .Include(d => d.Class)
                .Include(d => d.UploadedBy)
                .Where(d => d.Type == DocumentType.CourseDocument && 
                           studentClasses.Contains(d.ClassID ?? -1))
                .OrderByDescending(d => d.UploadDate)
                .ToListAsync();

            return View(documents);
        }

        // GET: Documents/Download/5
        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.Class)
                .FirstOrDefaultAsync(d => d.DocumentID == id);

            if (document == null)
            {
                return NotFound();
            }

            // Check if user has access to this document
            var user = await _userManager.GetUserAsync(User);
            if (User.IsInRole("Student"))
            {
                var hasAccess = await _context.StudentClasses
                    .AnyAsync(sc => sc.StudentId == user.Id && 
                                   sc.ClassID == document.ClassID);
                if (!hasAccess)
                {
                    return Forbid();
                }
            }
            else if (document.UploadedByUserId != user.Id && !User.IsInRole("Admin") && !User.IsInRole("Manager"))
            {
                return Forbid();
            }

            // Search for file with the document's filename in the uploads directory
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            var files = Directory.GetFiles(uploadsFolder);
            
            // Find any file that contains the document filename (handling the GUID prefix)
            var filePath = files.FirstOrDefault(f => Path.GetFileName(f).Contains(document.FileName));
            
            if (filePath == null || !System.IO.File.Exists(filePath))
            {
                // Fall back to the original path format
                filePath = Path.Combine(_environment.WebRootPath, "uploads", document.FileName);
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound("File not found on server. Please contact administrator.");
                }
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(Path.GetFileName(filePath)), Path.GetFileName(filePath));
        }

        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            switch (ext)
            {
                case ".pdf": return "application/pdf";
                case ".doc": return "application/msword";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls": return "application/vnd.ms-excel";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".png": return "image/png";
                case ".jpg":
                case ".jpeg": return "image/jpeg";
                default: return "application/octet-stream";
            }
        }
    }
} 