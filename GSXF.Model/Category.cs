using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GSXF.Model
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}
