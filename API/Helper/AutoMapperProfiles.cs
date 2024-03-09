using API.DTOs;
using API.Entity;
using API.Param;
using AutoMapper;

namespace API.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, Account>();
            CreateMap<NewAccountParam, Account>();
            CreateMap<AccountStaffDto, AccountStaffDto>();
            CreateMap<AccountMemberDto, AccountMemberDto>();
            CreateMap<ChangeStatusAccountParam, Account>();
            CreateMap<RuleChangeContentParam, Rule>();
            CreateMap<AccountMemberDto, Account>();
            CreateMap<AccountStaffDto, Account>();
            CreateMap<RealEstateDto, RealEstateDto>();
            CreateMap<News, NewsDto>();
            CreateMap<Rule, Rule>();
            CreateMap<RealEstate, RealEstateDto>();
            CreateMap<RealEstatePhoto, RealEstatePhotoDto>();
            CreateMap<Entity.Task, TaskDto>();
            CreateMap<MoneyTransaction, MoneyTransactionDto>()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.Type.TypeName));
            CreateMap<DepositAmount, DepositAmountDto>();
            CreateMap<AuctionAccounting, AuctionAccountingDto>();
            CreateMap<MoneyTransaction, MoneyTransactionDetailDto>()
                .ForMember(dest => dest.AccountSendName, opt => opt.MapFrom(src => src.AccountSend.AccountName))
                .ForMember(dest => dest.AccountReceiveName, opt => opt.MapFrom(src => src.AccountReceive.AccountName))
                .ForMember(dest => dest.ReasName, opt => opt.MapFrom(src => src.RealEstate.ReasName))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.Type.TypeName));
            CreateMap<DepositAmount, DepositDto>()
                .ForMember(dest => dest.ReasName, opt => opt.MapFrom(src => src.RealEstate.ReasName))
                .ForMember(dest => dest.AccountSignName, opt => opt.MapFrom(src => src.AccountSign.AccountName));
            CreateMap<DepositAmount, DepositDetailDto>()
                .ForMember(dest => dest.ReasName, opt => opt.MapFrom(src => src.RealEstate.ReasName))
                .ForMember(dest => dest.AccountSignName, opt => opt.MapFrom(src => src.AccountSign.AccountName));
        }
    }

}
