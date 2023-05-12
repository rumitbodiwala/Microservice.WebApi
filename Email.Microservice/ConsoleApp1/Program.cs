using Email.Microservice;
using Email.Microservice.Consumer;
using Email.Microservice.Job;
using Email.Microservice.Models;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

try
{
    IHost host = Host.CreateDefaultBuilder(args)
   .UseContentRoot(Directory.GetCurrentDirectory())
   .UseWindowsService()
   .ConfigureAppConfiguration((context, configurationBuilder) =>
   {
       // Configure the app here.
       configurationBuilder.AddEnvironmentVariables();
   })
   .ConfigureServices((hostContext, services) =>
   {
       services.AddHostedService<EmailService>();

       //for register job schedular 
       services.AddQuartz(typeof(EmailSendJob));

       services.AddMassTransit(x =>
       {
           x.AddConsumer<TestConsumer>();

           x.UsingRabbitMq((context, cfg) =>
           {
               cfg.Host(new Uri("rabbitmq://localhost/Email_Microservice"), h =>
               {
                   h.Username("guest");
                   h.Password("guest");
               });

               cfg.UseMessageRetry(p => p.Interval(1, TimeSpan.FromSeconds(5)));

               cfg.ReceiveEndpoint("TestConsumer", e =>
               {
                   e.PrefetchCount = 1;
                   e.ConcurrentMessageLimit = 2;
                   e.ConfigureConsumer<TestConsumer>(context);
               });

               cfg.ConfigureEndpoints(context);
           });

           services.AddMassTransit<IEmailServiceBus>(x =>
           {
               x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
               {
                   cfg.Host(new Uri("rabbitmq://localhost/Email_Microservice"), h =>
                   {
                       h.Username("guest");
                       h.Password("guest");
                   });
                   cfg.ConfigureEndpoints(context);
               }));
           });

       });

       services.AddSingleton<EmailService>();
       services.AddHostedService<EmailService>(x => x.GetService<EmailService>());
   })
   .Build();

    Console.WriteLine("Email Service Started...");
    await host.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString(), "Email Service terminated unexpectedly!");
}