using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 统计结果
    /// </summary>
    public class SumListInfo<T> : BaseListInfo
    {
        /// <summary>
        /// 分组名
        /// </summary>
        public T name { get; set; }
          
        /// <summary>
        /// 结果
        /// </summary>
        public decimal value { get; set; }
    }
}
