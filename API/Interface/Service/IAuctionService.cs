using API.DTOs;
using API.Helper;
using API.Param;


namespace API.Interface.Service
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
        Task<PageList<AuctionDto>> GetAuctionHisotoryForAttender(AuctionHistoryParam auctionAccountingParam);
        Task<bool> CreateAuction(AuctionCreateParam auctionCreateParam);
    }
}
