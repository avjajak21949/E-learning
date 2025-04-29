using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo03.Models
{
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }
        
        [Required]
        [Display(Name = "Class")]
        public int ClassID { get; set; }
        
        [Required]
        [Display(Name = "Day of Week")]
        public DayOfWeek DayOfWeek { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        
        [Required]
        [Display(Name = "Days of Week")]
        public string DaysOfWeek { get; set; } // Stored as "Mon,Wed,Fri"
        
        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
        
        [Display(Name = "Location")]
        public string Location { get; set; }
        
        [Required]
        [Range(1, 200)]
        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }
        
        // Navigation property
        [ForeignKey("ClassID")]
        public virtual Class Class { get; set; }
    }
}