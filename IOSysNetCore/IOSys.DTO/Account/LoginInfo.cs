using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Account
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginInfo : BaseInfo
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 家庭名称
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public string Token { get; set; }
    }
}
