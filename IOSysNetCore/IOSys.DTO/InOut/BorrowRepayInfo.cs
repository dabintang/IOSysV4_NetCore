using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.InOut
{
    /// <summary>
    /// 借还
    /// </summary>
    public class BorrowRepayInfo : BaseInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
		/// 借还日期
		/// </summary>
		public DateTime BRDate { get; set; }

        /// <summary>
        /// 对方名称
        /// </summary>
        public string Target { get; set; }

        /// <summary>
		/// 借还类型
        /// （1：借入；2：还入；3：借出；4：还出）
		/// </summary>
		public int BRType { get; set; }

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
