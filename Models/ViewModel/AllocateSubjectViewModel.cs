using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sms.Models.ViewModel
{
    public class AllocateSubjectsViewModel
    {
        public int StudentId { get; set; }
        public int[] SelectedSubjects { get; set; }
        public List<AllocateSubjectViewModel> Allocations { get; set; }
    }

    public class AllocateSubjectViewModel
    {
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
    }
}