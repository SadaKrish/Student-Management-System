﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sms.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
          // Student objStudentModel = new Student();
            return View();
        }
    }
}