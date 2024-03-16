using API.DTOs;
using API.Entity;
using API.Exceptions;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;
using API.Param.Enums;

namespace API.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IAccountRepository _accountRepository;

        public AuctionService(IAuctionRepository auctionRepository, IAccountRepository accountRepository)
        {
            _auctionRepository = auctionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<PageList<AttenderAuctionHistoryDto>> GetAuctionHisotoryForAttender(AuctionHistoryParam auctionAccountingParam)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(auctionAccountingParam.AccountId);
            if (account == null)
            {
                throw new BaseNotFoundException($"Account with ID {auctionAccountingParam.AccountId} not found.");
            }

            return await _auctionRepository.GetAuctionHistoryForAttenderAsync(auctionAccountingParam);
        }

        public async Task<PageList<AuctionDto>> GetAuctionHisotoryForOwner(AuctionHistoryParam auctionAccountingParam)
        {
            var account = await _accountRepository.GetAccountByAccountIdAsync(auctionAccountingParam.AccountId);
            if (account == null)
            {
                throw new BaseNotFoundException($"Account with ID {auctionAccountingParam.AccountId} not found.");
            }

            return await _auctionRepository.GetAuctionHistoryForOwnerAsync(auctionAccountingParam);
        }

        public async Task<Auction> CreateAuction(AuctionCreateParam auctionCreateParam)
        {
            var auction = await _auctionRepository.CreateAuction(auctionCreateParam);
            return auction;
        }

        public async Task<IEnumerable<DepositAmountUserDto>> GetAllUserForDeposit(int id)
        {
            var deposit = await _auctionRepository.GetAllUserForDeposit(id);
            return deposit;
        }

        public async Task<AuctionDetailFinish> GetAuctionDetailFinish(int id)
        {
            var auctiondetail = await _auctionRepository.GetAuctionDetailFinish(id);
            return auctiondetail;
        }

        public async Task<AuctionDetailOnGoing> GetAuctionDetailOnGoing(int id)
        {
            var auctiondetail = await _auctionRepository.GetAuctionDetailOnGoing(id);
            return auctiondetail;
        }


        public async Task<IEnumerable<AuctionDto>> GetAuctionsFinish()
        {
            var auctions = await _auctionRepository.GetAuctionsFinish();
            return auctions;
        }

        public async Task<IEnumerable<AuctionDto>> GetAuctionsNotYetAndOnGoing()
        {
            var auctions = await _auctionRepository.GetAuctionsNotYetAndOnGoing();
            return auctions;
        }

        public async Task<IEnumerable<ReasForAuctionDto>> GetAuctionsReasForCreate()
        {
            var real = await _auctionRepository.GetAuctionsReasForCreate();
            return real;
        }

        public async Task<PageList<AuctionDto>> GetRealEstates(AuctionParam auctionParam)
        {
            var auctions = await _auctionRepository.GetAuctionsAsync(auctionParam);
            return auctions;
        }

        public async Task<bool> ToggleAuctionStatus(string auctionId, string statusCode)
        {
            try
            {
                bool check = await _auctionRepository.EditAuctionStatus(auctionId, statusCode);
                if (check) { return true; }
                else return false;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<PageList<AuctionDto>> GetNotyetAndOnGoingAuction(AuctionParam auctionParam)
        {
            var auctions = await _auctionRepository.GetAuctionsAsync(auctionParam);
            var notyetAndOngoingAuctions = auctions.Where(a => int.Parse(a.Status) == (int)AuctionStatus.NotYet || int.Parse(a.Status) == (int)AuctionStatus.OnGoing);
            return auctions;
        }

        public async Task<AuctionDto> GetAuctionDetailByReasId(int reasId)
        {
            var auctionDetail = await _auctionRepository.GetAuctionDetailByReasIdAsync(reasId);

            if (auctionDetail == null)
            {
                throw new BaseNotFoundException($"Auction detail with ReasID {reasId} not found.");
            }

            return auctionDetail;
        }

        public async Task<List<int>> GetAuctionAttenders(int reasId)
        {
            return await _auctionRepository.GetAuctionAttenders(reasId);
        }

        public async Task<Auction> UpdateAuctionWhenStart(int auctionId)
        {
            Auction auction = _auctionRepository.GetAuction(auctionId);

            if (auction != null && auction.Status == (int)AuctionStatus.Pending)
            {
                auction.Status = (int)AuctionStatus.OnGoing;
                auction.DateEnd = DateTime.Now.AddHours(1);

                bool result = await _auctionRepository.UpdateAsync(auction);
                if (result)
                {
                    return auction;
                }
            }

            return null;
        }

        public async Task<List<int>> GetUserInAuction(int reasId)
        {
            return await _auctionRepository.GetUserIdInAuctionUsingReasId(reasId);
        }

        public async Task<Auction> GetAuctionByAuctionId(int auctionId)
        {
            return await _auctionRepository.GetAuctionByAuctionId(auctionId);
        }

    }
}
