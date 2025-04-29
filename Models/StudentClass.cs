using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo03.Models
{
    public class StudentClass
    {
        [Key]
        public int StudentClassID { get; set; }

        [Required]
        public int ClassID { get; set; }

        [Required]
        public string StudentId { get; set; }

        [ForeignKey("ClassID")]
        public virtual Class Class { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }
}
