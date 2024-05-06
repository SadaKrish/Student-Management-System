using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using sms.Models;

namespace sms.Controllers
{
    public class StudentSubjectTeacherAllocationController : Controller
    {
        private sms_db1Entities db = new sms_db1Entities();

        // GET: StudentSubjectTeacherAllocation
        public ActionResult Index()
        {
            var studentSubjectTeacherAllocations = db.Student_Subject_Teacher_Allocation.Include(s => s.Student).Include(s => s.Subject).Include(s => s.Teacher);
            return View(studentSubjectTeacherAllocations.ToList());
        }

        // GET: StudentSubjectTeacherAllocation/Details/5
        public ActionResult Details(string StdID, string TeacherID, string Sub_code)
        {
            if (StdID == null || TeacherID == null || Sub_code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_Subject_Teacher_Allocation student_Subject_Teacher_Allocation = db.Student_Subject_Teacher_Allocation.Find(StdID, TeacherID, Sub_code);
            if (student_Subject_Teacher_Allocation == null)
            {
                return HttpNotFound();
            }
            return View(student_Subject_Teacher_Allocation);
        }

        // GET: StudentSubjectTeacherAllocation/Create
        public ActionResult Create()
        {
            ViewBag.StdID = new SelectList(db.Students, "StdID", "FirstName");
            ViewBag.Sub_code = new SelectList(db.Subjects, "Sub_code", "Name");
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "FirstName");
            return View();
        }

        // POST: StudentSubjectTeacherAllocation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StdID,TeacherID,Sub_code")] Student_Subject_Teacher_Allocation student_Subject_Teacher_Allocation)
        {
            if (ModelState.IsValid)
            {
                db.Student_Subject_Teacher_Allocation.Add(student_Subject_Teacher_Allocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StdID = new SelectList(db.Students, "StdID", "FirstName", student_Subject_Teacher_Allocation.StdID);
            ViewBag.Sub_code = new SelectList(db.Subjects, "Sub_code", "Name", student_Subject_Teacher_Allocation.Sub_code);
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "FirstName", student_Subject_Teacher_Allocation.TeacherID);
            return View(student_Subject_Teacher_Allocation);
        }

        // GET: StudentSubjectTeacherAllocation/Edit/5
        public ActionResult Edit(string StdID, string TeacherID, string Sub_code)
        {
            if (StdID == null || TeacherID == null || Sub_code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_Subject_Teacher_Allocation student_Subject_Teacher_Allocation = db.Student_Subject_Teacher_Allocation.Find(StdID, TeacherID, Sub_code);
            if (student_Subject_Teacher_Allocation == null)
            {
                return HttpNotFound();
            }
            ViewBag.StdID = new SelectList(db.Students, "StdID", "FirstName", student_Subject_Teacher_Allocation.StdID);
            ViewBag.Sub_code = new SelectList(db.Subjects, "Sub_code", "Name", student_Subject_Teacher_Allocation.Sub_code);
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "FirstName", student_Subject_Teacher_Allocation.TeacherID);
            return View(student_Subject_Teacher_Allocation);
        }

        // POST: StudentSubjectTeacherAllocation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StdID,TeacherID,Sub_code")] Student_Subject_Teacher_Allocation student_Subject_Teacher_Allocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student_Subject_Teacher_Allocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StdID = new SelectList(db.Students, "StdID", "FirstName", student_Subject_Teacher_Allocation.StdID);
            ViewBag.Sub_code = new SelectList(db.Subjects, "Sub_code", "Name", student_Subject_Teacher_Allocation.Sub_code);
            ViewBag.TeacherID = new SelectList(db.Teachers, "TeacherID", "FirstName", student_Subject_Teacher_Allocation.TeacherID);
            return View(student_Subject_Teacher_Allocation);
        }

        // GET: StudentSubjectTeacherAllocation/Delete/5
        public ActionResult Delete(string StdID, string TeacherID, string Sub_code)
        {
            if (StdID == null || TeacherID == null || Sub_code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student_Subject_Teacher_Allocation student_Subject_Teacher_Allocation = db.Student_Subject_Teacher_Allocation.Find(StdID, TeacherID, Sub_code);
            if (student_Subject_Teacher_Allocation == null)
            {
                return HttpNotFound();
            }
            return View(student_Subject_Teacher_Allocation);
        }

        
        // POST: StudentSubjectTeacherAllocation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string StdID, string TeacherID, string Sub_code)
        {
            Student_Subject_Teacher_Allocation student_Subject_Teacher_Allocation = db.Student_Subject_Teacher_Allocation.Find(StdID, TeacherID, Sub_code);
            db.Student_Subject_Teacher_Allocation.Remove(student_Subject_Teacher_Allocation);
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

