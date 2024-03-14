using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Service
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
