﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GSXF.Model
{
    public class Company
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [Required(ErrorMessage ="机构名称不能为空")]
        public string Name { get; set; }
        /// <summary>
        /// 机构代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 资质类型1
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public QualificationType Type1 { get; set; }
        /// <summary>
        /// 资质等级1
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public QualificationLevel Level1 { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public QualificationType ? Type2 { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public QualificationLevel ? Level2 { get; set; }

        public string Number1 { get; set; }

        public DateTime ExpiryDate1 { get; set; }

        public string Number2 { get; set; }

        public DateTime ? ExpiryDate2 { get; set; }



        /// <summary>
        /// 注册资金
        /// </summary>
        public float Fund { get; set; }

        /// <summary>
        /// 法定代表人
        /// </summary>
        public string Delegate { get; set; }

        /// <summary>
        /// 法代办公电话
        /// </summary>
        public string DelegateOfficePhone { get; set; }

        /// <summary>
        /// 法代移动电话
        /// </summary>
        public string DelegateMobilePhone { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 联系人办公电话
        /// </summary>
        public string ContactOfficePhone { get; set; }

        /// <summary>
        /// 联系人移动电话
        /// </summary>
        public string ContactMobilePhone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string fax { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postcode { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }


        private int _Score;

        public int Score
        {
            get
            {
                return this._Score;
            }
            set
            {
                if (value > 100)
                {
                    _Score = 100;
                }
                else if (value < 0)
                {
                    _Score = 0;
                }
                else
                {
                    _Score = value;
                }
            }

        }

        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; }

        [JsonIgnore]
        public virtual ICollection<Project> Projects { get; set; }

        public string Address { get; set; }

        public float Area { get; set; }
    }
}
