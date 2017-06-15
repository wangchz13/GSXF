using System;
using System.Web.Mvc;
using GSXF.DataBase;
using GSXF.Model;
using GSXF.Web.Models;
using GSXF.Auxiliary;
using GSXF.Security;

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

    }
}