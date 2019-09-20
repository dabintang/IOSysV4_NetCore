using IOSys.DTO.APP;
using IOSys.Helper.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace IOSys.BLL
{
    /// <summary>
    /// app管理
    /// </summary>
    public class AppBLL : BaseBLL
    {
        #region 公开方法

        /// <summary>
        /// 获取app版本信息
        /// </summary>
        /// <returns></returns>
        public AppVerInfo GetAppVer()
        {
            var info = new AppVerInfo();
            info.Ver = IOSysJson.Inst.AppConfig.AppVer.Ver;
            info.IsForce = IOSysJson.Inst.AppConfig.AppVer.IsForce;
            info.Path = IOSysJson.Inst.AppConfig.AppVer.Path;

            return info;
        }

        #endregion
    }
}
