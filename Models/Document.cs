using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demo03.Models
{
    public enum DocumentType
    {
        StudentWork,
        CourseDocument
    }

    public enum SubmissionStatus
    {
        Submitted,
        Graded,
        Returned
    }

    public enum MaterialCategory
    {
        LectureNotes,
        Assignment,
        Reference,
        Syllabus,
        Other
    }

    public class Document
    {
        [Key]
        public int DocumentID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Document Title")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Document Type")]
        public DocumentType Type { get; set; }

        // For identifying who uploaded the document
        [Required]
        public string UploadedByUserId { get; set; }
        public IdentityUser UploadedBy { get; set; }

        // Associated Class/Course
        public int? ClassID { get; set; }
        public Class Class { get; set; }

        public SubmissionStatus? Status { get; set; }
        public string Feedback { get; set; }
        public DateTime? DueDate { get; set; }

        // For Course Materials
        public int? CourseID { get; set; }
        public Course Course { get; set; }

        public MaterialCategory? Category { get; set; }

        // Comments collection
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
} 