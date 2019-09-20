using IOSys.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IOSys.DTO.Report
{
    /// <summary>
    /// 月份支出分类统计
    /// </summary>
    public class MonthOutCategorySumListInfo : BaseListInfo
    {
        /// <summary>
		/// 支出分类ID
		/// </summary>
		public int ID { get; set; }

        /// <summary>
		/// 支出分类名称
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public EnmDataType DataType { get; set; }

        /// <summary>
        /// 月份支出类型统计
        /// </summary>
        public List<MonthOutTypeSumListInfo> LstSumOutType { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount
        {
            get
            {
                if (this.LstSumOutType == null)
                {
                    return 0;
                }

                return this.LstSumOutType.Sum(m => m.Amount);
            }
        }
    } 
}
