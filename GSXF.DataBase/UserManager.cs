using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSXF.Model;
using GSXF.Auxiliary;


namespace GSXF.DataBase
{
    
    public class UserManager : BaseManager<User>
    {

        private FireControlInstitutionManager fireManager = new FireControlInstitutionManager();
        private CompanyManager companyManager = new CompanyManager();
        private UserCompanyManager userCompanyManager = new UserCompanyManager();
        public User Find(string name)
        {
            return Find(u => u.Name == name);
        }


        public string getOrgName(int userID)
        {
            User user = Find(userID);
            string roleName = user.Role.Name;
            string orgName = string.Empty;
            if(roleName=="消防机构总队" || roleName=="消防机构支队" || roleName == "消防机构大队")
            {
                string fireCode = user.Name.Substring(0, 8);
                orgName  = fireManager.Find(f => f.Code == fireCode).Name;
            }else if(roleName == "服务机构")
            {
                Company company = companyManager.Find(userCompanyManager.Find(uc => uc.UserID == userID).CompanyID);
                orgName = company.Name;
            }
            else
            {

            }
            return orgName;
        }


        public Response Add(string name, string password, Role role=null)
        {
            User user = new User();
            user.Name = name;
            user.Password = password;
            user.RegTime = DateTime.Now;
            user.IsOnline = false;
            user.Role = role;
            return Add(user);
        }
        

        public Response Verify(string name, string password)
        {
            Response _resp = new Response();
            var _user = Find(u => u.Name == name);
            if(_user == null)
            {
                _resp.Code = 0;
                _resp.Message = "此账号不存在";
            }
            else if(_user.Password != password)
            {
                _resp.Code = 1;
                _resp.Message = "密码错误";
            }
            else if(_user.IsOnline)
            {
                _resp.Code = 2;
                _resp.Message = "当前用户已登录，无法再次登录";
            }
            else
            {
                _resp.Code = 3;
                _resp.Message = "登录成功";
            }
            return _resp;
        }
    }
}
