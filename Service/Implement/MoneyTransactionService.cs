using BusinessObject.Entity;
using BusinessObject.Enum;
using Repository.DTOs;
using Repository.Interface;
using Repository.Paging;
using Repository.Param;
using Service.Exceptions;
using Service.Interface;

namespace Service.Implement
{
    public class MoneyTransactionService : IMoneyTransactionService
    {
        private readonly IMoneyTransactionRepository _moneyTransactionRepository;
        private readonly IAccountRepository _accountRepository;

        public MoneyTransactionService(IMoneyTransactionRepository moneyTransactionRepository, IAccountRepository accountRepository)
        {
            _moneyTransactionRepository = moneyTransactionRepository;
            _accountRepository = accountRepository;
        }

        public async Task<MoneyTransactionDetailDto> GetMoneyTransactionDetail(int transactionId)
        {
            var transactionDetail = await _moneyTransactionRepository.GetMoneyTransactionDetailAsync(transactionId);

            if (transactionDetail == null)
            {
                throw new BaseNotFoundException($"Transaction detail with ID {transactionId} not found.");
            }

            return transactionDetail;
        }

        public Task<PageList<MoneyTransactionDto>> GetMoneyTransactions(MoneyTransactionRequest moneyTransactionRequest)
        {
            return _moneyTransactionRepository.GetMoneyTransactionsAsync(moneyTransactionRequest);
        }

        public async Task<bool> CreateMoneyTransaction(MoneyTransaction moneyTransaction)
        {
            return await _moneyTransactionRepository.CreateAsync(moneyTransaction);
        }

        public async Task<PageList<MoneyTransactionDto>> GetMemberMoneyTransactions(MemberMoneyTransactionParam memberMoneyTransactionParam, int accountId)
        {
            var memberAccount = await _accountRepository.GetAccountByAccountIdAsync(accountId);

            if (memberAccount == null || memberAccount.RoleId != (int)RoleEnum.Member)
            {
                throw new BaseNotFoundException($"This account with id {accountId} not found or not member role");
            }

            return await _moneyTransactionRepository.GetMemberMoneyTransactionsAsync(memberMoneyTransactionParam, accountId);
        }

        public async Task<MoneyTransactionDetailDto> GetMemberMoneyTransactionDetail(int accountId, int transactionId)
        {
            var memberAccount = await _accountRepository.GetAccountByAccountIdAsync(accountId);

            if (memberAccount == null || memberAccount.RoleId != (int)RoleEnum.Member)
            {
                throw new BaseNotFoundException($"This account with id {accountId} not found or not member role");
            }

            var transactionDetail = await _moneyTransactionRepository.GetMoneyTransactionDetailAsync(transactionId);

            if (transactionDetail == null)
            {
                throw new BaseNotFoundException($"Transaction detail with ID {transactionId} not found.");
            }

            return transactionDetail;
        }

        //public async Task<MoneyTransaction> CreateMoneyTransactionFromDepositPayment(DepositPaymentDto paymentDto)
        //{
        //    MoneyTransaction moneyTransaction = new MoneyTransaction();

        //    moneyTransaction.TypeId = 1;
        //    moneyTransaction.AccountSendId = paymentDto.CustomerId;
        //    moneyTransaction.TransactionStatus = (int)TransactionEnum.Received;
        //    moneyTransaction.Money = paymentDto.Money;
        //    moneyTransaction.DateExecution = paymentDto.PaymentTime;


        //    //MoneyTransactionDetail moneyTransactionDetail = new MoneyTransactionDetail();
        //    //moneyTransactionDetail.ReasId = paymentDto.ReasId;
        //    //moneyTransactionDetail.TotalAmmount = paymentDto.Money;
        //    //moneyTransactionDetail.PaidAmount = paymentDto.Money;
        //    //moneyTransactionDetail.RemainingAmount = 0;
        //    //moneyTransactionDetail.DateExecution = paymentDto.PaymentTime;
        //    //moneyTransactionDetail.AccountReceiveId = null;
        //    //moneyTransactionDetail.AuctionId = null;
        //    ////moneyTransactionDetail.MoneyTransactionDetailId = null;

        //    //await _moneyTransactionRepository.CreateMoneyTransactionAndMoneyTransactionDetail(moneyTransaction, moneyTransactionDetail);

        //    return moneyTransaction;
        //}
    }
}
