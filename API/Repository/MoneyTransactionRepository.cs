using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class MoneyTransactionRepository : BaseRepository<MoneyTransaction>, IMoneyTransactionRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public MoneyTransactionRepository(DataContext context, IMapper mapper) : base(context)
        {
            _dataContext = context;
            _mapper = mapper;
        }

        //public async Task<bool> CreateNewMoneyTransaction(TransactionMoneyCreateParam transactionMoneyCreateDto, int idAccount)
        //{
        //    MoneyTransaction moneyTransaction = new MoneyTransaction();
        //    moneyTransaction.TransactionStatus = (int)TransactionEnum.Received;
        //    moneyTransaction.TypeId = 3;
        //    moneyTransaction.DateExecution = DateTime.UtcNow;
        //    moneyTransaction.AccountSendId = idAccount;
        //    moneyTransaction.Money = transactionMoneyCreateDto.MoneyPaid;
        //    try
        //    {
        //        bool check = await CreateAsync(moneyTransaction);
        //        if (check)
        //        {
        //            return true;
        //        }
        //        else return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public async Task<int> GetIdTransactionWhenCreateNewTransaction()
        {
            return await _dataContext.MoneyTransaction.MaxAsync(x => x.TransactionId);
        }

        public async Task<PageList<MoneyTransactionDto>> GetMemberMoneyTransactionsAsync(
            MemberMoneyTransactionParam memberMoneyTransactionParam, int accountId)
        {
            var query = _dataContext.MoneyTransaction.AsQueryable();

            query = query.Where(m => m.AccountSendId == accountId);
            query = query.Include(m => m.Type);
            DateTime dateExecutionFrom;
            DateTime dateExecutionTo;

            if (!string.IsNullOrEmpty(memberMoneyTransactionParam.DateExecutionFrom))
            {
                dateExecutionFrom = DateTime.Parse(memberMoneyTransactionParam.DateExecutionFrom);
                query = query.Where(m => m.DateExecution >= dateExecutionFrom);
            }
            else if (!string.IsNullOrEmpty(memberMoneyTransactionParam.DateExecutionTo))
            {
                dateExecutionTo = DateTime.Parse(memberMoneyTransactionParam.DateExecutionTo);
                query = query.Where(m => m.DateExecution <= dateExecutionTo);
            }
            else if (!string.IsNullOrEmpty(memberMoneyTransactionParam.DateExecutionFrom) &&
                    !string.IsNullOrEmpty(memberMoneyTransactionParam.DateExecutionTo))
            {
                dateExecutionFrom = DateTime.Parse(memberMoneyTransactionParam.DateExecutionFrom);
                dateExecutionTo = DateTime.Parse(memberMoneyTransactionParam.DateExecutionTo);

                query = query.Where(m => m.DateExecution >= dateExecutionFrom
                    && m.DateExecution <= dateExecutionTo);
            }

            query = query.OrderByDescending(r => r.DateExecution);

            return await PageList<MoneyTransactionDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<MoneyTransactionDto>(_mapper.ConfigurationProvider),
                memberMoneyTransactionParam.PageNumber,
                memberMoneyTransactionParam.PageSize);
        }

        public async Task<MoneyTransactionDetailDto> GetMoneyTransactionDetailAsync(int transactionId)
        {
            var moneyTransaction = await _dataContext.MoneyTransaction
                .Include(m => m.AccountReceive)
                .Include(m => m.AccountSend)
                .Include(m => m.RealEstate)
                .Include(m => m.Type)
                .FirstOrDefaultAsync(m => m.TransactionId == transactionId);

            return _mapper.Map<MoneyTransactionDetailDto>(moneyTransaction);
        }

        public async Task<PageList<MoneyTransactionDto>> GetMoneyTransactionsAsync(MoneyTransactionRequest moneyTransactionRequest)
        {
            PaginationParams paginationParams = new PaginationParams();
            var query = _dataContext.MoneyTransaction.AsQueryable();

            query = query.Include(m => m.Type);
            DateTime dateExecutionFrom;
            DateTime dateExecutionTo;

            if (!string.IsNullOrEmpty(moneyTransactionRequest.DateExecutionFrom))
            {
                dateExecutionFrom = DateTime.Parse(moneyTransactionRequest.DateExecutionFrom);
                query = query.Where(m => m.DateExecution >= dateExecutionFrom);
            }
            else if (!string.IsNullOrEmpty(moneyTransactionRequest.DateExecutionTo))
            {
                dateExecutionTo = DateTime.Parse(moneyTransactionRequest.DateExecutionTo);
                query = query.Where(m => m.DateExecution <= dateExecutionTo);
            }
            else if (!string.IsNullOrEmpty(moneyTransactionRequest.DateExecutionFrom) &&
                    !string.IsNullOrEmpty(moneyTransactionRequest.DateExecutionTo))
            {
                dateExecutionFrom = DateTime.Parse(moneyTransactionRequest.DateExecutionFrom);
                dateExecutionTo = DateTime.Parse(moneyTransactionRequest.DateExecutionTo);

                query = query.Where(m => m.DateExecution >= dateExecutionFrom
                    && m.DateExecution <= dateExecutionTo);
            }

            query = query.OrderByDescending(r => r.DateExecution);

            return await PageList<MoneyTransactionDto>.CreateAsync(
                query.AsNoTracking().ProjectTo<MoneyTransactionDto>(_mapper.ConfigurationProvider),
                paginationParams.PageNumber,
                paginationParams.PageSize);
        }

        //public async System.Threading.Tasks.Task CreateMoneyTransactionAndMoneyTransactionDetail(MoneyTransaction moneyTransaction, MoneyTransactionDetail moneyTransactionDetail)
        //{
        //    try
        //    {
        //        //_dataContext.MoneyTransaction.Add(moneyTransaction);
        //        //_dataContext.SaveChanges();
        //        moneyTransactionDetail.MoneyTransaction = moneyTransaction;


        //        _dataContext.MoneyTransactionDetail.Add(moneyTransactionDetail);
        //        _dataContext.SaveChanges();


        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}
