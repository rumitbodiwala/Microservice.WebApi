using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Microservice.Job
{
    public class EmailSendJob: IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var userName = "Pricing Expiration System";
            try
            {
                var message = string.Format("Email scheduler is started on {0} at {1} | UTC Time : {2} ", "Email", DateTime.Now, DateTime.UtcNow);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
