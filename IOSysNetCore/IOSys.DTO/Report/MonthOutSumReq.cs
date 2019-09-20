using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 月份支出统计条件
    /// </summary>
    public class MonthOutSumReq : MonthSumReq
    {
        /// <summary>
        /// 月份
        /// </summary>
        public DateTime Month { get; set; }
    }
}
