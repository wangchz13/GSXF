using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GSXF.Model
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 证书类别
        /// </summary>
        public EmployeeLevel Level { get; set; }
        /// <summary>
        /// 证书编号
        /// </summary>
        public string CertificateNumber { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentificationNumber { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// 所属服务机构
        /// </summary>
        public virtual Company Company { get; set; }
    }
}
