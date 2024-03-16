using API.Services;

namespace API.ThirdServices
{
    public class SendMailAuctionSuccess
    {
        public static void SendMailWhenAuctionSuccess(string toEmail, string reasName, string reasAddress, DateOnly expectedPaymentDate, float winAmount, float depositAmount)
        {
            var mailContext = new MailContent();
            MailSetting mailSetting = new MailSetting();
            mailSetting.Mail = "reasspring2024@gmail.com";
            mailSetting.Host = "smtp.gmail.com";
            mailSetting.Port = 587;
            mailSetting.Passwork = "zgtj veex szof becd";
            mailSetting.DisplayName = "REAS";
            mailContext.To = toEmail;
            mailContext.Subject = "<h1>Congratulations! You Won the Auction for " + reasName + "!</h1>";
            mailContext.Body = "<p>Dear [Winner Name],</p>" +
                "<p>We are thrilled to announce that you are the winning bidder for the property located at <span class=\"bold\">" + reasAddress + "</span>, with a final bid of <span class=\"bold\">" + winAmount + "$</span>. Congratulations!</p>" +
                "<p>This email confirms the following details:</p>" +
                "<ul>" +
                "<li><strong>Winning Bid:</strong> <span class=\"bold\">" + winAmount + "$</span></li>" +
                "<li><strong>Already Paid:</strong> <span class=\"bold\">" + depositAmount + "$</span></li>" +
                "</ul>\r\n\r\n  <p>To secure your purchase, you need to contact us before <span class=\"bold\">" + expectedPaymentDate + "</span>" +
                "<p>We understand that this is an exciting time, and we are here to help every step of the way. Please feel free to reach out to us with any questions or concerns you may have. You can contact us directly at <span class=\"bold\">(+84) 123 345 2341</span> or by replying to this email.</p>" +
                "<p>We look forward to working with you to finalize the purchase of your new property!</p>" +
                "<p>Sincerely,</p>  " +
                "<p>REAS - Real Estate Auction flatform</p>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
