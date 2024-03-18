namespace Service.Mail
{
    public class SendEmailTest
    {
        public static void SendMailToTest(string toEmail)
        {
            var mailContext = new MailContent();
            MailSetting mailSetting = new MailSetting();
            mailSetting.Mail = "reasspring2024@gmail.com";
            mailSetting.Host = "smtp.gmail.com";
            mailSetting.Port = 587;
            mailSetting.Passwork = "zgtj veex szof becd";
            mailSetting.DisplayName = "REAS";
            mailContext.To = toEmail;
            mailContext.Subject = $"REAS TEST!";
            mailContext.Body = $@"<p>Dear Participant,</p>
            <p>We would like to remind you that you are a special species and deserve to pass the class. Be sure to join us!</p>
            <p>This email provides the following details:</p>
            <p>If you have any last-minute questions or need assistance, please feel free to contact us at (+84) 123 345 2341 or by replying to this email.</p>
            <p>We hope to see you at the auction!</p>
            <p>Best regards,</p>
            <p>REAS - Real Estate Auction Platform</p>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
