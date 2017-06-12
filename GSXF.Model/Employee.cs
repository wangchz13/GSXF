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
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public EmployeeLevel Level { get; set; }

        public string CertificateNumber { get; set; }

        public string IdentificationNumber { get; set; }

        public bool IsDirector { get; set; }

        public string OfficePhone { get; set; }

        public string MobilePhone { get; set; }

        [JsonIgnore]
        public virtual Company Company { get; set; }

        public string CompanyName { get; set; }

    }
}
