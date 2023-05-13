using Email.Microservice.Job;
using Email.Microservice.Models;
using Email.Microservice.Services;
using MassTransit.Transports;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using RabbitMQContract;

namespace Email.Microservice
{
    public class EmailService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IScheduler _scheduler;
        public EmailService(IServiceProvider serviceProvider, IScheduler scheduler)
        {
            _serviceProvider = serviceProvider;
            _scheduler = scheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var _emailBus = _serviceProvider.GetRequiredService<IEmailServiceBus>();
            await _emailBus.Publish<ITestModel>(new
            {
                Message = "Test Model"
            });

            await _emailBus.Publish<IOrderCreated>(new
            {
                OrderId = 1,
                Username = "Test"
            });

            //SendEmail sendEmail = new SendEmail();
            //sendEmail.SendEmails();

            QuartzServicesUtilities.StartJob<EmailSendJob>(_scheduler, 17, 15);

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {

        }

        public void Dispose()
        {

        }
    }
}
