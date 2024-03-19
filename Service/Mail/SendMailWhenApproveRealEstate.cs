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
            mailContext.Subject = "Agree to receive your real estate on the electronic forum of REAS Company (Real Estate Company)";
            mailContext.Body = "<h3>First of all, thank you " + "<strong>" + name + "</strong>" + " for your interest in our website and wanting to post real estate on the forum.</h3>" +
                                "<br><br><h4>We inform you that your real estate has been approved. Please log in to the website to proceed to the next step to post on the forum</h4>" +
                                "<br><br><h4>Reas thank you!</h4>";
            var sendmailservice = new SendMailService(mailSetting);
            sendmailservice.SendMail(mailContext);
        }
    }
}
