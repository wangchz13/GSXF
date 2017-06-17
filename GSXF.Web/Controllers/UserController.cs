using System;
using System.Web.Mvc;
using GSXF.DataBase;
using GSXF.Model;
using GSXF.Web.Models;
using GSXF.Auxiliary;
using GSXF.Security;

using Spire.License;
using Spire.Pdf;
using Spire.Pdf.Graphics;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace GSXF.Web.Controllers
{
    [UserAuthorize]
    public class UserController : Controller
    {
        private static UserManager userManager = new UserManager();
        // GET: User
        public ActionResult Index()
        {
            var user = Session["User"] as User;
            if (user == null)
                return RedirectToAction("Login");

            ViewBag.User = user;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            var user = Session["User"] as User;
            if (user != null)
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login)
        {
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }

            resp = userManager.Verify(login.Name, login.Password);
            if (resp.Code != 3)
                return Json(resp);

            var currentUser = Session["User"] as User;
            if(currentUser != null)
            {
                currentUser.IsOnline = false;
                userManager.Update(currentUser);
            }

            var user = userManager.Find(login.Name);
            user.LoginTime = DateTime.Now;
            user.LoginIP = Request.UserHostAddress;
            user.IsOnline = true;
            userManager.Update(user);
            Session.Add("User", user);
            return Json(resp);
        }


        [AllowAnonymous]
        public ActionResult LoginOut()
        { 
            User user = Session["User"] as User;
            if (user != null)
            {
                user.IsOnline = false;
                userManager.Update(user);
            }
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult Menu()
        {
            User user = Session["User"] as User;
            if (user == null)
                return Content("请登录账号");
            var roleName = user.Role.Name;
            ViewBag.Role = roleName;
            return PartialView();
        }

        public ActionResult MyInfo()
        {
            User user = Session["User"] as User;
            if(user == null)
            {
                return Content("账号已过期，请重新登录");
            }
            ViewBag.Name = user.Name;
            ViewBag.Regtime = user.RegTime;
            var roleName = user.Role.Name;
            ViewBag.Role = roleName;
            return View();
        }

        




        #region 超级管理员
        /// <summary>
        /// 超级管理员页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index1()
        {
            return View();
        }
        #endregion

        #region 消防机构总队
        /// <summary>
        /// 消防机构总队首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index2()
        {
            return View();
        }
        /// <summary>
        /// 项目监管
        /// </summary>
        /// <returns></returns>
        public ActionResult XMJG()
        {
            return View();
        }
        /// <summary>
        /// 机构备案
        /// </summary>
        /// <returns></returns>
        public ActionResult JGBA()
        {

            return View();
        }
        /// <summary>
        /// 注册消防工程师备案
        /// </summary>
        /// <returns></returns>
        public ActionResult ZCGCSBA()
        {
            return View();
        }

        public ActionResult XMCX()
        {
            return View();
        }

        public ActionResult JGCX()
        {
            return View();
        }

        public ActionResult ZCGCSCX()
        {
            return View();
        }
        #endregion

        #region 消防机构支队
        public ActionResult Index3()
        {
            return View();
        }
        #endregion

        #region 消防机构大队
        /// <summary>
        /// 消防机构大队首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index4()
        {
            return View();
        }
        #endregion

        #region 服务机构
        /// <summary>
        /// 服务机构首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index5()
        {
            return View();
        }

        public ActionResult XMDJ()
        {
            return View();
        }

        public ActionResult RYBA()
        {
            return View();
        }
        #endregion


        [HttpPost]
        public ActionResult rcjc()
        {
            Response resp = new Response();

            

            string id = Request.Form["id"];
            string date = Request.Form["date"];

            int projectID = int.Parse(id);
            ProjectManager projectManager = new ProjectManager();
            Project project = projectManager.Find(projectID);

            project.rcjcsj = DateTime.Parse(date);
            project.Progress = ProjectProgress.入场检测;
            
            if (Request.Files.Count == 0)
            {
                resp.Code = -1;
                resp.Message = "上传文件不能为空";
                return Json(resp);
            }
            var file = Request.Files[0];
            if (file.ContentLength >= 20971520)
            {
                resp.Code = -2;
                resp.Message = "上传文件太大";
                return Json(resp);
            }
            var fileName = file.FileName;

            if (fileName.Substring(fileName.Length - 4, 4) != ".rar")
            {
                resp.Code = -3;
                resp.Message = "文件格式不符合要求";
                return Json(resp);
            }

            var filePath = Server.MapPath(string.Format("~/{0}/Datas", "Upload"));

            var path = string.Format(filePath + "\\{0}", fileName);
            file.SaveAs(path);
            FileManager fileManager = new FileManager();
            Model.File f = new Model.File();
            f.Name = project.Name + "_入场检测材料";
            f.Path = path;
            var res = fileManager.Add(f);
            project.DataFile = f;
            projectManager.Update(project);
            return View();
        }

        [HttpPost]
        public ActionResult cjbg()
        {
            Response resp = new Response();



            string id = Request.Form["id"];
            int projectID = int.Parse(id);
            ProjectManager projectManager = new ProjectManager();
            Project project = projectManager.Find(projectID);

            

            if (Request.Files.Count == 0)
            {
                resp.Code = -1;
                resp.Message = "上传文件不能为空";
                return Json(resp);
            }
            var file = Request.Files[0];
            if (file.ContentLength >= 20971520)
            {
                resp.Code = -2;
                resp.Message = "上传文件太大";
                return Json(resp);
            }
            var fileName = file.FileName;

            if (fileName.Substring(fileName.Length - 4, 4) != ".pdf")
            {
                resp.Code = -3;
                resp.Message = "文件格式不符合要求";
                return Json(resp);
            }

            var filePath = Server.MapPath(string.Format("~/{0}/Reports", "Upload"));

            var path = string.Format(filePath + "\\{0}", fileName);
            file.SaveAs(path);


            PdfDocument pdf = new PdfDocument();
            pdf.LoadFromFile(path);
            PdfPageBase page = pdf.Pages[0];
            PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 24);
            PdfSolidBrush brush = new PdfSolidBrush(Color.LightBlue);
            PdfStringFormat leftAlignment = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
            PdfStringFormat rightAlignment = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
            page.Canvas.DrawString("123456789000", font, brush, page.Canvas.ClientSize.Width, 20, rightAlignment);
            pdf.SaveToFile(path);


            FileManager fileManager = new FileManager();
            Model.File f = new Model.File();
            f.Name = project.Name + "_报告";
            f.Path = path;
            var res = fileManager.Add(f);
            project.ReportFile = f;


            project.Progress = ProjectProgress.出具报告;
            projectManager.Update(project);
            return View();
        }

        public ActionResult tjba()
        {
            string id = Request.Form["id"];
            int pid = int.Parse(id);

            ProjectManager projectManager = new ProjectManager();
            Project project = projectManager.Find(pid);
            string result = Request.Form["result"];
            bool res = result == "0" ? true : false;
            project.Result = res;
            project.Progress = ProjectProgress.提交备案;
            projectManager.Update(project);
            return Content("成功");
        }
    }
}