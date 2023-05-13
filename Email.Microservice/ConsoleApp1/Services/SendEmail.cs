//using MailKit.Net.Smtp;
//using MimeKit;
using System.Net;
using System.Net.Mail;

namespace Email.Microservice.Services
{
    public class SendEmail
    {
        public void SendEmails()
        {
            try
            {
                var htmlString = "<h1>This is a test mail</h1>";
                MailMessage message = new MailMessage();
                message.From = new MailAddress("dhaval4triveni@gmail.com");
                message.To.Add(new MailAddress("dhaval4triveni@gmail.com"));
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html
                message.Body = htmlString;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("dhruvesh@calcuquote.com", "hnbosmcwalszotnz");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }
        }
    }
}
