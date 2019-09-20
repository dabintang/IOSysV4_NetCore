using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 分页列表
    /// </summary>
    public class PageList<T> : List<T>
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collection">列表内容</param>
        /// <param name="totalRecord">总条数</param>
        public PageList(IEnumerable<T> collection, int totalRecord) 
            : base(collection)
        {
            this.TotalRecord = totalRecord;
        }
    }
}
