using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        public virtual Company Company { get; set; }
    }
}
