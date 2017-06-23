using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSXF.Auxiliary;
using GSXF.Model;

namespace GSXF.DataBase
{
    public class ArticleManager : BaseManager<Article>
    {
        private static FileManager fileManager = new FileManager();

        public override Response Add(Article entity)
        {
            return base.Add(entity);
        }

        public override Response Delete(int ID)
        {

            fileManager.Delete(Find(ID).File.ID);
            return base.Delete(ID);
        }
    }
}
