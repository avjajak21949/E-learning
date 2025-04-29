using System;
using System.ComponentModel.DataAnnotations;

namespace Demo03.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string SenderId { get; set; }
        
        [Required]
        public string SenderName { get; set; }
        
        [Required]
        public string Content { get; set; }
        
        [Required]
        public string ReceiverId { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
} 