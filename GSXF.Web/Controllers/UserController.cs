using System;
using System.Web.Mvc;
using System.Linq;
using Newtonsoft.Json;
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

        private static int employeeRow = 0;
        private static int officeAddressRow = 0;
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

        public ActionResult JGBA()
        {

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
        // 首页
        public ActionResult Index2()
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
        #endregion

        /// <summary>
        /// 添加人员信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public ActionResult AddEmployee(List<Employee> list)
        {
            foreach (var i in list)
            {
                employeeManager.Add(i);
            }
            employeeRow = list.Count;
            return Json("");
        }
        /// <summary>
        /// 添加办公地址信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOfficeAddress(List<OfficeAddress> list)
        {
            foreach (var i in list)
            {
                officeAddressManager.Add(i);
            }
            officeAddressRow = list.Count;
            return Json(1);
        }

        [HttpPost]
        public ActionResult AddCompany(Company company)
        {
            if (employeeRow != 0)
            {
                var employeeList = employeeManager.FindList().OrderByDescending(m => m.ID).Take(employeeRow);
                company.Employees = employeeList.ToList();
                employeeRow = 0;
            }
            if (officeAddressRow != 0)
            {
                var addressList = officeAddressManager.FindList().OrderByDescending(m => m.ID).Take(officeAddressRow);
                company.OfficeAddresses = addressList.ToList();
                officeAddressRow = 0;

            }
            companyManager.Add(company);

            return CreateCompanyAccount(company.ID);
        }

        [HttpPost]
        public ActionResult AddProject(Project project)
        {
            User user = getCurrentUser();
            int companyID = userCompanyManager.Find(u => u.UserID == user.ID).CompanyID;
            project.Company = companyManager.Find(companyID);
            project.CompanyName = project.Company.Name;
            projectManager.Add(project);
            return Json("添加项目成功");
        }

        public ActionResult CreateCompanyAccount(int companyID)
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

            string password = Verification.Text(6);
            userManager.Add(name, Encryption.SHA256(password));
            User user = userManager.Find(name);
            userRoleManager.Add(user.ID, 5);

            UserCompany uc = new UserCompany();
            uc.UserID = user.ID;
            uc.CompanyID = companyID;
            userCompanyManager.Add(uc);

            return Json(new { Name = name, Password = password });
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

    }
}