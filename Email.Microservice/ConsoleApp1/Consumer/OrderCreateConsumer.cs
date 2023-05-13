using Email.Microservice.Services;
using MassTransit;
using RabbitMQContract;

namespace Email.Microservice.Consumer
{
    public class OrderCreateConsumer : IConsumer<IOrderCreated>
    {
        public async Task Consume(ConsumeContext<IOrderCreated> context)
        {
            Console.WriteLine("Order is placed....");
            SendEmail sendEmail = new SendEmail();
            sendEmail.SendEmails();
        }
    }
}
