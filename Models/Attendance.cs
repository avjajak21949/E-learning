using System;
using System.ComponentModel.DataAnnotations;

namespace Demo03.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }

        [Required]
        public string StudentID { get; set; }

        [Required]
        public int ClassID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Required]
        public bool IsPresent { get; set; }

        // Navigation properties
        public virtual Student Student { get; set; }
        public virtual Class Class { get; set; }
    }
} 