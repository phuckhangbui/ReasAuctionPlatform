using API.DTOs;

namespace API.Interface.Service
{
    public interface IAuctionAccountingService
    {
        Task<AuctionAccountingDto> UpdateAuctionAccounting(AuctionDetailDto auctionDetailDto);
        System.Threading.Tasks.Task SendWinnerEmail(AuctionAccountingDto auctionAccounting);
        Task<AuctionAccountingDto> GetAuctionAccounting(int auctionId);
    }
}
