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
        private static RoleManager roleManager = new RoleManager();
        private static UserRoleManager userRoleManager = new UserRoleManager();

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
        /// 员工证书等级列表
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
        /// 查询项目
        /// </summary>
        /// <returns></returns>
        public ActionResult Projects()
        {
            IQueryable<Project> projects = projectManager.FindList();
            string type = Request.QueryString["Type"];
            string city = Request.QueryString["City"];
            string lTime = Request.QueryString["lTime"];
            string rTime = Request.QueryString["rTime"];
            string result = Request.QueryString["Result"];
            string name = Request.QueryString["Name"];

            //按项目类型查询
            if(type!= null && type!= "-1")
            {
                ProjectType pType = (ProjectType)int.Parse(type);
                projects = projects.Where(p => p.Type == pType);
            }

            //按所属地区查询
            if(city!=null && city != "-1")
            {
                City c = (City)int.Parse(city);
                projects = projects.Where(p => p.City ==c);
            }

            //按报告备案时间查询
            if(lTime != null && lTime != "")
            {
                DateTime begin = Convert.ToDateTime(lTime);
                projects = projects.Where(p => p.RecordDate >= begin);
            }
            if(rTime !=null && rTime != "")
            {
                DateTime end = Convert.ToDateTime(rTime);
                projects = projects.Where(p => p.RecordDate <= end);
            }

            //按检测结果查询
            if(result!=null && result != "-1")
            {
                bool r = result == "0" ? true : false;
                projects = projects.Where(p => p.Result == r);
            }

            //按项目名称查询
            if(name !=null && name != "")
            {
                projects = projects.Where(p => p.Name.Contains(name));
            }
            User user = Session["User"] as User;
            string roleName = getUserRoleName(user);
            if(roleName == "服务机构")
            {
                int companyID = userCompanyManager.Find(ur => ur.UserID == user.ID).CompanyID;
                projects = projects.Where(p => p.Company.ID == companyID);
            }



            return Content(JsonConvert.SerializeObject(projects));
        }

        public string WorkingProjects()
        {
            IQueryable<Project> projects = projectManager.FindList(p => p.Progress != ProjectProgress.提交备案);
            User user = Session["User"] as User;
            string roleName = getUserRoleName(user);
            if(roleName == "服务机构")
            {
                int companyID = userCompanyManager.Find(ur => ur.UserID == user.ID).CompanyID;
                projects = projects.Where(p => p.Company.ID == companyID);
            }
            return JsonConvert.SerializeObject(projects);
        }

        /// <summary>
        /// 服务机构信息
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult Companies()
        {
            var companies = companyManager.FindList();
            string qt = Request.QueryString["QualificationType"];
            string ql = Request.QueryString["QualificationLevel"];
            string name = Request.QueryString["Name"];
            if(qt!=null && qt != "-1")
            {
                QualificationType t = (QualificationType)int.Parse(qt);
                companies = companies.Where(c => c.QualificationType == t);
            }

            if(ql!=null && ql != "-1")
            {
                QualificationLevel l = (QualificationLevel)int.Parse(ql);
                companies = companies.Where(c => c.QualificationLevel == l);
            }

            if(name != null && name != "")
            {
                companies = companies.Where(c => c.Name.Contains(name));
            }
            return Content(JsonConvert.SerializeObject(companies));
        }



        public string MyProjects()
        {
            User user = Session["User"] as User;
            int companyID = userCompanyManager.Find(ur => ur.UserID == user.ID).CompanyID;
            var res = projectManager.FindList(p => p.Company.ID == companyID).ToList();
            return JsonConvert.SerializeObject(res);
        }

        public string Employees()
        {
            var employees = employeeManager.FindList();
            string name = Request.QueryString["Name"];
            string level = Request.QueryString["Level"];
            string cn = Request.QueryString["CertificateNumber"];
            string idn = Request.QueryString["IdentificationNumber"];
            if(name!=null && name != "")
            {
                employees = employees.Where(e => e.Name.Contains(name));
            }
            if(level != null && level!= "-1")
            {
                EmployeeLevel el = (EmployeeLevel)int.Parse(level);
                employees = employees.Where(e => e.Level == el);
            }
            if(cn != null && cn != "")
            {
                employees = employees.Where(e => e.CertificateNumber == cn);
            }
            if(idn != null && idn != "")
            {
                employees = employees.Where(e => e.IdentificationNumber == idn);
            }

            User user = Session["User"] as User;
            if (user != null)
            {
                string roleName = getUserRoleName(user);
                if(roleName == "消防机构总队")
                {
                    employees = employees.Where(e => e.Level == EmployeeLevel.一级注册消防工程师 || e.Level == EmployeeLevel.二级注册消防工程师 || e.Level==EmployeeLevel.临时注册消防工程师);
                }

            }

            return JsonConvert.SerializeObject(employees);
        }

        public string MyEmployees()
        {
            User user = Session["User"] as User;
            int companyID = userCompanyManager.Find(ur => ur.UserID == user.ID).CompanyID;
            Company company = companyManager.Find(companyID);
            var res = company.Employees.ToList();
            return JsonConvert.SerializeObject(res);
        }

        public ActionResult Users()
        {
            var users = userManager.FindList();
            string name = Request.QueryString["Name"];
            if (name != null)
            {
                users = users.Where(c => c.Name.Contains(name));
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

        public string getUserRoleName(User user)
        {
            if (user == null)
                return string.Empty;
            int roleID = userRoleManager.Find(ur => ur.UserID == user.ID).RoleID;
            return roleManager.Find(r => r.ID == roleID).Name;
        }
    }
}