using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOSys.BLL;
using IOSys.DTO;
using IOSys.DTO.APP;
using IOSys.Helper.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IOSys.API.Controllers
{
    /// <summary>
    /// APP管理
    /// </summary>
    public class AppController : BaseController
    {
        #region 变量

        private AppBLL _bll;
        /// <summary>
        /// APP管理 逻辑层
        /// </summary>
        private AppBLL bll
        {
            get
            {
                if (this._bll == null)
                {
                    this._bll = new AppBLL();
                    this._bll.LoginInfo = this.LoginInfo;
                    this._bll.Lang = this.Lang;
                }

                return this._bll;
            }
        }

        #endregion

        #region 接口

        /// <summary>
        /// 获取最新app版本信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAppVer")]
        [AllowAnonymous]
        public ResultInfo<AppVerInfo> GetAppVer()
        {
            var info = this.bll.GetAppVer();

            return new ResultInfo<AppVerInfo>(true, this.bll.Res.Gen.OK, info);
        }

        #endregion
    }
}