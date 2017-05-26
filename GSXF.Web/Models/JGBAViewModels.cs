using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSXF.DataBase;
using GSXF.Model;

namespace GSXF.Web.Models
{
    public class JGBAViewModel
    {
        public Company Company { get; set; }
        public IEnumerable<OfficeAddress> OfficeAddresses { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}