using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Training.Models;

namespace Training.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        public ActionResult Index()
        {
            var users = db.Users.ToList();
            var courses = db.Courses.ToList();
            AssignInstructorToCourseViewModel viewModel = new AssignInstructorToCourseViewModel { Courses = courses, Users = users };
            return View(viewModel);
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            //var course2 = db.Courses.Where(c => c.Id == id.Value).Include(i => i.CourseInstructor.Id;
            //Course course = db.Courses.AsQueryable<Courses>().Select(x => x.id == id.Value);
            string query = "Select dbo.AspNetUsers.UserName from dbo.AspNetUsers,dbo.Courses where (dbo.AspNetUsers.Id = dbo.Courses.InstructorId) AND (dbo.Courses.Id = " + id.Value + ")";
            var queryResult = db.Database.SqlQuery<string>(query);
            string name = queryResult.FirstOrDefault();
            if (name == null) name = "Not Assigned";
            if (course == null)
            {
                return HttpNotFound();
            }

            ViewBag.instructor_name = name;
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult PartialCreate(string id)
        {
            TempData["id"] = id;
            return RedirectToAction("Create");
        }

        public ActionResult CreateWithInstructor(string instructor_id, string instructor_name)
        {
            ViewBag.instructor_id = instructor_id;
            ViewBag.instructor_name = instructor_name;
            return View("Create");
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string courseInstructorId, [Bind(Include = "Coursename,CourseDescription,CourseDate,InstructorId")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.InstructorId = courseInstructorId;

                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Coursename,CourseDescription,CourseDate,InstructorId")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}