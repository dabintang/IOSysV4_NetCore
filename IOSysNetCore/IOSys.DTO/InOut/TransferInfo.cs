using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.InOut
{
    /// <summary>
    /// 转账信息
    /// </summary>
    public class TransferInfo : BaseInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 转账日期
        /// </summary>
        public DateTime TransferDate { get; set; }

        /// <summary>
        /// 源账户ID
        /// </summary>
        public int FromAmountAccountID { get; set; }

        /// <summary>
        /// 目标账户ID
        /// </summary>
        public int ToAmountAccountID { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }
}
