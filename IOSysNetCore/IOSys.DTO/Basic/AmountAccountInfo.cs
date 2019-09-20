using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Basic
{
    /// <summary>
    /// 账户
    /// </summary>
    public class AmountAccountInfo : BaseInfo
    {
        /// <summary>
		/// 主键
		/// </summary>
		public int ID { get; set; }

        /// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
		/// 是否可用
		/// </summary>
		public bool IsActive { get; set; }

        /// <summary>
		/// 备注
		/// </summary>
		public string Remark { get; set; } = "";
    }
}
