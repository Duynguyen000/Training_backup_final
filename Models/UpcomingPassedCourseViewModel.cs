using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Training.Models
{
    public class UpcomingPassedCourseViewModel
    {
        public IEnumerable<CourseHomeViewModel> UpcomingCourses { get; set; }

        public IEnumerable<CourseHomeViewModel> PassedCourses { get; set; }
    }
}