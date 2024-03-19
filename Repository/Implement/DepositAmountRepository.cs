using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Entity;
using BusinessObject.Enum;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.DTOs;
using Repository.Interface;
using Repository.Paging;
using Repository.Param;

namespace Repository.Implement
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
            var query = _context.RealEstate.OrderByDescending(q => q.DateStart).Select(x => new DepositDto
            {
                reasId = x.ReasId,
                reasName = x.ReasName,
                dateEnd = x.DateEnd,
                dateStart = x.DateStart,
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
            (searchDepositAmountDto.AmountFrom == 0 && searchDepositAmountDto.AmountTo == 0 ||
            x.Amount >= searchDepositAmountDto.AmountFrom &&
            x.Amount <= searchDepositAmountDto.AmountTo &&
            (searchDepositAmountDto.DepositDateFrom == null && searchDepositAmountDto.DepositDateTo == null ||
            x.DepositDate >= searchDepositAmountDto.DepositDateFrom &&
            x.DepositDate <= searchDepositAmountDto.DepositDateTo)))
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

        public async Task UpdateDepositStatusToWaitingForRefund(int reasId)
        {
            var deposits = await _context
                .DepositAmount
                .Where(d => d.ReasId == reasId && d.Status == (int)UserDepositEnum.Deposited)
                .ToListAsync();

            deposits.ForEach(deposit => deposit.Status = (int)UserDepositEnum.Waiting_for_refund);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepositStatusToLostDepositInCaseAuctionNoAttender(int reasId)
        {
            var deposits = await _context
                .DepositAmount
                .Where(d => d.ReasId == reasId && d.Status == (int)UserDepositEnum.Deposited)
                .ToListAsync();

            deposits.ForEach(deposit => deposit.Status = (int)UserDepositEnum.LostDeposit);

            await _context.SaveChangesAsync();
        }

        public async Task<PageList<AccountDepositedDto>> GetAccountsHadDeposited(PaginationParams paginationParams, int reasId)
        {
            var query = _context.Account
                .Join(_context.DepositAmount,
                    account => account.AccountId,
                    depositAmount => depositAmount.AccountSignId,
                    (account, depositAmount) => new { Account = account, DepositAmount = depositAmount })
                .Join(_context.RealEstate,
                    joinResult => joinResult.DepositAmount.ReasId,
                    realEstate => realEstate.ReasId,
                    (joinResult, realEstate) => new { joinResult.Account, joinResult.DepositAmount, RealEstate = realEstate })
                .Where(joinResult => joinResult.DepositAmount.ReasId == reasId &&
                                     joinResult.DepositAmount.Status == (int)UserDepositEnum.Deposited &&
                                     joinResult.RealEstate.ReasStatus == (int)RealEstateStatus.Selling)
                .Select(joinResult => new AccountDepositedDto
                {
                    AccountId = joinResult.Account.AccountId,
                    AccountName = joinResult.Account.AccountName,
                    AccountEmail = joinResult.Account.AccountEmail,
                    PhoneNumber = joinResult.Account.PhoneNumber,
                    Account_Status = joinResult.Account.Account_Status.ToString(),
                    Date_Created = joinResult.Account.Date_Created
                });

            return await PageList<AccountDepositedDto>.CreateAsync(
                query.AsNoTracking(),
                paginationParams.PageNumber,
                paginationParams.PageSize);
        }
    }
}
