using System;
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
    public class MeetingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MeetingsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Meetings
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (!User.IsInRole("Teacher") && !User.IsInRole("Student"))
            {
                TempData["Error"] = "You don't have permission to view meetings.";
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                IQueryable<Meeting> query = _context.Meetings
                    .Include(m => m.Class)
                    .Include(m => m.Host);

                if (User.IsInRole("Teacher"))
                {
                    query = query.Where(m => m.HostUserId == user.Id);
                }
                else
                {
                    var studentClasses = await _context.StudentClasses
                        .Where(sc => sc.StudentId == user.Id)
                        .Select(sc => sc.ClassID)
                        .ToListAsync();

                    query = query.Where(m => studentClasses.Contains(m.ClassID));
                }

                return View(await query.ToListAsync());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading meetings.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Meetings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .Include(m => m.Class)
                .Include(m => m.Host)
                .FirstOrDefaultAsync(m => m.MeetingID == id);

            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

        // GET: Meetings/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Teacher"))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name");
            return View();
        }

        // POST: Meetings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("ClassID,StartTime,MeetingLink")] Meeting meeting)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Teacher"))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            meeting.HostUserId = user.Id;
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(meeting);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Meeting created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to create meeting. Please try again.");
                    TempData["Error"] = "Error creating meeting.";
                }
            }
            
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name", meeting.ClassID);
            return View(meeting);
        }

        // GET: Meetings/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Teacher"))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            if (meeting.HostUserId != user.Id)
            {
                return Forbid();
            }

            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name", meeting.ClassID);
            return View(meeting);
        }

        // POST: Meetings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("MeetingID,ClassID,StartTime,MeetingLink")] Meeting meeting)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Teacher"))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (id != meeting.MeetingID)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            meeting.HostUserId = user.Id;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meeting);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Meeting updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingExists(meeting.MeetingID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "ClassID", "Name", meeting.ClassID);
            return View(meeting);
        }

        // GET: Meetings/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .Include(m => m.Class)
                .Include(m => m.Host)
                .FirstOrDefaultAsync(m => m.MeetingID == id);

            if (meeting == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (meeting.HostUserId != user.Id)
            {
                return Forbid();
            }

            return View(meeting);
        }

        // POST: Meetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (meeting.HostUserId != user.Id)
            {
                return Forbid();
            }

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingExists(int id)
        {
            return _context.Meetings.Any(e => e.MeetingID == id);
        }
    }
} 