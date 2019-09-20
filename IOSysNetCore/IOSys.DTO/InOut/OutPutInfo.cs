using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.InOut
{
    /// <summary>
    /// 支出信息
    /// </summary>
    public class OutPutInfo : BaseInfo
    {
        /// <summary>
		/// 主键
		/// </summary>
		public int ID { get; set; }

        /// <summary>
		/// 支出日期
		/// </summary>
		public DateTime OutDate { get; set; }

        /// <summary>
        /// 支出类型ID
        /// </summary>
        public int OutTypeID { get; set; }

        /// <summary>
		/// 账户
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
