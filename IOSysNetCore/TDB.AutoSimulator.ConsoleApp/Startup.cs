using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TDB.AutoSimulator.ConsoleApp.Configs;

namespace TDB.AutoSimulator.ConsoleApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddMvc();
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "自动/模拟接口", Version = "v1" });

                //接口注释
                var xmlAPI = Path.Combine(AppContext.BaseDirectory, "TDB.AutoSimulator.ConsoleApp.xml");
                c.IncludeXmlComments(xmlAPI);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            //配置跨域
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.SetPreflightMaxAge(TimeSpan.FromDays(1));

                builder.WithOrigins(AppConfig.Inst.App.LstWithOrigins.ToArray());
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "自动/模拟接口 V1");
            });

            app.UseMvc();
        }
    }
}
