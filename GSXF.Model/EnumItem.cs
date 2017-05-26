using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GSXF.Model
{

    /// <summary>
    /// 资质类型
    /// </summary>
    public enum QualificationType
    {
        维修保养检测,
        消防安全评估
    }

    /// <summary>
    /// 资质等级
    /// </summary>
    public enum QualificationLevel
    {
        一级,
        二级,
        三级,
        临时一级,
        临时二级,
        临时三级
    }

    /// <summary>
    /// 项目类型
    /// </summary>
    public enum ProjectType
    {
        竣工检测,
        年度检测,
        维护保养,
        安全评估
    }

    /// <summary>
    /// 项目进度
    /// </summary>
    public enum ProjectProgress
    {
        项目登记,
        入场检测,
        出具报告,
        提交备案
    }

    /// <summary>
    /// 十五个地区
    /// </summary>
    public enum City
    {
        兰州,
        天水,
        金昌,
        白银,
        武威,
        平凉,
        酒泉,
        庆阳,
        定西,
        陇南,
        临夏,
        嘉峪关,
        甘南,
        张掖,
        兰州新区
    }

    /// <summary>
    /// 员工等级(由于C#枚举成员中不能包含括号，所以此处使用简写来代替)
    /// </summary>
    public enum EmployeeLevel
    {
        一级注册消防工程师,
        二级注册消防工程师,
        /// <summary>
        /// 建（构）筑物消防员（初级）
        /// </summary>
        初级,
        /// <summary>
        /// 建（构）筑物消防员（中级）
        /// </summary>
        中级,
        /// <summary>
        /// 建（构）筑物消防员（高级）
        /// </summary>
        高级
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum Gender
    {
        男,
        女,
        未知
    }

    public enum InstitutionType
    {
        消防机构,
        服务机构
    }

    public enum BuildType
    {
        公用建筑,
        民用建筑
    }

    public enum FireRisk
    {
        甲,
        乙,
        丙,
        丁,
        戊
    }

    
}
