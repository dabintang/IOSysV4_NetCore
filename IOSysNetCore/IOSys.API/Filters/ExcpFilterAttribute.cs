using IOSys.DTO;
using IOSys.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IOSys.API.Filters
{
    /// <summary>
    /// 异常过滤器特性
    /// </summary>
    public class ExcpFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            //如果异常以及被处理过了，不再处理
            if (context.ExceptionHandled)
            {
                return;
            }

            //写日志
            var msg = string.Format("未知异常（Action：{0}）", context.ActionDescriptor.DisplayName);
            LogHelper.Error(context.Exception, msg);

            //获取接口返回类型
            //var returnType = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.ReturnType.GenericTypeArguments.FirstOrDefault();
            var returnType = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.ReturnType;
            if (returnType != null && (returnType.BaseType == typeof(Task) || returnType == typeof(Task)))
            {
                returnType = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.ReturnType.GenericTypeArguments.FirstOrDefault();
            }

            if (returnType != null)
            {
                //创建返回对象并赋值
                var result = Activator.CreateInstance(returnType);
                if (result is IResult)
                {
                    ((IResult)result).IsOK = false;
                    ((IResult)result).Msg = context.Exception.Message;

                    //返回错误信息
                    context.Result = new ObjectResult(result);
                }
                else
                {
                    //返回错误信息
                    context.Result = new ObjectResult(new ResultInfo<bool>(false, new MsgInfo() { Code = "IO_Gen_002", Msg = context.Exception.Message }, false));
                }
            }
            else
            {
                //返回错误信息
                context.Result = new ObjectResult(new ResultInfo<bool>(false, new MsgInfo() { Code = "IO_Gen_002", Msg = context.Exception.Message }, false));
            }

            base.OnException(context);
        }
    }
}
