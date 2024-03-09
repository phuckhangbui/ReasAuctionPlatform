using API.DTOs;
using API.Interface.Repository;
using API.Param;

namespace API.Interface.Service
{
    public interface IAccountService
    {
        IAccountRepository AccountRepository { get; }
        Task<UserDto> LoginGoogleByMember(LoginGoogleParam loginGoogleDto);
        Task<UserDto> LoginByAdminOrStaff(LoginDto loginDto);
    }
}
