using IOSys.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 年度统计条件
    /// </summary>
    public class YearSumReq : BaseReq
    {
        /// <summary>
        /// 是否包含借还
        /// </summary>
        public bool IsContainBorrowRepay { get; set; }
    }
}
