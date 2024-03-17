using Repository.DTOs;
using Repository.Interface;
using Repository.Param;

namespace Service.Interface
{
    public interface IAccountService
    {
        IAccountRepository AccountRepository { get; }
        Task<UserDto> LoginGoogleByMember(LoginGoogleParam loginGoogleDto);
        Task<UserDto> LoginByAdminOrStaff(LoginDto loginDto);
        Task<string> GetFirebaseToken(int accountId);
    }
}
