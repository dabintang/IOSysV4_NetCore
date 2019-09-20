using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOSys.BLL;
using IOSys.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IOSys.API.Controllers
{
    /// <summary>
    /// 工具
    /// </summary>
    public class ToolsController : BaseController
    {
        #region 变量

        private ToolsBLL _bll;
        /// <summary>
        /// 工具 逻辑层
        /// </summary>
        private ToolsBLL bll
        {
            get
            {
                if (this._bll == null)
                {
                    this._bll = new ToolsBLL();
                    this._bll.LoginInfo = this.LoginInfo;
                    this._bll.Lang = this.Lang;
                }

                return this._bll;
            }
        }

        #endregion

        #region 接口

        /// <summary>
        /// AES加密
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("EncryptAES")]
        [AllowAnonymous]
        public ResultInfo<string> EncryptAES(string text)
        {
            var encryptStr = this.bll.EncryptAES(text);

            return new ResultInfo<string>(true, this.bll.Res.Gen.OK, encryptStr);
        }

        #endregion
    }
}