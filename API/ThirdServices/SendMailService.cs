using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;

namespace API.Services
{
    public class SendMailService
    {
        MailSetting setting { get; set; }
        public SendMailService(MailSetting mailSetting)
        {
            setting = mailSetting;
        }
        public void SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(setting.DisplayName, setting.Mail);
            email.From.Add(new MailboxAddress(setting.DisplayName, setting.Mail));
            email.To.Add(new MailboxAddress(mailContent.To, mailContent.To));
            email.Subject = mailContent.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtpClient.Connect(setting.Host, setting.Port, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(setting.Mail, setting.Passwork);
                smtpClient.Send(email);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            smtpClient.Disconnect(true);

        }
    }

    public class MailContent
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
