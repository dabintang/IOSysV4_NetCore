using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Account
{
    /// <summary>
    /// 签名信息
    /// </summary>
    public class TokenInfo : BaseInfo
    {
        /// <summary>
		/// 用户ID
		/// </summary>
        public int UserID { get; set; }

        /// <summary>
		/// 家庭ID
		/// </summary>
		public int FamilyID { get; set; }
    }
}
