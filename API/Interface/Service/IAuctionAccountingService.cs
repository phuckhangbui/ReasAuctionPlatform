using API.DTOs;

namespace API.Interface.Service
{
    public interface IAuctionAccountingService
    {
        Task<AuctionAccountingDto> CreateAuctionAccounting(AuctionDetailDto auctionDetailDto);
        System.Threading.Tasks.Task SendWinnerEmail(AuctionAccountingDto auctionAccounting);
        Task<AuctionAccountingDto> GetAuctionAccounting(int auctionId);
        Task<AuctionAccountingDto> UpdateAuctionAccountingWinner(AuctionDetailDto auctionUpdateInformation);
    }
}
