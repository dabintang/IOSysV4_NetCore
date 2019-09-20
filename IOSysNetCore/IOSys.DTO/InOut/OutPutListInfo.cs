using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.InOut
{
    /// <summary>
    /// 支出列表
    /// </summary>
    public class OutPutListInfo : BaseListInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
		/// 支出分类名称
		/// </summary>
		public string OutCategoryName { get; set; }

        /// <summary>
		/// 支出类型名称
		/// </summary>
		public string OutTypeName { get; set; }

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
