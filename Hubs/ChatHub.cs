using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using Demo03.Data;
using Demo03.Models;
using System;

namespace Demo03.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task SendMessage(string message, string recipientId)
        {
            var senderId = Context.UserIdentifier;
            var senderName = Context.User.Identity.Name;
            
            // Save message to database
            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                SenderName = senderName,
                Content = message,
                Timestamp = DateTime.Now,
                ReceiverId = recipientId
            };
            
            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();
            
            // Send to specific recipient
            await Clients.User(recipientId).SendAsync("ReceiveMessage", senderId, senderName, message);
            
            // Send back to sender for confirmation
            await Clients.Caller.SendAsync("ReceiveMessage", senderId, senderName, message);
        }

        public async Task JoinChat(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task LeaveChat(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
    }
} 