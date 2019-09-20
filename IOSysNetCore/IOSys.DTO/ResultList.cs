using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 列表结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultList<T> : IResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsOK { get; set; }

        /// <summary>
        /// 信息编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public List<T> LstInfo { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ResultList()
        {
            this.Code = string.Empty;
            this.Msg = string.Empty;
            this.LstInfo = new List<T>();
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="isOK">是否成功</param>
        /// <param name="msg">信息</param>
        /// <param name="lstInfo">结果</param>
        public ResultList(bool isOK, MsgInfo msg, List<T> lstInfo)
        {
            this.IsOK = isOK;
            this.Code = msg.Code;
            this.Msg = msg.Msg;
            this.LstInfo = lstInfo;
        }
    }
}
