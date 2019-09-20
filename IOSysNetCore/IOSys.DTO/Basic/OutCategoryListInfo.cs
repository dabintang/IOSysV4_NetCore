using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Basic
{
    /// <summary>
    /// 支出分类列表
    /// </summary>
    public class OutCategoryListInfo : BaseListInfo
    {
        /// <summary>
		/// 主键
		/// </summary>
		public int ID { get; set; }

        /// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }

        /// <summary>
		/// 是否可用
		/// </summary>
		public bool IsActive { get; set; }
    }
}
