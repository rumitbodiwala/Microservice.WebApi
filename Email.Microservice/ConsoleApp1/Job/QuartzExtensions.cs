using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz.Impl;

namespace Email.Microservice.Job
{
    public static class QuartzExtensions
    {
        public static void AddQuartz(this IServiceCollection services, params Type[] jobs)
        {
            services.Add(jobs.Select(jobType => new ServiceDescriptor(jobType, jobType, ServiceLifetime.Transient)));

            services.AddSingleton(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = new QuartzJobFactory(services.BuildServiceProvider());
                scheduler.Start();
                return scheduler;
            });
        }
    }
}
