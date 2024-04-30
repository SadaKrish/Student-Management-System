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
    public class StudentsController : Controller
    {
        private sms_dbEntities db = new sms_dbEntities();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return PartialView("_DetailsPartial");
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return PartialView("_CreateNewPartial");
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StdID,FirstName,LastName,Gender,DOB,Address,ContactNo,Enable")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return PartialView("EditPartial");
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StdID,FirstName,LastName,Gender,DOB,Address,ContactNo,Enable")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView("_EditPartial");
        }

        // GET: Students/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
        //public ActionResult StudentsSubjects(string id)
        //{
        //    var student = db.Students.Include(s => s.Subjects).FirstOrDefault(s => s.StdID == id);
        //    var subject = student != null ? student.Subjects : new List<Subject>();
        //    return View(subject);
        //}

        //public ActionResult Teachers(string id)
        //{
        //    var studentTeachers = db.StudentTeachers
        //        .Include(st => st.Teacher)
        //        .Where(st => st.StudentId == id)
        //        .Select(st => st.Teacher)
        //        .ToList();

        //    return View(studentTeachers);
        //}
        public ActionResult StudentDetails(string id)
        {
            // Retrieve the student with the specified ID including related teachers and subjects
            var student = db.Students
                            .Include(s => s.Teachers)
                            .Include(s => s.Subjects)
                            .FirstOrDefault(s => s.StdID == id);

            if (student == null)
            {
                return HttpNotFound();
            }

            return PartialView("_StudentDetails", student);
            //return View(student);
        }



    }
}
