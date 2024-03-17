using BusinessObject.Entity;
using Repository.DTOs;

namespace Repository.Interface
{
    public interface IRealEstateDetailRepository : IBaseRepository<RealEstateDetail>
    {
        Task<RealEstateDetailDto> GetRealEstateDetail(int id);
        Task<RealEstateDetailDto> GetRealEstateMemberDetail(int id);
        Task<RealEstateDetailDto> GetRealEstateDetailByAdminOrStaff(int id);
    }
}
