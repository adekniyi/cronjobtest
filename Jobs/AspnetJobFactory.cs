using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using System;

namespace cronjobtest.Jobs
{
    public class AspnetJobFactory : SimpleJobFactory
    {
        IServiceProvider _provider;
        public AspnetJobFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return (IJob)this._provider.GetService(bundle.JobDetail.JobType);
            }
            catch (Exception ex)
            {

                throw new SchedulerException(string.Format("Something went wrong {0}",bundle.JobDetail.Key));
            }
        }
    }
}
