using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 带合计金额的分页列表
    /// </summary>
    public class PageAmountList<T> : PageList<T>
    {
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collection">列表内容</param>
        /// <param name="totalRecord">总条数</param>
        /// <param name="totalAmount">总金额</param>
        public PageAmountList(IEnumerable<T> collection, int totalRecord, decimal totalAmount) 
            : base(collection, totalRecord)
        {
            this.TotalAmount = totalAmount;
        }
    }
}
