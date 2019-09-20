using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Account
{
    /// <summary>
    /// 修改密码参数
    /// </summary>
    public class ChangePwdReq : BaseReq
    {
        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPassword { get; set; }
    }
}
