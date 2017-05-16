using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GSXF.Core
{
    public class Project
    {
        [Key]
        public int ID { get; set; }

        public string ProjectName { get; set; }

        public string OwnerName { get; set; }

        public ProjectType Type { get; set; }

        public string ProjectCity { get; set; }

        public string AddressDetail { get; set; }

        public string Contact { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public string ServiceName { get; set; }

        public string ServiceContent { get; set; }

        public DateTime RecordDate { get; set; }


    }
}
