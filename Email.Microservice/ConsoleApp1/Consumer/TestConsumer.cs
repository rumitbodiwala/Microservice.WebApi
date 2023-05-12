using Email.Microservice.Models;
using MassTransit;

namespace Email.Microservice.Consumer
{
    public class TestConsumer : IConsumer<ITestModel>
    {
        public async Task Consume(ConsumeContext<ITestModel> context)
        {
            Console.WriteLine("Test Consumer Started....");
        }
    }
}
