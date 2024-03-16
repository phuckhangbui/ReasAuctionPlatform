using API.DTOs;
using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;

namespace API.Services
{
    public class MemberAccountService : IMemberAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public MemberAccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<UserProfileDto> GetUserProfile(int id)
        {
            return await _accountRepository.GetMemberProfileDetail(id);
        }

        public async Task<bool?> UpdateUserProfile(UserUpdateProfileInfo userProfileDto)
        {
            Account oldAccountInfo = await _accountRepository.GetAccountOnId(userProfileDto.AccountId);

            if (oldAccountInfo == null)
            {
                return null;
            }

            oldAccountInfo.Username = userProfileDto.Username;
            oldAccountInfo.AccountName = userProfileDto.AccountName;
            oldAccountInfo.PhoneNumber = userProfileDto.PhoneNumber;
            oldAccountInfo.Citizen_identification = userProfileDto.CitizenIdentification;
            oldAccountInfo.Address = userProfileDto.Address;
            oldAccountInfo.MajorId = userProfileDto.MajorId;
            oldAccountInfo.BankingCode = userProfileDto.BankingCode;
            oldAccountInfo.BankingNumber = userProfileDto.PhoneNumber;


            return await _accountRepository.UpdateAsync(oldAccountInfo);
        }
    }
}
