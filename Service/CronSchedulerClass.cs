using cronjobtest.Jobs;
using Quartz;

namespace cronjobtest.Service
{
    public class CronSchedulerClass
    {
        //private IScheduler _scheduler;
        //public CronSchedulerClass(IScheduler scheduler)
        //{
        //    _scheduler = scheduler;
        //}
       public static async void start(IScheduler _scheduler)
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
                                             .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).WithRepeatCount(5))
                                             //.WithDailyTimeIntervalSchedule(x => x.OnEveryDay()
                                             //                                     .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(17,0)))
                                             .Build();

            await _scheduler.ScheduleJob(trigger);
        }
    }
}
