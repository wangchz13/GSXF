using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GSXF.DataBase;
using GSXF.Model;


namespace GSXF.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {

        private static UserManager userManager = new UserManager();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 1;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            User user = Session["User"] as User;
            //string name = user.Name;
            //user = userManager.Find(name);
            if(user != null)
            {
                user.IsOnline = false;
                userManager.Update(user);
            }
        }
    }
}
