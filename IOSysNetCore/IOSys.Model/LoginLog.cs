using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IOSys.Model
{
    /// <summary>
    /// 登录日志
    /// </summary>
    [Table("LoginLog")]
    public class LoginLog
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
		/// 用户ID
		/// </summary>
        public int UserID { get; set; }

        /// <summary>
		/// token
		/// </summary>
        [MaxLength(1024)]
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// 客户的IP
        /// </summary>
        [MaxLength(64)]
        [Required]
        public string IP { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
    }
}
