using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSXF.DataBase;

namespace GSXF.Web.Controllers
{
    public class BaseController : Controller
    {
        public static ProjectManager projectManager = new ProjectManager();
        public static CompanyManager companyManager = new CompanyManager();
        public static UserCompanyManager userCompanyManager = new UserCompanyManager();
        public static EmployeeManager employeeManager = new EmployeeManager();
        public static FireControlInstitutionManager fireManager = new FireControlInstitutionManager();
        public static UserManager userManager = new UserManager();
        public static RoleManager roleManager = new RoleManager();
        public static EvaluationManager evaManager = new EvaluationManager();
        public static ArticleManager articleManager = new ArticleManager();
        public static FileManager fileManager = new FileManager();
        public static EvaluationManager evaluationManager = new EvaluationManager();



        protected override void OnException(ExceptionContext filterContext)
        {
            // 错误日志编写    
            string controllerNamer = filterContext.RouteData.Values["controller"].ToString();
            string actionName = filterContext.RouteData.Values["action"].ToString();
            string exception = filterContext.Exception.ToString();
            // 执行基类中的OnException    
            base.OnException(filterContext);
        }
    }
}