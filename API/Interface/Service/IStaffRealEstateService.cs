using API.DTOs;
using API.Interface.Repository;
using API.Param;

namespace API.Interface.Service
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
