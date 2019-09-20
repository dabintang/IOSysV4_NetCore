using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.Basic
{
    /// <summary>
    /// 支出分类-类型列表
    /// </summary>
    public class OutCategoryTypeListInfo : OutCategoryListInfo
    {
        /// <summary>
        /// 支出类型列表
        /// </summary>
        public List<OutTypeListInfo> LstOutType { get; set; } = new List<OutTypeListInfo>();
    }
}
