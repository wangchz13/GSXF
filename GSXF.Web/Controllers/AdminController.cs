using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSXF.Web.Models;
using GSXF.DataBase;
using GSXF.Model;

namespace GSXF.Web.Controllers
{
    public class AdminController : Controller
    {

        private static UserManager userManager = new UserManager();
        private static UserRoleManager userRoleManager = new UserRoleManager();
        private static RoleManager roleManager = new RoleManager();

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string name = form["Name"];
            string password = form["Password"];
            int institutionType = int.Parse(form["InstitutionType"]);
            if (userManager.Verify(name, password).Code == 3)
            {
                var user = userManager.Find(name); 
                user.LoginTime = DateTime.Now;
                user.LoginIP = Request.UserHostAddress;
                userManager.Update(user);
                Session.Add("User", user);

                var _roleID = userRoleManager.Find(u => u.UserID == user.ID).RoleID;
                var _roleName = roleManager.Find(_roleID).Name;
                if(_roleName == "消防机构总队")
                {
                    return RedirectToAction("Index_XFJG", "Admin");
                }else if(_roleName == "服务机构")
                {
                    return RedirectToAction("Index_FWJG", "Admin");
                }
            }

            return Content("用户名或密码不存在！");
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }
        /// <summary>
        /// 执业申请
        /// </summary>
        /// <returns></returns>
        public ActionResult ZYSQ()
        {
            return View();
        }
        /// <summary>
        /// 项目登记
        /// </summary>
        /// <returns></returns>
        public ActionResult XMDJ()
        {
            return View();
        }
        /// <summary>
        /// 图表展示
        /// </summary>
        /// <returns></returns>
        public ActionResult TBZS()
        {
            return View();
        }
        /// <summary>
        /// 评价页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PJYM()
        {
            return View();
        }

        public ActionResult ZCGCS()
        {
            return View();
        }
        //!---------------------------------------------------------------------------------
        /// <summary>
        /// 消防机构首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index_XFJG()
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
        /// 报告备案
        /// </summary>
        /// <returns></returns>
        public ActionResult BGBA()
        {
            return View();
        }
        /// <summary>
        /// 项目查询
        /// </summary>
        /// <returns></returns>
        public ActionResult XMCX()
        {
            return View();
        }
        /// <summary>
        /// 省内机构
        /// </summary>
        /// <returns></returns>
        public ActionResult SNJG()
        {
            return View();
        }
        /// <summary>
        /// 省外机构
        /// </summary>
        /// <returns></returns>
        public ActionResult SWJG()
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
        /// 服务质量
        /// </summary>
        /// <returns></returns>
        public ActionResult FWZL()
        {
            return View();
        }
        public ActionResult FWZL2()
        {
            return View();
        }
        public ActionResult FWZL3()
        {
            return View();
        }
        public ActionResult FWZL4()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index_FWJG()
        {
            return View();
        }

        public ActionResult DJXM()
        {
            return View();
        }

        public ActionResult CXXM()
        {
            return View();
        }
        /// <summary>
        /// 人员备案
        /// </summary>
        /// <returns></returns>
        public ActionResult RYBA()
        {
            return View();
        }
    }
}