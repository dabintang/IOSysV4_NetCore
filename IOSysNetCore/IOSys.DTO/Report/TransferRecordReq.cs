using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 转账明细查询条件
    /// </summary>
    public class TransferRecordReq : PageBaseReq
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 源账户ID集合
        /// </summary>
        public List<int> LstFromAmountAccountID { get; set; }

        /// <summary>
        /// 目标账户ID集合
        /// </summary>
        public List<int> LstToAmountAccountID { get; set; }

        /// <summary>
        /// 备注（模糊匹配）
        /// </summary>
        public string Remark { get; set; }
    }
}
