using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GSXF.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace GSXF.Web.Controllers
{
    public class DataController : Controller
    {
        // GET: Data
        public ActionResult Index()
        {
            
            return View();
        }

        public string GenderList()
        {
            var data = EnumToObject(typeof(Gender));
            return JsonConvert.SerializeObject(data);
        }

        public string EmployeeLevelList()
        {
            var data = EnumToObject(typeof(EmployeeLevel));
            string str = JsonConvert.SerializeObject(data);
            JArray obj = JArray.Parse(str);
            foreach(var i in obj)
            {
                if(i["Text"].ToString() == "初级")
                {
                    i["Text"] = "建(构)筑物消防员(初级)";
                }
                else if(i["Text"].ToString() == "中级")
                {
                    i["Text"] = "建(构)筑物消防员(中级)";
                }
                else if (i["Text"].ToString() == "高级")
                {
                    i["Text"] = "建(构)筑物消防员(高级)";
                }
            }
            return obj.ToString();
        }

        






















        /// <summary>
        /// 枚举转对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private object EnumToObject(Type type)
        {
            var data = Enum.GetNames(type).Select(text => new
            {
                Id = (int)Enum.Parse(type, text),
                Text = text
            }).ToArray();
            return data;
        }
    }
}