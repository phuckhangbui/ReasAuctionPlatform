
﻿//using API.Data;
//using API.DTOs;
//using API.Entity;
//using API.Interface.Repository;
//using API.Param;
//using API.Validate;
//using AutoMapper;
//using Microsoft.EntityFrameworkCore;


//namespace API.Repository
//{
//    public class MoneyTransactionDetailRepository : BaseRepository<MoneyTransactionDetail>, IMoneyTransactionDetailRepository
//    {
//        private readonly DataContext _dataContext;
//        private readonly IAccountRepository _accountRepository;
//        private readonly IMapper _mapper;
//        public MoneyTransactionDetailRepository(DataContext context, 
//            IAccountRepository accountRepository,
//            IMapper mapper) : base(context)
//        {
//            _dataContext = context;
//            _accountRepository = accountRepository;
//            _mapper = mapper;
//        }

//        public async Task<bool> CreateNewMoneyTransaction(TransactionMoneyCreateParam transactionMoneyCreateDto, int idTransaction)
//        {
//            MoneyTransactionDetail moneyTransactionDetail = new MoneyTransactionDetail();
//            moneyTransactionDetail.MoneyTransactionId = idTransaction;
//            moneyTransactionDetail.AccountReceiveId = await _accountRepository.GetIdAccountToReceiveMoney();
//            moneyTransactionDetail.DateExecution = DateTime.UtcNow;
//            moneyTransactionDetail.ReasId = transactionMoneyCreateDto.IdReas;
//            moneyTransactionDetail.TotalAmmount = transactionMoneyCreateDto.Money;
//            moneyTransactionDetail.PaidAmount = transactionMoneyCreateDto.MoneyPaid;
//            int kq = (int)(transactionMoneyCreateDto.Money - transactionMoneyCreateDto.MoneyPaid);
//            moneyTransactionDetail.RemainingAmount = kq;
//            try
//            {
//                bool check = await CreateAsync(moneyTransactionDetail);
//                if (check) { return true; }
//                else return false;
//            }catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public async Task<MoneyTransactionDetailDto> GetMoneyTransactionDetailAsync(int transactionId)
//        {
//            var transactionDetail = await _dataContext.MoneyTransactionDetail.SingleOrDefaultAsync(m => m.MoneyTransactionId == transactionId);
//            if (transactionDetail != null)
//            {
//                return _mapper.Map<MoneyTransactionDetailDto>(transactionDetail);
//            }

//            return null;
//        }
//    }
//}
