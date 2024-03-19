using Repository.DTOs;
using Repository.Interface;
using Repository.Param;

namespace Service.Interface
{
    public interface IStaffRealEstateService
    {
        IAccountRepository AccountRepository { get; }
        Task<IEnumerable<ManageRealEstateDto>> GetRealEstateOnGoingByStaff();
        Task<IEnumerable<ManageRealEstateDto>> GetRealEstateOnGoingByStaffBySearch(SearchRealEsateAdminParam searchRealEstateDto);
        Task<RealEstateDetailDto> GetRealEstateOnGoingDetailByStaff(int id);
        Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstateExceptOnGoingByStaff();
        Task<IEnumerable<ManageRealEstateDto>> GetRealEstateExceptOnGoingByStaffBySearch(SearchRealEsateAdminParam searchRealEstateDto);
        Task<RealEstateDetailDto> GetRealEstateExceptOnGoingDetailByStaff(int id);
        Task<bool> UpdateStatusRealEstateByStaff(ReasStatusParam reasStatusDto);
    }
}
