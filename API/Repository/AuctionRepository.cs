using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using API.Param.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class AuctionRepository : BaseRepository<Auction>, IAuctionRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AuctionRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PageList<AuctionDto>> GetAuctionsAsync(AuctionParam auctionParam)
        {
            var query = _context.Auction.AsQueryable();

            query = query.Where(a => a.Status != (int)AuctionStatus.NotYet);
            query = query.OrderByDescending(a => a.DateStart);

            if (!string.IsNullOrEmpty(auctionParam.Keyword))
            {
                query = query.Where(a =>
                    a.RealEstate.ReasName.ToLower().Contains(auctionParam.Keyword.ToLower()) ||
                    a.RealEstate.ReasAddress.ToLower().Contains(auctionParam.Keyword.ToLower()) ||
                    (a.DateStart >= auctionParam.TimeStart && a.DateStart <= auctionParam.TimeEnd));
            }

            return await PageList<AuctionDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<AuctionDto>(_mapper.ConfigurationProvider),
            auctionParam.PageNumber,
            auctionParam.PageSize);
        }

        public async Task<IEnumerable<AuctionDto>> GetAuctionsNotYetAndOnGoing()
        {
            var getName = new GetStatusName();
            var query = _context.Auction.OrderByDescending(a => a.DateStart).Where(b => b.Status.Equals((int)AuctionStatus.NotYet) && b.Status.Equals((int)AuctionStatus.OnGoing) && b.Status.Equals((int)AuctionStatus.Cancel)).Select(x => new AuctionDto
            {
                AuctionId = x.AuctionId,
                ReasId = x.ReasId,
                ReasName = _context.RealEstate.Where(y => y.ReasId == x.ReasId).Select(z => z.ReasName).FirstOrDefault(),
                FloorBid = Convert.ToDouble(x.FloorBid),
                DateStart = x.DateStart,
                Status = getName.GetStatusAuctionName(x.Status),
            });
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<AuctionDto>> GetAuctionsFinish()
        {
            var getName = new GetStatusName();
            var query = _context.Auction.OrderByDescending(a => a.DateStart).Where(b => b.Status.Equals((int)AuctionStatus.Finish)).Select(x => new AuctionDto
            {
                AuctionId = x.AuctionId,
                ReasId = x.ReasId,
                ReasName = _context.RealEstate.Where(y => y.ReasId == x.ReasId).Select(z => z.ReasName).FirstOrDefault(),
                FloorBid = Convert.ToDouble(x.FloorBid),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
                Status = getName.GetStatusAuctionName(x.Status),
            });
            return await query.ToListAsync();
        }

        public async Task<bool> EditAuctionStatus(string autionId, string statusCode)
        {
            try
            {
                Auction auction = await _context.Auction.FindAsync(int.Parse(autionId));

                if (auction == null)
                {
                    throw new Exception();
                }

                auction.Status = int.Parse(statusCode);
                bool check = await UpdateAsync(auction);
                if (check) return true;
                else return false;
            }
            catch (Exception ex) { return false; }
        }

        public Auction GetAuction(int auctionId)
        {
            return _context.Auction.Find(auctionId);
        }

        public async Task<PageList<AuctionDto>> GetAuctionHistoryForOwnerAsync(AuctionHistoryParam auctionAccountingParam)
        {
            var query = _context.AuctionsAccounting
                .Where(aa => aa.AccountOwnerId == auctionAccountingParam.AccountId
                        && aa.Auction.Status == (int)AuctionStatus.Finish
                        || aa.Auction.Status == (int)AuctionStatus.Cancel)
                .Select(aa => aa.Auction)
                .AsQueryable();

            var pageList = await PageList<Auction>.CreateAsync(query, auctionAccountingParam.PageNumber, auctionAccountingParam.PageSize);

            var auctionDtos = _mapper.Map<List<AuctionDto>>(pageList);

            return new PageList<AuctionDto>(auctionDtos, pageList.TotalCount, pageList.CurrentPage, pageList.PageSize);
        }

        public async Task<PageList<AuctionDto>> GetAuctionHistoryForAttenderAsync(AuctionHistoryParam auctionAccountingParam)
        {
            var query = _context.DepositAmount
                .Where(d => d.AccountSignId == auctionAccountingParam.AccountId)
                .Join(
                    _context.RealEstate,
                    deposit => deposit.ReasId,
                    realEstate => realEstate.ReasId,
                    (deposit, realEstate) => new { Deposit = deposit, RealEstate = realEstate }
                )
                .Join(
                    _context.Auction,
                    depositRealEstate => depositRealEstate.RealEstate.ReasId,
                    auction => auction.ReasId,
                    (depositRealEstate, auction) => auction
                )
                .Where(a => a.Status == (int)AuctionStatus.Finish)
                .Distinct()
                .AsQueryable();

            var pageList = await PageList<Auction>.CreateAsync(query, auctionAccountingParam.PageNumber, auctionAccountingParam.PageSize);

            var auctionDtos = _mapper.Map<List<AuctionDto>>(pageList);

            return new PageList<AuctionDto>(auctionDtos, pageList.TotalCount, pageList.CurrentPage, pageList.PageSize);

        }

        public async Task<AuctionDetailOnGoing> GetAuctionDetailOnGoing(int id)
        {
            var getName = new GetStatusName();
            var auctiondetail = _context.Auction.Where(x => x.AuctionId == id).Select(y => new AuctionDetailOnGoing
            {
                AuctionId = y.AuctionId,
                ReasId = y.ReasId,
                ReasName = _context.RealEstate.Where(z => z.ReasId == y.ReasId).Select(z => z.ReasName).FirstOrDefault(),
                FloorBid = Convert.ToDouble(y.FloorBid),
                AccountCreateId = y.AccountCreateId,
                AccountCreateName = y.AccountCreateName,
                AccountOwnerId = _context.RealEstate.Where(a => a.ReasId == y.ReasId).Select(a => a.AccountOwnerId).FirstOrDefault(),
                AccountOwnerName = _context.RealEstate.Where(a => a.ReasId == y.ReasId).Select(a => a.AccountOwnerName).FirstOrDefault(),
                AccountOwnerEmail = _context.Account.Where(b => b.AccountId == (_context.RealEstate.Where(a => a.ReasId == y.ReasId).Select(a => a.AccountOwnerId).FirstOrDefault())).
                Select(b => b.AccountEmail).FirstOrDefault(),
                AccountOwnerPhone = _context.Account.Where(b => b.AccountId == (_context.RealEstate.Where(a => a.ReasId == y.ReasId).Select(a => a.AccountOwnerId).FirstOrDefault())).
                Select(b => b.PhoneNumber).FirstOrDefault(),
                DateStart = y.DateStart,
                Status = getName.GetStatusAuctionName(y.Status)
            });
            return await auctiondetail.FirstOrDefaultAsync();
        }

        public async Task<AuctionDetailFinish> GetAuctionDetailFinish(int id)
        {
            var getName = new GetStatusName();
            var auctiondetail = _context.Auction.Where(x => x.AuctionId == id).Select(y => new AuctionDetailFinish
            {
                AuctionId = y.AuctionId,
                ReasId = y.ReasId,
                ReasName = _context.RealEstate.Where(z => z.ReasId == y.ReasId).Select(z => z.ReasName).FirstOrDefault(),
                FloorBid = Convert.ToDouble(y.FloorBid),
                AccountCreateId = y.AccountCreateId,
                AccountCreateName = y.AccountCreateName,
                AccountOwnerId = _context.RealEstate.Where(a => a.ReasId == y.ReasId).Select(a => a.AccountOwnerId).FirstOrDefault(),
                AccountOwnerName = _context.RealEstate.Where(a => a.ReasId == y.ReasId).Select(a => a.AccountOwnerName).FirstOrDefault(),
                AccountOwnerEmail = _context.Account.Where(b => b.AccountId == (_context.RealEstate.Where(a => a.ReasId == y.ReasId).Select(a => a.AccountOwnerId).FirstOrDefault())).
                Select(b => b.AccountEmail).FirstOrDefault(),
                AccountOwnerPhone = _context.Account.Where(b => b.AccountId == (_context.RealEstate.Where(a => a.ReasId == y.ReasId).Select(a => a.AccountOwnerId).FirstOrDefault())).
                Select(b => b.PhoneNumber).FirstOrDefault(),
                AccountWinnerId = _context.AuctionsAccounting.Where(c => c.AuctionId == id).Select(c => c.AccountWinId).FirstOrDefault(),
                AccountWinnerName = _context.AuctionsAccounting.Where(c => c.AuctionId == id).Select(c => c.AccountWinName).FirstOrDefault(),
                AccountWinnerEmail = _context.Account.Where(b => b.AccountId == (_context.AuctionsAccounting.Where(c => c.AuctionId == id).Select(c => c.AccountWinId).FirstOrDefault())).
                Select(c => c.AccountEmail).FirstOrDefault(),
                AccountWinnerPhone = _context.Account.Where(b => b.AccountId == (_context.AuctionsAccounting.Where(c => c.AuctionId == id).Select(c => c.AccountWinId).FirstOrDefault())).
                Select(c => c.PhoneNumber).FirstOrDefault(),
                DepositAmout = Convert.ToDouble(_context.AuctionsAccounting.Where(c => c.AuctionId == id).Select(c => c.DepositAmount).FirstOrDefault()),
                FinalAmount = Convert.ToDouble(_context.AuctionsAccounting.Where(c => c.AuctionId == id).Select(c => c.MaxAmount).FirstOrDefault()),
                CommisionAmount = Convert.ToDouble(_context.AuctionsAccounting.Where(c => c.AuctionId == id).Select(c => c.CommissionAmount).FirstOrDefault()),
                OwnerReceiveAmount = Convert.ToDouble(_context.AuctionsAccounting.Where(c => c.AuctionId == id).Select(c => c.AmountOwnerReceived).FirstOrDefault()),
                DateEnd = y.DateEnd,
                DateStart = y.DateStart,
                Status = getName.GetStatusAuctionName(y.Status),
            });
            return await auctiondetail.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ReasForAuctionDto>> GetAuctionsReasForCreate()
        {
            var real = _context.DepositAmount.Select(x => new ReasForAuctionDto
            {
                reasId = x.ReasId,
                reasName = _context.RealEstate.Where(y => y.ReasId == x.ReasId).Select(z => z.ReasName).FirstOrDefault(),
                numberOfUser = _context.DepositAmount.Where(y => y.ReasId == x.ReasId).Count(),
            }).Distinct();
            return await real.ToListAsync();
        }

        public async Task<IEnumerable<DepositAmountUserDto>> GetAllUserForDeposit(int id)
        {
            var getName = new GetStatusName();
            var deposit = _context.DepositAmount.Where(x => (x.Status == (int)UserDepositEnum.Pending || x.Status == (int)UserDepositEnum.Deposited) && x.ReasId == id).Select(x => new DepositAmountUserDto
            {
                reasId = x.ReasId,
                accountSignId = x.AccountSignId,
                accountName = _context.Account.Where(y => y.AccountId == x.AccountSignId).Select(z => z.AccountName).FirstOrDefault(),
                accountEmail = _context.Account.Where(y => y.AccountId == x.AccountSignId).Select(z => z.AccountEmail).FirstOrDefault(),
                accountPhone = _context.Account.Where(y => y.AccountId == x.AccountSignId).Select(z => z.PhoneNumber).FirstOrDefault(),
                amount = x.Amount,
                depositDate = (DateTime)x.DepositDate,
                status = getName.GetStatusDepositName(x.Status),
            });
            return await deposit.ToListAsync();
        }

        public async Task<bool> CreateAuction(AuctionCreateParam auctionCreateParam)
        {
            try
            {
                var auction = new Auction();
                auction.ReasId = auctionCreateParam.ReasId;
                auction.AccountCreateId = auctionCreateParam.AccountCreateId;
                auction.AccountCreateName = _context.Account.Where(x => x.AccountId == auctionCreateParam.AccountCreateId).Select(x => x.AccountName).FirstOrDefault();
                auction.DateStart = auctionCreateParam.DateStart;
                auction.FloorBid = _context.RealEstate.Where(x => x.ReasId == auctionCreateParam.ReasId).Select(x => x.ReasPrice).FirstOrDefault();
                auction.Status = 0;
                bool check = await CreateAsync(auction);
                if (check)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<AuctionDto> GetAuctionDetailByReasIdAsync(int reasId)
        {
            var auction = await _context.Auction
                .Include(a => a.RealEstate)
                .SingleOrDefaultAsync(a => a.ReasId == reasId);

            return _mapper.Map<AuctionDto>(auction);
        }

        public async Task<List<int>> GetUserIdInAuctionUsingReasId(int reasId)
        {
            var deposit = await _context.DepositAmount.Where(x => x.Status == (int)UserDepositEnum.Deposited && x.ReasId == reasId).ToListAsync();

            return deposit.Select(d => d.AccountSignId).ToList();
        }
    }
}
