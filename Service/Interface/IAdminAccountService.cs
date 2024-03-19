using Repository.DTOs;
using Repository.Interface;
using Repository.Param;

namespace Service.Interface
{
    public interface IAdminAccountService
    {
        IAccountRepository AccountRepository { get; }
        Task<IEnumerable<AccountStaffDto>> GetStaffAccountBySearch(AccountParams accountParams);
        Task<IEnumerable<AccountMemberDto>> GetMemberAccountBySearch(AccountParams accountParams);
        Task<IEnumerable<AccountMemberDto>> GetMemberAccounts();
        Task<IEnumerable<AccountStaffDto>> GetStaffAccounts();
        Task<StaffInformationDto> GetStaffDetail(int idAccount);
        Task<MemberInformationDto> GetMemberDetail(int idAccount);
        Task<bool> ChangeStatusAccount(ChangeStatusAccountParam statusAccountParam);
        Task<bool> CreateNewAccountForStaff(NewAccountParam accountParam);
    }
}
