using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 消息
    /// </summary>
    public class MsgInfo
    {
        /// <summary>
        /// 消息编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 拼接消息
        /// </summary>
        /// <param name="param">拼接参数</param>
        /// <returns></returns>
        public MsgInfo FormatMsg(params object[] param)
        {
            this.Msg = string.Format(this.Msg, param);
            return this;
        }
    }
}
