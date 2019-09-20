using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 月份合计金额的分页结果
    /// </summary>
    public class ResultPageMonthAmountList<T> : ResultList<T>
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
        /// <param name="isOK">是否成功</param>
        /// <param name="msg">信息</param>
        /// <param name="lstInfo">结果</param>
        /// <param name="month">月份</param>
        /// <param name="totalAmount">合计金额</param>
        /// <param name="hasPreMonth">是否还有前一个月的数据</param>
        public ResultPageMonthAmountList(bool isOK, MsgInfo msg, List<T> lstInfo, DateTime month, decimal totalAmount, bool hasPreMonth)
            : base(isOK, msg, lstInfo)
        {
            this.Month = month;
            this.TotalAmount = totalAmount;
            this.HasPreMonth = hasPreMonth;
        }
    }
}
