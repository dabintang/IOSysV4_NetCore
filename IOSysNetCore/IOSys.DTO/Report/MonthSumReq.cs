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
        /// 年份（可空；为空时统计全部年份，不为空时只统计指定年份的月份数据）
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// 是否包含借还
        /// </summary>
        public bool IsContainBorrowRepay { get; set; }
    }
}
