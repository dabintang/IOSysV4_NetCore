using System;
using System.Collections.Generic;
using System.Text;

namespace TDB.AutoSimulator.ConsoleApp.Configs
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class AppConfigInfo
    {
        /// <summary>
        /// 收支系统api地址
        /// </summary>
        public string IOApiUrl { get; set; }

        /// <summary>
        /// 允许跨域访问的地址
        /// </summary>
        public List<string> LstWithOrigins { get; set; }

        /// <summary>
        /// 收支系统默认用户登录名
        /// </summary>
        public string IOLoginName { get; set; }

        /// <summary>
        /// 收支系统默认用户密码
        /// </summary>
        public string IOLoginPwd { get; set; }
    }
}
