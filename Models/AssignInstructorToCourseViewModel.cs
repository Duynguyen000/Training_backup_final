using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training.Models
{
    public class AssignInstructorToCourseViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}