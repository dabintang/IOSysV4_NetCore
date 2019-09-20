using IOSys.Helper.Config;
using IOSys.Helper.Const;
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
    /// 配置帮助类
    /// </summary>
    public class ConfigHelper
    {
        #region 公共方法

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public static void Init()
        {
            //初始化系统配置信息
            InitAppConfig();

            //初始化提示信息（多语言）
            InitMsgConfig();
        }

        /// <summary>
        /// 获取提示信息
        /// </summary>
        /// <param name="lang">语音</param>
        /// <returns></returns>
        public static ResourceConfig GetMsg(string lang)
        {
            if (IOSysJson.Inst.DicMsg.ContainsKey(lang) == false)
            {
                lang = SysConst.Value.DefaultLang;
            }

            return IOSysJson.Inst.DicMsg[lang];
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化系统配置信息
        /// </summary>
        private static void InitAppConfig()
        {
            if (IOSysJson.Inst.AppConfig == null)
            {
                string fullFileName = ComHelper.GetFullFileName("JSON/App.json");
                string jsonText = File.ReadAllText(fullFileName);
                IOSysJson.Inst.AppConfig = JsonConvert.DeserializeObject<AppConfig>(jsonText);
            }
        }

        /// <summary>
        /// 初始化提示信息（多语言）
        /// </summary>
        private static void InitMsgConfig()
        {
            if (IOSysJson.Inst.DicMsg == null)
            {
                IOSysJson.Inst.DicMsg = new Dictionary<string, ResourceConfig>();

                var path = ComHelper.GetFullFileName("JSON");
                var arrFullFileName = Directory.GetFiles(path, "resource-*.json");
                foreach (var fullFileName in arrFullFileName)
                {
                    var jsonText = File.ReadAllText(fullFileName);
                    var msgConfig = JsonConvert.DeserializeObject<ResourceConfig>(jsonText);

                    var fileName = Path.GetFileName(fullFileName);
                    //"resource-".Length=9   ".json".Length=5
                    var key = fileName.ToLower().Substring(9, fileName.Length - 14);

                    IOSysJson.Inst.DicMsg[key] = msgConfig;
                }
            }
        }

        #endregion
    }
}
