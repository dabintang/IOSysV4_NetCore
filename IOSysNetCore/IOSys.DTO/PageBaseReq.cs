using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 分页请求类 基类
    /// </summary>
    public class PageBaseReq : BaseReq
    {
        /// <summary>
        /// 获取页码
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 获取跳过多少条
        /// </summary>
        public int GetSkip()
        {
            int skip = (this.PageNum - 1) * this.PageSize;
            skip = Math.Max(skip, 0);
            return skip;
        }

        /// <summary>
        /// 是否设置分页
        /// </summary>
        /// <returns></returns>
        public bool IsSetPage()
        {
            return (this.PageSize > 0 && this.PageSize < Int32.MaxValue);
        }
    }
}
