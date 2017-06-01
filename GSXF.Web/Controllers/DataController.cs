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
        private static FireControlInstitutionManager fireManager = new FireControlInstitutionManager();
        private static UserManager userManager = new UserManager();


        #region 下拉菜单
        /// <summary>
        /// 性别列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GenderList()
        {
            var data = EnumToObject(typeof(Gender));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 地区列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CityList()
        {
            var data = EnumToObject(typeof(City));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 员工等级列表
        /// </summary>
        /// <returns></returns>
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
        #endregion


        /// <summary>
        /// 统计数据
        /// </summary>
        /// <returns></returns>
        public ActionResult StatisticsData()
        {
            int[][] data = new int[4][];
            data[0] = projectManager.GetProjectCount(ProjectType.竣工检测);
            data[1] = projectManager.GetProjectCount(ProjectType.年度检测);
            data[2] = projectManager.GetProjectCount(ProjectType.维护保养);
            data[3] = projectManager.GetProjectCount(ProjectType.安全评估);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 项目信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public ActionResult Projects(int page=0, int rows=0)
        {
            List<Project> projects = new List<Project>();
            if(page==0 && rows == 0)
            {
                projects = projectManager.FindList().ToList();
            }
            else
            {
                Paging<Project> projectPage = new Paging<Project>();
                projectPage.PageIndex = page;
                projectPage.PageSize = rows;
                projects = projectManager.FindPageList(projectPage).Items;
            }
            
            return Json(projects, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 服务机构信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Companies(int page = 0, int rows = 0)
        {
            List<Company> companies = new List<Company>();
            if (page == 0 && rows == 0)
            {
                companies = companyManager.FindList().ToList();
            }
            else
            {
                Paging<Company> companyPage = new Paging<Company>();
                companyPage.PageIndex = page;
                companyPage.PageSize = rows;
                companies = companyManager.FindPageList(companyPage).Items;
            }
            return Json(companies, JsonRequestBehavior.AllowGet);
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

        public ActionResult Users(int page = 0, int rows = 0)
        {
            List<User> users = new List<User>();
            if(page == 0 && rows == 0)
            {
                users = userManager.FindList().ToList();
            }
            else
            {
                Paging<User> userPage = new Paging<User>();
                userPage.PageIndex = page;
                userPage.PageSize = rows;
                users = userManager.FindPageList(userPage).Items; 
            }
            var data = users.Select(u => new { Name = u.Name, LoginTime = u.LoginTime, LoginIP = u.LoginIP, IsOnline = u.IsOnline });
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        public ActionResult test()
        {
            var res = fireManager.FindList();
            return Json(res, JsonRequestBehavior.AllowGet);
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