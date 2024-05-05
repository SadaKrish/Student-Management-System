using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
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

        // GET: Students/Create
        public ActionResult Create()
        {
            return PartialView("_CreateNewPartial");
        }

       
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
        public JsonResult IsStdIDAvailable(string stdID)
        {
            bool isAvailable = !db.Students.Any(s => s.StdID == stdID);
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
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
            return PartialView("_EditPartial",student);
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
            return PartialView("_EditPartial",student);
        }

        // POST: Students/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Student student = db.Students.Find(id);
        //    if (student == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return PartialView("_DeletePartial", student);
        //}

        // POST: Students/Delete/5
        
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return Json(new { success = false, message = "Student not found." });
                }

                db.Students.Remove(student);
                db.SaveChanges();

                return Json(new { success = true, message = "Student deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the student: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult ToggleEnable(string id, bool enable)
        {
            try
            {
                var student = db.Students.Find(id);
                if (student == null)
                {
                    return Json(new { success = false, message = "Student not found." });
                }

                student.Enable = enable;
                db.SaveChanges();

                return Json(new { success = true, message = enable ? "Student enabled successfully." : "Student disabled successfully." });
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

        //validation check for id
       

    }
}
