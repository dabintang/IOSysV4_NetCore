using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOSys.Model
{
    /// <summary>
    /// 借还
    /// </summary>
    [Table("BorrowRepay")]
    public class BorrowRepay : IBaseIDModel
    {
        /// <summary>
		/// 主键
		/// </summary>
		public int ID { get; set; }

        /// <summary>
        /// 家庭ID
        /// </summary>
        public int FamilyID { get; set; }

        /// <summary>
		/// 借还日期
		/// </summary>
		public DateTime BRDate { get; set; }

        /// <summary>
        /// 对方名称
        /// </summary>
        [MaxLength(128)]
        [Required]
        public string Target { get; set; }

        /// <summary>
		/// 借还类型
        /// （1：借入；2：还入；3：借出；4：还出）
		/// </summary>
		public int BRType { get; set; }

        /// <summary>
		/// 账户ID
		/// </summary>
		public int AmountAccountID { get; set; }

        /// <summary>
		/// 金额
		/// </summary>
		public decimal Amount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1024)]
        [Required]
        public string Remark { get; set; }

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
