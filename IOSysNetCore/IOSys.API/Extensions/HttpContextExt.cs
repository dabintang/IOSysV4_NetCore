﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IOSys.API.Extensions
{
    /// <summary>
    /// HttpContext扩展类
    /// </summary>
    public static class HttpContextExt
    {
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="context">Encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns></returns>
        public static string GetClientIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();//负载均衡
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }

            return ip;
        }

        /// <summary>
        /// 获取头部参数
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetHeaderValue(this HttpContext context, string key)
        {
            if (context.Request.Headers != null)
            {
                StringValues val;
                if (context.Request.Headers.TryGetValue(key, out val))
                {
                    return val.ToString();
                }
            }

            return string.Empty;
        }
    }
}
