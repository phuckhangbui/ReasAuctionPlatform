using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Paging;
using Repository.Param;

namespace Repository.Interface
{
    public interface IRealEstateRepository : IBaseRepository<RealEstate>
    {
        Task<Account> UpdateRealEstateStatusAsync(ReasStatusParam reasStatusDto);
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

        Task<RealEstate> CreateRealEstateAsync(NewRealEstateParam newRealEstateParam, int userMember);
    }
}
