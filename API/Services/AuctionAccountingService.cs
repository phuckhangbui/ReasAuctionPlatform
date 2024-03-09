using API.DTOs;
using API.Entity;
using API.Exceptions;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param.Enums;
using API.ThirdServices;
using AutoMapper;

namespace API.Services
{
    public class AuctionAccountingService : IAuctionAccountingService
    {
        public readonly IAuctionAccountingRepository _auctionAccountingRepository;
        public readonly IAuctionRepository _auctionRepository;
        public readonly IAccountRepository _accountRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;
        private readonly IDepositAmountRepository _depositAmountRepository;

        private readonly IMapper _mapper;

        readonly float COMMISSION_PERCENT = 0.02f;
        readonly int DATE_UNTIL_PAY = 3;

        public AuctionAccountingService(IAuctionAccountingRepository auctionAccountingRepository, IAuctionRepository auctionRepository, IAccountRepository accountRepository, IRealEstateDetailRepository realEstateDetailRepository, IDepositAmountRepository depositAmountRepository, IMapper mapper)
        {
            _auctionAccountingRepository = auctionAccountingRepository;
            _auctionRepository = auctionRepository;
            _accountRepository = accountRepository;
            _realEstateDetailRepository = realEstateDetailRepository;
            _depositAmountRepository = depositAmountRepository;
            _mapper = mapper;
        }

        public async System.Threading.Tasks.Task<AuctionAccountingDto> UpdateAuctionAccounting(AuctionDetailDto auctionDetailDto)
        {
            //get auction accounting
            AuctionAccounting auctionAccountingOld = _auctionAccountingRepository.GetAuctionAccountingByAuctionId(auctionDetailDto.AuctionId);

            if (auctionAccountingOld != null)
            {
                ICollection<AuctionAccounting> list = new List<AuctionAccounting>();
                list.Add(auctionAccountingOld);
                await _auctionAccountingRepository.DeleteAsync(list);
            }
            AuctionAccounting auctionAccounting = new AuctionAccounting();
            Auction auction = _auctionRepository.GetAuction(auctionDetailDto.AuctionId);
            var realEstate = await _realEstateDetailRepository.GetRealEstateDetail(auction.ReasId);

            if (realEstate.ReasStatus != (int)RealEstateStatus.Auctioning)
            {
                return null;
            }

            //create new auction accounting base on input
            Account accountWin = await _accountRepository.GetAccountByAccountIdAsync(auctionDetailDto.AccountWinId);
            DepositAmount depositAmount = _depositAmountRepository.GetDepositAmount(auctionDetailDto.AccountWinId, auction.ReasId);

            auctionAccounting.AuctionId = auctionDetailDto.AuctionId;
            auctionAccounting.ReasId = auction.ReasId;
            auctionAccounting.AccountWinId = auctionDetailDto.AccountWinId;
            auctionAccounting.AccountWinName = accountWin.AccountName;
            auctionAccounting.AccountOwnerId = realEstate.AccountOwnerId;
            auctionAccounting.AccountOwnerName = realEstate.AccountOwnerName;
            auctionAccounting.EstimatedPaymentDate = DateTime.Now.AddDays(DATE_UNTIL_PAY);

            auctionAccounting.MaxAmount = auctionDetailDto.WinAmount;
            auctionAccounting.DepositAmount = depositAmount.Amount;
            auctionAccounting.CommissionAmount = auctionDetailDto.WinAmount * COMMISSION_PERCENT;
            auctionAccounting.AmountOwnerReceived = auctionDetailDto.WinAmount - auctionAccounting.CommissionAmount;

            await _auctionAccountingRepository.CreateAsync(auctionAccounting);

            AuctionAccountingDto auctionAccountingDto = _mapper.Map<AuctionAccounting, AuctionAccountingDto>(auctionAccounting);
            return auctionAccountingDto;

        }

        public async System.Threading.Tasks.Task SendWinnerEmail(AuctionAccountingDto auctionAccounting)
        {
            Auction auction = _auctionRepository.GetAuction(auctionAccounting.AuctionId);
            var realEstate = await _realEstateDetailRepository.GetRealEstateDetail(auction.ReasId);
            Account accountWin = await _accountRepository.GetAccountByAccountIdAsync(auctionAccounting.AccountWinId);


            SendMailAuctionSuccess.SendMailWhenAuctionSuccess(accountWin.AccountEmail, realEstate.ReasName, realEstate.ReasAddress, DateOnly.FromDateTime(auctionAccounting.EstimatedPaymentDate), auctionAccounting.MaxAmount, auctionAccounting.DepositAmount);
        }

        public async Task<AuctionAccountingDto> GetAuctionAccounting(int auctionId)
        {
            var auctionAccouting = _auctionAccountingRepository.GetAuctionAccountingByAuctionId(auctionId);
            if (auctionAccouting == null) 
            {
                throw new BaseNotFoundException($"AuctionAccounting with auction ID {auctionId} not found.");
            }
            
            return _mapper.Map<AuctionAccountingDto>(auctionAccouting);
        }
    }
}
