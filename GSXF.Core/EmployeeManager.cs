using GSXF.Auxiliary;

namespace GSXF.Core
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
