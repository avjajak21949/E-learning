using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demo03.Models
{
    public class Meeting
    {
        [Key]
        public int MeetingID { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Meeting Link")]
        public string MeetingLink { get; set; }

        public string HostUserId { get; set; }

        [ForeignKey("HostUserId")]
        public IdentityUser Host { get; set; }

        [Required]
        public int ClassID { get; set; }

        [ForeignKey("ClassID")]
        public Class Class { get; set; }
    }
} 