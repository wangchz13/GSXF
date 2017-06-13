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
        /// 项目类型
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectType Type { get; set; }
        /// <summary>
        /// 项目地区
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public City City { get; set; }
        [JsonIgnore]
        public virtual FireControlInstitution FireControlInstitution { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 建筑类型
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public BuildType BuildType { get; set; }
        /// <summary>
        /// 火灾风险
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public FireRisk FireRisk { get; set; }
        /// <summary>
        /// 建筑面积
        /// </summary>
        public float Area { get; set; }
        /// <summary>
        /// 建筑总层数
        /// </summary>
        public string AllLayer { get; set; }
        /// <summary>
        /// 服务层数
        /// </summary>
        public string ServiceLayer { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNumber { get; set; }
        /// <summary>
        /// 合同签订日期
        /// </summary>
        public DateTime SignDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ProjectHead { get; set; }

        public string TechHead { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ProjectContact { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 服务对象
        /// </summary>
        public string Object { get; set; }
        /// <summary>
        /// 委托单位联系人
        /// </summary>
        public string ObjectContact { get; set; }
        /// <summary>
        /// 委托单位电话
        /// </summary>
        public string ObjectContactPhone { get; set; }
        /// <summary>
        /// 委托单位邮件
        /// </summary>
        public string ObjectEmail { get; set; }
        /// <summary>
        /// 所属服务机构
        /// </summary>
        [JsonIgnore]
        public virtual Company Company { get; set; }
        /// <summary>
        /// 项目进度
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ProjectProgress Progress { get; set; }
        
        /// <summary>
        /// 检测结果
        /// </summary>
        public bool Result { get; set; }

        public DateTime RecordDate { get; set; }
    }
}
