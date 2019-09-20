
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.InOut
{
    /// <summary>
    /// 转账列表
    /// </summary>
    public class TransferListInfo : BaseListInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
		/// 源账户名称
		/// </summary>
		public string FromAmountAccountName { get; set; }

        /// <summary>
        /// 目标账户名称
        /// </summary>
        public string ToAmountAccountName { get; set; }

        /// <summary>
		/// 金额
		/// </summary>
		public decimal Amount { get; set; }
    }
}
