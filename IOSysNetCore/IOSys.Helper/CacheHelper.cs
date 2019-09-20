using IOSys.Helper.Inner;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.Helper
{
    /// <summary>
    /// 缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="timeOutMinute">过期分钟数（默认30分钟）</param>
        public static void Set<T>(string key, T value, int timeOutMinute = 30)
        {
            MSCache.Inst.Set<T>(key, value, timeOutMinute);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <param name="keyParam">key参数</param>
        /// <returns></returns>
        public static T Get<T>(string key, params object[] keyParam)
        {
            var fullKey = string.Format(key, keyParam);
            return MSCache.Inst.Get<T>(fullKey);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">key</param>
        public static void Remove(string key)
        {
            MSCache.Inst.Remove(key);
        }
    }
}
