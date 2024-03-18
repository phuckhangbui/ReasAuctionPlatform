using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Paging;
using Repository.Param;

namespace Service.Interface
{
    public interface IRealEstateService
    {
        Task<PageList<RealEstateDto>> ListRealEstate();
        Task<PageList<RealEstateDto>> SearchRealEstateForMember(SearchRealEstateParam searchRealEstateDto);
        Task<RealEstateDetailDto> ViewRealEstateDetail(int id);
        Task<bool> UpdateRealEstateStatus(int reasId, int status);
        RealEstate GetRealEstate(int reasId);
        Task<bool> UpdateRealEstateStatus(int reasId, int status, bool IsReupYet);
        Task<bool> UpdateRealEstateIsReupYet(int reasId, bool IsReupYet);
        Task<PageList<RealEstateDto>> GetRealEstates(SearchRealEstateParam searchRealEstateParam);
    }
}
