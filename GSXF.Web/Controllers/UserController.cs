using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using GSXF.Security;
using GSXF.DataBase;
using GSXF.Model;
using GSXF.Web.Models;
using GSXF.Auxiliary;

namespace GSXF.Web.Controllers
{
    [UserAuthorize]
    public class UserController : Controller
    {
        private static UserRoleManager userRoleManager = new UserRoleManager();
        private static RoleManager roleManager = new RoleManager();
        private static UserManager userManager = new UserManager();

        // GET: User
        public ActionResult Index()
        {
            var user = Session["User"] as User;
            var roleID = userRoleManager.Find(ur => ur.UserID == user.ID).RoleID;
            var roleName = roleManager.Find(roleID).Name;
            if (roleName == "Root")
                return View("ROOT");
            else if (roleName == "消防机构总队")
                return View("ZongDui");
            else if (roleName == "消防机构支队")
                return View("ZhiDui");
            else if (roleName == "消防机构大队")
                return View("ZongDui");
            else if (roleName == "服务机构")
                return View("Company");

            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            Response resp = userManager.Verify(login.Name, login.Password);
            if (resp.Code == 3)
            {
                var user = userManager.Find(login.Name);
                user.LoginTime = DateTime.Now;
                user.LoginIP = Request.UserHostAddress;
                user.IsOnline = true;
                userManager.Update(user);
                Session.Add("User", user);
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index");
            }
            return Json(resp);
        }

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
        /// <summary>
        /// 超级管理员页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ROOT()
        {
            return View();
        }

        public ActionResult ZongDui()
        {
            return View();
        }

        public ActionResult ZhiDui()
        {
            return View();
        }

        public ActionResult DaDui()
        {
            return View();
        }

        public ActionResult Company()
        {
            return View();
        }
    }
}