using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;

namespace API.Services
{
    public class RealEstateService : IRealEstateService
    {
        private readonly IRealEstateRepository _real_estate_repository;
        private readonly IRealEstateDetailRepository _real_estate_detail_repository;

        public RealEstateService(IRealEstateRepository real_estate_repository, IRealEstateDetailRepository real_estate_detail_repository)
        {
            _real_estate_repository = real_estate_repository;
            _real_estate_detail_repository = real_estate_detail_repository;
        }

        public async Task<PageList<RealEstateDto>> ListRealEstate()
        {
            var reals = await _real_estate_repository.GetAllRealEstateOnRealEstatePage();
            return reals;
        }

        public async Task<PageList<RealEstateDto>> SearchRealEstateForMember(SearchRealEstateParam searchRealEstateDto)
        {
            var reals = await _real_estate_repository.SearchRealEstateByKey(searchRealEstateDto);
            return reals;
        }

        public async Task<RealEstateDetailDto> ViewRealEstateDetail(int id)
        {
            var _real_estate_detail = await _real_estate_detail_repository.GetRealEstateDetail(id);
            return _real_estate_detail;
        }

        public async Task<bool> UpdateRealEstateStatus(int reasId, int status)
        {
            var realEsate = _real_estate_repository.GetRealEstate(reasId);
            if (realEsate != null)
            {
                realEsate.ReasStatus = status;
                return await _real_estate_repository.UpdateAsync(realEsate);
            }
            return false;
        }

        public RealEstate GetRealEstate(int reasId)
        {
            return _real_estate_repository.GetRealEstate(reasId);
        }
    }
}
