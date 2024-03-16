namespace Service.Mail
{
    public class SendMailWhenRejectRealEstate
    {
        public static void SendEmailWhenRejectRealEstate(string toEmail, string name, string message)
        {
            var mailContext = new MailContent();
            MailSetting mailSetting = new MailSetting();
            mailSetting.Mail = "reasspring2024@gmail.com";
            mailSetting.Host = "smtp.gmail.com";
            mailSetting.Port = 587;
            mailSetting.Passwork = "zgtj veex szof becd";
            mailSetting.DisplayName = "REAS";
            mailContext.To = toEmail;
            mailContext.Subject = "Từ chối bất động sản của bạn lên diễn đàn điện tử của Công Ty REAS (Công ty Bất Động Sản)"; ;
            mailContext.Body = "<h3>Lời nói đầu tiên xin cảm ơn đến bạn " + "<strong>" + name + "</strong>" + " đã quan tâm đến website của chúng tôi và muốn đăng bất động sản lên diễn đàn. Tuy nhiên, bất động sản của bạn không được tiếp nhận. Bởi vì một số lí do sau:</h3>" +
                                "<br><p><strong>Lí do:</strong> " + message + "</p>" +
                                "<br><br><h4>Vui lòng điền lại thông tin phù hợp để có thể sử dung tất cả dịch vụ tiện ích của website chúng tôi.</h4>" +
                                "<br><br><h4>Reas xin cảm ơn!</h4>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
