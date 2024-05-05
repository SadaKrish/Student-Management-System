using sms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace sms.Controllers
{
    public class AllocationController : Controller
    {
        //private readonly SMS_DBEntities _db;
        private readonly sms_dbEntities db = new sms_dbEntities();
        public AllocationController()
        {
            db = new sms_dbEntities(); // Initialize the DbContext
        }

        private List<Teacher> GetTeachers()
        {
            var teachers = db.Teachers.ToList();
            return teachers;
        }

        private List<Subject> GetSubjects()
        {
            var subject = db.Subjects.ToList();
            return subject;
        }

        private List<Student> GetStudents()
        {
            var student = db.Students.ToList();
            return student;
        }

        public ActionResult Allocation()
        {

            var students = GetStudents();
            ViewBag.StudentIdList = new SelectList(students, "StdID", "StdID");

            var teachers = GetTeachers();
            ViewBag.TeacherIdList = new SelectList(teachers, "TeacherID", "FirstName");

            var subjects = GetSubjects();
            ViewBag.Subjects = new SelectList(subjects, "Sub_code", "Sub_code", "Name");


            return View();
        }

        // GET: Allocation/AllocatedSubjects
        public ActionResult AllocatedSubjects()
        {
            var allocatedSubjects = db.Subjects.Include(s => s.Teachers).ToList();
            return PartialView("_AllocatedSubjects", allocatedSubjects);
        }


        //Check SubjectAllocation
        [HttpPost]
        public ActionResult CheckSubjectAllocated(string teacherId, string subjectCode)
        {
            try
            {
                // Check if the subject is allocated to any student
                var allocatedToStudent = db.Subjects
                                             .Any(s => s.Sub_code == subjectCode && s.Students.Any());

                return Json(new { success = !allocatedToStudent });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        //Edit Allocation 
        [HttpPost]
        public ActionResult EditSubjectAllocation(string teacherId, string subjectCode)
        {
            try
            {
                // Fetch the details for editing the subject allocation
                var teacher = db.Teachers.Include(t => t.Subjects).FirstOrDefault(t => t.TeacherID == teacherId);
                if (teacher == null)
                {
                    return Json(new { success = false, message = "Teacher not found." });
                }

                var subject = teacher.Subjects.FirstOrDefault(s => s.Sub_code == subjectCode);
                if (subject == null)
                {
                    return Json(new { success = false, message = "Subject not found for this teacher." });
                }

                // Check if the subject is assigned to any student
                var assignedToStudent = subject.Students.Any();
                if (assignedToStudent)
                {
                    // If subject is allocated for any student, prevent editing
                    return Json(new { success = false, message = "Subject is allocated for one or more students. Cannot edit." });
                }

                // Return the subject details for editing
                var data = new { teacher, subject };
                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }



        // POST: Allocation/AssignSubjectToTeacher
        [HttpPost]
        public ActionResult AssignSubjectToTeacher(string teacherId, string subjectCode)
        {
            try
            {
                var teacher = db.Teachers.Include(t => t.Subjects).FirstOrDefault(t => t.TeacherID == teacherId);
                if (teacher == null)
                {
                    return Json(new { success = false, message = "Teacher not found." });
                }

                var subject = db.Subjects.FirstOrDefault(s => s.Sub_code == subjectCode);
                if (subject == null)
                {
                    return Json(new { success = false, message = "Subject not found." });
                }

                teacher.Subjects.Add(subject);
                subject.Teachers.Add(teacher);

                db.SaveChanges();

                return Json(new { success = true, message = "Subject assigned to the teacher successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }


        //FOR STUDENTS
        public ActionResult AllocatedStudents()
        {
            try
            {
                var allocatedStudents = db.Students.Include(s => s.Subjects.Select(sub => sub.Teachers)).ToList();
                return PartialView("_AllocatedStudent", allocatedStudents);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an empty list if an error occurs
                ViewBag.ErrorMessage = "An error occurred while retrieving allocated students: " + ex.Message;
                return PartialView("_AllocatedStudent", new List<Student>());
            }
        }


        // POST: Allocation/AssignSubjectAndTeacherToStudent
        [HttpPost]
        public ActionResult AssignSubjectAndTeacherToStudent(string studentId, string subjectCode, string teacherId)
        {
            try
            {
                var student = db.Students.Include(s => s.Subjects).FirstOrDefault(s => s.StdID == studentId);
                if (student == null)
                {
                    return Json(new { success = false, message = "Student not found." });
                }

                var subject = db.Subjects.FirstOrDefault(s => s.Sub_code == subjectCode);
                if (subject == null)
                {
                    return Json(new { success = false, message = "Subject not found." });
                }

                var teacher = db.Teachers.FirstOrDefault(t => t.TeacherID == teacherId);
                if (teacher == null)
                {
                    return Json(new { success = false, message = "Teacher not found." });
                }

                // Assign subject and teacher to the student
                student.Subjects.Add(subject);
                subject.Students.Add(student);
                subject.Teachers.Add(teacher);

                db.SaveChanges();

                return Json(new { success = true, message = "Subject and teacher assigned to the student successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult GetTeachersForSubject(string subjectCode)
        {
            try
            {
                // Assuming you have a database context named "con"
                var teachers = db.Subjects
                                   .Include(s => s.Teachers)
                                   .FirstOrDefault(s => s.Sub_code == subjectCode)?
                                   .Teachers;

                if (teachers == null)
                {
                    return Json(new { success = false, message = "Teachers not found for the selected subject." });
                }

                //list of teacher
                var teacherList = teachers.Select(t => new SelectListItem
                {
                    Value = t.TeacherID,
                    Text = t.FirstName
                });

                return PartialView("_TeachersDropdown", teacherList);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult RemoveStudentAssignment(string studentId, string subjectCode)
        {
            try
            {
                var student = db.Students.Include(s => s.Subjects).FirstOrDefault(s => s.StdID == studentId);
                if (student == null)
                {
                    return Json(new { success = false, message = "Student not found." });
                }

                var subject = student.Subjects.FirstOrDefault(s => s.Sub_code == subjectCode);
                if (subject == null)
                {
                    return Json(new { success = false, message = "Subject not found for this student." });
                }

                // Remove the association between the student and the subject
                student.Subjects.Remove(subject);
                db.SaveChanges();

                return Json(new { success = true, message = "Student assignment removed successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }



        [HttpPost]
        public ActionResult RemoveSubjectFromTeacher(string teacherId, string subjectCode)
        {
            try
            {
                var teacher = db.Teachers.Include(t => t.Subjects).FirstOrDefault(t => t.TeacherID == teacherId);
                if (teacher == null)
                {
                    return Json(new { success = false, message = "Teacher not found." });
                }

                var subject = teacher.Subjects.FirstOrDefault(s => s.Sub_code == subjectCode);
                if (subject == null)
                {
                    return Json(new { success = false, message = "Subject not found for this teacher." });
                }

                // Check if the subject is assigned to any student
                var assignedToStudent = subject.Students.Any();
                if (assignedToStudent)
                {
                    // Subject assigned to students, prompt for confirmation
                    return Json(new { success = false, confirm = true, message = "Subject assigned to students", teacherId = teacherId, subjectCode = subjectCode });
                }

                // If not assigned to any student, proceed with removing the subject from the teacher
                teacher.Subjects.Remove(subject);
                db.SaveChanges();

                return Json(new { success = true, message = "Subject removed from teacher successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult RemoveAllStudentAssignments(string teacherId, string subjectCode)
        {
            try
            {
                // Get the teacher
                var teacher = db.Teachers.Include(t => t.Subjects).FirstOrDefault(t => t.TeacherID == teacherId);
                if (teacher == null)
                {
                    return Json(new { success = false, message = "Teacher not found." });
                }

                // Get the subject
                var subject = teacher.Subjects.FirstOrDefault(s => s.Sub_code == subjectCode);
                if (subject == null)
                {
                    return Json(new { success = false, message = "Subject not found for this teacher." });
                }

                // Remove the association between the teacher and the subject
                teacher.Subjects.Remove(subject);

                // Get the list of students associated with the specified teacher and subject
                var students = db.Students.Where(s => s.Subjects.Any(sub => sub.Teachers.Any(t => t.TeacherID == teacherId && sub.Sub_code == subjectCode))).ToList();

                // Remove the associations between students and the specified subject
                foreach (var student in students)
                {
                    var subjectToRemove = student.Subjects.FirstOrDefault(sub => sub.Sub_code == subjectCode);
                    if (subjectToRemove != null)
                    {
                        student.Subjects.Remove(subjectToRemove);
                    }
                }

                // Save changes to the database
                db.SaveChanges();

                return Json(new { success = true, message = "All student assignments for the specified teacher and subject have been removed successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });

            }
        }

    }

 }