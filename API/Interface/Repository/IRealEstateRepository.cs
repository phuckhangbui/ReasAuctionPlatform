using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{
    public interface IRealEstateRepository : IBaseRepository<RealEstate>
    {
        Task<bool> UpdateRealEstateStatusAsync(ReasStatusParam reasStatusDto);
        Task<bool> CheckRealEstateExist(int reasId);
        Task<PageList<RealEstateDto>> GetOwnerRealEstate(int idOwner);
        Task<PageList<RealEstateDto>> GetOwnerRealEstateBySearch(int idOwner, SearchRealEstateParam searchRealEstateDto);
        Task<IEnumerable<ManageRealEstateDto>> GetRealEstateOnGoing();
        Task<IEnumerable<ManageRealEstateDto>> GetRealEstateOnGoingBySearch(SearchRealEsateAdminParam searchRealEstateDto);
        Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstateExceptOnGoingBySearch(SearchRealEsateAdminParam searchRealEstateDto);
        Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstateExceptOnGoing();
        Task<PageList<RealEstateDto>> SearchRealEstateByKey(SearchRealEstateParam searchRealEstateDto);
        Task<PageList<RealEstateDto>> GetAllRealEstateOnRealEstatePage();
        RealEstate GetRealEstate(int id);
        Task<string> GetRealEstateName(int id);
    }
}
