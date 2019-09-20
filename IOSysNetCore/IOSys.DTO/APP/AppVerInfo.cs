using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.DTO.APP
{
    /// <summary>
    /// app版本信息
    /// </summary>
    public class AppVerInfo : BaseInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Ver { get; set; }

        /// <summary>
        /// 是否强制更新
        /// </summary>
        public bool IsForce { get; set; }

        /// <summary>
        /// APP安装包路径
        /// </summary>
        public string Path { get; set; }
    }
}
