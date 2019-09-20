using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 单个结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultInfo<T> : IResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsOK { get; set; }

        /// <summary>
        /// 消息编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public T Info { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ResultInfo()
        {
            this.Code = string.Empty;
            this.Msg = string.Empty;
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="isOK">是否成功</param>
        /// <param name="msg">信息</param>
        /// <param name="info">结果</param>
        public ResultInfo(bool isOK, MsgInfo msg, T info)
        {
            this.IsOK = isOK;
            this.Code = msg.Code;
            this.Msg = msg.Msg;
            this.Info = info;
        }
    }
}
