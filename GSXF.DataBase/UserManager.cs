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
        public User Find(string name)
        {
            return Find(u => u.Name == name);
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
