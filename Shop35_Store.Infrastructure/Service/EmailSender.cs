using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Shop35_Store.Infrastructure.Service
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string UserEmail, string Body, string Subject)
        {
            string smtpHost = _configuration["SmtpSettings:Host"];
            int smtpPort = _configuration.GetValue<int>("SmtpSettings:Port");
            string smtpUsername = _configuration["SmtpSettings:Username"];
            string smtpPassword = _configuration["SmtpSettings:Password"];
            bool enableSsl = _configuration.GetValue<bool>("SmtpSettings:EnableSsl");

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = enableSsl;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpUsername);
                mailMessage.To.Add(UserEmail);
                mailMessage.Subject = Subject;
                mailMessage.Body = Body;
                mailMessage.IsBodyHtml = true;

                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send email. Error: " + ex.Message);
                }
            }
        }
    }
}
