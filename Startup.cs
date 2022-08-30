using cronjobtest.Jobs;
using cronjobtest.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace cronjobtest
{
    public class Startup
    {
        private IScheduler _quartzSchedular;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _quartzSchedular = ConfigueQuartz();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<SimpleJob>();
            services.AddSingleton(provider => _quartzSchedular);
        }

        private void OnShutdown()
        {
            if (!_quartzSchedular.IsShutdown) _quartzSchedular.Shutdown();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            _quartzSchedular.JobFactory = new AspnetJobFactory(app.ApplicationServices);
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            CronSchedulerClass.start(_quartzSchedular);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public IScheduler ConfigueQuartz()
        {
            NameValueCollection props = new NameValueCollection
            {
                {"quartz.serializer.type","binary" },
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            var schedular = factory.GetScheduler().Result;
            schedular.Start().Wait();

            return schedular;
        }
    }
}
