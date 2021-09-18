using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Training.Models
{
    public class CourseHomeViewModel
    {
        public string Coursename { get; set; }
        public string CourseDescription { get; set; }

        public string InstructorId { get; set; }
        public string InstructorEmail { get; set; }
        public string InstructorFullName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CourseDate { get; set; }
    }
}