using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Account
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : BaseInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; } = "";

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; } = "";

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; } = "";

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = "";

        /// <summary>
		/// 备注
		/// </summary>
        public string Remark { get; set; } = "";
    }
}
