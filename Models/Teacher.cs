using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demo03.Models
{
    public class Teacher : IdentityUser
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Specialization")]
        public string Specialization { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string CreatedByEmployerId { get; set; }

        [ForeignKey("CreatedByEmployerId")]
        public IdentityUser Employer { get; set; }

        // Navigation properties
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Meeting> Meetings { get; set; }
        public virtual ICollection<Document> Documents { get; set; }

        public Teacher()
        {
            Classes = new HashSet<Class>();
            Meetings = new HashSet<Meeting>();
            Documents = new HashSet<Document>();
        }
    }
} 