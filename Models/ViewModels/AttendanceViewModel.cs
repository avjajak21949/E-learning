using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Demo03.Models
{
    public class AttendanceViewModel
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public List<StudentAttendanceViewModel> StudentAttendance { get; set; }
    }

    public class StudentAttendanceViewModel
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public bool IsPresent { get; set; }
    }
} 