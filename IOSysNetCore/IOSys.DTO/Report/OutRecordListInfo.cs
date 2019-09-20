using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 支出明细查询结果
    /// </summary>
    public class OutRecordListInfo : BaseListInfo
    {
        /// <summary>
        /// 顺序
        /// </summary>
        public int Seq { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
		/// 支出日期
		/// </summary>
		public DateTime OutDate { get; set; }

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

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
