using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GSXF.Core
{
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        public StaffLevel Level { get; set; }

        public string CertificateNumber { get; set; }

        public string IdentificationNumber { get; set; }

        public bool IsDirector { get; set; }

        public string OfficePhone { get; set; }

        public string MobilePhone { get; set; }

        public virtual Company Company { get; set; }
    }
}
