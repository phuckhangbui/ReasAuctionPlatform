using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Paging;
using Repository.Param;

namespace Service.Interface
{
    public interface IDepositAmountService
    {
        Task<IEnumerable<DepositDto>> GetRealEstateForDepositAsync();

        Task<DepositAmountDto> CreateDepositAmount(int customerId, int reasId);

        Task<DepositAmountDto> UpdateStatusToDeposited(int depositId, DateTime paymentTime);

        DepositAmountDto GetDepositAmount(int customerId, int reasId);
        DepositAmount GetDepositAmount(int depositId);
        Task<IEnumerable<DepositAmountUserDto>> GetDepositDetail(int depositId);

        Task<bool> ChangeStatusWhenRefund(RefundTransactionParam refundTransactionParam);
        Task<PageList<AccountDepositedDto>> GetAccountsHadDeposited(PaginationParams paginationParams, int reasId);
        Task<DepositAmountDto> UpdateStatus(int accountId, int reasId, int status);
    }
}
