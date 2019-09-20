using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.Model
{
    /// <summary>
    /// 数据库表模型接口
    /// （主键为ID）
    /// </summary>
    public interface IBaseIDModel
    {
        /// <summary>
		/// 主键
		/// </summary>
		int ID { get; set; }

        /// <summary>
		/// 创建者ID
		/// </summary>
		int CreatorID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
		/// 更新者ID
		/// </summary>
		int? UpdateID { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime? UpdateTime { get; set; }
    }
}
