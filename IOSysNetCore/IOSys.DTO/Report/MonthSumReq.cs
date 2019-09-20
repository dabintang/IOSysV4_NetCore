using IOSys.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 月份统计条件
    /// </summary>
    public class MonthSumReq : BaseReq
    {
        /// <summary>
        /// 是否包含借还
        /// </summary>
        public bool IsContainBorrowRepay { get; set; }
    }
}
