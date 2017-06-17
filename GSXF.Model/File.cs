using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GSXF.Model
{
    public class File
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public virtual User uploadUser { get; set; }
    }
}
