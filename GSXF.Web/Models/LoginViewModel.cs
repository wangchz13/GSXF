using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace GSXF.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="煞笔名字不能为空")]
        public string Name { get; set; }
        [Required(ErrorMessage = "煞笔密码不能为空")]
        public string Password { get; set; }
    }
}