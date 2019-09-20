using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOSys.API.Extensions;
using IOSys.API.Filters;
using IOSys.BLL;
using IOSys.DTO;
using IOSys.DTO.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IOSys.API.Controllers
{
    /// <summary>
    /// 账号
    /// </summary>
    public class AccountController : BaseController
    {
        #region 变量

        private AccountBLL _bll;
        /// <summary>
        /// 账号 逻辑层
        /// </summary>
        private AccountBLL bll
        {
            get
            {
                if (this._bll == null)
                {
                    this._bll = new AccountBLL();
                    this._bll.LoginInfo = this.LoginInfo;
                    this._bll.Lang = this.Lang;
                }

                return this._bll;
            }
        }

        #endregion

        #region 接口

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="req">登录参数</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<ResultInfo<LoginInfo>> LoginAsync([FromBody]LoginReq req)
        {
            //获取客户端IP
            var ip = this.HttpContext.GetClientIp();

            return await this.bll.LoginAsync(req, ip);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCurrentUser")]
        public async Task<ResultInfo<UserInfo>> GetCurrentUserAsync()
        {
            var item = await this.bll.GetUserAsync(this.LoginInfo.UserID);
            return new ResultInfo<UserInfo>(true, this.bll.Res.Gen.OK, item);
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="info">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveUser")]
        [ParamsLog("保存用户")]
        public async Task<ResultInfo<int>> SaveUserAsync([FromBody]UserInfo info)
        {
            var result = await this.bll.SaveUserAsync(info);
            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="req">参数</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ChangePassword")]
        [ParamsLog("修改密码")]
        public async Task<ResultInfo<bool>> ChangePassword([FromBody]ChangePwdReq req)
        {
            var result = await this.bll.ChangePassword(req);
            return result;
        }

        #endregion
    }
}