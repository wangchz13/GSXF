using System;
using System.ComponentModel.DataAnnotations;
namespace GSXF.Core
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

        public DateTime LastLoginTime { get; set; }

        public string LastLoginIP { get; set; }

        public DateTime RegTime { get; set; }
    }
}
