using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Account
{
    /// <summary>
    /// 登录参数
    /// </summary>
    public class LoginReq : BaseReq
    {
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
