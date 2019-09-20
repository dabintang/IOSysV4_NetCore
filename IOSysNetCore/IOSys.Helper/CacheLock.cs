using IOSys.Helper.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOSys.Helper
{
    /// <summary>
    /// 用缓存控制的锁
    /// </summary>
    public class CacheLock : IDisposable
    {
        #region 静态方法

        /// <summary>
        /// 按家庭上锁，执行方法
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="func">要执行的方法</param>
        /// <param name="maxWaitMinutes">最多等待分钟数</param>
        /// <returns></returns>
        public static async Task<LockResult<T>> DoByLockFamilyAsync<T>(int familyID, Func<Task<T>> func, int maxWaitMinutes = -1)
        {
            maxWaitMinutes = Math.Max(maxWaitMinutes, IOSysJson.Inst.AppConfig.WaiteLockMinute);

            //上锁
            using (var cacheLock = await CacheLock.LockFamily(familyID, maxWaitMinutes))
            {
                //超时
                if (cacheLock.IsLockedByOther)
                {
                    LogHelper.Info($"因等待时间过长，方法取消执行：{func.ToString()}");
                    return new LockResult<T>(false, default(T));
                }

                //执行方法
                var retFunc = await func();

                return new LockResult<T>(true, retFunc);
            }
        }

        /// <summary>
        /// 按家庭上锁，执行方法
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="func">要执行的方法</param>
        /// <param name="maxWaitMinutes">最多等待分钟数</param>
        /// <returns></returns>
        public static async Task<bool> DoByLockFamilyAsync(int familyID, Func<Task> func, int maxWaitMinutes = -1)
        {
            var ret = await DoByLockFamilyAsync(familyID, async () => { await func(); return true; }, maxWaitMinutes);

            return ret.IsDone;
        }

        /// <summary>
        /// 按家庭上锁，执行方法
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="func">要执行的方法</param>
        /// <param name="maxWaitMinutes">最多等待分钟数</param>
        /// <returns></returns>
        public static bool DoByLockFamily(int familyID, Action func, int maxWaitMinutes = -1)
        {
            var ret = DoByLockFamilyAsync(familyID, async () => await Task.Factory.StartNew(() => { func(); return true; }), maxWaitMinutes).Result;

            return ret.IsDone;
        }

        /// <summary>
        /// 按家庭上锁
        /// </summary>
        /// <param name="familyID">家庭ID</param>
        /// <param name="maxWaitMinutes">最多等待分钟数（默认1分钟）</param>
        /// <returns></returns>
        public static async Task<CacheLock> LockFamily(int familyID, int maxWaitMinutes = -1)
        {
            maxWaitMinutes = Math.Max(maxWaitMinutes, IOSysJson.Inst.AppConfig.WaiteLockMinute);

            var key = string.Format("{0}{1}", Key_Family, familyID);
            return await Lock(key, maxWaitMinutes);
        }

        /// <summary>
        /// 对key上锁
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="maxWaitMinutes">最多等待分钟数</param>
        /// <returns></returns>
        private static async Task<CacheLock> Lock(string key, int maxWaitMinutes)
        {
            DateTime startTime = DateTime.Now;

            var lockVal = CacheHelper.Get<string>(key);
            while (lockVal != null)
            {
                //超过等待分钟
                if ((DateTime.Now - startTime).TotalMinutes > maxWaitMinutes)
                {
                    return new CacheLock(key, true);
                }

                await Task.Delay(100);
                lockVal = CacheHelper.Get<string>(key);
            }

            CacheHelper.Set(key, "", MaxLockTime);
            return new CacheLock(key, false);
        }

        #endregion

        #region 变量/属性

        /// <summary>
        /// key
        /// </summary>
        private string Key { get; set; }

        /// <summary>
        /// 是否被他人锁着
        /// </summary>
        public bool IsLockedByOther { get; private set; }

        #endregion

        #region 常量

        /// <summary>
        /// 按家庭上锁key前缀
        /// </summary>
        public const string Key_Family = "Key_Family_";

        /// <summary>
        /// 最大上锁时间（10分钟）
        /// </summary>
        public const int MaxLockTime = 10;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="isLocked">是否被他人锁着</param>
        private CacheLock(string key, bool isLockedByOther)
        {
            this.Key = key;
            this.IsLockedByOther = isLockedByOther;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            CacheHelper.Remove(this.Key);
        }

        #endregion
    }

    /// <summary>
    /// 加锁执行的方法返回类型
    /// </summary>
    public class LockResult<T>
    {
        /// <summary>
        /// 是否已执行方法
        /// </summary>
        public bool IsDone { get; private set; }

        /// <summary>
        /// 结果
        /// </summary>
        public T Result { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isDone"></param>
        /// <param name="result"></param>
        public LockResult(bool isDone, T result)
        {
            this.IsDone = isDone;
            this.Result = result;
        }
    }
}
