using BusinessObject.Entity;
using BusinessObject.Enum;
using Repository.DTOs;
using Repository.Interface;
using Repository.Param;
using Service.Interface;
using Service.Mail;

namespace Service.Implement
{
    public class AdminRealEstateService : IAdminRealEstateService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IRealEstateDetailRepository _realEstateDetailRepository;
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly INotificatonService _notificatonService;


        public AdminRealEstateService(IAccountRepository accountRepository, IRealEstateDetailRepository realEstateDetailRepository, IRealEstateRepository realEstateRepository, INotificatonService notificatonService)
        {
            _accountRepository = accountRepository;
            _realEstateDetailRepository = realEstateDetailRepository;
            _realEstateRepository = realEstateRepository;
            _notificatonService = notificatonService;
        }

        public IAccountRepository AccountRepository => _accountRepository;

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

        public async Task<bool> UpdateStatusRealEstateByAdmin(ReasStatusParam reasStatusParam)
        {
            Account account = await _realEstateRepository.UpdateRealEstateStatusAsync(reasStatusParam);
            if (account != null)
            {
                if (reasStatusParam.reasStatus == (int)RealEstateStatus.Cancel)
                {
                    SendMailWhenRejectRealEstate.SendEmailWhenRejectRealEstate(account.AccountEmail, account.AccountName, reasStatusParam.messageString);
                }
                else if (reasStatusParam.reasStatus == (int)RealEstateStatus.Approved)
                {
                    SendMailWhenApproveRealEstate.SendEmailWhenApproveRealEstate(account.AccountEmail, account.AccountName);

                }
                return true;
            }
            else { return false; }
        }
    }
}
