using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace WineCellar.Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();
            
            emailMessage.From.Add(MailboxAddress.Parse("info@wcm.com"));
            emailMessage.To.Add(MailboxAddress.Parse(email));

            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            using(var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.ethereal.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate(emailSettings.Email, emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
