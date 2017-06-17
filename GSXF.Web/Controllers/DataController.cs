using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GSXF.DataBase;
using Newtonsoft.Json;
using GSXF.Auxiliary;
using GSXF.Model;
using GSXF.Security;

namespace GSXF.Web.Controllers
{

    [UserAuthorize]
    public class DataController : Controller
    {
        private static ProjectManager projectManager = new ProjectManager();
        private static CompanyManager companyManager = new CompanyManager();
        private static UserCompanyManager userCompanyManager = new UserCompanyManager();
        private static EmployeeManager employeeManager = new EmployeeManager();
        private static FireControlInstitutionManager fireManager = new FireControlInstitutionManager();
        private static UserManager userManager = new UserManager();
        private static RoleManager roleManager = new RoleManager();
        private static OfficeAddressManager officeAddressManager = new OfficeAddressManager();


        private static List<Employee> employees = new List<Employee>();
        private static List<OfficeAddress> offices = new List<OfficeAddress>();

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

            if (type == typeof(EmployeeLevel))
            {
                data = Enum.GetNames(type).Select(text => new
                {
                    Id = (int)Enum.Parse(type, text),
                    Text = text == "初级" ? "建(构)筑物消防员(初级)" : (text == "中级" ? "建(构)筑物消防员(中级)" : (text == "高级"?"建(构)筑物消防员(高级)":text))
                }).ToArray();
            }
            return data;
        }

        /// <summary>
        /// 性别列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getGenderList()
        {
            var data = EnumToObject(typeof(Gender));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 地区列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getCityList()
        {
            var data = EnumToObject(typeof(City));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 员工证书等级列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getEmployeeLevelList()
        {
            var data = EnumToObject(typeof(EmployeeLevel));
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
        /// 获取所有项目（默认只会获取进度到提交备案的项目）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getProjects(int userID = 0)
        {
            //TODO: FindList函数中药加上p.progress == 提交备案


            IQueryable<Project> projects = projectManager.FindList();
            string type = Request.QueryString["Type"];
            string city = Request.QueryString["City"];
            string lTime = Request.QueryString["lTime"];
            string rTime = Request.QueryString["rTime"];
            string result = Request.QueryString["Result"];
            string name = Request.QueryString["Name"];
            string progress = Request.QueryString["Progress"];

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

            if(progress != null && progress!= ""){
                var p = int.Parse(progress);
                ProjectProgress pp = (ProjectProgress)p;
                projects = projects.Where(project => project.Progress == pp);
            }

            User user = Session["User"] as User;
            string roleName = string.Empty;
            if (user != null)
            {
                roleName = user.Role.Name;
            }

            if(roleName == "服务机构")
            {
                int companyID = userCompanyManager.Find(ur => ur.UserID == user.ID).CompanyID;
                projects = projects.Where(p => p.Company.ID == companyID);
            }

            var data = projects.Select(p => new
            {
                ID = p.ID,
                Name = p.Name,
                CompanyName = p.Company.Name,
                Type = p.Type.ToString(),
                City = p.City.ToString(),
                RecordDate = p.RecordDate.ToString(),
                Result = p.Result
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得指定项目
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getProject(int projectID = 0)
        {
            //TODO: slect掉循环引用的部分
            Project project = projectManager.Find(projectID);
            return Json(project, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取正在进行中的项目
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getWorkingProjects()
        {
            IQueryable<Project> projects = projectManager.FindList(p => p.Progress != ProjectProgress.提交备案);

            //选出处于某个进度的项目
            string progress = Request.QueryString["Progress"];
            if (progress != null && progress != "")
            {
                var p = int.Parse(progress);
                ProjectProgress pp = (ProjectProgress)p;
                projects = projects.Where(project => project.Progress == pp);
            }


            User user = Session["User"] as User;
            string roleName = user.Role.Name;

            if(roleName == "服务机构")
            {
                int companyID = userCompanyManager.Find(ur => ur.UserID == user.ID).CompanyID;
                projects = projects.Where(p => p.Company.ID == companyID);
            }










            var data = projects.Select(p => new
            {
                ID = p.ID,
                Name = p.Name,
                CompanyName = p.Company.Name,
                FireName = p.FireControlInstitution.Name,
                Progress = p.Progress.ToString(),

            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="project"></param>
        /// <param name="jg">所属消防机构</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult addProject(Project project, string jg)
        {
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }
            User user = Session["User"] as User;
            int companyID = userCompanyManager.Find(u => u.UserID == user.ID).CompanyID;
            project.Company = companyManager.Find(companyID);

            int fireID = int.Parse(jg);
            project.FireControlInstitution = fireManager.Find(fireID);
            project.RecordDate = DateTime.Now;
            project.Progress = ProjectProgress.入场检测;
            resp = projectManager.Add(project);
            resp.Data = null;
            return Json(resp);
        }



        /// <summary>
        /// 获得所有服务机构信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult getCompanies()
        {
            var companies = companyManager.FindList();
            string qt = Request.QueryString["QualificationType"];
            string ql = Request.QueryString["QualificationLevel"];
            string name = Request.QueryString["Name"];
            if(qt!=null && qt != "-1")
            {
                QualificationType t = (QualificationType)int.Parse(qt);
                companies = companies.Where(c => c.Type1 == t || c.Type2 == t);
            }

            if(ql!=null && ql != "-1")
            {
                QualificationLevel l = (QualificationLevel)int.Parse(ql);
                companies = companies.Where(c => c.Level1 == l || c.Level2 == l);
            }

            if(name != null && name != "")
            {
                companies = companies.Where(c => c.Name.Contains(name));
            }
            return Content(JsonConvert.SerializeObject(companies));
        }

        /// <summary>
        /// 获得指定服务机构
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        ///
        [HttpGet]
        public ActionResult getCompany(int companyID = 0)
        {
            Company company = companyManager.Find(companyID);
            return Content(JsonConvert.SerializeObject(company));
        }

        /// <summary>
        /// 添加服务机构
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult addCompany(Company company)
        {
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }
            if (employees.Count != 0)
            {
                company.Employees = employees;
            }

            if (offices.Count != 0)
            {
                company.OfficeAddresses = offices;
            }


            resp = companyManager.Add(company);
            resp.Data = CreateCompanyUser(company.ID);
            return Json(resp);
        }

        /// <summary>
        /// 获得所有人员信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult getEmployees(int companyID=0)
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
            string roleName = user.Role.Name;
            if(roleName == "消防机构总队")
            {
                employees = employees.Where(e => e.Level == EmployeeLevel.一级注册消防工程师 || e.Level == EmployeeLevel.二级注册消防工程师 || e.Level==EmployeeLevel.临时注册消防工程师);
            }

            var data = employees.Select(e => new
            {
                ID = e.ID,
                Name = e.Name,
                Gender = e.Gender.ToString(),
                Level = e.Level.ToString(),
                CertificateNumber = e.CertificateNumber,
                IdentificationNumber = e.IdentificationNumber,
                CompanyName = e.Company == null?"无":e.Company.Name
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获得指定人员信息
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult getEmployee(int employeeID=0)
        {
            Employee employee = employeeManager.Find(employeeID);
            if(employee != null)
            {
                var data = new
                {
                    ID = employee.ID,
                    Name = employee.Name,
                    Gender = employee.Gender.ToString(),
                    Level = employee.Level.ToString(),
                    CertificateNumber = employee.CertificateNumber,
                    IdentificationNumber = employee.IdentificationNumber,
                    MobilePhone = employee.MobilePhone,
                    CompanyName = employee.Company==null?"无":employee.Company.Name
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json("找不到此人员",JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult setEmployee(Employee e, string companyName)
        {
            Employee employee = employeeManager.Find(e.ID);
            
            employee.Name = e.Name;
            employee.Gender = e.Gender;
            employee.Level = e.Level;
            employee.CertificateNumber = e.CertificateNumber;
            employee.IdentificationNumber = e.IdentificationNumber;
            employee.MobilePhone = e.MobilePhone;
            if(companyName == "0")
            {
                employee.Company = null;
            }

            var data = employeeManager.Update(employee);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加人员信息（单个）
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult addEmployee(Employee employee)
        {
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }

            Employee e = employeeManager.Find(i => i.IdentificationNumber == employee.IdentificationNumber);
            if (e != null)
            {
                resp.Code = -2;
                resp.Message = "检测到身份证号为【" + employee.IdentificationNumber + "】的人员已备案，备案失败！";
                return Json(resp);
            }
            resp = employeeManager.Add(employee);
            return Json("123");

        }

        /// <summary>
        /// 添加人员信息（多个）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult addEmployees(List<Employee> list)
        {
            employees = new List<Employee>();
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }

            foreach (var i in list)
            {
                var employee = employeeManager.Find(e => e.IdentificationNumber == i.IdentificationNumber);
                if (employee != null)
                {
                    if (employee.Company != null)
                    {
                        resp.Code = -2;
                        resp.Message = "身份证号为【" + employee.IdentificationNumber + "】的人员已在【" + employee.Company.Name + "】公司注册，备案失败！";
                        return Json(resp);
                    }
                }
            }

            foreach (var i in list)
            {
                var employee = employeeManager.Find(e => e.IdentificationNumber == i.IdentificationNumber);
                if (employee != null)
                {
                    employees.Add(employee);
                }
                else
                {
                    resp = employeeManager.Add(i);
                    employees.Add(i);
                }
            }
            return Json(resp);
        }

        [HttpPost]
        public ActionResult deleteEmployee(int employeeID)
        {
            var data = employeeManager.Delete(employeeID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult getUsers()
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

        public ActionResult getUser(int userID = 0)
        {
            User user = userManager.Find(userID);
            return Json(user, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult addOfficeAddress(List<OfficeAddress> list)
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
                resp = officeAddressManager.Add(i);
                offices.Add(i);
            }
            return Json(resp);
        }





        public ActionResult test()
        {
            var res = fireManager.FindList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }


        private object CreateCompanyUser(int companyID)
        {
            string name = string.Empty;
            char[] ver = new Char[4];
            Random random = new Random();
            char[] dict = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            for (int i = 0; i < 4; i++)
            {
                ver[i] = dict[random.Next(dict.Length - 1)];
            }
            name = new string(ver);
            name = "GS-" + name;
            if(userManager.Find(name) != null)
            {
                name += "0";
            }
            string password = Verification.Text(6);
            userManager.Add(name, password);
            User user = userManager.Find(name);
            user.Role = roleManager.Find(5);

            UserCompany uc = new UserCompany();
            uc.UserID = user.ID;
            uc.CompanyID = companyID;
            userCompanyManager.Add(uc);

            return new { Name = name, Password = password };
        }








    }
}