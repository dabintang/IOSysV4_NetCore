using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TDB.AutoSimulator.ConsoleApp.Configs
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class AppConfig
    {
        #region 单例

        /// <summary>
        /// 构造函数
        /// </summary>
        private AppConfig()
        {
            string fullFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs/App.json");
            string jsonText = File.ReadAllText(fullFileName);
            this.App = JsonConvert.DeserializeObject<AppConfigInfo>(jsonText);
        }

        private static object _lock = new object();
        private static AppConfig _inst = null;
        /// <summary>
        /// 单例
        /// </summary>
        public static AppConfig Inst
        {
            get
            {
                if (_inst == null)
                {
                    lock (_lock)
                    {
                        if (_inst == null)
                        {
                            _inst = new AppConfig();
                        }
                    }
                }

                return _inst;
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 系统配置
        /// </summary>
        public AppConfigInfo App { get; private set; }

        #endregion
    }
}
