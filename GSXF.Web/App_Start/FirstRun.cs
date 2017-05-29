using System;
using System.Web;
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
using GSXF.Security;

namespace GSXF.Web
{
    public class FirstRun
    {
        private static UserManager userManager = new UserManager();
        private static FireControlInstitutionManager fireManager = new FireControlInstitutionManager();
        private static RoleManager roleManager = new RoleManager();
        private static UserRoleManager userRoleManager = new UserRoleManager();
        public static void  Start()
        {
            RoleInit();
            FireControlInstitutionInit();
            UserInit();
            UserRoleInit();
        }
        /// <summary>
        /// 初始化角色表
        /// </summary>
        private static void RoleInit()
        {
            roleManager.Add(new Role() { Name = "Root", Description = "超级管理员" });
            roleManager.Add(new Role() { Name = "消防机构总队", Description = "消防机构总队" });
            roleManager.Add(new Role() { Name = "消防机构支队", Description = "消防机构支队" });
            roleManager.Add(new Role() { Name = "消防机构大队", Description = "消防机构大队" });
            roleManager.Add(new Role() { Name = "服务机构", Description = "服务机构" });
        }
        /// <summary>
        /// 初始化消防机构表
        /// </summary>
        private static void FireControlInstitutionInit()
        {
            string filepath = HttpContext.Current.Server.MapPath("~/Content/Json/institutioninfo.json");
            string jsonstr = string.Empty;
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                {
                    jsonstr = sr.ReadToEnd().ToString();
                }
            }
            JArray array = JArray.Parse(jsonstr);
            FireControlInstitution fire = new FireControlInstitution();
            foreach (var i in array)
            {
                fire.Name = i["Name"].ToString();
                fire.Code = i["Code"].ToString();
                fireManager.Add(fire);
            }
        }

        /// <summary>
        /// 初始化用户表
        /// </summary>
        private static void UserInit()
        {
            //添加超级管理员账号
            userManager.Add("Root",Encryption.SHA256("root"));

            //添加各消防机构账号
            var fireList = fireManager.FindList().ToList();
            foreach(var i in fireList)
            {
                string name = i.Code + "00";
                string password = Encryption.SHA256(name);
                userManager.Add(name, password);
            }
        }

        private static void UserRoleInit()
        {

            var roles = roleManager.FindList().ToList();
            var users = userManager.FindList().ToList();

            //为超级管理员分配权限
            User root = userManager.Find(1);
            foreach(var i in roles)
            {
                //超级管理员具备所有权限
                userRoleManager.Add(root.ID, i.ID);
            }

            //为各消防机构分配权限
            for(int i = 1; i < users.Count; i++)
            {
                //算出用户名所对应的机构代码
                string code = users[i].Name.Substring(0, 8);
                //找到代码对应的机构名称
                string name = fireManager.Find(f => f.Code == code).Name;

                //从机构名称中计算出机构类型，有三种情况，"总队"、"支队"和"大队"
                //TODO:将这部分放到机构信息里去，但不映射到数据库
                string type = name.Substring(name.Length - 2);

                if(type == "总队")
                {
                    userRoleManager.Add(users[i].ID, 2);
                }else if (type == "支队")
                {
                    userRoleManager.Add(users[i].ID, 3);
                }
                else if (type == "大队")
                {
                    userRoleManager.Add(users[i].ID, 4);
                }
            }
        }
    }
}