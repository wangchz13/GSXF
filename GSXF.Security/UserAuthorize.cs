using System.Web;
using System.Web.Mvc;

namespace GSXF.Security
{
    /// <summary>
    /// 用户认证
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["User"] == null)
                return false;
            else
                return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.RequestContext.HttpContext.Response.Write("用户未登录或登录信息过期，点此<a href='/' target='_self'>重新登录</a>！");
            filterContext.RequestContext.HttpContext.Response.End();
        }
    }
}
