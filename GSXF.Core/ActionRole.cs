using System.ComponentModel.DataAnnotations;

namespace GSXF.Core
{
    /// <summary>
    /// Action和Role之间的表
    /// 多对多的关系只能新加一张表处理，为了让代码统一，一对多也加一张表吧。。。。。。。。。
    /// </summary>
    public class ActionRole
    {
        [Key]
        public int ID { get; set; }

        public int ActionID { get; set; }

        public int RoleID { get; set; }

        public bool IsAllowed { get; set; }
    }
}
