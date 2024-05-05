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
    public class SubjectsController : Controller
    {
        private sms_dbEntities db = new sms_dbEntities();

        // GET: Subjects
        public ActionResult Index()
        {
            return View(db.Subjects.ToList());
        }

        // GET: Subjects/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: Subjects/Create
        public ActionResult Create()
        {
            return PartialView("_CreateNew");
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Sub_code,Name,Enable")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subject);
        }

        // GET: Subjects/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return PartialView("_EditPartial",subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Sub_code,Name,Enable")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        // GET: Subjects/Delete/5
        // GET: Students/Delete/5

         public ActionResult Delete(string id)
        {
            try
            {
                // Find the student with the given ID
                var subject = db.Subjects.Find(id);

                if (subject == null)
                {
                    // Return error response if student not found
                    return Json(new { success = false, message = "Subject not found." });
                }

                // Remove the student from the database
                db.Subjects.Remove(subject);
                db.SaveChanges();

                // Return success response
                return Json(new { success = true, message = "Subject deleted successfully." });
            }
            catch (Exception ex)
            {
                // Return error response if an exception occurs
                return Json(new { success = false, message = "An error occurred while deleting the subject." });
            }
        }

        [HttpPost]
        public ActionResult ToggleEnable(string id, bool enable)
        {
            try
            {
                var subject = db.Subjects.Find(id);
                if (subject == null)
                {
                    return Json(new { success = false, message = "Subject not found." });
                }

                subject.Enable = enable;
                db.SaveChanges();

                return Json(new { success = true, message = enable ? "Subject enabled successfully." : "Subject disabled successfully." });
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
        //subhect details
        public ActionResult SubjectDetails(string id)
        {
            // Retrieve the subject with the specified ID including related students and teachers
            var subject = db.Subjects
                            .Include(s => s.Students)
                            .Include(s => s.Teachers)
                            .FirstOrDefault(s => s.Sub_code == id);

            if (subject == null)
            {
                return HttpNotFound();
            }

            return PartialView("_SubjectDetails", subject);
            // Or return View(subject) if you want to return a full view instead of a partial view
        }
        
    }
}
