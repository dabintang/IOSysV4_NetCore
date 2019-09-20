using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IOSys.API.Filters;
using IOSys.DTO;
using IOSys.Helper;
using IOSys.Helper.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace IOSys.API
{
    /// <summary>
    /// 启动配置
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc(AddFilters);
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            ////返回的json大小写和对象定义一致
            //mvcBuilder.AddJsonOptions(option => { option.SerializerSettings.ContractResolver = new DefaultContractResolver(); });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v4", new Info { Title = "收支系统接口", Version = "v4" });

                //接口注释
                var xmlAPI = Path.Combine(AppContext.BaseDirectory, "IOSys.API.xml");
                c.IncludeXmlComments(xmlAPI);

                //参数注释
                var xmlDTO = Path.Combine(AppContext.BaseDirectory, "IOSys.DTO.xml");
                c.IncludeXmlComments(xmlDTO);

                //生成token输入框
                c.OperationFilter<SwaggerOperationFilter>();
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //全局异常（这好像没什么用，过滤器异常都捕获了）
            //app.UseExceptionHandler(configure => configure.Run(ErrorEvent));

            //配置跨域
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.SetPreflightMaxAge(TimeSpan.FromDays(1));

                //builder.WithOrigins(IOSysJson.Inst.AppConfig.LstWithOrigins.ToArray());
                builder.AllowAnyOrigin();

            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v4/swagger.json", "收支系统接口 V4");
            });

            app.UseMvc();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
                RequestPath = new PathString("/Files"),
                //设置不限制content-type 该设置可以下载所有类型的文件，但是不建议这么设置，因为不安全
                //ServeUnknownFileTypes = true 
                //下面设置可以下载apk类型的文件
                ContentTypeProvider = new FileExtensionContentTypeProvider(new Dictionary<string, string>
                {
                    { ".apk","application/vnd.android.package-archive"}
                })
            });
        }

        /// <summary>
        /// 添加过滤器
        /// </summary>
        /// <param name="option">配置</param>
        private void AddFilters(MvcOptions option)
        {
            //验权过滤器
            option.Filters.Add(typeof(AuthFilterAttribute));
            //行为过滤器
            option.Filters.Add(typeof(ActFilterAttribute));
            //异常过滤器
            option.Filters.Add(typeof(ExcpFilterAttribute));
        }

        ///// <summary>
        ///// 全局异常处理
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private Task ErrorEvent(HttpContext context)
        //{
        //    var feature = context.Features.Get<IExceptionHandlerFeature>();
        //    var error = feature?.Error;
        //    var queryStr = context.Request.QueryString.ToString();
        //    if (error != null)
        //    {
        //        //写日志
        //        LogHelper.Error("未知异常：{0}", queryStr);
        //    }
        //    else
        //    {
        //        //写日志
        //        LogHelper.Error(error, "未知异常：{0}", queryStr);
        //    }

        //    //返回错误信息
        //    var errRet = new ObjectResult(new ResultInfo<bool>(false, new MsgInfo() { Code = "IO_Gen_002", Msg = error?.Message }, false));
        //    return context.Response.WriteAsync(JsonConvert.SerializeObject(errRet));
        //}
    }
}
