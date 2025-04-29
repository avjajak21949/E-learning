using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Demo03.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Demo03.Controllers
{
    [Authorize]
    public class MeetingRoomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MeetingRoomController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MeetingRoom/{id}
        public async Task<IActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .Include(m => m.Host)
                .Include(m => m.Class)
                .FirstOrDefaultAsync(m => m.MeetingLink.EndsWith(id));

            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }
    }
} 