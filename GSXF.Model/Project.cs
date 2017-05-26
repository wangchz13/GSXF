using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GSXF.Model
{
    public class Project
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        [JsonIgnore]
        public virtual Company Company { get; set; }

        public string CompanyName { get; set; }

        /// <summary>
        /// 被服务机构
        /// </summary>
        public string Object { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectType Type { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public City City { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BuildType BuildType { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public FireRisk FireRisk { get; set; }

        public float Area { get; set; }

        public int AllLayer { get; set; }

        public int ServiceLayer { get; set; }

        public string ProjectCity { get; set; }

        public string AddressDetail { get; set; }

        public string Contact { get; set; }

        public string PDirector { get; set; }

        public string TDirector { get; set; }

        public string OContact { get; set; }

        public string OContactPhone { get; set; }

        public string OContactEmail { get; set; }

        public string ContractNumber { get; set; }

        public DateTime RecordDate { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectProgress Progress { get; set; }

        public string Note { get; set; }
    }
}
