using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GSXF.Model
{
    public  class OfficeAddress
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public float Area { get; set; }

        [JsonIgnore]
        public virtual Company Company { get; set; }
    }
}
