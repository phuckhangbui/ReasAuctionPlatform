using Repository.DTOs;

namespace Service.Mail
{
    public class SendMailWhenCreateAuction
    {
        public static void SendMailForMemberWhenCreateAuction(IEnumerable<NameUserDto> toEmail, string reasName, DateTime dateStart)
        {
            foreach (NameUserDto email in toEmail)
            {
                var mailContext = new MailContent();
                MailSetting mailSetting = new MailSetting();
                mailSetting.Mail = "reasspring2024@gmail.com";
                mailSetting.Host = "smtp.gmail.com";
                mailSetting.Port = 587;
                mailSetting.Passwork = "zgtj veex szof becd";
                mailSetting.DisplayName = "REAS";
                mailContext.To = email.EmailName;
                mailContext.Subject = "THÔNG BÁO DIỄN RA AUCTION"; ;
                mailContext.Body = "<h3>Lời nói đầu tiên chúng tôi xin chúc bạn một ngày tốt lành.</h3>" +
                                    "<br><br><h4>Chúng tôi thông báo bất động sản với tên " + "<strong>" + reasName + "</strong></h4>" +
                                    "<br><br><h4>Thời gian diễn ra vào lúc : " + dateStart.TimeOfDay.ToString() + " ngày " + dateStart.Date.ToString() + "</h4>" +
                                    "<br><br><h4>Reas xin cảm ơn!</h4>";
                var sendmailservice = new SendMailService(mailSetting);
                sendmailservice.SendMail(mailContext);
            }
        }
    }
}
