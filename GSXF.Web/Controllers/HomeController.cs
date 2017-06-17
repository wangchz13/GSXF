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

        [HttpGet]
        public ActionResult test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult test(FormCollection form)
        {
            if(Request.Files.Count == 0)
            {
                return View();
            }
            var file = Request.Files[0];
            if(file.ContentLength == 0)
            {
                return View();
            }
            var fileName = file.FileName;
            
            var filePath = Server.MapPath(string.Format("~/{0}", "Upload"));

            var path = string.Format(filePath + "\\{0}", fileName);
            file.SaveAs(path);
            return View();
        }
    }
}