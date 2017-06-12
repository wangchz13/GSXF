﻿using System;
using System.Web.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
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
        private static ArticleManager articleManager = new ArticleManager();
        private static EmployeeManager employeeManager = new EmployeeManager();
        private static OfficeAddressManager officeAddressManager = new OfficeAddressManager();
        private static CompanyManager companyManager = new CompanyManager();
        private static UserManager userManager = new UserManager();
        private static ProjectManager projectManager = new ProjectManager();
        private static UserCompanyManager userCompanyManager = new UserCompanyManager();
        private static UserRoleManager userRoleManager = new UserRoleManager();
        private static RoleManager roleManager = new RoleManager();
        private static FireControlInstitutionManager fireManager = new FireControlInstitutionManager();

        private static List<Employee> employees = new List<Employee>();
        private static List<OfficeAddress> offices = new List<OfficeAddress>();

        // GET: User
        public ActionResult Index()
        {
            var user = Session["User"] as User;
            //if (user == null)
            //    return RedirectToAction("Login");

            //var roleID = userRoleManager.FindList(ur => ur.UserID == user.ID).ToList()[0].RoleID;
            //var roleName = roleManager.Find(roleID).Name;

            //if (roleName == "Root")
            //{
            //    ViewBag.Title = "超级管理员";
            //}

            //var code = user.Name.Substring(0, 8);
            //ViewBag.Title = fireManager.Find(f => f.Code == code).Name;

            ViewBag.User = user;
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
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }

            string password = Encryption.SHA256(login.Password);
            resp = userManager.Verify(login.Name, password);
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

        public ActionResult Menu()
        {
            User user = Session["User"] as User;
            var roleID = userRoleManager.Find(ur => ur.UserID == user.ID).RoleID;
            var roleName = roleManager.Find(roleID).Name;
            ViewBag.Role = roleName;
            return PartialView();
        }

        public ActionResult MyInfo()
        {
            User user = Session["User"] as User;
            ViewBag.Name = user.Name;
            ViewBag.Regtime = user.RegTime;
            var roleID = userRoleManager.Find(ur => ur.UserID == user.ID).RoleID;
            var roleName = roleManager.Find(roleID).Name;
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

        /// <summary>
        /// 添加人员信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public ActionResult AddEmployee(List<Employee> list)
        {
            employees = new List<Employee>();
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }
            foreach(var i in list)
            {
                var employee = employeeManager.Find(e => e.IdentificationNumber == i.IdentificationNumber);
                if (employee != null)
                {
                    if (employee.Company != null)
                    {
                        resp.Code = -2;
                        resp.Message = "身份证号为【" + employee.IdentificationNumber + "】的人员已在【"+employee.Company.Name+"】公司注册，备案失败！";
                        return Json(resp);
                    }
                }
            }

            foreach (var i in list)
            {
                var employee = employeeManager.Find(e => e.IdentificationNumber == i.IdentificationNumber);
                if(employee != null)
                {
                    employees.Add(employee);
                }
                else
                {
                    employeeManager.Add(i);
                    employees.Add(i);
                }
            }
            return Json(resp);
        }
        public ActionResult AddZCGCS(Employee e)
        {
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }
            resp = employeeManager.Add(e);
            return Json(resp);
        }
        /// <summary>
        /// 添加办公地址信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOfficeAddress(List<OfficeAddress> list)
        {
            offices = new List<OfficeAddress>();
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }
            foreach (var i in list)
            {
                officeAddressManager.Add(i);
                offices.Add(i);
            }
            return Json(resp);
        }

        /// <summary>
        /// 添加服务机构
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCompany(Company company)
        {
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }
            if(employees.Count != 0)
            {
                company.Employees = employees; 
            }

            if(offices.Count != 0)
            {
                company.OfficeAddresses = offices;   
            }
            resp = companyManager.Add(company);
            resp.Data = CreateCompanyAccount(company.ID);
            return Json(resp);
        }

        [HttpPost]
        public ActionResult AddProject(Project project,string jg)
        {
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }
            User user = getCurrentUser();
            int companyID = userCompanyManager.Find(u => u.UserID == user.ID).CompanyID;
            project.Company = companyManager.Find(companyID);
            project.CompanyName = project.Company.Name;

            int fireID = int.Parse(jg);
            project.FireControlInstitution = fireManager.Find(fireID);
            projectManager.Add(project);
            return Content("添加成功");
        }

        public object CreateCompanyAccount(int companyID)
        {
            string name = string.Empty;
            char[] ver = new Char[4];
            Random random = new Random();
            char[] dict = {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (int i = 0; i < 4; i++)
            {
                ver[i] = dict[random.Next(dict.Length - 1)];
            }
            name = new string(ver);
            name = "GS-" + name;
            string password = Verification.Text(6);
            userManager.Add(name, Encryption.SHA256(password));
            User user = userManager.Find(name);
            userRoleManager.Add(user.ID, 5);

            UserCompany uc = new UserCompany();
            uc.UserID = user.ID;
            uc.CompanyID = companyID;
            userCompanyManager.Add(uc);

            return new { Name = name, Password = password };
        }



        public ActionResult CreateAcount(string name, int roleID)
        {
            string password = Verification.Text(6);
            userManager.Add(name, Encryption.SHA256(password));
            User user = userManager.Find(name);
            userRoleManager.Add(user.ID, roleID);
            return Json(new { Name = name, Password = password });
        }


        public User getCurrentUser()
        {
            return Session["User"] as User;
        }

        public ActionResult ResetPassword(string name,string password)
        {
            Response resp = new Response();
            User user = userManager.Find(name);
            user.Password = Encryption.SHA256(password);
            resp = userManager.Update(user);
            return Json(resp);
        }

    }
}