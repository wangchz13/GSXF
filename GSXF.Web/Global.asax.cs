using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using GSXF.DataBase;
using GSXF.Model;


namespace GSXF.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {

        private static UserManager userManager = new UserManager();
        private static FireControlInstitutionManager fireManager = new FireControlInstitutionManager();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            if (userManager.FindList().Count() == 0)
            {
                FirstRun.Start();
            }

            //对所有用户进行下线处理
            var users = userManager.FindList().ToList();
            foreach(var i in users)
            {
                i.IsOnline = false;
                userManager.Update(i);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 60;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            User user = Session["User"] as User;
            if(user != null)
            {
                user.IsOnline = false;
                userManager.Update(user);
            }
        }
    }
}
