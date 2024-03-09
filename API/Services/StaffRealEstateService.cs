using API.DTOs;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;

namespace API.Services
{
    public class StaffRealEstateService : IStaffRealEstateService
    {
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;

        public StaffRealEstateService(IRealEstateRepository realEstateRepository, IAccountRepository accountRepository, IRealEstateDetailRepository realEstateDetailRepository)
        {
            _realEstateRepository = realEstateRepository;
            _accountRepository = accountRepository;
            _realEstateDetailRepository = realEstateDetailRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstateExceptOnGoingByStaff()
        {
            var reals = await _realEstateRepository.GetAllRealEstateExceptOnGoing();
            return reals;
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetRealEstateExceptOnGoingByStaffBySearch(SearchRealEsateAdminParam searchRealEstateDto)
        {
            var reals = await _realEstateRepository.GetAllRealEstateExceptOnGoingBySearch(searchRealEstateDto);
            return reals;
        }

        public async Task<RealEstateDetailDto> GetRealEstateExceptOnGoingDetailByStaff(int id)
        {
            var real_estate_detail = await _realEstateDetailRepository.GetRealEstateDetail(id);
            return real_estate_detail;
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetRealEstateOnGoingByStaff()
        {
            var reals = await _realEstateRepository.GetRealEstateOnGoing();
            return reals;
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetRealEstateOnGoingByStaffBySearch(SearchRealEsateAdminParam searchRealEstateDto)
        {
            var reals = await _realEstateRepository.GetRealEstateOnGoingBySearch(searchRealEstateDto);
            return reals;
        }

        public async Task<RealEstateDetailDto> GetRealEstateOnGoingDetailByStaff(int id)
        {
            var real_estate_detail = await _realEstateDetailRepository.GetRealEstateDetail(id);
            return real_estate_detail;
        }

        public async Task<bool> UpdateStatusRealEstateByStaff(ReasStatusParam reasStatusDto)
        {
            try
            {
                bool updateReal = await _realEstateRepository.UpdateRealEstateStatusAsync(reasStatusDto);
                if (updateReal) { return true; }
                else { return false; }
            }
            catch { return false; }
        }
    }
}
