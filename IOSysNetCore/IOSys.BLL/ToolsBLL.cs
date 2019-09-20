using IOSys.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.BLL
{
    /// <summary>
    /// 工具 逻辑层
    /// </summary>
    public class ToolsBLL : BaseBLL
    {
        #region 公开方法

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string EncryptAES(string text)
        {
            var encryptStr = EncryptHelper.EncryptAES(text);
            return encryptStr;
        }

        #endregion
    }
}
