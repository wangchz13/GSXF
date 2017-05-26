using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSXF.Security;
using GSXF.DataBase;
using GSXF.Model;

namespace GSXF.Web.Controllers
{
    [UserAuthorize]
    public class UserController : Controller
    {
        private static UserRoleManager userRoleManager = new UserRoleManager();
        private static RoleManager roleManager = new RoleManager();



        // GET: User
        public ActionResult Index()
        {
            var _user = Session["User"] as User;
            var _roleID = userRoleManager.Find(ur => ur.UserID == _user.ID).RoleID;
            var _roleName = roleManager.Find(_roleID).Name;
            if (_roleName == "Root")
                return View("ROOT");
            else if (_roleName == "消防机构总队")
                return View("ZongDui");
            else if (_roleName == "消防机构支队")
                return View("ZhiDui");
            else if (_roleName == "消防机构大队")
                return View("ZongDui");
            else if (_roleName == "服务机构")
                return View("Company");

            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string name, string password)
        {
            return View();
        }

        /// <summary>
        /// 超级管理员页面
        /// </summary>
        /// <returns></returns>
        private ActionResult ROOT()
        {
            return View();
        }

        private ActionResult ZongDui()
        {
            return View();
        }

        private ActionResult ZhiDui()
        {
            return View();
        }

        private ActionResult DaDui()
        {
            return View();
        }

        private ActionResult Company()
        {
            return View();
        }
    }
}