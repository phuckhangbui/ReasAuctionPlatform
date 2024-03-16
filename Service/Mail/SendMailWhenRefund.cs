namespace Service.Mail
{
    public class SendMailWhenRefund
    {
        public static void SendMailWhenRefundMoney(string toEmail, string reasName, string reasAddress, double refundAmount)
        {
            // Khai báo các thông tin cần thiết cho email
            var mailContext = new MailContent();
            MailSetting mailSetting = new MailSetting();
            mailSetting.Mail = "reasspring2024@gmail.com";
            mailSetting.Host = "smtp.gmail.com";
            mailSetting.Port = 587;
            mailSetting.Passwork = "zgtj veex szof becd";
            mailSetting.DisplayName = "REAS";

            // Định dạng nội dung email
            string emailSubject = "Refund Confirmation: " + reasName;
            string emailBody = "<p>Dear Customer,</p>" +
                               "<p>We are pleased to inform you that a refund has been successfully processed for your recent transaction regarding the property located at <span class=\"bold\">" + reasAddress + "</span>.</p>" +
                               "<p>The refund amount of <span class=\"bold\">" + refundAmount + "VNĐ</span> has been credited back to your account.</p>" +
                               "<p>If you have any further questions or concerns, please don't hesitate to contact us.</p>" +
                               "<p>Best Regards,<br/>REAS - Real Estate Auction Platform</p>";

            // Gán các thông tin vào mailContext
            mailContext.To = toEmail;
            mailContext.Subject = emailSubject;
            mailContext.Body = emailBody;

            // Gửi email
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
