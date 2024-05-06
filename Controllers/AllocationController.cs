using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sms.Models;
using System.Data.Entity;


namespace sms.Controllers
{
    public class AllocationController : Controller
    {
        private sms_db1Entities db = new sms_db1Entities(); // Replace YourDatabaseContext with your actual database context class

        // GET: Allocation
        public ActionResult Index()
        {
            return View();
        }

        // GET: Allocation/TeacherSubjectAllocation
        public ActionResult TeacherSubjectAllocation()
        {
            ViewBag.Teachers = db.Teachers.ToList();
            ViewBag.Subjects = db.Subjects.ToList();
            return View();
        }

        // POST: Allocation/SaveTeacherSubjectAllocation
        [HttpPost]
        public ActionResult SaveTeacherSubjectAllocation(string teacherId, List<string> subjectIds)
        {
            if (string.IsNullOrEmpty(teacherId) || subjectIds == null || subjectIds.Count == 0)
            {
                // Handle invalid input
                return Json(new { success = false, message = "Invalid input data." });
            }

            try
            {
                var teacher = db.Teachers.SingleOrDefault(t => t.TeacherID == teacherId);
                if (teacher == null)
                {
                    // Handle teacher not found
                    return Json(new { success = false, message = "Teacher not found." });
                }

                foreach (var subjectId in subjectIds)
                {
                    var allocation = new Teacher_Subject_Allocation
                    {
                        TeacherID = teacherId,
                        Sub_code = subjectId
                    };
                    db.Set<Teacher_Subject_Allocation>().Add(allocation); // Add allocation object to DbSet
                }

                db.SaveChanges();
                return Json(new { success = true, message = "Teacher-subject allocations saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error occurred while saving teacher-subject allocations: " + ex.Message });
            }
        }


    }
}
