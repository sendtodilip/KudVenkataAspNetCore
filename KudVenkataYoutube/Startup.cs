using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KudVenkataYoutube.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KudVenkataYoutube
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc().AddXmlSerializerFormatters();
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            //services.AddScoped<IEmployeeRepository, MockEmployeeRepository>();
            //services.AddTransient<IEmployeeRepository, MockEmployeeRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions()
                {
                    SourceCodeLineCount = 20
                };
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseStatusCodePages();
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            //DefaultFilesOptions defaultFiles = new DefaultFilesOptions();
            //defaultFiles.DefaultFileNames.Clear();
            //defaultFiles.DefaultFileNames.Add("foo.html");
            ////app.UseDefaultFiles(defaultFiles);
            app.UseStaticFiles();
            //app.UseFileServer();
            //app.UseMvcWithDefaultRoute();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseMvc();
            //app.Use(async (context, next) =>
            //{
            //    logger.LogInformation("MW1 :Incoming Request.");                
            //    await next();
            //    logger.LogInformation("MW1 :Outgoing Response.");
            //});

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hello World1");
            //    await next.Invoke();
            //});

            //app.Run(async (context) =>
            //{
            //    //throw new Exception("Exception from asp.net core application.");
            //    await context.Response.WriteAsync("Hello MVC");
            //});
        }
    }
}
