﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace GSXF.Core
{
    public class UserCompany
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
    }
}
