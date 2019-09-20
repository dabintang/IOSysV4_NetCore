using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// //账号流水 请求类
    /// </summary>
    public class AccountTurnoverListReq : BaseReq
    {
        /// <summary>
        /// 月份
        /// </summary>
        public DateTime Month { get; set; }

        /// <summary>
        /// 账户ID集合
        /// </summary>
        public List<int> LstAmountAccountID { get; set; }
    }
}
