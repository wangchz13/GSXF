using System.ComponentModel.DataAnnotations;

namespace GSXF.Core
{
    /// <summary>
    /// 资质类型
    /// </summary>
    public enum QualificationType
    {
        /// <summary>
        /// 维修保养检测
        /// </summary>
        WXBYJC,
        /// <summary>
        /// 消防安全评估
        /// </summary>
        XFAQPG
    }
    /// <summary>
    /// 资质等级
    /// </summary>
    public enum QualificationLevel
    {
        /// <summary>
        /// 一级
        /// </summary>
        YJ,
        /// <summary>
        /// 二级
        /// </summary>
        EJ,
        /// <summary>
        /// 三级
        /// </summary>
        SJ,
        /// <summary>
        /// 临时一级
        /// </summary>
        LSYJ,
        /// <summary>
        /// 临时二级
        /// </summary>
        LSEJ,
        /// <summary>
        /// 临时三级
        /// </summary>
        LSSJ
    }
    /// <summary>
    /// 项目类型
    /// </summary>
    public enum ProjectType
    {
        /// <summary>
        /// 竣工检测
        /// </summary>
        JGJC,
        /// <summary>
        /// 年度检测
        /// </summary>
        NDJC,
        /// <summary>
        /// 维护保养
        /// </summary>
        WHBY,
        /// <summary>
        /// 安全评估
        /// </summary>
        AQPG
    }
    /// <summary>
    /// 项目进度
    /// </summary>
    public enum ProjectProgress
    {
        /// <summary>
        /// 项目登记
        /// </summary>
        XMDJ,
        /// <summary>
        /// 入场检测
        /// </summary>
        RCJC,
        /// <summary>
        /// 出具报告
        /// </summary>
        CJBG,
        /// <summary>
        /// 提交备案
        /// </summary>
        TJBA
    }
    /// <summary>
    /// 十五个地区
    /// </summary>
    public enum City
    {
        /// <summary>
        /// 兰州
        /// </summary>
        LZ,
        /// <summary>
        /// 天水
        /// </summary>
        TS,
        /// <summary>
        /// 金昌
        /// </summary>
        JC,
        /// <summary>
        /// 白银
        /// </summary>
        BY,
        /// <summary>
        /// 武威
        /// </summary>
        WW,
        /// <summary>
        /// 平凉
        /// </summary>
        PL,
        /// <summary>
        /// 金昌
        /// </summary>
        JQ,
        /// <summary>
        /// 庆阳
        /// </summary>
        QY,
        /// <summary>
        /// 定西
        /// </summary>
        DX,
        /// <summary>
        /// 陇南
        /// </summary>
        LN,
        /// <summary>
        /// 临夏
        /// </summary>
        LX,
        /// <summary>
        /// 嘉峪关
        /// </summary>
        JYG,
        /// <summary>
        /// 甘南
        /// </summary>
        GN,
        /// <summary>
        /// 张掖
        /// </summary>
        ZY,
        /// <summary>
        /// 兰州新区
        /// </summary>
        LZXQ
    }
    /// <summary>
    /// 员工等级
    /// </summary>
    public enum StaffLevel
    {
        /// <summary>
        /// 一级注册消防工程师
        /// </summary>
        YJXFGCS,
        /// <summary>
        /// 二级注册消防工程师
        /// </summary>
        EJXFGCS,
        /// <summary>
        /// 建（构）筑物消防员（初级）
        /// </summary>
        CJXFY,
        /// <summary>
        /// 建（构）筑物消防员（中级）
        /// </summary>
        ZJXFY,
        /// <summary>
        /// 建（构）筑物消防员（高级）
        /// </summary>
        GJXFY
    }
}
