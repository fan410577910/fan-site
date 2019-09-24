using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FAN.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region 配置文件读取
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");//设置配置文件所在路径
            var configRoot = builder.Build();
            var value = configRoot.GetSection("Logging").GetSection("LogLevel").GetSection("Default").Value;
            //Console.WriteLine(value);
            #endregion

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
            {
                Dictionary<string, string> dict = new Dictionary<string, string> {
                    { "Size","10"},
                    { "Color","RED"}
                    //[""] = "",
                };

                configurationBuilder
                .AddInMemoryCollection(dict)//从内存添加
                .AddJsonFile("hosts.json",false,true)//从JSON文件添加
                .Build();
            })
                .UseStartup<Startup>();
    }
}
