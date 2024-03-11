using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Service
{
    public interface IDepositAmountService
    {
        Task<PageList<DepositDto>> GetDepositAmounts(DepositAmountParam depositAmountParam);

        Task<DepositAmountDto> CreateDepositAmount(int customerId, int reasId);

        Task<DepositAmountDto> UpdateStatusToDeposited(int depositId, DateTime paymentTime);

        DepositAmountDto GetDepositAmount(int customerId, int reasId);
        DepositAmount GetDepositAmount(int depositId);
        DepositDetailDto GetDepositDetail(int depositId);
        Task<PageList<AccountDepositedDto>> GetAccountsHadDeposited(PaginationParams paginationParams, int reasId);
        Task<DepositAmountDto> UpdateStatus(int accountId, int reasId, int status);
    }
}
