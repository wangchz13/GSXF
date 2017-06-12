using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GSXF.Model
{
    public class FireControlInstitution
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        [JsonIgnore]
        public virtual ICollection<Project> Projects { get; set; }
    }
}
