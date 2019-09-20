using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 分页列表结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultPageList<T> : ResultList<T>
    {
        /// <summary>
        /// 返回的第一条记录位置（第几条）
        /// </summary>
        public int StartNum { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// 是否还有更多的记录
        /// </summary>
        public bool HasMore
        {
            get
            {
                return (this.StartNum + this.LstInfo.Count - 1) < this.TotalRecord;
            }
        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ResultPageList() 
            : base()
        {
            this.StartNum = 0;
            this.TotalRecord = 0;
        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="isOK">是否成功</param>
        /// <param name="msg">信息</param>
        /// <param name="lstInfo">结果</param>
        /// <param name="startNum">返回的第一条记录位置（第几条）</param>
        /// <param name="totalRecord">总条数</param>
        public ResultPageList(bool isOK, MsgInfo msg, List<T> lstInfo, int startNum, int totalRecord) 
            : base(isOK, msg, lstInfo)
        {
            this.StartNum = startNum;
            this.TotalRecord = totalRecord;
        }
    }
}
