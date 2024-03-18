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
            mailContext.Subject = "Provision of UserName and Password for Employees of REAS Company (Real Estate Company)";
            mailContext.Body = "<h3>First of all, we would like to thank you, " + "<strong>" + accountName + "</strong>" + ", for joining our company. Below are the details of your employee account.</h3>" +
                                "<br><p><strong>Username:</strong> " + username + "</p>" +
                                "<br><p><strong>Password:</strong> " + password + "</p>" +
                                "<br><br><h4>Please note that your account information should be kept secure. You will be responsible if your information is leaked. Account password information is only provided once.</h4>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
