using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOSys.Helper.Config
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Token超时时间（单位：天）
        /// </summary>
        public int TokenTimeOutDay { get; set; }

        ///// <summary>
        ///// 系统URL
        ///// </summary>
        //public string SysUrl { get; set; }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string DBConnStr { get; set; }

        /// <summary>
        /// 允许跨域访问的地址
        /// </summary>
        public List<string> LstWithOrigins { get; set; }

        /// <summary>
        /// 允许匿名访问的IP
        /// </summary>
        public List<string> LstAnonymousIP { get; set; }

        /// <summary>
        /// 数据库备份路径
        /// </summary>
        public string DBBackupPath { get; set; }

        /// <summary>
        /// 数据库备份接收邮箱
        /// </summary>
        public string DBBackupToEmail { get; set; }

        /// <summary>
        /// 默认等待解锁超时时间（单位：分钟）
        /// </summary>
        public int WaiteLockMinute { get; set; }

        /// <summary>
        /// email配置
        /// </summary>
        public EmailConfig Email { get; set; }

        /// <summary>
        /// APP版本信息
        /// </summary>
        public AppVerConfig AppVer { get; set; }
    }

    /// <summary>
    /// APP版本信息
    /// </summary>
    public class AppVerConfig
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

    /// <summary>
    /// email配置
    /// </summary>
    public class EmailConfig
    {
        /// <summary>
        /// SMTP 服务器地址
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// SMTP 服务器端口
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// SMTP密码
        /// </summary>
        public string SmtpPwd { get; set; }

        /// <summary>
        /// 发件人名称
        /// </summary>
        public string FromEmailName { get; set; }

        /// <summary>
        /// 发件人邮箱
        /// </summary>
        public string FromEmail { get; set; }
    }
}
