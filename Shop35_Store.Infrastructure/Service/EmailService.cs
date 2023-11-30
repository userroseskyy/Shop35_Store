using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shop35_Store.Infrastructure.Service
{
    public class EmailService
    {
        public Task Execute(string UserEmail, string Body, string Subject)
        {





            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.Gmail.com";
            client.EnableSsl = true;
            client.Timeout = 1000000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("fdf73523@gmail.com", "@Aa123456@Aa");
            MailMessage message = new MailMessage("fdf73523@gmail.com", UserEmail, Subject, Body);
            message.IsBodyHtml = true;
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            client.Send(message);
            return Task.CompletedTask;



        }

    }
}
