using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Enum
{
    /// <summary>
    /// 借还统计方式
    /// （0：对方；1：类型；2：账户；3：月份；4：年度；）
    /// </summary>
    public enum EnmBorrowRepayGroupType
    {
        /// <summary>
        /// 对方
        /// </summary>
        Target = 0,

        /// <summary>
        /// 类型
        /// </summary>
        BRType = 1,

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
        Year = 4
    }
}
