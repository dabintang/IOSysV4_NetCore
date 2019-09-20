using IOSys.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace IOSys.DAL
{
    /// <summary>
    /// 数据层基类
    /// </summary>
    public class BaseDAL
    {
        #region 方法

        /// <summary>
        /// 设置排序和分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="req">条件</param>
        protected void SetSortPage<T>(ref IQueryable<T> q, PageBaseReq req)
        {
            //排序
            if (string.IsNullOrWhiteSpace(req.Sort) == false)
            {
                q = q.OrderBy(req.Sort);
            }

            //分页
            if (req.IsSetPage())
            {
                q = q.Skip(req.GetSkip()).Take(req.PageSize);
            }
        }

        #endregion
    }
}
