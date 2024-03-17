using Repository.DTOs;

namespace Service.Mail
{
    public class SendMailLosingAuction
    {
        public static void SendMailLosingAuctionToAttender(AuctionAttenderDto auctionAttenderDto, string reasName, DateTime auctionStartTime, DateTime auctionEndTime)
        {
            var mailContext = new MailContent();
            MailSetting mailSetting = new MailSetting();
            mailSetting.Mail = "reasspring2024@gmail.com";
            mailSetting.Host = "smtp.gmail.com";
            mailSetting.Port = 587;
            mailSetting.Passwork = "zgtj veex szof becd";
            mailSetting.DisplayName = "REAS";
            mailContext.To = auctionAttenderDto.AccountEmail;
            mailContext.Subject = "Notification: Auction Result for " + reasName;
            mailContext.Body = "<p>Dear Attendee,</p>" +
                "<p>We regret to inform you that you were not the winning bidder for the property auctioned at <span class=\"bold\">" + reasName + "</span>. The auction took place from <span class=\"bold\">" + auctionStartTime + "</span> to <span class=\"bold\">" + auctionEndTime + "</span>.</p>" +
                "<p>While you were not successful in this auction, we appreciate your participation. Your payment information for the refund process is provided below:</p>" +
                "<ul>" +
                "<li><strong>Banking Number:</strong> <span class=\"bold\">" + auctionAttenderDto.BankingNumber + "</span></li>" +
                "<li><strong>Banking Code:</strong> <span class=\"bold\">" + auctionAttenderDto.BankingCode + "</span></li>" +
                "</ul>\r\n\r\n" +
                "<p>If you have any further questions or concerns, please don't hesitate to contact us at (+84) 123 345 2341 or by replying to this email.</p>" +
                "<p>Thank you for your participation and interest in our auction events.</p>" +
                "<p>Sincerely,</p>" +
                "<p>REAS - Real Estate Auction Platform</p>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
