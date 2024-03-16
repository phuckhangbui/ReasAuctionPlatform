using Repository.DTOs;

namespace Service.Interface
{
    public interface IAuctionAccountingService
    {
        Task<AuctionAccountingDto> CreateAuctionAccounting(AuctionDetailDto auctionDetailDto);
        Task SendWinnerEmail(AuctionAccountingDto auctionAccounting);
        Task<AuctionAccountingDto> GetAuctionAccounting(int auctionId);
        Task<AuctionAccountingDto> UpdateAuctionAccountingWinner(AuctionDetailDto auctionUpdateInformation);
        Task<AuctionAccountingDto> UpdateAuctionAccountingWhenNoWinnerRemain(int auctionId);
    }
}
