using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 月份支出类型统计
    /// </summary>
    public class MonthOutTypeSumListInfo : BaseListInfo
    {
        /// <summary>
        /// 支出类型ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
		/// 支出类型名称
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
