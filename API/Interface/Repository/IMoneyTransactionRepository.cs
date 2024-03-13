using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{
    public interface IMoneyTransactionRepository : IBaseRepository<MoneyTransaction>
    {
        //Task<bool> CreateNewMoneyTransaction(TransactionMoneyCreateParam transactionMoneyCreateDto, int idAccount);
        Task<int> GetIdTransactionWhenCreateNewTransaction();
        Task<PageList<MoneyTransactionDto>> GetMoneyTransactionsAsync(MoneyTransactionRequest moneyTransactionRequest);
        Task<MoneyTransactionDetailDto> GetMoneyTransactionDetailAsync(int transactionId);
        Task<bool> InsertTransactionWhenRefund(RefundTransactionParam refundTransactionParam);
    }
}
