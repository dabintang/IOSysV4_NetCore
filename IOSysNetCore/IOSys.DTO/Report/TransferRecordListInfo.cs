using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 转账明细查询结果
    /// </summary>
    public class TransferRecordListInfo : BaseListInfo
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
		/// 转账日期
		/// </summary>
		public DateTime TransferDate { get; set; }

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

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
