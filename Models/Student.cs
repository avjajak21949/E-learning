using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demo03.Models
{
    public class Student : IdentityUser
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Department { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string CreatedByEmployerId { get; set; }
        [ForeignKey("CreatedByEmployerId")]
        public IdentityUser Employer { get; set; }

        // Navigation properties
        public virtual ICollection<StudentClass> StudentClasses { get; set; }

        public Student()
        {
            StudentClasses = new HashSet<StudentClass>();
        }
    }
} 