using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ordering.Microservice.Data;
using System;
using MassTransit;


namespace Ordering.Microservice
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(
                  _configuration.GetConnectionString("DefaultConnection"),
                  b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"{0}\Customer.Microservice.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Customer Microservice API",
                });
            });

            #region RabbitMQConfiguration

            AddRabbitMQConfiguration(services);

            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer.Microservice");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #region RabbitMQ

        private void AddRabbitMQConfiguration(IServiceCollection services)
        {
            var rabbitMqConfig = _configuration.GetSection("RabbitMq");
            var portCQRabbitMQHost = Convert.ToString(rabbitMqConfig.GetValue<string>("PortCQRabbitMqHost"));
            var rabbitMqUserName = Convert.ToString(rabbitMqConfig.GetValue<string>("RabbitMqUserName"));
            var rabbitMqPassword = Convert.ToString(rabbitMqConfig.GetValue<string>("RabbitMqPassword"));

            if (string.IsNullOrWhiteSpace(portCQRabbitMQHost) || string.IsNullOrWhiteSpace(rabbitMqUserName) || string.IsNullOrWhiteSpace(rabbitMqPassword))
                return;

            //var decryptRabbitMqPassword = EncryptDecrypt.AES_Decrypt(rabbitMqPassword);

            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(new Uri(portCQRabbitMQHost), h =>
                    {
                        h.Username(rabbitMqUserName);
                        h.Password(rabbitMqPassword);
                    });
                }));
            });
            services.AddMassTransitHostedService();
        }
        #endregion
    }
}
