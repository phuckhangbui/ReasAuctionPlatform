using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Paging;
using Repository.Param;

namespace Repository.Interface
{
    public interface IMoneyTransactionRepository : IBaseRepository<MoneyTransaction>
    {
        //Task<bool> CreateNewMoneyTransaction(TransactionMoneyCreateParam transactionMoneyCreateDto, int idAccount);
        Task<int> GetIdTransactionWhenCreateNewTransaction();
        Task<PageList<MoneyTransactionDto>> GetMoneyTransactionsAsync(MoneyTransactionRequest moneyTransactionRequest);
        Task<MoneyTransactionDetailDto> GetMoneyTransactionDetailAsync(int transactionId);
        Task<bool> InsertTransactionWhenRefund(RefundTransactionParam refundTransactionParam);
        Task<PageList<MoneyTransactionDto>> GetMemberMoneyTransactionsAsync(MemberMoneyTransactionParam memberMoneyTransactionParam, int accountId);
    }
}
