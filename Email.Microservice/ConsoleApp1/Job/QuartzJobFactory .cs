using Quartz;
using Quartz.Spi;

namespace Email.Microservice.Job
{
    public class QuartzJobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public QuartzJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var jobDetail = bundle.JobDetail;

            try
            {
                var job = (IJob)_serviceProvider.GetService(jobDetail.JobType);
                return job;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void ReturnJob(IJob job) { }
    }
}
