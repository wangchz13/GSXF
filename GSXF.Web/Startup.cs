using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GSXF.Web.Startup))]
namespace GSXF.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
