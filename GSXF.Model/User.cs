using System;
using System.ComponentModel.DataAnnotations;
namespace GSXF.Model
{
    /// <summary>
    /// 用户类
    /// </summary>
    public class User
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public Nullable<DateTime> LastLoginTime { get; set; }

        public string LastLoginIP { get; set; }

        public Nullable<DateTime> RegTime { get; set; }

        public InstitutionType institutionType { get; set; }
    }
}
