using System;
using System.Collections.Generic;
using System.Text;

namespace TDB.AutoSimulator.ConsoleApp.DTO
{
    /// <summary>
    /// 模拟收支 请求类
    /// </summary>
    public class SimulateInOutReq
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}
