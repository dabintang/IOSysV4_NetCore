using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IOSys.Helper
{
    /// <summary>
    /// 上锁帮助类
    /// </summary>
    public class LockHelper
    {
        /// <summary>
        /// 对key上锁，如果已有锁，则等待之前的锁解锁都在上锁
        /// </summary>
        /// <param name="key"></param>
        public static void Lock(string key)
        {
            var lockObj = CacheHelper.Get<object>(key);
            while(lockObj != null)
            {
                Thread.Sleep(10);
                lockObj = CacheHelper.Get<object>(key);
            }

            CacheHelper.Set<object>(key, new object());
        }

        /// <summary>
        /// 对key解锁
        /// </summary>
        /// <param name="key"></param>
        public static void Unlock(string key)
        {
            CacheHelper.Remove(key);
        }
    }
}
