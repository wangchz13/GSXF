using System;
using System.ComponentModel.DataAnnotations;

namespace GSXF.Core
{
    public class Article
    {
        [Key]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public virtual Category Category { get; set; }
    }
}
