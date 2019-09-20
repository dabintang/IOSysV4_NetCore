using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IOSys.Helper
{
    /// <summary>
    /// 转换帮助类
    /// </summary>
    public class CvtHelper
    {
        /// <summary>
        /// 对象转换为字典
        /// </summary>
        /// <param name="obj">待转化的对象</param>
        /// <param name="isIgnoreNull">是否忽略NULL</param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(object obj, bool isIgnoreNull)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();

            Type t = obj.GetType(); // 获取对象对应的类， 对应的类型
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance); // 获取当前type公共属性

            foreach (PropertyInfo p in pi)
            {
                MethodInfo m = p.GetGetMethod();

                if (m != null && m.IsPublic)
                {
                    var value = m.Invoke(obj, new object[] { });

                    // 进行判NULL处理 
                    if (value != null || !isIgnoreNull)
                    {
                        map.Add(p.Name, value); // 向字典添加元素
                    }
                }
            }
            return map;
        }

        /// <summary>
        /// 转UTC时间戳，单位（秒）
        /// </summary>
        /// <param name="clientTime">本地时间</param>
        /// <returns></returns>
        public static long ToUTCSecond(DateTime clientTime)
        {
            var startTime = TimeZoneInfo.ConvertTimeFromUtc(new System.DateTime(1970, 1, 1, 0, 0, 0, 0), TimeZoneInfo.Local);
            var secondsSinceEpoch = Convert.ToInt64((clientTime - startTime).TotalSeconds);

            return secondsSinceEpoch;
        }

        /// <summary>
        /// 天数转秒数
        /// </summary>
        /// <param name="day">天数</param>
        /// <returns></returns>
        public static long DayToSecond(int day)
        {
            return day * 86400;
        }

        /// <summary>
        /// 转字符串
        /// （如为null，返回空字符串）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStr<T>(T value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return Convert.ToString(value);
        }
    }
}
