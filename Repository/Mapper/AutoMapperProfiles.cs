using AutoMapper;
using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Param;

namespace Repository.Mapper
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
            CreateMap<MoneyTransaction, MoneyTransactionDto>()
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.Type.TypeName));
            CreateMap<DepositAmount, DepositAmountDto>();
            CreateMap<AuctionAccounting, AuctionAccountingDto>();
            CreateMap<MoneyTransaction, MoneyTransactionDetailDto>()
                .ForMember(dest => dest.AccountSendName, opt => opt.MapFrom(src => src.AccountSend.AccountName))
                .ForMember(dest => dest.AccountReceiveName, opt => opt.MapFrom(src => src.AccountReceive.AccountName))
                .ForMember(dest => dest.ReasName, opt => opt.MapFrom(src => src.RealEstate.ReasName))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.Type.TypeName))
                .ForMember(dest => dest.AccountSendBankingNumber, opt => opt.MapFrom(src => src.AccountSend.BankingNumber))
                .ForMember(dest => dest.AccountSendBankingCode, opt => opt.MapFrom(src => src.AccountSend.BankingCode))
                .ForMember(dest => dest.AccountReceiveBankingNumber, opt => opt.MapFrom(src => src.AccountReceive.BankingNumber))
                .ForMember(dest => dest.AccountReceiveBankingCode, opt => opt.MapFrom(src => src.AccountReceive.BankingCode));
            CreateMap<Auction, AuctionDto>()
                .ForMember(dest => dest.ReasName, opt => opt.MapFrom(src => src.RealEstate.ReasName));
            CreateMap<Account, UserProfileDto>()
                .ForMember(dest => dest.MajorName, opt => opt.MapFrom(src => src.Major.MajorName))
                .ForMember(dest => dest.CitizenIdentification, opt => opt.MapFrom(src => src.Citizen_identification))
                .ReverseMap();
            CreateMap<Account, AuctionAttenderDto>();
        }
    }

}
