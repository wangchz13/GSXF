using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSXF.DataBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using GSXF.Auxiliary;
using GSXF.Model;

namespace GSXF.Web.Controllers
{
    public class DataController : Controller
    {
        private static ProjectManager projectManager = new ProjectManager();
        private static CompanyManager companyManager = new CompanyManager();
        private static UserCompanyManager userCompanyManager = new UserCompanyManager();
        private static EmployeeManager employeeManager = new EmployeeManager();
        // GET: Data
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult GenderList()
        {
            var data = EnumToObject(typeof(Gender));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CityList()
        {
            var data = EnumToObject(typeof(City));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EmployeeLevelList()
        {
            var data = EnumToObject(typeof(EmployeeLevel));
            string str = JsonConvert.SerializeObject(data);
            JArray obj = JArray.Parse(str);
            foreach(var i in obj)
            {
                if(i["Text"].ToString() == "初级")
                {
                    i["Text"] = "建(构)筑物消防员(初级)";
                }
                else if(i["Text"].ToString() == "中级")
                {
                    i["Text"] = "建(构)筑物消防员(中级)";
                }
                else if (i["Text"].ToString() == "高级")
                {
                    i["Text"] = "建(构)筑物消防员(高级)";
                }
            }
            return Content(JsonConvert.SerializeObject(obj));
        }

        public ActionResult StatisticsData()
        {
            int[][] data = new int[4][];
            data[0] = projectManager.GetProjectCount(ProjectType.竣工检测);
            data[1] = projectManager.GetProjectCount(ProjectType.年度检测);
            data[2] = projectManager.GetProjectCount(ProjectType.维护保养);
            data[3] = projectManager.GetProjectCount(ProjectType.安全评估);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Projects(int pageSize=0, int pageIndex=0)
        {
            Paging<Project> projectPage = new Paging<Project>();
            if(pageSize != 0 && pageIndex != 0)
            {
                projectPage.PageSize = pageSize;
                projectPage.PageIndex = pageIndex;
            }
            var data = projectManager.FindPageList(projectPage).Items;
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public string Companies(int pageSize = 0, int pageIndex = 0)
        {
            Paging<Company> companyPage = new Paging<Company>();
            if(pageSize != 0 && pageIndex != 0)
            {
                companyPage.PageSize = pageSize;
                companyPage.PageIndex = pageIndex;
            }
            var res = companyManager.FindPageList(companyPage).Items;
            return JsonConvert.SerializeObject(res);
        }

        public string AllProjects()
        {
            var res = projectManager.FindList().ToList();
            return JsonConvert.SerializeObject(res);
        }


        public string MyProjects()
        {
            User user = Session["User"] as User;
            int companyID = userCompanyManager.Find(ur => ur.UserID == user.ID).CompanyID;
            var res = projectManager.FindList(p => p.Company.ID == companyID).ToList();
            return JsonConvert.SerializeObject(res);
        }

        public string MyEmployees()
        {
            User user = Session["User"] as User;
            int companyID = userCompanyManager.Find(ur => ur.UserID == user.ID).CompanyID;
            Company company = companyManager.Find(companyID);
            var res = company.Employees.ToList();
            return JsonConvert.SerializeObject(res);
        }

        public string AllEmployees()
        {
            var res = employeeManager.FindList().ToList();
            return JsonConvert.SerializeObject(res);
        }











        /// <summary>
        /// 枚举转对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object EnumToObject(Type type)
        {
            var data = Enum.GetNames(type).Select(text => new
            {
                Id = (int)Enum.Parse(type, text),
                Text = text
            }).ToArray();
            return data;
        }
    }
}