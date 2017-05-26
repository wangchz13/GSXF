using System.ComponentModel.DataAnnotations;

namespace GSXF.Model
{
    /// <summary>
    /// Action和Role之间的表
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
