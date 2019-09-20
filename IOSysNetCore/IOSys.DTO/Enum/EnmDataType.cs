using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Enum
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum EnmDataType
    {
        /// <summary>
        /// 收入
        /// </summary>
        In = 1,

        /// <summary>
        /// 支出
        /// </summary>
        Out = 2,

        /// <summary>
        /// 转账
        /// </summary>
        Transfer = 3,

        /// <summary>
        /// 借还
        /// </summary>
        BorrowRepay = 4
    }
}
