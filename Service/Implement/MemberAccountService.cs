using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
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

        public async Task<bool?> UpdateUserProfile(UserProfileDto userProfileDto)
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
