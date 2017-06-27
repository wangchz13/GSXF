using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.IO;
using System.Web;
using GSXF.DataBase;
using Newtonsoft.Json;
using GSXF.Auxiliary;
using GSXF.Model;
using GSXF.Security;
using GSXF.Web.Models;

namespace GSXF.Web.Controllers
{

    [UserAuthorize]
    public class DataController : BaseController
    {
        
        private static List<Employee> employees = new List<Employee>();

        [AllowAnonymous]
        public static string getSiteConfig(string key)
        {

            SiteConfig siteConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").GetSection("SiteConfig") as GSXF.Model.SiteConfig;
            if(key == "EmailName")
            {
                return siteConfig.EmailName;
            }
            if(key == "EmailPassword")
            {
                return siteConfig.EmailPassword;
            }
            if(key == "SiteUrl")
            {
                return siteConfig.SiteUrl;
            }

            return "";
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login)
        {

            Response resp = new Response();

            var currentUser = Session["User"] as User;
            if (currentUser != null)
            {
                currentUser.IsOnline = false;
                userManager.Update(currentUser);
            }


            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }

            if (Session["VerificationCode"].ToString() != login.VerficationCode.ToUpper())
            {
                resp.Code = -2;
                resp.Message = "验证码错误";
                return Json(resp);
            }

            resp = userManager.Verify(login.Name, login.Password);

            if (resp.Code != 3)
            {
                return Json(resp);
            }


            var user = userManager.Find(login.Name);
            user.LoginTime = DateTime.Now;
            user.LoginIP = Request.UserHostAddress;
            user.IsOnline = true;
            userManager.Update(user);


            Session.Add("User", user);
            return Json(resp);
        }


        [AllowAnonymous]
        public ActionResult LoginOut()
        {
            User user = Session["User"] as User;
            if (user != null)
            {
                user = userManager.Find(user.ID);
                user.IsOnline = false;
                userManager.Update(user);
            }
            Session.Clear();
            return Redirect("/");
        }




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

        private City getCityByFireName(string name)
        {
            switch (name)
            {
                case "兰州市公安消防支队":
                    return City.兰州;
                case "嘉峪关市公安消防支队":
                    return City.嘉峪关;
                case "金昌市公安消防支队":
                    return City.金昌;
                case "白银市公安消防支队":
                    return City.白银;
                case "天水市公安消防支队":
                    return City.天水;
                case "酒泉市公安消防支队":
                    return City.酒泉;
                case "张掖市公安消防支队":
                    return City.张掖;
                case "武威市公安消防支队":
                    return City.武威;
                case "定西市公安消防支队":
                    return City.定西;
                case "陇南市公安消防支队":
                    return City.陇南;
                case "平凉市公安消防支队":
                    return City.平凉;
                case "庆阳市公安消防支队":
                    return City.庆阳;
                case "临夏回族自治州公安消防支队":
                    return City.临夏;
                case "甘南州公安消防支队":
                    return City.甘南;
                case "兰州新区公安消防支队":
                    return City.兰州新区;
                default:
                    return City.兰州;
            }
        }


        [HttpGet]
        public ActionResult StatisticsData2()
        {
            var user = Session["User"] as User;
            var roleName = user.Role.Name;
            string fireCode = user.Name.Substring(0, 8);
            var fire = fireManager.Find(f => f.Code == fireCode);
            var projects = projectManager.FindList();

            if(roleName == "消防机构支队")
            {

                City city = getCityByFireName(fire.Name);
                projects = projects.Where(p => p.City == city);
            }
            else if(roleName == "消防机构大队")
            {
                projects = projects.Where(p=>p.FireControlInstitution.ID == fire.ID);

            }

            int[] data = new int[4];
            data[0] = projects.Where(p => p.Type == ProjectType.竣工检测).Count();
            data[1] = projects.Where(p => p.Type == ProjectType.年度检测).Count();
            data[2] = projects.Where(p => p.Type == ProjectType.维护保养).Count();
            data[3] = projects.Where(p => p.Type == ProjectType.安全评估).Count();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult getArticles()
        {
            string category = Request.QueryString["category"];
            var articles = articleManager.FindList().OrderByDescending(a => a.ID).ToList();
            if (category!=null && category != "")
            {
                Category c = (Category)int.Parse(category);
                articles = articles.Where(a => a.Category == c).ToList();
            }
            
            var data = articles.Select(a => new {
                ID = a.ID,
                Title = a.Title,
                Category = a.Category.ToString(),
                Path = a.File.Path,
                CommitDate = a.CommitDate.ToString()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult deleteArticle(int id)
        {
            Response resp = new Response();

            resp = articleManager.Delete(id);
            return Json(resp);
        }






        /// <summary>
        /// 获取所有已完成备案的项目
        /// </summary>
        /// <param name="flag">flag=true,表示数据是首页获取，不按照当前登录账号选择项目</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult getProjects(bool flag=false)
        {

            var projects = projectManager.FindList();
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
            //按项目进度查询
            if (progress != null && progress != "")
            {
                var p = int.Parse(progress);
                ProjectProgress pp = (ProjectProgress)p;
                projects = projects.Where(project => project.Progress == pp);
            }

            if (!flag)
            {
                User user = Session["User"] as User;
                string roleName = user == null ? "" : user.Role.Name;

                string fireCode = user.Name.Substring(0, 8);
                var fire = fireManager.Find(f => f.Code == fireCode);

                if (roleName == "消防机构支队")
                {
                    City c = getCityByFireName(fire.Name);
                    projects = projects.Where(p => p.City == c);
                }


                if (roleName == "消防机构大队")
                {

                    projects = projects.Where(p => p.FireControlInstitution.ID == fire.ID);
                }


                if (roleName == "服务机构")
                {
                    int companyID = userCompanyManager.Find(ur => ur.UserID == user.ID).CompanyID;
                    projects = projects.Where(p => p.Company.ID == companyID);
                }
            }
            else
            {
                projects = projects.Where(p => p.Progress == ProjectProgress.提交备案);
            }
            


            projects = projects.OrderByDescending(p => p.ID);

            var list = projects.ToList();
            var data = list.Select(p => new
            {
                ID = p.ID,
                Name = p.Name,
                CompanyName = p.Company.Name,
                Type = p.Type.ToString(),
                City = p.City.ToString(),
                RecordDate = p.RecordDate.ToString(),
                Result = p.Result==true?"合格":"不合格"
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

            var data = new
            {
                ID = project.ID,
                Name = project.Name,
                Type = project.Type,
                City = project.City,
                jg = project.FireControlInstitution.ID,
                Address = project.Address,
                BuildType = project.BuildType,
                FireRisk = project.FireRisk,
                Area = project.Area,
                AllLayer = project.AllLayer,
                ServiceLayer = project.ServiceLayer,
                ContractNumber = project.ContractNumber,
                SignDate = project.SignDate.Year.ToString()+'-'+ project.SignDate.Month.ToString()+'-'+ project.SignDate.Day.ToString(),
                StartDate = project.StartDate.Year.ToString()+'-'+ project.StartDate.Month.ToString()+'-'+ project.StartDate.Day.ToString(),
                EndDate = project.EndDate.Year.ToString()+'-' + project.EndDate.Month.ToString()+'-' + project.EndDate.Day.ToString(),
                ProjectHead = project.ProjectHead,
                TechHead = project.TechHead,
                ProjectContact = project.ProjectContact,
                Note = project.Note,
                Object = project.Object,
                ObjectContact = project.ObjectContact,
                ObjectContactPhone = project.ObjectContactPhone,
                ObjectEmail = project.ObjectEmail,
                Progress = (int)project.Progress,
                HasData = project.DataFile!=null,
                HasReport = project.ReportFile!=null
            };

            return Json(data, JsonRequestBehavior.AllowGet);
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
            string roleName = user == null ? "" : user.Role.Name;


            //总队只能看到一个季度的项目
            if(roleName == "消防机构总队")
            {
                //DateTime leftDate = DateTime.Now.AddMonths(-3);
                //projects = projects.Where(p => p.RegTime >= leftDate);
            }
            string fireCode = user.Name.Substring(0, 8);
            var fire = fireManager.Find(f => f.Code == fireCode);
            //支队大队只能看到管辖单位为自己的项目
            if (roleName == "消防机构大队")
            {
                
                
                projects = projects.Where(p => p.FireControlInstitution.ID == fire.ID);
            }

            if(roleName == "消防机构支队")
            {
                City city = getCityByFireName(fire.Name);
                projects = projects.Where(p => p.City == city);
            }
            //服务机构只能看到属于自己的项目
            if(roleName == "服务机构")
            {
                int companyID = userCompanyManager.Find(ur => ur.UserID == user.ID).CompanyID;
                projects = projects.Where(p => p.Company.ID == companyID);
            }



            projects = projects.OrderByDescending(p => p.ID);

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
            project.RegTime = DateTime.Now;
            

            //搞查询明码
            string companyCodeString = project.Company.Code;
            string dateString = DateTime.Now.Year.ToString()
                + string.Format("{0:D2}", DateTime.Now.Month)
                + string.Format("{0:D2}", DateTime.Now.Day);

            string typeString = string.Format("{0:D2}", (int)project.Type + 1);
            DateTime lastYear = DateTime.Now.AddYears(-1);
            int count = projectManager.FindList(p => p.Type == project.Type &&  p.RegTime >= lastYear && p.RegTime <= DateTime.Now).Count();
            string countString = string.Format("{0:D4}", count + 1);

            project.ReportFileCode = companyCodeString + dateString + typeString + countString;
            project.Progress = ProjectProgress.项目登记;
            resp = projectManager.Add(project);

            DateTime date = new DateTime(2017, 7, 15);
            if(DateTime.Now > date)
            {
                int emailcount = projectManager.FindList(p => p.ObjectEmail == project.ObjectEmail).Count();

                if (emailcount >= 5)
                {

                    int score = 0 - emailcount / 5 * 10;

                    Evaluation eva = new Evaluation();
                    eva.Project = project;
                    eva.Result = score;
                    eva.Source = EvaluationSource.系统检测;
                    eva.Note = "该机构存在恶意刷分行为。";
                    evaManager.Add(eva);

                    project.Company.Score = project.Company.Score + score;
                    companyManager.Update(project.Company);
                }
            }

            
            
            return Json("123");
        }

        [HttpPost]
        public ActionResult setProject(Project p)
        {
            Response resp = new Response();

            var project = projectManager.Find(p.ID);

            project.Name = p.Name;
            project.Address = p.Address;
            project.BuildType = p.BuildType;
            project.FireRisk = p.FireRisk;
            project.Area = p.Area;
            project.AllLayer = p.AllLayer;
            project.ServiceLayer = p.ServiceLayer;
            project.ContractNumber = p.ContractNumber;
            project.SignDate = p.SignDate;
            project.StartDate = p.StartDate;
            project.EndDate = p.EndDate;
            project.ProjectHead = p.ProjectHead;
            project.TechHead = p.TechHead;
            project.ProjectContact = p.ProjectContact;
            project.Note = p.Note;
            project.Object = p.Object;
            project.ObjectContact = p.ObjectContact;
            project.ObjectContactPhone = p.ObjectContactPhone;

            resp = projectManager.Update(project);
            return Json(resp);
        }

        [HttpPost]
        public ActionResult setCompany(Company c)
        {
            var company = companyManager.Find(c.ID);
            company.Name = c.Name;
            company.Type1 = c.Type1;
            company.Level1 = c.Level1;
            company.Type2 = c.Type2;
            company.Level2 = c.Level2;
            company.Number1 = c.Number1;
            company.ExpiryDate1 = c.ExpiryDate1;
            company.ExpiryDate2 = c.ExpiryDate2;
            company.Fund = c.Fund;
            company.Delegate = c.Delegate;
            company.DelegateOfficePhone = c.DelegateOfficePhone;
            company.DelegateMobilePhone = c.DelegateMobilePhone;
            company.Contact = c.Contact;
            company.ContactOfficePhone = c.ContactOfficePhone;
            company.ContactMobilePhone = c.ContactMobilePhone;
            company.fax = c.fax;
            company.Postcode = c.Postcode;
            company.Email = c.Email;
            company.Address = c.Address;
            company.Area = c.Area;

            companyManager.Update(company);
            return Json("123");
        }

        [HttpPost]
        public ActionResult deleteProject(int id)
        {
            Response resp = new Auxiliary.Response();
            var project = projectManager.Find(id);

            var evas = evaluationManager.FindList(e => e.Project.ID == project.ID).ToList();
            foreach(var i in evas)
            {
                evaluationManager.Delete(i.ID);
            }

            if (project.DataFile != null)
            {
                fileManager.Delete(project.DataFile.ID);
                project.DataFile = null;
            }

            if (project.ReportFile != null)
            {
                fileManager.Delete(project.ReportFile.ID);
                project.ReportFile = null;
            }

            projectManager.Update(project);

            resp = projectManager.Delete(id);
            return Json(resp);
        }


        /// <summary>
        /// 获得所有服务机构信息
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
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

            companies = companies.OrderByDescending(c => c.ID);

            var data = companies.Select(c => new
            {
                ID = c.ID,
                Name = c.Name,
                Type = c.Type1.ToString() + (c.Type2 == null ? "" : "/" + c.Type2.ToString()),
                Level = c.Level1.ToString() + (c.Type2 == null ? "" : "/" + c.Level2.ToString()),
                Number = c.Number1.ToString() + (c.Type2 == null ? "" : "/" + c.Number2.ToString()),
                State = (c.ExpiryDate1 > DateTime.Now ? "有效" : "无效") + (c.Type2 == null ? "" : ("/" + (c.ExpiryDate2 > DateTime.Now ? "有效" : "无效"))),
                Score = c.Score
            });
            return Json(data, JsonRequestBehavior.AllowGet);
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
            Company company = companyManager.Find(c => c.ID == companyID);

            var data = new
            {
                ID = company.ID,
                Name = company.Name,
                Type1 = company.Type1,
                Level1 = company.Level1,
                Type2 = company.Type2,
                Level2 = company.Level2,
                Number1 = company.Number1,
                ExpiryDate1 = company.ExpiryDate1.Year.ToString()+'-'+ company.ExpiryDate1.Month.ToString()+'-'+ company.ExpiryDate1.Day.ToString(),
                Number2 = company.Number2,
                ExpiryDate2 = company.ExpiryDate2==null?"": ((DateTime)company.ExpiryDate2).Year.ToString() + '-' + ((DateTime)company.ExpiryDate2).Month.ToString() + '-' + ((DateTime)company.ExpiryDate2).Day.ToString(),
                Fund = company.Fund,
                Delegate = company.Delegate,
                DelegateOfficePhone = company.DelegateOfficePhone,
                DelegateMobilePhone = company.DelegateMobilePhone,
                Contact = company.Contact,
                ContactOfficePhone = company.ContactOfficePhone,
                ContactMobilePhone = company.ContactMobilePhone,
                fax = company.fax,
                Postcode = company.Postcode,
                Email = company.Email,
                Address = company.Address,
                Area = company.Area

            };
            return Json(data, JsonRequestBehavior.AllowGet);
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


            var companies = companyManager.FindList().ToList();
            string code = string.Empty;
            if (companies.Count == 0)
            {
                code = string.Format("GS{0:D4}", 1);
            }
            else
            {
                var lastCompany = companies.Last();
                int a = int.Parse(lastCompany.Code.Substring(2, 4)) + 1;
                code = string.Format("GS{0:D4}", a);
            }

            company.Code = code;
            resp = companyManager.Add(company);
            resp.Data = CreateCompanyUser(company);
            return Json(resp);
        }

        /// <summary>
        /// 获得所有人员信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult getEmployees()
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


            employees = employees.Where(e => e.Level == EmployeeLevel.一级注册消防工程师 || e.Level == EmployeeLevel.二级注册消防工程师 || e.Level == EmployeeLevel.临时注册消防工程师);

            employees = employees.OrderByDescending(e => e.ID);

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


        [HttpGet]

        public ActionResult getMyEmployees()
        {
            User user = Session["User"] as User;
            var employees = employeeManager.FindList();

            Company company = companyManager.Find(userCompanyManager.Find(uc => uc.UserID == user.ID).CompanyID);
            employees = employees.Where(e => e.Company.ID == company.ID);

            employees = employees.OrderByDescending(e => e.ID);

            var data = employees.Select(e => new
            {
                ID = e.ID,
                Name = e.Name,
                Gender = e.Gender.ToString(),
                Level = e.Level.ToString(),
                CertificateNumber = e.CertificateNumber,
                IdentificationNumber = e.IdentificationNumber,
                CompanyName = e.Company == null ? "无" : e.Company.Name,
                MobilePhone = e.MobilePhone
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

            var resp = employeeManager.Update(employee);
            return Json(resp);
        }

        /// <summary>
        /// 添加人员信息（单个）
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult addEmployee(Employee employee)
        {

            var user = Session["User"] as User;
            string roleName = user.Role.Name;
            Response resp = new Response();
            if (!ModelState.IsValid)
            {
                resp.Code = -1;
                resp.Message = "数据格式有误，提交失败";
                return Json(resp);
            }

            Employee e = employeeManager.Find(i => i.IdentificationNumber == employee.IdentificationNumber);
            if (roleName == "消防机构总队")
            {
                if (e != null)
                {
                    resp.Code = -2;
                    resp.Message = "检测到身份证号为【" + employee.IdentificationNumber + "】的人员已备案，备案失败！";
                    return Json(resp);
                }
                else
                {
                    resp = employeeManager.Add(employee);
                }
            }
            else if(roleName == "服务机构")
            {
                Company company = companyManager.Find(userCompanyManager.Find(uc => uc.UserID == user.ID).CompanyID);
                if (e != null)
                {
                    e.Company = company;
                    e.MobilePhone = employee.MobilePhone;
                    employeeManager.Update(e);
                }
                else
                {
                    employee.Company = company;
                    employeeManager.Add(employee);
                }
            }
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
                var employee = employeeManager.Find(e => e.IdentificationNumber == i.IdentificationNumber && (e.Level==EmployeeLevel.临时注册消防工程师||e.Level==EmployeeLevel.一级注册消防工程师||e.Level==EmployeeLevel.二级注册消防工程师));
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
                    resp.Code = 1;
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

            User user = Session["User"] as User;
            string roleName = user.Role.Name;
            if(roleName == "消防机构总队")
            {
                var data = employeeManager.Delete(employeeID);
                return Json(data);
            }else if(roleName == "服务机构")
            {
                var employee = employeeManager.Find(employeeID);
                employee.Company = null;
                employeeManager.Update(employee);
            }

            return Json("123");
        }
        [HttpGet]
        public ActionResult getUsers()
        {
            var users = userManager.FindList(u => u.Name != "Root");
            string name = Request.QueryString["Name"];
            string role = Request.QueryString["Role"];

            if (name != null && name != "")
            {
                users = users.Where(u => u.Name.Contains(name));
            }

            if (role != null && role != "" && role != "0")
            {
                int r = int.Parse(role);
                users = users.Where(u => u.Role.ID == r);
            }

            users = users.OrderByDescending(u => u.ID);

            var list = users.ToList();


            var data = list.Select(u => new
            {
                ID = u.ID,
                Name = u.Name,
                LoginIP = u.LoginIP,
                LoginTime = u.LoginTime.ToString(),
                RegTime = u.RegTime.ToString(),
                IsOnline = u.IsOnline==true?"在线":"离线",
                OrgName = userManager.getOrgName(u.ID)
            });
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult getMyUsers()
        {
            User user = Session["User"] as User;
            string code = user.Name.Substring(0, user.Name.Length - 2);
            var users = userManager.FindList(u => u.Name.Contains(code));
            users = users.OrderByDescending(u => u.ID);

            var list = users.ToList();
            var data = list.Select(u => new
            {
                ID = u.ID,
                Name = u.Name,
                LoginIP = u.LoginIP,
                LoginTime = u.LoginTime.ToString(),
                RegTime = u.RegTime.ToString(),
                IsOnline = u.IsOnline == true ? "在线" : "离线",
                Note = u.Note
            });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult setUserNote(int id,string Note)
        {
            User user = userManager.Find(u => u.ID == id);
            user.Note = Note;
            userManager.Update(user);
            return Json("123");
        }

        public ActionResult getUser(int userID = 0)
        {
            User user = userManager.Find(userID);
            var data = new
            {
                ID = user.ID,
                Name = user.Name,
                LoginIP = user.LoginIP,
                LoginTime = user.LoginTime == null ? "" : ((DateTime)user.LoginTime).ToShortDateString() + ((DateTime)user.LoginTime).ToShortTimeString(),
                RegTime = user.RegTime == null ? "" : ((DateTime)user.RegTime).ToShortDateString() + ((DateTime)user.RegTime).ToShortTimeString(),
                IsOnline = user.IsOnline == true ? "在线" : "离线",
                OrgName = userManager.getOrgName(user.ID)

            };

            return Json(user, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        [AllowAnonymous]
        public ActionResult getEvaluations()
        {


            var evas = evaluationManager.FindList(e=>e.Source != EvaluationSource.系统检测);


            var companyName = Request.QueryString["CompanyName"];
            var source = Request.QueryString["Source"];

            if(companyName != null && companyName != "")
            {
                evas = evas.Where(e => e.Project.Company.Name == companyName);
            }


            if(source != null && source != "-1")
            {
                EvaluationSource es = (EvaluationSource)int.Parse(source);
                evas = evas.Where(e => e.Source == es);
            }
            evas = evas.OrderByDescending(e => e.ID);

            var data = evas.Select(e=>new{
                CompanyName = e.Project.Company.Name,
                ProjectName = e.Project.Name,
                Note = e.Note,
                Result = e.Result,
                Source = e.Source.ToString()
            });
            return Json(data,JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult addEvaluation()
        {
            return Json("");
        }






        [HttpPost]
        public ActionResult resetPassword(int id,string Password)
        {

            var user = userManager.Find(id);
            user.Password = Password;
            var data = userManager.Update(user);

            data.Data = null;
            return Json(data);
        }


        [HttpPost]
        public ActionResult setPassword()
        {

            Response resp = new Auxiliary.Response();

            string password = Request.Form["password"];

            var user = Session["User"] as User;

            user.Password = password;
            userManager.Update(user);
            resp.Code = 1;
            resp.Message = "密码修改成功！";
            return Json(resp);
        }





        private object CreateCompanyUser(Company company)
        {


            string name = company.Code + "00";
            string password = name;
            userManager.Add(name, password, roleManager.Find(5));


            User user = userManager.Find(name);

            UserCompany uc = new UserCompany();
            uc.UserID = user.ID;
            uc.CompanyID = company.ID;
            userCompanyManager.Add(uc);

            return new { Name = name, Password = password };
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult getFile(int projectID,int type)
        {
            //对当前数据库刷新
            Project project = projectManager.Find(projectID);

            UploadFile file = type == 0 ? project.DataFile : project.ReportFile;

            if (file==null|| !System.IO.File.Exists(Server.MapPath(file.Path)))
            {
                return Content("文件不存在，下载失败");
            }

            string path = file.Path;
            string name = file.Name;

            var contentType = MimeMapping.GetMimeMapping(name);
            return File(path,contentType, name);
        }


        [HttpPost]
        public ActionResult addArticle()
        {
            Response resp = new Auxiliary.Response();

            if (Request.Files.Count == 0)
            {
                resp.Code = -1;
                resp.Message = "上传文件不能为空";
                return Json(resp);
            }
            var file = Request.Files[0];
            if (file.ContentLength >= 20971520)
            {
                resp.Code = -2;
                resp.Message = "上传文件太大";
                return Json(resp);
            }
            var fileName = file.FileName;

            if (fileName.Substring(fileName.Length - 4, 4) != ".pdf")
            {
                resp.Code = -3;
                resp.Message = "文件格式不符合要求";
                return Json(resp);
            }
            fileName = Verification.Text(20) + ".pdf";
            var filePath = Server.MapPath("~/Upload/Articles");

            var path = string.Format(filePath + "\\{0}", fileName);
            file.SaveAs(path);

            UploadFile ufile = new UploadFile();
            ufile.Name = fileName;
            ufile.Path = "~/Upload/Articles/" + fileName;
            fileManager.Add(ufile);




            Article article = new Article();
            article.Title = Request.Form["Title"];
            article.Category = (Category)int.Parse(Request.Form["Category"]);
            article.File = fileManager.Find(f => f.Name == ufile.Name);
            article.CommitDate = DateTime.Now;

            articleManager.Add(article);
            return Json("");
        }


        public ActionResult CreateUser()
        {

            Response resp = new Auxiliary.Response();


            User user = Session["User"] as User;

            string code = user.Name.Substring(0, user.Name.Length - 2);
            var users = userManager.FindList(u => u.Name.Contains(code));
            if(users.Count() > 10)
            {
                resp.Code = -1;
                resp.Message = "添加失败，最多只能有10个账号";
                return Json(resp,JsonRequestBehavior.AllowGet);
            }

            string str = string.Format("{0:D2}", users.Count());

            string fullName = code + str;

            
            resp = userManager.Add(fullName,fullName,roleManager.Find(user.Role.ID));

            if (user.Role.Name == "消防机构总队" || user.Role.Name == "消防机构支队" || user.Role.Name == "消防机构大队")
            {

            }
            if (user.Role.Name == "服务机构")
            {
                UserCompany userCompany = new UserCompany();
                userCompany.UserID = userManager.Find(u => u.Name == fullName).ID;
                userCompany.CompanyID = userCompanyManager.Find(uc => uc.UserID == user.ID).CompanyID;
                userCompanyManager.Add(userCompany);
            }
            resp.Data = new
            {
                Name = fullName,
                Password = fullName
            };
            return Json(resp,JsonRequestBehavior.AllowGet);
        }



        

    }
}