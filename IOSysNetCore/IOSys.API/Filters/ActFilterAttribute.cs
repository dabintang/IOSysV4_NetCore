using IOSys.API.Controllers;
using IOSys.API.Extensions;
using IOSys.Helper.Const;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOSys.API.Filters
{
    /// <summary>
    /// 行为过滤器特性(1)
    /// </summary>
    public class ActFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 接口执行前
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //使用语音
            var lang = context.HttpContext.GetHeaderValue(SysConst.Key.Lang);
            if (string.IsNullOrWhiteSpace(lang))
            {
                //默认使用中文
                lang = SysConst.Value.DefaultLang;
            }

            //给控制器赋值使用语音
            var baseCtl = context.Controller as BaseController;
            if (baseCtl != null)
            {
                baseCtl.Lang = lang;
            }

            //判断是否需要些日志
            var attrLog = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(m => m is ParamsLogAttribute) as ParamsLogAttribute;
            if (attrLog != null)
            {
                attrLog.WriteLogIn(context);
            }

            base.OnActionExecuting(context);
        }

        ///// <summary>
        ///// 接口执行后(2)
        ///// </summary>
        ///// <param name="context"></param>
        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    base.OnActionExecuted(context);

        //    //判断是否需要些日志
        //    var attrLog = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(m => m is ParamsLogAttribute) as ParamsLogAttribute;
        //    if (attrLog != null)
        //    {
        //        attrLog.WriteLogOut(context);
        //    }
        //}

        ///// <summary>
        ///// (3)
        ///// </summary>
        ///// <param name="context"></param>
        //public override void OnResultExecuting(ResultExecutingContext context)
        //{
        //    base.OnResultExecuting(context);
        //}

        /// <summary>
        /// 接口执行返回结果后(4)
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);

            //判断是否需要些日志
            var attrLog = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(m => m is ParamsLogAttribute) as ParamsLogAttribute;
            if (attrLog != null)
            {
                attrLog.WriteLogOut(context);
            }
        }
    }
}
