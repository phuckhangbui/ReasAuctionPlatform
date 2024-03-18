using Repository.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                mailContext.Subject = "AUCTION NOTIFICATION"; ;
                mailContext.Body = "<h3>First of all, we wish you a good day.</h3>" +
                                    "<br><br><h4>We inform you about the real estate named " + "<strong>" + reasName + "</strong></h4>" +
                                    "<br><br><h4>The event will take place at: " + dateStart.TimeOfDay.ToString() + " on " + dateStart.Date.ToString() + "</h4>" +
                                    "<br><br><h4>Reas thank you!</h4>";
                var sendmailservice = new SendMailService(mailSetting);
                sendmailservice.SendMail(mailContext);
            }
        }
    }
}
