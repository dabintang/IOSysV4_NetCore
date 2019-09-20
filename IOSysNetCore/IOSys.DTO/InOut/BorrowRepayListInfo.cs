using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.InOut
{
    /// <summary>
    /// 借还列表
    /// </summary>
    public class BorrowRepayListInfo : BaseListInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 对方名称
        /// </summary>
        public string Target { get; set; }

        /// <summary>
		/// 借还类型名称
		/// </summary>
		public string BRTypeName { get; set; }

        /// <summary>
		/// 账户名称
		/// </summary>
		public string AmountAccountName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
