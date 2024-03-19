using BusinessObject.Enum;
using Repository.Data;
using Repository.DTOs;
using Repository.Interface;

namespace Repository.Implement
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DataContext _dataContext;
        public DashboardRepository(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IEnumerable<NewsAdminDto>> Get3NewNewsInDashboard()
        {
            var news = _dataContext.News.OrderByDescending(x => x.DateCreated).Select(x => new NewsAdminDto
            {
                DateCreated = x.DateCreated,
                NewsId = x.NewsId,
                NewsSumary = x.NewsSumary,
                NewsTitle = x.NewsTitle,
                Thumbnail = x.Thumbnail,
            }).Take(3);
            return news;
        }

        public async Task<IEnumerable<RealEstateMonthDto>> GetAmountRealEstateEachMonth()
        {
            var reals = _dataContext.RealEstate
        .GroupBy(x => x.DateCreated.Month)
        .Select(group => new RealEstateMonthDto
        {
            month = new DateTime(1, group.Key, 1).ToString("MMMM"),
            numberOfReas = group.Count()
        });
            return reals;
        }

        public async Task<IEnumerable<RealEstateEachTypeDto>> GetAmountRealEstateEachType()
        {
            var reals = _dataContext.type_REAS.Select(x => new RealEstateEachTypeDto
            {
                typeId = x.Type_ReasId,
                typeName = x.Type_Reas_Name,
                numberOfReas = _dataContext.RealEstate.Where(y => y.Type_Reas == x.Type_ReasId).Count(),
            });
            return reals;
        }

        public async Task<IEnumerable<UserJoinAuctionsDto>> GetListUserJoinAuctions()
        {
            var users = _dataContext.DepositAmount
                        .GroupBy(x => x.AccountSignId)
                        .Select(y => new UserJoinAuctionsDto
                        {
                            accountId = y.Key,
                            accountEmail = _dataContext.Account.Where(z => z.AccountId == y.Key).Select(z => z.AccountEmail).FirstOrDefault(),
                            accountName = _dataContext.Account.Where(z => z.AccountId == y.Key).Select(z => z.AccountName).FirstOrDefault(),
                            numberOfAuction = y.Count(),
                        })
                        .OrderByDescending(user => user.numberOfAuction)
                        .Take(6);
            return users;
        }

        public async Task<int> GetStaffActive()
        {
            var countUsers = _dataContext.Account.Where(x => x.RoleId == 2 || x.Account_Status == 1).Count();
            return countUsers;
        }

        public async Task<TotalUserAuctionReasDto> GetTotalOfUserAndAuctionAndReas()
        {
            TotalUserAuctionReasDto total = new TotalUserAuctionReasDto();
            total.numberOfUser = _dataContext.Account.Where(x => x.RoleId == 3).Count();
            total.numberOfReas = _dataContext.RealEstate.Count();
            total.numberOfAuction = _dataContext.Auction.Count();
            return total;
        }

        public async Task<double> GetTotalRevenue()
        {
            var totalRevenue = _dataContext.MoneyTransaction.Where(x => x.TypeId == (int)TransactionType.Upload_Fee || x.TypeId == (int)TransactionType.Commistion_Fee)
                                                            .Select(x => x.Money).Sum();
            return totalRevenue;
        }
    }
}
