using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FAN.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            


            

            EndPoints enpPoints = this.Configuration.GetSection("Hosts").Get<EndPoints>();

            Logging logging = new Logging();
            this.Configuration.GetSection("Logging").Bind(logging);

            LogLevel logLevel = this.Configuration.GetSection("Logging:LogLevel").Get<LogLevel>();

            services.Configure<Logging>(this.Configuration.GetSection("Logging"));
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }
    public class LogLevel
    {
        public string Default { get; set; }
    }

    public class EndPoints
    {
        public Dictionary<string, EndPoint> Protocols { get; set; }
    }
    public class EndPoint
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public Certificate Certificate { get; set; }
    }
    public class Certificate
    {
        public string FileName { get; set; }
        public string Password { get; set; }
    }
}
