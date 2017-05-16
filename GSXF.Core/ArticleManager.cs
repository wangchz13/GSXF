using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSXF.Auxiliary;

namespace GSXF.Core
{
    public class ArticleManager : BaseManager<Article>
    {
        public override Response Add(Article entity)
        {
            return base.Add(entity);
        }
    }
}
