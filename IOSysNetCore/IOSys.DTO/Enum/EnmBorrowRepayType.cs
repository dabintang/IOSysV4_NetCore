using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Enum
{
    /// <summary>
    /// 借还类型
    /// （1：借入；2：还入；3：借出；4：还出）
    /// </summary>
    public enum EnmBorrowRepayType
    {
        /// <summary>
        /// 借入
        /// </summary>
        BorrowIn = 1,

        /// <summary>
        /// 还入
        /// </summary>
        RepayIn = 2,

        /// <summary>
        /// 借出
        /// </summary>
        BorrowOut = 3,

        /// <summary>
        /// 还出
        /// </summary>
        RepayOut = 4,
    }
}
