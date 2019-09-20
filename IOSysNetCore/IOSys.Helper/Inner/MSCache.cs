using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.Helper.Inner
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    class MSCache
    {
        #region 单例

        /// <summary>
        /// 构造函数
        /// </summary>
        private MSCache()
        {
        }

        private static object _lock = new object();
        private static MSCache _inst = null;
        /// <summary>
        /// 单例
        /// </summary>
        internal static MSCache Inst
        {
            get
            {
                if (_inst == null)
                {
                    lock (_lock)
                    {
                        if (_inst == null)
                        {
                            _inst = new MSCache();
                        }
                    }
                }

                return _inst;
            }
        }

        #endregion

        #region 变量

        /// <summary>
        /// 内存缓存
        /// </summary>
        private MemoryCache cache = new MemoryCache(Options.Create(
            new MemoryCacheOptions()
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(10) //过期扫描频 10分钟
            }));

        #endregion

        #region 公共方法

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="timeOutMinute">过期分钟数（默认30分钟）</param>
        public void Set<T>(string key, T value, int timeOutMinute = 30)
        {
            var option = new MemoryCacheEntryOptions();
            option.SlidingExpiration = TimeSpan.FromMinutes(timeOutMinute);

            this.cache.Set(key, value, option);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return this.cache.Get<T>(key);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">key</param>
        public void Remove(string key)
        {
            this.cache.Remove(key);
        }

        #endregion
    }
}
