using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 带合计金额的分页结果
    /// </summary>
    public class ResultPageAmountList<T> : ResultPageList<T>
    {
        /// <summary>
        /// 合计金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ResultPageAmountList()
            : base()
        {
            this.TotalAmount = 0;
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="isOK">是否成功</param>
        /// <param name="msg">信息</param>
        /// <param name="lstInfo">结果</param>
        /// <param name="startNum">返回的第一条记录位置（第几条）</param>
        /// <param name="totalRecord">总条数</param>
        /// <param name="totalAmount">合计金额</param>
        public ResultPageAmountList(bool isOK, MsgInfo msg, List<T> lstInfo, int startNum, int totalRecord, decimal totalAmount)
            : base(isOK, msg, lstInfo, startNum, totalRecord)
        {
            this.TotalAmount = totalAmount;
        }
    }
}
