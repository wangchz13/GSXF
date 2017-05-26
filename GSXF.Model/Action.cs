using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace GSXF.Model
{
    /// <summary>
    /// 动作类
    /// </summary>
    public class Action
    {
        [Key]
        public int ID { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public bool IsAllowedNoneRoles { get; set; }

        public bool IsAllowedAllRoles { get; set; }

        public string Description { get; set; }
    }
}
