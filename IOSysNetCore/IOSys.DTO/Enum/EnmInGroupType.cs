using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Enum
{
    /// <summary>
    /// 收入统计方式
    /// （1：类型；2：账户；3：月份；4：年度）
    /// </summary>
    public enum EnmInGroupType
    {
        /// <summary>
        /// 类型
        /// </summary>
        InType = 1,

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
