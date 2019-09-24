using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(FAN.WebAPI.Startup1))]

namespace FAN.WebAPI
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello, world.");
            });

            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
            this.ConfigurationAuth(app);
        }
        private void ConfigurationAuth(IAppBuilder app)
        {
            string a = "";
        }
    }
}
