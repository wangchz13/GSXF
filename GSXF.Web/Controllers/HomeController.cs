﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSXF.Auxiliary;
using GSXF.Core;

namespace GSXF.Web.Controllers
{
    public class HomeController : Controller
    {
        private static ArticleManager articleManager = new ArticleManager();
        private static EmployeeManager employeeManager = new EmployeeManager();
        public ActionResult Default()
        {
            return View();
        }

        public ActionResult Index()
        {
            articleManager.Add(new Article() { Title = "test" });
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult JGCX()
        {
            return View();
        }

        public ActionResult XMCX()
        {
            return View();
        }

        public ActionResult AddEmployee(List<Employee> list)
        {
            string result = "";
            foreach(var i in list)
            {
                employeeManager.Add(i);
                
            }
            return Json(new { rows = list.Count.ToString(), result = result });
        }
    }
}