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
        private sms_db1Entities db = new sms_db1Entities();

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
            return PartialView("_EditPartial",subject);
        }

        // GET: Subjects/Delete/5

        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Subject subject = db.Subjects.Find(id);
                if (subject == null)
                {
                    return Json(new { success = false, message = "Subject not found." });
                }

                db.Subjects.Remove(subject);
                db.SaveChanges();

                return Json(new { success = true, message = "Subject deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the subject " + ex.Message });
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
        public ActionResult SubjectDetails(string subjectCode)
        {
            if (string.IsNullOrEmpty(subjectCode))
            {
                return HttpNotFound(); // Handle invalid subjectCode
            }

            // Retrieve the subject with the given subjectCode
            var subject = db.Subjects
                .Include(s => s.Teachers) // Include the Teachers navigation property
                .Include(s => s.Student_Subject_Teacher_Allocation) // Include the Student_Subject_Teacher_Allocation navigation property
                .SingleOrDefault(s => s.Sub_code == subjectCode);

            if (subject == null)
            {
                return HttpNotFound(); // Handle subject not found
            }

            // Pass the subject and associated teachers to the view
            return PartialView("_SubjectDetails",subject);
        }

    }
}
