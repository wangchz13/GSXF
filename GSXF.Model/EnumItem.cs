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
        嘉峪关,
        
        金昌,
        白银,
        天水,
        酒泉,
        张掖,
        武威,
        定西,
        陇南,
        平凉,
        
        庆阳,
        
        
        临夏,
        
        甘南,
        
        兰州新区
    }

    /// <summary>
    /// 员工等级(由于C#枚举成员中不能包含括号，所以此处使用简写来代替)
    /// </summary>
    public enum EmployeeLevel
    {
        临时注册消防工程师,
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
        高级,
        
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

    public enum BuildType
    {
        公用建筑,
        民用建筑
    }

    public enum FireRisk
    {
        无 = -1,
        甲,
        乙,
        丙,
        丁,
        戊
    }

    public enum EvaluationSource
    {
        客户评价,
        项目抽查,
        系统检测
    }


    public enum Category
    {
        通知公告,
        技术标准,
        相关法规,
        办事指南
    }
}
