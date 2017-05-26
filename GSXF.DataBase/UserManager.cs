using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSXF.Model;

namespace GSXF.DataBase
{
    
    public class UserManager : BaseManager<User>
    {
        public User Find(string name)
        {
            return Find(u => u.Name == name);
        }

        public bool Verify(string name, string password, InstitutionType institutionType)
        {
            var user = Find(u => u.Name == name && u.Password == password && u.institutionType == institutionType);
            return user != null;
        }
    }
}
