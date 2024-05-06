using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sms.Models;

namespace sms.Controllers
{
    public class TeachersController : Controller
    {
        private sms_db1Entities db = new sms_db1Entities();

        // GET: Teachers
        public ActionResult Index()
        {
            return View(db.Teachers.ToList());
        }

        // GET: Teachers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return PartialView("_CreateNew");
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TeacherID,FirstName,LastName,Gender,DOB,Address,ContactNo,Enable")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return PartialView("_EditPartial",teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TeacherID,FirstName,LastName,Gender,DOB,Address,ContactNo,Enable")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, redirectTo = Url.Action("Index") });
            }
            return PartialView("_EditPartial", teacher);
        }


        // GET: Teachers/Delete/5
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Teacher teacher = db.Teachers.Find(id);
                if (teacher == null)
                {
                    return Json(new { success = false, message = "Teacher not found." });
                }

                db.Teachers.Remove(teacher);
                db.SaveChanges();

                return Json(new { success = true, message = "Teacher deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the teacher: " + ex.Message });
            }
        }
        //check box
        [HttpPost]
        public ActionResult ToggleEnable(string id, bool enable)
        {
            try
            {
                var teacher = db.Teachers.Find(id);
                if (teacher == null)
                {
                    return Json(new { success = false, message = "Teacher not found." });
                }

                teacher.Enable = enable;
                db.SaveChanges();

                return Json(new { success = true, message = enable ? "Teacher enabled successfully." : "Teacher disabled successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult TeacherDetails(string teacherId)
        {
            // Retrieve the student with the given studentId
            var teacher= db.Teachers.Include(s => s.Student_Subject_Teacher_Allocation)
                                      .SingleOrDefault(s => s.TeacherID == teacherId);

            if (teacher == null)
            {
                return HttpNotFound(); // Or handle the case when student is not found
            }

            // Pass the student, teachers, and subjects to the view
            ViewBag.Teachers = teacher;
            ViewBag.Teachers = teacher.Student_Subject_Teacher_Allocation.Select(a => a.Student);
            ViewBag.Subjects = teacher.Student_Subject_Teacher_Allocation.Select(a => a.Subject);

            return PartialView("_TeacherDetails", teacher);
        }
    }
}
