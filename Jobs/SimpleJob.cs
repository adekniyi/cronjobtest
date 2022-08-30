using cronjobtest.Service;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace cronjobtest.Jobs
{
    public class SimpleJob : IJob
    {
        IEmailService _emailService;
        public SimpleJob(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _emailService.Send("adekniyi@gmail.com", "DI", "Dependency injection in quartz jobs");
        }
    }
}
