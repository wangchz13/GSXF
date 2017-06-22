using System;
using System.ComponentModel.DataAnnotations;

namespace GSXF.Model
{
    public class Article
    {
        [Key]
        public int ID { get; set; }

        public string Title { get; set; }

        public virtual UploadFile File { get; set; }

        public Category Category { get; set; }
    }
}
