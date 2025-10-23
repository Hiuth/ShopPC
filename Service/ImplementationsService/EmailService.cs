using Microsoft.Extensions.Options;
using ShopPC.Configuration;
using System.Net;
using System.Net.Mail;

namespace ShopPC.Service.ImplementationsService
{
    public class EmailService
    {
        private readonly EmailConfig _emailConfig;

        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_emailConfig.Host, _emailConfig.Port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password);

                var mail = new MailMessage
                {
                    From = new MailAddress(_emailConfig.From, _emailConfig.DisplayName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mail.To.Add(toEmail);

                await client.SendMailAsync(mail);
            }
        }
    }
}
