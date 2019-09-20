using IOSys.API.Controllers;
using IOSys.API.Extensions;
using IOSys.DTO;
using IOSys.DTO.Account;
using IOSys.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOSys.API.Filters
{
    /// <summary>
    /// 授权过滤器特性
    /// </summary>
    public class AuthFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 验权
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //允许匿名
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            //验证结果
            ResultInfo<TokenInfo> ret = null;

            //获取token
            var token = ((HttpRequestHeaders)context.HttpContext.Request.Headers).HeaderAuthorization.FirstOrDefault();

            //token传入0时，判断是否允许此IP匿名访问
            if (token == "0")
            {
                //获取客户端IP
                var ip = context.HttpContext.GetClientIp();
                //是否允许匿名访问IP
                ret = ComHelper.AllowAnonymous(ip);
            }
            else
            {
                //JWT验证
                ret = JWTHelper.CheckToken<TokenInfo>(token, context.HttpContext.GetClientIp());
            }

            //验证不通过
            if (ret.IsOK == false)
            {
                LogHelper.Info($"访问接口[{context.HttpContext.Request.Path}]被拒绝（{ret.Msg}）");

                context.Result = new UnauthorizedResult();
                return;
            }

            //给控制器赋值token信息
            var baseCtl = context.Controller as BaseController;
            if (baseCtl != null)
            {
                baseCtl.LoginInfo = ret.Info;
            }
        }
    }
}
