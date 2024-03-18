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
            mailContext.Subject = "Rejection of your real estate on the electronic forum of REAS Company (Real Estate Company)";
            mailContext.Body = "<h3>First of all, thank you " + "<strong>" + name + "</strong>" + " for your interest in our website and wanting to post real estate on the forum. However, your real estate has not been accepted due to the following reasons:</h3>" +
                                "<br><p><strong>Reason:</strong> " + message + "</p>" +
                                "<br><br><h4>Please provide appropriate information to be able to use all the utility services of our website.</h4>" +
                                "<br><br><h4>Reas thank you!</h4>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
