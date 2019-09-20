using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 借还统计结果
    /// </summary>
    public class BorrowRepaySumListInfo<T> : SumListInfo<T>
    {
        /// <summary>
		/// 借还类型
        /// （1：借入；2：还入；3：借出；4：还出）
		/// </summary>
		public int BRType { get; set; }
    }
}
