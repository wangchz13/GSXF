using System;
using System.ComponentModel.DataAnnotations;

namespace GSXF.Model
{
    public class Article
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 上传文件对象
        /// </summary>
        public virtual UploadFile File { get; set; }
        /// <summary>
        /// 文章栏目
        /// </summary>
        public Category Category { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CommitDate { get; set; }
    }
}
