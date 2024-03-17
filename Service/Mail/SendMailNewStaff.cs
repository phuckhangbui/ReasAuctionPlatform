namespace Service.Mail
{
    public class SendMailNewStaff
    {
        public static void SendEmailWhenCreateNewStaff(string toEmail, string username, string password, string accountName)
        {
            var mailContext = new MailContent();
            MailSetting mailSetting = new MailSetting();
            mailSetting.Mail = "reasspring2024@gmail.com";
            mailSetting.Host = "smtp.gmail.com";
            mailSetting.Port = 587;
            mailSetting.Passwork = "zgtj veex szof becd";
            mailSetting.DisplayName = "REAS";
            mailContext.To = toEmail;
            mailContext.Subject = "Cấp Tài Khoản UserName, Password Cho Nhân Viên Của Công Ty REAS (Công ty Bất Động Sản)"; ;
            mailContext.Body = "<h3>Lời nói đầu tiên xin cảm ơn đến bạn " + "<strong>" + accountName + "</strong>" + " đã tham gia vào công ty. Sau đây là thông tin tài khoản nhân viên của bạn.</h3>" +
                                "<br><p><strong>Username:</strong> " + username + "</p>" +
                                "<br><strong>Password:</strong> " + password + "</p>" +
                                "<br><br><h4>Lưu ý thông tin tài khoản của bạn nên được bảo mật nếu không bạn sẽ chịu trách nhiệm nếu bị lộ thông tin. Thông tin tài khoản mật khẩu chỉ được cấp 1 lần duy nhất.</h4>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
