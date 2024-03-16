using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Paging;
using Repository.Param;

namespace Service.Interface
{
    public interface IAuctionService
    {
        Task<PageList<AuctionDto>> GetRealEstates(AuctionParam auctionParam);
        Task<IEnumerable<AuctionDto>> GetAuctionsNotYetAndOnGoing();
        Task<AuctionDetailOnGoing> GetAuctionDetailOnGoing(int id);
        Task<AuctionDetailFinish> GetAuctionDetailFinish(int id);
        Task<IEnumerable<AuctionDto>> GetAuctionsFinish();
        Task<IEnumerable<ReasForAuctionDto>> GetAuctionsReasForCreate();
        Task<IEnumerable<DepositAmountUserDto>> GetAllUserForDeposit(int id);
        Task<bool> ToggleAuctionStatus(string auctionId, string statusCode);
        Task<PageList<AuctionDto>> GetAuctionHisotoryForOwner(AuctionHistoryParam auctionAccountingParam);
        Task<PageList<AttenderAuctionHistoryDto>> GetAuctionHisotoryForAttender(AuctionHistoryParam auctionAccountingParam);
        Task<PageList<AuctionDto>> GetNotyetAndOnGoingAuction(AuctionParam auctionParam);
        Task<AuctionCreationResult> CreateAuction(AuctionCreateParam auctionCreateParam);
        Task<AuctionDto> GetAuctionDetailByReasId(int reasId);
        Task<List<int>> GetAuctionAttenders(int reasId);
        Task<Auction> UpdateAuctionWhenStart(int auctionId);
        Task<List<int>> GetUserInAuction(int reasId);
    }
}
