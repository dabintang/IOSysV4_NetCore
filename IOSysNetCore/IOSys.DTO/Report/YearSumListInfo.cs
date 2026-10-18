using IOSys.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 年度统计结果
    /// </summary>
    public class YearSumListInfo<T> : SumListInfo<T>
    {
        /// <summary>
		/// 数据类型
		/// </summary>
		public EnmDataType DataType { get; set; }

        /// <summary>
		/// 数据类型名称
		/// </summary>
		public string DataTypeName { get; set; }
    }
}
