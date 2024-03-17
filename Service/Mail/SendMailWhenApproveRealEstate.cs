namespace Service.Mail
{
    public class SendMailWhenApproveRealEstate
    {
        public static void SendEmailWhenApproveRealEstate(string toEmail, string name)
        {
            var mailContext = new MailContent();
            MailSetting mailSetting = new MailSetting();
            mailSetting.Mail = "reasspring2024@gmail.com";
            mailSetting.Host = "smtp.gmail.com";
            mailSetting.Port = 587;
            mailSetting.Passwork = "zgtj veex szof becd";
            mailSetting.DisplayName = "REAS";
            mailContext.To = toEmail;
            mailContext.Subject = "Đồng ý tiếp nhận bất động sản của bạn lên diễn đàn điện tử của Công Ty REAS (Công ty Bất Động Sản)"; ;
            mailContext.Body = "<h3>Lời nói đầu tiên xin cảm ơn đến bạn " + "<strong>" + name + "</strong>" + " đã quan tâm đến website của chúng tôi và muốn đăng bất động sản lên diễn đàn.</h3>" +
                                "<br><br><h4>Chúng tôi thông báo bất động sản của bạn đã được chấp thuận. Vui lòng đăng nhập vào website để đến bước tiếp theo để có thể đăng bài lên diễn đàn</h4>" +
                                "<br><br><h4>Reas xin cảm ơn!</h4>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
