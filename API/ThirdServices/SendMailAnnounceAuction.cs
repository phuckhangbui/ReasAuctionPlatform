using API.Services;

namespace API.ThirdServices
{
    public class SendMailAnnounceAuction
    {
        public static void SendMailToAnnounceAuctionStartTime(string toEmail, string reasName, DateTime auctionStartDate)
        {
            var mailContext = new MailContent();
            MailSetting mailSetting = new MailSetting();
            mailSetting.Mail = "reasspring2024@gmail.com";
            mailSetting.Host = "smtp.gmail.com";
            mailSetting.Port = 587;
            mailSetting.Passwork = "zgtj veex szof becd";
            mailSetting.DisplayName = "REAS";
            mailContext.To = toEmail;
            mailContext.Subject = $"Auction Reminder: {reasName} Starts Soon!";
            mailContext.Body = $@"<p>Dear Participant,</p>
            <p>We would like to remind you that the auction for the real estate <strong>{reasName}</strong> is about to start in 5 minutes. Be sure to join us!</p>
            <p>This email provides the following details:</p>
            <ul>
                <li><strong>Real Estate Name:</strong> <span class='bold'>{reasName}</span></li>
                <li><strong>Auction Start Date:</strong> <span class='bold'>{auctionStartDate.ToString("dddd, MMMM dd, yyyy HH:mm tt")}</span></li>
            </ul>
            <p>If you have any last-minute questions or need assistance, please feel free to contact us at (+84) 123 345 2341 or by replying to this email.</p>
            <p>We hope to see you at the auction!</p>
            <p>Best regards,</p>
            <p>REAS - Real Estate Auction Platform</p>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
