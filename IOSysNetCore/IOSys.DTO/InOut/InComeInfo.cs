using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.InOut
{
    /// <summary>
    /// 收入
    /// </summary>
    public class InComeInfo : BaseInfo
    {
        /// <summary>
		/// 主键
		/// </summary>
		public int ID { get; set; }

        /// <summary>
		/// 收入日期
		/// </summary>
		public DateTime InDate { get; set; }

        /// <summary>
		/// 收入类型ID
		/// </summary>
		public int InTypeID { get; set; }

        /// <summary>
        /// 账户ID
        /// </summary>
        public int AmountAccountID { get; set; }

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
