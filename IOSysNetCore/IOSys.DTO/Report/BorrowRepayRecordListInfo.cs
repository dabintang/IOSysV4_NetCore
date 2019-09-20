using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 借还明细查询结果
    /// </summary>
    public class BorrowRepayRecordListInfo : BaseListInfo
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

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
