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
        private static ArticleManager articleManager = new ArticleManager();
        private static EmployeeManager employeeManager = new EmployeeManager();
        private static OfficeAddressManager officeAddressManager = new OfficeAddressManager();
        private static CompanyManager companyManager = new CompanyManager();

        private static ProjectManager projectManager = new ProjectManager();
        private static UserCompanyManager userCompanyManager = new UserCompanyManager();

        private static int employeeRow = 0;
        private static int officeAddressRow = 0;
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
            return Json(new { rows = list.Count });
        }
        /// <summary>
        /// 添加办公地址信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOfficeAddress(List<OfficeAddress> list)
        {
            foreach(var i in list)
            {
                officeAddressManager.Add(i);
            }
            officeAddressRow = list.Count;
            return Json(new { rows = list.Count });
        }

        [HttpPost]
        public ActionResult AddCompany(Company company)
        {
            if(employeeRow != 0)
            {
                var employeeList = employeeManager.FindList().OrderByDescending(m => m.ID).Take(employeeRow);
                company.Employees = employeeList.ToList();
                employeeRow = 0;
            }
            if(officeAddressRow != 0)
            {
                var addressList = officeAddressManager.FindList().OrderByDescending(m => m.ID).Take(officeAddressRow);
                company.OfficeAddresses = addressList.ToList();
                officeAddressRow = 0;

            }
            companyManager.Add(company);

            return Json("success");
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

        public User getCurrentUser()
        {
            return Session["User"] as User;
        }
    }
}