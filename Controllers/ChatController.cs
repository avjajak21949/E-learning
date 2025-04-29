using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Demo03.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo03.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("history/{userId}")]
        public async Task<IActionResult> GetHistory(string userId)
        {
            try
            {
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                
                var messages = await _context.ChatMessages
                    .Where(m => 
                        (m.SenderId == currentUserId && m.ReceiverId == userId) || 
                        (m.SenderId == userId && m.ReceiverId == currentUserId))
                    .OrderByDescending(m => m.Timestamp)
                    .Take(50) // Get last 50 messages
                    .OrderBy(m => m.Timestamp) // Order them chronologically
                    .Select(m => new
                    {
                        senderId = m.SenderId,
                        senderName = m.SenderName,
                        content = m.Content,
                        timestamp = m.Timestamp
                    })
                    .ToListAsync();

                return Ok(new { success = true, messages = messages });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error retrieving chat history" });
            }
        }
    }
} 