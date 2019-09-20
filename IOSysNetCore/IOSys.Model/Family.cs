using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IOSys.Model
{
    /// <summary>
    /// 家庭
    /// </summary>
    [Table("Family")]
	public class Family : IBaseIDModel
    {
		/// <summary>
        /// 主键
		/// </summary>
		public int ID{get;set;}

        /// <summary>
        /// 家庭名称
        /// </summary>
        [MaxLength(64)]
        [Required]
        public string Name{get;set;}

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1024)]
        [Required]
        public string Remark{get;set;}

        /// <summary>
        /// 创建者ID
        /// </summary>
        public int CreatorID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
		/// 更新者ID
		/// </summary>
		public int? UpdateID { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}