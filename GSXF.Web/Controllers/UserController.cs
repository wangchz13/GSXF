using System;
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
                return View("Index1");
            else if (roleName == "消防机构总队")
                return View("Index2");
            else if (roleName == "消防机构支队")
                return View("Index3");
            else if (roleName == "消防机构大队")
                return View("Index4");
            else if (roleName == "服务机构")
                return View("Index5");
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var user = Session["User"] as User;
            if (user != null)
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            Response resp = userManager.Verify(login.Name, login.Password);
            if (resp.Code == 3)
            {
                var user = userManager.Find(login.Name);
                user.LoginTime = DateTime.Now;
                user.LoginIP = Request.UserHostAddress;
                user.IsOnline = true;
                userManager.Update(user);
                Session.Add("User", user);
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

        #region 角色首页
        /// <summary>
        /// 超级管理员页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index1()
        {
            return View();
        }
        /// <summary>
        /// 消防机构总队首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index2()
        {
            return View();
        }
        /// <summary>
        /// 消防机构支队首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index3()
        {
            return View();
        }
        /// <summary>
        /// 消防机构大队首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index4()
        {
            return View();
        }
        /// <summary>
        /// 服务机构首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index5()
        {
            return View();
        }

        #endregion

        #region 后台所有Action
        public ActionResult MyInfo()
        {
            return View();
        }
        #endregion
    }
}