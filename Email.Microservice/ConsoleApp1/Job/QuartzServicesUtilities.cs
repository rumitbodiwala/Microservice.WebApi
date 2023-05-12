using Quartz;

namespace Email.Microservice.Job
{
    public static class QuartzServicesUtilities
    {
        public static void StartJob<TJob>(IScheduler scheduler, int pricingExpirationScheduleHours, int pricingExpirationScheduleMins) where TJob : IJob
        {
            var jobKey = new JobKey($"PricingExpiration", "CQ");
            if (scheduler.CheckExists(jobKey).Result)
            {
                scheduler.DeleteJob(jobKey);
            }


            /* construct a job */
            IJobDetail job =
                JobBuilder.Create<EmailSendJob>()
                .WithIdentity(jobKey)
                .Build();

            /* prepare a job */
            ITrigger trigger =
               TriggerBuilder.Create()
               .ForJob(job)
               .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(pricingExpirationScheduleHours, pricingExpirationScheduleMins).InTimeZone(TimeZoneInfo.Local))
               .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
