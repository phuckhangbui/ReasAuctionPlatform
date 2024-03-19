namespace Service.Mail
{
    public class SendMailWhenChangeWinner
    {
        public static void SendMailWhenChangeWinnerAuction(string toEmail, string reasName, string reasAddress, DateTime expectedPaymentDate, float winAmount, double depositAmount)
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
                "<p>Because of some problem. We change new winner with this auction</p>" +
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

        public static void SendMailWhenForFirstWinnerChangeWinnerAuction(string toEmail, string winnerName, string reasName, string reasAddress)
        {
            var mailContext = new MailContent();
            MailSetting mailSetting = new MailSetting();
            mailSetting.Mail = "reasspring2024@gmail.com";
            mailSetting.Host = "smtp.gmail.com";
            mailSetting.Port = 587;
            mailSetting.Passwork = "zgtj veex szof becd";
            mailSetting.DisplayName = "REAS";
            mailContext.To = toEmail;
            mailContext.Subject = "<h1>We Regret to Inform: Change in Auction Winner for " + reasName + "</h1>";
            mailContext.Body = "<p>Dear " + winnerName + ",</p>" +
                "<p>We regret to inform you that due to some unforeseen circumstances, we were unable to contact you regarding your winning bid in the auction for the property located at <span class=\"bold\">" + reasAddress + "</span>.</p>" +
                "<p>Unfortunately, as we didn't receive a response from you, we had to change the auction winner. The new winner has been determined based on the bid amount, and the property has been transferred to them.</p>" +
                "<p>We sincerely apologize for any inconvenience this may cause you.</p>" +
                "<p>If you have any questions or concerns, please feel free to contact us at (+84) 123 345 2341 or by replying to this email.</p>" +
                "<p>Thank you for your understanding.</p>" +
                "<p>Sincerely,</p>" +
                "<p>REAS - Real Estate Auction Platform</p>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }

    }
}
