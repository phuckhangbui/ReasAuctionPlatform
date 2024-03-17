using Repository.DTOs;

namespace Service.Interface
{
    public interface IDashboardService
    {
        Task<IEnumerable<RealEstateEachTypeDto>> GetAmountRealEstateEachType();
        Task<IEnumerable<RealEstateMonthDto>> GetAmountRealEstateEachMonth();
        Task<TotalUserAuctionReasDto> GetTotalOfUserAndAuctionAndReas();
        Task<IEnumerable<NewsAdminDto>> Get3NewNewsInDashboard();
        Task<double> GetTotalRevenue();
        Task<IEnumerable<UserJoinAuctionsDto>> GetListUserJoinAuctions();
        Task<int> GetStaffActive();
    }
}
