using Training.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Training.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var courses = from c in db.Courses
                          join u in db.Users on c.InstructorId equals u.Id into ucg
                          from u in ucg.DefaultIfEmpty()
                          select new CourseHomeViewModel
                          {
                              Coursename = c.Coursename,
                              CourseDescription = c.CourseDescription,
                              InstructorEmail = u.Email,
                              InstructorFullName = u.FullName,
                              CourseDate = c.CourseDate,
                          };

            var upcomingCourses = courses.Where(e => e.CourseDate > DateTime.Now);
            var passedCourses = courses.Where(e => e.CourseDate <= DateTime.Now);
            return View(new UpcomingPassedCourseViewModel()
            {
                UpcomingCourses = upcomingCourses,
                PassedCourses = passedCourses,
            });
        }
    }
}