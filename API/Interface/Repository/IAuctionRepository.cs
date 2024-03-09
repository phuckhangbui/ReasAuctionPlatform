using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{

    public interface IAuctionRepository : IBaseRepository<Auction>
    {
        Task<PageList<AuctionDto>> GetAuctionsAsync(AuctionParam auctionParam);

        Task<IEnumerable<AuctionDto>> GetAuctionsNotYetAndOnGoing();
        Task<AuctionDetailOnGoing> GetAuctionDetailOnGoing(int id);
        Task<AuctionDetailFinish> GetAuctionDetailFinish(int id);
        Task<IEnumerable<AuctionDto>> GetAuctionsFinish();

        Task<IEnumerable<ReasForAuctionDto>> GetAuctionsReasForCreate();
        Task<IEnumerable<DepositAmountUserDto>> GetAllUserForDeposit(int id);
        Task<bool> EditAuctionStatus(string autionId, string statusCode);
        Task<bool> CreateAuction(AuctionCreateParam auctionCreateParam);
        Auction GetAuction(int auctionId);
        Task<PageList<AuctionDto>> GetAuctionHistoryForAttenderAsync(AuctionHistoryParam auctionAccountingParam);
        Task<PageList<AuctionDto>> GetAuctionHistoryForOwnerAsync(AuctionHistoryParam auctionAccountingParam);
    }
}
