using Repository.DTOs;

namespace Service.Interface
{
    public interface IMemberAccountService
    {
        Task<UserProfileDto> GetUserProfile(int id);
        Task<bool?> UpdateUserProfile(UserProfileDto userProfileDto);
    }
}
