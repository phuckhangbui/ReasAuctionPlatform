using System.Net.Mail;
using System.Net;
using API.Entity;

namespace API.Services
{
    public class MailSetting
    {
        public string Mail { get; set; } 
        public string DisplayName { get; set; }
        public string Passwork { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        
    }
}
