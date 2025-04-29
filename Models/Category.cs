using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo03.Models
{
    public class Category
    {
        public Category()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        public int CategoryID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
