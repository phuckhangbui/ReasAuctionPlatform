using BusinessObject.Enum;
using Repository.DTOs;
using Repository.Interface;
using Repository.Param;
using Service.Interface;

namespace Service.Implement
{
    public class AdminRealEstateService : IAdminRealEstateService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;
        private readonly IRealEstateRepository _realEstateRepository;

        public AdminRealEstateService(IAccountRepository accountRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstateRepository realEstateRepository)
        {
            _accountRepository = accountRepository;
            _realEstateDetailRepository = realEstateDetailRepository;
            _realEstateRepository = realEstateRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<bool> BlockRealEstate(int reasId)
        {
            ReasStatusParam reasStatus = new ReasStatusParam();
            reasStatus.reasId = reasId;
            reasStatus.reasStatus = (int)RealEstateStatus.Block;
            reasStatus.messageString = "";

            bool check = await _realEstateRepository.UpdateRealEstateStatusAsync(reasStatus);
            if (check)
            {
                return true;
            }
            else { return false; }
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstateExceptOnGoingByAdmin()
        {
            var reals = await _realEstateRepository.GetAllRealEstateExceptOnGoing();
            return reals;
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstatesBySearch(SearchRealEsateAdminParam searchRealEstateParam)
        {
            var reals = await _realEstateRepository.GetAllRealEstateExceptOnGoingBySearch(searchRealEstateParam);
            return reals;
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstatesPendingBySearch(SearchRealEsateAdminParam searchRealEstateParam)
        {
            var reals = await _realEstateRepository.GetRealEstateOnGoingBySearch(searchRealEstateParam);
            return reals;
        }

        public async Task<RealEstateDetailDto> GetRealEstateAllDetail(int reasId)
        {
            var realEstateDetailDto = await _realEstateDetailRepository.GetRealEstateDetailByAdminOrStaff(reasId);
            return realEstateDetailDto;
        }

        public async Task<string> GetRealEstateName(int id)
        {
            var nameReas = await _realEstateRepository.GetRealEstateName(id);
            return nameReas;
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetRealEstateOnGoingByAdmin()
        {
            var reals = await _realEstateRepository.GetRealEstateOnGoing();
            return reals;
        }

        public async Task<RealEstateDetailDto> GetRealEstatePendingDetail(int reasId)
        {
            var realEstateDetailDto = await _realEstateDetailRepository.GetRealEstateDetailByAdminOrStaff(reasId);
            return realEstateDetailDto;
        }

        public async Task<bool> UnblockRealEstate(int reasId)
        {
            ReasStatusParam reasStatus = new ReasStatusParam();
            reasStatus.reasId = reasId;
            reasStatus.reasStatus = (int)RealEstateStatus.Selling;
            reasStatus.messageString = "";

            bool check = await _realEstateRepository.UpdateRealEstateStatusAsync(reasStatus);
            if (check)
            {
                return true;
            }
            else { return false; }
        }

        public async Task<bool> UpdateStatusRealEstateByAdmin(ReasStatusParam reasStatusParam)
        {
            bool updateReal = await _realEstateRepository.UpdateRealEstateStatusAsync(reasStatusParam);
            if (updateReal)
            {
                return true;
            }
            else { return false; }
        }
    }
}
