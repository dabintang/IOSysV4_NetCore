using IOSys.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 支出统计条件
    /// </summary>
    public class OutSumReq : BaseReq
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
        /// 统计类型
        /// （0：分类；1：类型；2：账户；3：月份；4：年度）
        /// </summary>
        public EnmOutGroupType GroupType { get; set; }

        /// <summary>
        /// 是否包含借还
        /// </summary>
        public bool IsContainBorrowRepay { get; set; }
    }
}
