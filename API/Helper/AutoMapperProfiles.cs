﻿using API.DTOs;
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
            CreateMap<Auction, AuctionDto>()
                .ForMember(dest => dest.AccountCreateName, opt => opt.MapFrom(src => src.AccountCreateName));
            CreateMap<Entity.Task, TaskDto>();
            CreateMap<MoneyTransaction, MoneyTransactionDto>();
            CreateMap<MoneyTransactionDetail, MoneyTransactionDetailDto>();
            CreateMap<DepositAmount, DepositAmountDto>();
            CreateMap<AuctionAccounting, AuctionAccountingDto>();

        }
    }

}
