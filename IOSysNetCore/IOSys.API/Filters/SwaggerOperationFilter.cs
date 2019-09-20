using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace IOSys.API.Filters
{
    /// <summary>
    /// swagger 生成token输入框
    /// </summary>
    public class SwaggerOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            //使用语音
            //operation.Parameters.Add(new NonBodyParameter { Name = "Lang", In = "header", Description = "Langue", Required = false, Type = "string" });

            //获取方法信息
            MethodInfo methodInfo = null;
            if (context.ApiDescription.TryGetMethodInfo(out methodInfo) == false)
            {
                return;
            }

            //判断方法是否允许匿名
            var allowAnonymous = methodInfo.GetCustomAttributes().Where(m => m is IAllowAnonymous).Count() > 0 ||
                                 methodInfo.ReflectedType.GetCustomAttributes().Where(m => m is IAllowAnonymous).Count() > 0;

            //不允许匿名，添加token参数
            if (allowAnonymous == false)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }

                operation.Parameters.Add(new NonBodyParameter { Name = "Authorization", In = "header", Description = "Token", Required = false, Type = "string" });
            }
        }
    }
}
