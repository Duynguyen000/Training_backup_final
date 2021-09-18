using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training.Models
{
    public class Course
    {
        public int id { get; set; }
        public string Coursename { get; set; }
        public string CourseDescription { get; set; }
        public DateTime CourseDate { get; set; }

        public string InstructorId { get; set; }
    }
}