using GSXF.Auxiliary;
using GSXF.Model;

namespace GSXF.DataBase
{
    public class EmployeeManager : BaseManager<Employee>
    {
        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Response Add(Employee entity)
        {

            return base.Add(entity);
        }
    }
}
