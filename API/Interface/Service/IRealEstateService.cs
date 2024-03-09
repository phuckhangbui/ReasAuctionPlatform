using API.DTOs;
using API.Helper;
using API.Param;

namespace API.Interface.Service
{
    public interface IRealEstateService
    {
        Task<PageList<RealEstateDto>> ListRealEstate();
        Task<PageList<RealEstateDto>> SearchRealEstateForMember(SearchRealEstateParam searchRealEstateDto);
        Task<RealEstateDetailDto> ViewRealEstateDetail(int id);

    }
}
