using API.Data;
using API.Errors;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using API.Repository;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection ApplicationServices(this IServiceCollection services
            , IConfiguration config)
        {
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IRealEstateRepository, RealEstateRepository>();
            services.AddScoped<IRealEstateDetailRepository, RealEstateDetailRepository>();
            services.AddScoped<IRealEstatePhotoRepository, RealEstatePhotoRepository>();
            services.AddScoped<IRuleRepository, RuleRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<IMoneyTransactionRepository, MoneyTransactionRepository>();
            services.AddScoped<IDepositAmountRepository, DepositAmountRepository>();
            services.AddScoped<ITypeReasRepository, TypeReasRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IAuctionAccountingRepository, AuctionAccountingRepository>();
            services.AddScoped<IParticipantHistoryRepository, ParticipantHistoryRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IRealEstateService, RealEstateService>();
            services.AddScoped<IRuleService, RuleService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IAdminAccountService, AdminAccountService>();
            services.AddScoped<IAdminNewsService, AdminNewsService>();
            services.AddScoped<IAdminRealEstateService, AdminRealEstateService>();
            services.AddScoped<IMemberDepositAmountService, MemberDepositAmountService>();
            services.AddScoped<IMemberRealEstateService, MemberRealEstateService>();
            services.AddScoped<IMemberRuleService, MemberRuleService>();
            services.AddScoped<IStaffRealEstateService, StaffRealEstateService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IMoneyTransactionService, MoneyTransactionService>();
            services.AddScoped<IDepositAmountService, DepositAmountService>();
            services.AddScoped<IParticipantHistoryService, ParticipantHistoryService>();
            services.AddScoped<IMemberAccountService, MemberAccountService>();
            services.AddScoped<INotificatonService, NotificationService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IDashboardService, DashboardService>();


            services.AddScoped<IAuctionAccountingService, AuctionAccountingService>();

            services.AddSingleton<IFirebaseMessagingService>(provider =>
            {
                // Assuming jsonCredentialsPath is configured elsewhere, possibly in appsettings.json
                var jsonCredentialsPath = config["Firebase:CredentialsPath"];
                return new FirebaseMessagingService(jsonCredentialsPath);
            });

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<ITokenService, TokenService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //the current position of the mapping profile
            services.Configure<CloudinarySetting>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });


            return services;
        }
    }
}
