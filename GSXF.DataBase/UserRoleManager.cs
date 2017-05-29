using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSXF.Model;
using GSXF.Auxiliary;


namespace GSXF.DataBase
{
    public class UserRoleManager : BaseManager<UserRole>
    {
        public Response Add(int userID, int roleID)
        {
            UserRole userRole = new UserRole();
            userRole.UserID = userID;
            userRole.RoleID = roleID;
            return Add(userRole);
        }
    }
}
