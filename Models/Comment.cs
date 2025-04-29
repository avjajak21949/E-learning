using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Demo03.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string UserID { get; set; }
        public IdentityUser User { get; set; }

        [Required]
        public int DocumentID { get; set; }
        public Document Document { get; set; }
    }
} 