using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using TDB.AutoSimulator.ConsoleApp.Configs;

namespace TDB.AutoSimulator.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("模拟收支");

            var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseUrls(AppConfig.Inst.App.MyUrl)
            .UseStartup<Startup>()
            .Build();

            host.Run();
        }
    }
}
