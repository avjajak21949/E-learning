using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Demo03.Data;
using Demo03.Models;

namespace Demo03.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Blog
        public async Task<IActionResult> Index()
        {
            var documents = await _context.Documents
                .Include(d => d.UploadedBy)
                .Include(d => d.Comments)
                    .ThenInclude(c => c.User)
                .OrderByDescending(d => d.UploadDate)
                .ToListAsync();

            return View(documents);
        }

        // GET: Blog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .Include(d => d.UploadedBy)
                .Include(d => d.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.DocumentID == id);

            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Blog/Comment/5
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Comment(int documentId, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return BadRequest("Comment content cannot be empty");
            }

            var user = await _userManager.GetUserAsync(User);
            var document = await _context.Documents.FindAsync(documentId);

            if (document == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Content = content,
                DocumentID = documentId,
                UserID = user.Id,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = documentId });
        }

        // GET: Blog/Download/5
        [Authorize]
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
                if (!hasAccess && document.UploadedByUserId != user.Id)
                {
                    return Forbid();
                }
            }

            // Search for file with the document's filename in the uploads directory
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            var files = Directory.GetFiles(uploadsFolder);
            
            // Find any file that contains the document filename (handling the GUID prefix)
            var filePath = files.FirstOrDefault(f => Path.GetFileName(f).Contains(document.FileName));
            
            if (filePath == null || !System.IO.File.Exists(filePath))
            {
                // Fall back to the original path format
                filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", document.FileName);
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