using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Enum
{
    /// <summary>
    /// 支出统计方式
    /// （0：分类；1：类型；2：账户；3：月份；4：年度）
    /// </summary>
    public enum EnmOutGroupType
    {
        /// <summary>
        /// 分类
        /// </summary>
        OutCategory = 0,

        /// <summary>
        /// 类型
        /// </summary>
        OutType = 1,

        /// <summary>
        /// 账户
        /// </summary>
        AmountAccount = 2,

        /// <summary>
        /// 月份
        /// </summary>
        Month = 3,

        /// <summary>
        /// 年度
        /// </summary>
        Year = 4,
    }
}
