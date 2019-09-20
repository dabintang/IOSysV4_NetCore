using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.InOut
{
    /// <summary>
    /// 收入列表
    /// </summary>
    public class InComeListInfo : BaseListInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
		/// 收入类型名称
		/// </summary>
		public string InTypeName { get; set; }

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
