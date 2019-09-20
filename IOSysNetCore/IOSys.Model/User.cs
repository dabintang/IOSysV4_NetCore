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
    /// 用户
    /// </summary>
    [Table("User")]
    public class User : IBaseIDModel
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
        /// 登录名
        /// </summary>
        [MaxLength(64)]
        [Required]
        public string LoginName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(64)]
        [Required]
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [MaxLength(256)]
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [MaxLength(64)]
        [Required]
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(64)]
        [Required]
        public string Email { get; set; }
        
        /// <summary>
		/// 是否可用
		/// </summary>
		public bool IsActive{get;set;}

        /// <summary>
		/// 是否删除
		/// </summary>
		public bool IsDelete { get; set; }

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
        public DateTime CreateTime{get;set;}

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