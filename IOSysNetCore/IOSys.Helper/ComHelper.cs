using IOSys.DTO;
using IOSys.DTO.Account;
using IOSys.Helper.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOSys.Helper
{
    /// <summary>
    /// 通用帮助类
    /// </summary>
    public class ComHelper
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static DateTime Now()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 上月末
        /// </summary>
        /// <returns></returns>
        public static DateTime PreMonthEnd()
        {
            var date = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01")).AddMilliseconds(-1);
            return date;
        }

        /// <summary>
        /// 本月初
        /// </summary>
        /// <returns></returns>
        public static DateTime CurMonthStart()
        {
            var date = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01"));
            return date;
        }

        /// <summary>
        /// 自动帮助拼接上程序根路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFullFileName(string fileName)
        {
            string appDataPath = AppDomain.CurrentDomain.BaseDirectory;
            string fullFileName = Path.Combine(appDataPath, fileName);

            return fullFileName;
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">需要复制的对象</param>
        /// <returns></returns>
        public static T Clone<T>(T obj) where T : class
        {
            if (obj == null)
            {
                return null;
            }

            var jsonStr = JsonConvert.SerializeObject(obj);
            var newObj = JsonConvert.DeserializeObject<T>(jsonStr);

            return newObj;
        }

        /// <summary>
        /// 是否允许此IP匿名访问
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public static ResultInfo<TokenInfo> AllowAnonymous(string ip)
        {
            var msgCfg = ConfigHelper.GetMsg("zh");

            if (IOSysJson.Inst.AppConfig.LstAnonymousIP == null || IOSysJson.Inst.AppConfig.LstAnonymousIP.Contains(ip) == false)
            {
                return new ResultInfo<TokenInfo>(false, msgCfg.Gen.IPError.FormatMsg(ip), null);
            }

            var info = new TokenInfo();
            info.UserID = 0;
            info.FamilyID = 0;

            var ret = new ResultInfo<TokenInfo>(true, msgCfg.Gen.TokenOK, info);
            return ret;
        }
    }
}
