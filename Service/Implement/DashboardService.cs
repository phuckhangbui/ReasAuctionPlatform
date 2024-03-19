using Repository.DTOs;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<IEnumerable<NewsAdminDto>> Get3NewNewsInDashboard()
        {
            var news = await _dashboardRepository.Get3NewNewsInDashboard();
            return news;
        }

        public async Task<IEnumerable<RealEstateMonthDto>> GetAmountRealEstateEachMonth()
        {
            var reals = await _dashboardRepository.GetAmountRealEstateEachMonth();
            return reals;
        }

        public async Task<IEnumerable<RealEstateEachTypeDto>> GetAmountRealEstateEachType()
        {
            var reals = await _dashboardRepository.GetAmountRealEstateEachType();
            return reals;
        }

        public async Task<IEnumerable<UserJoinAuctionsDto>> GetListUserJoinAuctions()
        {
            var users = await _dashboardRepository.GetListUserJoinAuctions();
            return users;
        }

        public async Task<int> GetStaffActive()
        {
            var countUser = await _dashboardRepository.GetStaffActive();
            return countUser;
        }

        public async Task<TotalUserAuctionReasDto> GetTotalOfUserAndAuctionAndReas()
        {
            var total = await _dashboardRepository.GetTotalOfUserAndAuctionAndReas();
            return total;
        }

        public async Task<double> GetTotalRevenue()
        {
            var totalRevenue = await _dashboardRepository.GetTotalRevenue();
            return totalRevenue;
        }
    }
}
