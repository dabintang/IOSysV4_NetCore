using IOSys.API.Controllers;
using IOSys.Helper;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOSys.API.Filters
{
    /// <summary>
    /// 接口输入输出参数写日志 特性
    /// </summary>
    public class ParamsLogAttribute : Attribute
    {
        /// <summary>
        /// 接口描述
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="describe">接口描述</param>
        public ParamsLogAttribute(string describe)
        {
            this.Describe = describe;
        }

        /// <summary>
        /// 进入接口时写日志
        /// </summary>
        /// <param name="context"></param>
        public void WriteLogIn(ActionExecutingContext context)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"进入接口：{this.Describe}({context.ActionDescriptor.DisplayName})，哈希：{context.HttpContext.GetHashCode()}");
                sb.AppendLine("入参：");
                foreach (var key in context.ActionArguments.Keys)
                {
                    var strVal = JsonConvert.SerializeObject(context.ActionArguments[key]);
                    sb.AppendLine($"参数名：{key}；参数值：{strVal}");
                }

                var baseApiCtl = context.Controller as BaseController;
                if (baseApiCtl != null && baseApiCtl.LoginInfo != null)
                {
                    sb.Append($"登录信息：{JsonConvert.SerializeObject(baseApiCtl.LoginInfo)}");
                }

                LogHelper.Debug(sb.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "写日志出错（ParamsLogAttribute.WriteLogIn）");
            }
        }

        /// <summary>
        /// 离开接口时写日志
        /// </summary>
        /// <param name="context"></param>
        public void WriteLogOut(ResultExecutedContext context)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"离开接口：{this.Describe}({context.ActionDescriptor.DisplayName})，哈希：{context.HttpContext.GetHashCode()}");
                //sb.AppendLine("入参：");
                //foreach (var key in context.ActionArguments.Keys)
                //{
                //    var strVal = JsonConvert.SerializeObject(context.ActionArguments[key]);
                //    sb.AppendLine($"参数名：{key}；参数值：{strVal}");
                //}

                if (context.Exception == null)
                {
                    sb.Append($"出参：{JsonConvert.SerializeObject(((Microsoft.AspNetCore.Mvc.ObjectResult)context.Result).Value)}");
                    LogHelper.Debug(sb.ToString());
                }
                else
                {
                    sb.Append("抛异常：{context.Exception.Message}");
                    LogHelper.Debug(context.Exception, sb.ToString());
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "写日志出错（ParamsLogAttribute.WriteLogOut）");
            }
        }
    }
}
