using IOSys.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 账号流水列表信息
    /// </summary>
    public class AccountTurnoverListInfo : BaseListInfo
    {
        /// <summary>
        /// 数据类型
        /// </summary>
        public EnmDataType DataType { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        public string AmountAccountName { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
