using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 借还明细查询条件
    /// </summary>
    public class BorrowRepayRecordReq : PageBaseReq
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
        /// 对方名称
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 借还类型集合
        /// </summary>
        public List<int> LstBRType { get; set; }

        /// <summary>
        /// 账户ID集合
        /// </summary>
        public List<int> LstAmountAccountID { get; set; }

        /// <summary>
        /// 备注（模糊匹配）
        /// </summary>
        public string Remark { get; set; }
    }
}
