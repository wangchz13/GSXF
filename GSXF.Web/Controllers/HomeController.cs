using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSXF.Auxiliary;
using Newtonsoft.Json;
using GSXF.DataBase;
using GSXF.Model;


namespace GSXF.Web.Controllers
{
    
    public class HomeController : Controller
    {

        public ActionResult Default()
        {
            return View();
        }

        public ActionResult Index()
        {
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
        public ActionResult GCSCX()
        {
            return View();
        }
    }
}