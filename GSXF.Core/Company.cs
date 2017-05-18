using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GSXF.Core
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
        /// 资质类型
        /// </summary>
        public QualificationType QualificationType { get; set; }

        /// <summary>
        /// 资质等级
        /// </summary>
        public QualificationLevel QualificationLevel { get; set; }

        /// <summary>
        /// 证书编号
        /// </summary>
        public string CertificateNumber { get; set; }

        /// <summary>
        /// 证书有效期
        /// </summary>
        public Nullable<DateTime> CertificateExpiryDate { get; set; }

        /// <summary>
        /// 办公地址
        /// </summary>
        public virtual ICollection<OfficeAddress> OfficeAddresses { get; set; }

        /// <summary>
        /// 注册资金
        /// </summary>
        public float RegisteredCapital { get; set; }

        /// <summary>
        /// 法定代表人
        /// </summary>
        public string LegalRepresentative { get; set; }

        /// <summary>
        /// 法代办公电话
        /// </summary>
        public string LROfficePhone { get; set; }

        /// <summary>
        /// 法代移动电话
        /// </summary>
        public string LRMobilePhone { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 联系人办公电话
        /// </summary>
        public string COfficePhone { get; set; }

        /// <summary>
        /// 联系人移动电话
        /// </summary>
        public string CMobilePhone { get; set; }

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

        /// <summary>
        /// 执业开通状态
        /// </summary>
        public bool Approved { get; set; }


        public ICollection<Employee> Emoloyees { get; set; }
    }
}
