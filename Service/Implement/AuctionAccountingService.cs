using AutoMapper;
using BusinessObject.Entity;
using BusinessObject.Enum;
using Repository.DTOs;
using Repository.Interface;
using Service.Exceptions;
using Service.Interface;
using Service.Mail;

namespace Service.Implement
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

        public async Task<AuctionAccountingDto> CreateAuctionAccounting(AuctionDetailDto auctionDetailDto)
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
            auctionAccounting.CommissionAmount = Math.Floor(auctionDetailDto.WinAmount * COMMISSION_PERCENT);
            auctionAccounting.AmountOwnerReceived = auctionDetailDto.WinAmount - auctionAccounting.CommissionAmount;

            await _auctionAccountingRepository.CreateAsync(auctionAccounting);

            AuctionAccountingDto auctionAccountingDto = _mapper.Map<AuctionAccounting, AuctionAccountingDto>(auctionAccounting);
            return auctionAccountingDto;

        }

        public async Task SendWinnerEmail(AuctionAccountingDto auctionAccounting)
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

        public async Task<AuctionAccountingDto> UpdateAuctionAccountingWinner(AuctionDetailDto auctionUpdateInformation)
        {
            AuctionAccounting auctionAccounting = _auctionAccountingRepository.GetAuctionAccountingByAuctionId(auctionUpdateInformation.AuctionId);

            Account accountWin = await _accountRepository.GetAccountByAccountIdAsync(auctionUpdateInformation.AccountWinId);

            auctionAccounting.AccountWinId = accountWin.AccountId;
            auctionAccounting.AccountWinName = accountWin.AccountName;
            auctionAccounting.MaxAmount = auctionUpdateInformation.WinAmount;
            auctionAccounting.EstimatedPaymentDate = DateTime.Now.AddDays(DATE_UNTIL_PAY);
            auctionAccounting.CommissionAmount = Math.Floor(auctionUpdateInformation.WinAmount * COMMISSION_PERCENT);
            auctionAccounting.AmountOwnerReceived = auctionUpdateInformation.WinAmount - auctionAccounting.CommissionAmount;

            await _auctionAccountingRepository.UpdateAsync(auctionAccounting);

            AuctionAccountingDto auctionAccountingDto = _mapper.Map<AuctionAccounting, AuctionAccountingDto>(auctionAccounting);
            return auctionAccountingDto;

        }

        public async Task<AuctionAccountingDto> UpdateAuctionAccountingWhenNoWinnerRemain(int auctionId)
        {
            AuctionAccounting auctionAccounting = _auctionAccountingRepository.GetAuctionAccountingByAuctionId(auctionId);

            //keep the AccountWin 
            auctionAccounting.MaxAmount = 0;
            auctionAccounting.CommissionAmount = 0;
            auctionAccounting.AmountOwnerReceived = 0;

            await _auctionAccountingRepository.UpdateAsync(auctionAccounting);

            AuctionAccountingDto auctionAccountingDto = _mapper.Map<AuctionAccounting, AuctionAccountingDto>(auctionAccounting);
            return auctionAccountingDto;
        }
    }
}
