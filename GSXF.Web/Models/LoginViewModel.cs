using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSXF.Web.Models
{
    public class LoginViewModel
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public int InstitutionType { get; set; }
    }
}