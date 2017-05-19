using System.ComponentModel.DataAnnotations;

namespace GSXF.Core
{
    public class UserRole
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int RoleID { get; set; }
    }
}
