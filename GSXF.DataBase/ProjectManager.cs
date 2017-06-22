using GSXF.Auxiliary;
using System;
using System.Linq;
using GSXF.Model;

namespace GSXF.DataBase
{
    public class ProjectManager : BaseManager<Project>
    {
        public int[] GetProjectCount(ProjectType type)
        {
            var res = new int[15];
            for(int i = 0; i < 15; i++)
            {
                res[i] = FindList(p => p.City == (City)i && p.Type == type).Count();
            }
            return res;
        }

        public int GetProjectCount2(ProjectType type)
        {
            return FindList(p => p.Type == type).Count();
        }
    }
}
