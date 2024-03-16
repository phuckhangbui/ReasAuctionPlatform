using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Paging;
using Repository.Param;

namespace Service.Interface
{
    public interface IMoneyTransactionService
    {
        Task<PageList<MoneyTransactionDto>> GetMoneyTransactions(MoneyTransactionRequest moneyTransactionRequest);
        Task<MoneyTransactionDetailDto> GetMoneyTransactionDetail(int transactionId);
        //Task<MoneyTransaction> CreateMoneyTransactionFromDepositPayment(DepositPaymentDto paymentDto);

        Task<bool> CreateMoneyTransaction(MoneyTransaction moneyTransaction);
        Task<PageList<MoneyTransactionDto>> GetMemberMoneyTransactions(MemberMoneyTransactionParam memberMoneyTransactionParam, int accountId);
        Task<MoneyTransactionDetailDto> GetMemberMoneyTransactionDetail(int accountId, int transactionId);
    }
}
