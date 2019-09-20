using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOSys.DTO.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IOSys.API.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        #region 变量

        /// <summary>
        /// token附带的登录信息
        /// </summary>
        public TokenInfo LoginInfo { get; internal set; }

        /// <summary>
        /// 使用语音
        /// </summary>
        public string Lang { get; internal set; }

        #endregion
    }
}