using API.DTOs;

namespace API.Interface.Service
{
    public interface IMemberAccountService
    {
        Task<UserProfileDto> GetUserProfile(int id);
        Task<bool?> UpdateUserProfile(UserUpdateProfileInfo userProfileDto);
    }
}
