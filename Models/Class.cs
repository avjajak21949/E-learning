using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demo03.Models
{
    public class Class
    {
        [Key]
        public int ClassID { get; set; }
        
        [Required]
        public int CourseID { get; set; }
        public Course Course { get; set; }

        [Required]
        public string TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string ScheduleInfo { get; set; }

        [Required]
        public int MaxCapacity { get; set; } = 30;

        public string CreatedByEmployerId { get; set; }

        [ForeignKey("CreatedByEmployerId")]
        public IdentityUser Employer { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Meeting> Meetings { get; set; }
        public virtual ICollection<StudentClass> StudentClasses { get; set; }

        public Class()
        {
            Schedules = new HashSet<Schedule>();
            Meetings = new HashSet<Meeting>();
            StudentClasses = new HashSet<StudentClass>();
        }
    }
}

