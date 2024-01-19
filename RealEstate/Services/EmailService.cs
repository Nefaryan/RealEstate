using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RealEstate.Models.Mail;
using RealEstate.Services.IServices;
using System.Threading.Tasks;

namespace RealEstate.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSetting _mailSetting;
        public EmailService(IOptions<MailSetting> mailSetting)
        {
            _mailSetting = mailSetting.Value;
        }
        public async Task SendEmailAsync(MailData mailData)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSetting.Mail);
            email.To.Add(MailboxAddress.Parse(mailData.ToEmail));
            email.Subject = mailData.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = mailData.Body
            };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
