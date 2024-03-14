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
    public class DepositAmountRepository : BaseRepository<DepositAmount>, IDepositAmountRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public DepositAmountRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DepositDto>> GetRealEstateForDepositAsync()
        {
            var query = _context.RealEstate.OrderByDescending(q => q.DateStart).Where(x => x.ReasStatus.Equals((int)RealEstateStatus.Auctioning) || x.ReasStatus.Equals((int)RealEstateStatus.Selling) || x.ReasStatus.Equals((int)RealEstateStatus.Sold)).Select(x => new DepositDto
            {
                reasId = x.ReasId,
                reasName = x.ReasName,
                dateEnd = x.DateEnd,
                status = x.ReasStatus,
            });

            return await query.ToListAsync();
        }

        public async Task<PageList<DepositAmountDto>> GetDepositAmoutForMember(int id)
        {
            var getNameStaus = new GetStatusName();
            PaginationParams paginationParams = new PaginationParams();
            var depositAmountByAccount = _context.DepositAmount.Where(x => x.AccountSignId == id).Select(x => new DepositAmountDto
            {
                DepositId = x.DepositId,
                Amount = x.Amount,
                AccountSignId = x.AccountSignId,
                DepositDate = (DateTime)x.DepositDate,
                CreateDepositDate = x.CreateDepositDate,
                ReasId = x.ReasId,
                RuleId = x.RuleId,
                Status = x.Status,
                DisplayStatus = getNameStaus.GetDepositAmountStatusName(x.Status),
            });
            depositAmountByAccount = depositAmountByAccount.OrderByDescending(x => x.DepositDate);
            return await PageList<DepositAmountDto>.CreateAsync(
            depositAmountByAccount.AsNoTracking().ProjectTo<DepositAmountDto>(_mapper.ConfigurationProvider),
            paginationParams.PageNumber,
            paginationParams.PageSize);
        }

        public async Task<PageList<DepositAmountDto>> GetDepositAmoutForMemberBySearch(SearchDepositAmountParam searchDepositAmountDto, int id)
        {
            var getNameStaus = new GetStatusName();
            PaginationParams paginationParams = new PaginationParams();
            var depositAmountBySearch = _context.DepositAmount.Where(x => x.AccountSignId == id &&
            ((searchDepositAmountDto.AmountFrom == 0 && searchDepositAmountDto.AmountTo == 0) ||
            ((x.Amount >= searchDepositAmountDto.AmountFrom) &&
            (x.Amount <= searchDepositAmountDto.AmountTo)) &&
            ((searchDepositAmountDto.DepositDateFrom == null && searchDepositAmountDto.DepositDateTo == null) ||
            (x.DepositDate >= searchDepositAmountDto.DepositDateFrom &&
            x.DepositDate <= searchDepositAmountDto.DepositDateTo))))
            .Select(x => new DepositAmountDto
            {
                DepositId = x.DepositId,
                Amount = x.Amount,
                AccountSignId = x.AccountSignId,
                DepositDate = (DateTime)x.DepositDate,
                CreateDepositDate = x.CreateDepositDate,
                ReasId = x.ReasId,
                RuleId = x.RuleId,
                Status = x.Status,
                DisplayStatus = getNameStaus.GetDepositAmountStatusName(x.Status),
            });

            depositAmountBySearch = depositAmountBySearch.OrderByDescending(x => x.DepositDate);
            return await PageList<DepositAmountDto>.CreateAsync(
            depositAmountBySearch.AsNoTracking().ProjectTo<DepositAmountDto>(_mapper.ConfigurationProvider),
            paginationParams.PageNumber,
            paginationParams.PageSize);
        }


        public List<DepositAmount> GetDepositAmounts(int accountSignId, int reasId) => _context.DepositAmount.Where(d => d.AccountSignId == accountSignId && d.ReasId == reasId).ToList();

        public DepositAmount GetDepositAmount(int accountSignId, int reasId)
        {
            return _context.DepositAmount
                .FirstOrDefault(d => d.AccountSignId == accountSignId && d.ReasId == reasId);
        }


        public DepositAmount GetDepositAmount(int depositId)
        {
            return _context.DepositAmount.FirstOrDefault(d => d.DepositId == depositId);
        }

        public async Task<bool> ChangeStatusWaiting(int id)
        {
            var deposit = _context.DepositAmount.Where(x => x.DepositId == id).FirstOrDefault();
            deposit.Status = 3;
            bool check = await UpdateAsync(deposit);
            if (check)
            {
                return true;
            }
            else { return false; }
        }

        public async System.Threading.Tasks.Task UpdateDepositStatusToWaitingForRefund(int reasId)
        {
            var deposits = await _context
                .DepositAmount
                .Where(d => d.ReasId == reasId && d.Status == (int)UserDepositEnum.Deposited)
                .ToListAsync();

            deposits.ForEach(deposit => deposit.Status = (int)UserDepositEnum.Waiting_for_refund);

            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateDepositStatusToLostDepositInCaseAuctionNoAttender(int reasId)
        {
            var deposits = await _context
                .DepositAmount
                .Where(d => d.ReasId == reasId && d.Status == (int)UserDepositEnum.Deposited)
                .ToListAsync();

            deposits.ForEach(deposit => deposit.Status = (int)UserDepositEnum.LostDeposit);

            await _context.SaveChangesAsync();
        }
    }
}
