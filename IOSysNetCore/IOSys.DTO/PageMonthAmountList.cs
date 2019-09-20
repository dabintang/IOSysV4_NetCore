using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 月份合计金额的分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageMonthAmountList<T> : List<T>
    {
        /// <summary>
        /// 月份
        /// </summary>
        public DateTime Month { get; set; }

        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 是否还有前一个月的数据
        /// </summary>
        public bool HasPreMonth { get; set; }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="collection">列表内容</param>
        /// <param name="month">月份</param>
        /// <param name="totalAmount">合计金额</param>
        /// <param name="hasPreMonth">是否还有前一个月的数据</param>
        public PageMonthAmountList(IEnumerable<T> collection, DateTime month, decimal totalAmount, bool hasPreMonth)
            : base(collection)
        {
            this.Month = month;
            this.TotalAmount = totalAmount;
            this.HasPreMonth = hasPreMonth;
        }
    }
}
