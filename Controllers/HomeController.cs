using cronjobtest.Jobs;
using cronjobtest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace cronjobtest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IScheduler _scheduler;
        public HomeController(ILogger<HomeController> logger, IScheduler scheduler)
        {
            _logger = logger;
            _scheduler = scheduler;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> StartSimpleJob()
        {
            IJobDetail job = JobBuilder.Create<SimpleJob>()
                                        //.UsingJobData("username", "devhow")
                                        //.UsingJobData("password", "Security!!")
                                        .WithIdentity("simplejob", "quartsexamples")
                                        .StoreDurably()
                                        .Build();
            //job.JobDataMap.Put("user", new JobUserParameter)

            await _scheduler.AddJob(job, true);
            ITrigger trigger = TriggerBuilder.Create()
                                             .ForJob(job)
                                             //.UsingJobData("triggerparam","Simple trigger 1 param")
                                             .WithIdentity("testtrigger", "quartzexamples")
                                             .StartNow()
                                             .WithSimpleSchedule(x=>x.WithIntervalInSeconds(10).WithRepeatCount(5))
                                             //.WithDailyTimeIntervalSchedule(x => x.OnEveryDay()
                                             //                                     .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(17,0)))
                                             .Build();

            await _scheduler.ScheduleJob(trigger);
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
