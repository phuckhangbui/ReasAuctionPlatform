using API.Services;


namespace API.Extensions
{
    public class MailServiveExtension
    {
        IConfiguration _configuration;
        public MailServiveExtension(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureService(IServiceCollection services)
        {
            services.AddOptions();
            var mailSetting = _configuration.GetSection("MailSetting");
            services.Configure<MailSetting>(mailSetting);

            services.AddTransient<SendMailService>();
        }
    }
}
