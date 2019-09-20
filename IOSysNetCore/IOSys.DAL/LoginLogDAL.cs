using IOSys.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IOSys.DAL
{
    /// <summary>
    /// 登录日志 数据层
    /// </summary>
    public class LoginLogDAL : BaseDAL
    {
        #region 单例

        /// <summary>
        /// 构造函数
        /// </summary>
        private LoginLogDAL()
        {
        }

        private static object _lock = new object();
        private static LoginLogDAL _inst = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static LoginLogDAL Inst
        {
            get
            {
                if (_inst == null)
                {
                    lock (_lock)
                    {
                        if (_inst == null)
                        {
                            _inst = new LoginLogDAL();
                        }
                    }
                }

                return _inst;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 异步添加登录日志
        /// </summary>
        /// <param name="log">登录日志</param>
        /// <returns></returns>
        public async Task AddLogAsync(LoginLog log)
        {
            using (var db = new IOSysContext())
            {
                await db.LoginLogs.AddAsync(log);

                await db.SaveChangesAsync();
            }
        }

        #endregion
    }
}
