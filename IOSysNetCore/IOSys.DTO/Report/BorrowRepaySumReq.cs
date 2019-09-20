using IOSys.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 借还统计条件
    /// </summary>
    public class BorrowRepaySumReq : BaseReq
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
        /// 借还类型集合
        /// （1：借入；2：还入；3：借出；4：还出）
        /// </summary>
        public List<int> LstBRType { get; set; } = new List<int>();

        /// <summary>
        /// 统计类型
        /// （0：对方；1：类型；2：账户；3：月份；4：年度；）
        /// </summary>
        public EnmBorrowRepayGroupType GroupType { get; set; }

        /// <summary>
        /// 是否显示已还清记录
        /// </summary>
        public bool IsShowZero { get; set; }
    }
}
