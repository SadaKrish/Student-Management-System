//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace sms.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student_Subject_Teacher_Allocation
    {
        public string StdID { get; set; }
        public string TeacherID { get; set; }
        public string Sub_code { get; set; }
    
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
