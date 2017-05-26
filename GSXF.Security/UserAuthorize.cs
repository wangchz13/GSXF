using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using GSXF.DataBase;
using GSXF.Model;

namespace GSXF.Security
{
    /// <summary>
    /// 用户认证
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        private static UserManager userManager = new UserManager();
        private static ActionManager actionManager = new ActionManager();
        private static UserRoleManager userRoleManager = new UserRoleManager();
        private static ActionRoleManager actionRoleManager = new ActionRoleManager();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.Session["User"] as User;
            if(user == null)
            {
                user = userManager.Find(u => u.Name == "Guest");
            }
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            var isAllowed = IsAllowed(user, controller, action);
            if (!isAllowed)
            {
                filterContext.RequestContext.HttpContext.Response.Write("无权访问");
                filterContext.RequestContext.HttpContext.Response.End();
            }
        }

        private bool IsAllowed(User user, string controller, string action)
        {
            var controllerAction = actionManager.Find(a => a.ControllerName == controller && a.ActionName == action);

            //记录不存在，允许访问
            if (controllerAction == null)
            {
                return true;
            }

            //Action允许所有人访问，允许访问
            if (controllerAction.IsAllowedNoneRoles)
            {
                return true;
            }

            //Action允许有角色的用户访问，允许访问
            if (controllerAction.IsAllowedAllRoles)
            {
                //只要用户有角色就可以访问
                var roles = userRoleManager.Find(ur => ur.ID == user.ID);
                return roles != null;
            }
            //下面是Action对指定角色允许访问
            //选出Action对应的角色
            var actionRoles = actionRoleManager.FindList(ar => ar.ActionID == controllerAction.ID).ToList();
            if(actionRoles.Count == 0)
            {
                return true;
            }
            //找到用户角色，此处不考虑多个角色问题
            var userRole = userRoleManager.Find(ur => ur.ID == user.ID);

            //查找Action允许访问的角色
            var allowedRoles = actionRoleManager.FindList(ar => ar.ActionID == controllerAction.ID).Select(a => a.RoleID).ToList();
            if (allowedRoles.Count > 0)
            {
                foreach (var i in allowedRoles)
                {
                    if (i == userRole.RoleID)
                        return true;
                }
                return false;
            }
            return true;
        }
    }
}
