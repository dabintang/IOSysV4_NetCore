using IOSys.Helper.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOSys.Helper.Config
{
    /// <summary>
    /// 系统的json信息
    /// </summary>
    public class IOSysJson
    {
        #region 单例

        /// <summary>
        /// 构造函数
        /// </summary>
        private IOSysJson()
        {
        }

        private static object _lock = new object();
        private static IOSysJson _inst = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static IOSysJson Inst
        {
            get
            {
                if (_inst == null)
                {
                    lock (_lock)
                    {
                        if (_inst == null)
                        {
                            _inst = new IOSysJson();
                        }
                    }
                }

                return _inst;
            }
        }

        #endregion

        #region 变量

        /// <summary>
        ///系统配置信息
        /// </summary>
        public AppConfig AppConfig { get; set; }

        /// <summary>
        /// 提示信息（多语言）
        /// （key：语言类型，value：提示信息）
        /// </summary>
        internal Dictionary<string, ResourceConfig> DicMsg { get; set; }

        #endregion
    }
}
