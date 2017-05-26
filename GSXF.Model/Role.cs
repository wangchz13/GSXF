using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GSXF.Model
{
    /// <summary>
    /// 角色类
    /// </summary>
    public class Role
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
