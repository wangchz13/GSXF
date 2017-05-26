using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSXF.Auxiliary;
using GSXF.Model;

namespace GSXF.DataBase
{
    public class CompanyManager : BaseManager<Company>
    {
        public override Response Add(Company entity)
        {
            return base.Add(entity);
        }

        public override Response Update(Company entity)
        {
            return base.Update(entity);
        }

        public override Response Delete(int ID)
        {
            return base.Delete(ID);
        }


    }
}
