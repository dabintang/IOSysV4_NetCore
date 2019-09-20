using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 用户总计信息
    /// </summary>
    public class UserTotalInfo : BaseInfo
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 家庭名称
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// 总资产
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 当前年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 当前年总收入
        /// </summary>
        public decimal TotalInCurYear { get; set; }

        /// <summary>
        /// 当前年总支出
        /// </summary>
        public decimal TotalOutCurYear { get; set; }

        /// <summary>
        /// 当前年总借入/还入
        /// </summary>
        public decimal TotalBRInCurYear { get; set; }

        /// <summary>
        /// 当前年总借出/还出
        /// </summary>
        public decimal TotalBROutCurYear { get; set; }

        /// <summary>
        /// 当前月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 当前月总收入
        /// </summary>
        public decimal TotalInCurMonth { get; set; }

        /// <summary>
        /// 当前月总支出
        /// </summary>
        public decimal TotalOutCurMonth { get; set; }

        /// <summary>
        /// 当前月总借入/还入
        /// </summary>
        public decimal TotalBRInCurMonth { get; set; }

        /// <summary>
        /// 当前月总借出/还出
        /// </summary>
        public decimal TotalBROutCurMonth { get; set; }
    }
}
