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

        public DateTime? LoginTime { get; set; }

        public string LoginIP { get; set; }

        public DateTime? RegTime { get; set; }

        public bool IsOnline { get; set; }
    }
}
