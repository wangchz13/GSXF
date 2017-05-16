using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GSXF.Core
{
    public class Staff
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }

        public StaffLevel Level { get; set; }

        public string CertificateNumber { get; set; }

        public string IdentificationNumber { get; set; }

        public bool IsDirector { get; set; }

        public string OfficePhone { get; set; }

        public string MobilePhone { get; set; }
    }
}
