using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO
{
    /// <summary>
    /// 主键ID 作为请求条件
    /// </summary>
    public class IDReq : BaseReq
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
    }
}
