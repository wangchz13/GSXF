﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GSXF.Core
{
    public  class OfficeAddress
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public float Area { get; set; }
    }
}
