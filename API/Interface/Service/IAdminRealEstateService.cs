using API.DTOs;
using API.Interface.Repository;
using API.Param;

namespace API.Interface.Service
{
    public interface IAdminRealEstateService
    {
        IAccountRepository AccountRepository { get; }
        Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstatesBySearch(SearchRealEsateAdminParam searchRealEstateParam);
        Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstatesPendingBySearch(SearchRealEsateAdminParam searchRealEstateParam);
        Task<RealEstateDetailDto> GetRealEstatePendingDetail(int reasId);
        Task<RealEstateDetailDto> GetRealEstateAllDetail(int reasId);
        Task<IEnumerable<ManageRealEstateDto>> GetRealEstateOnGoingByAdmin();
        Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstateExceptOnGoingByAdmin();
        Task<bool> UpdateStatusRealEstateByAdmin(ReasStatusParam reasStatusParam);
        Task<string> GetRealEstateName(int id);
    }
}
