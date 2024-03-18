using Repository.DTOs;
using static Repository.DTOs.AuctionResultDto;

namespace Service.Interface
{
    public interface IFirebaseAuctionService
    {
        Task<AuctionData> GetAuctionDataAsync(int auctionId);
    }
}
