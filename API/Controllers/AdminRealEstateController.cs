using API.Errors;
using API.Helper;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using API.Param.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminRealEstateController : BaseApiController
    {
        private readonly IAdminRealEstateService _adminRealEstateService;

        private const string BaseUri = "/api/admin/";

        public AdminRealEstateController(IAdminRealEstateService adminRealEstateService)
        {
            _adminRealEstateService = adminRealEstateService;
        }

        [HttpGet(BaseUri + "real-estate/all/search")]
        public async Task<IActionResult> GetAllRealEstatesBySearch([FromQuery] SearchRealEsateAdminParam searchRealEstateParam)
        {
            int idAmin = GetIdAdmin(_adminRealEstateService.AccountRepository);
            if (idAmin != 0)
            {
                var reals = await _adminRealEstateService.GetAllRealEstatesBySearch(searchRealEstateParam);
                if (reals != null)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(reals);
                }
                else
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "real-estate/pending/search")]
        public async Task<IActionResult> GetAllRealEstatesPendingBySearch([FromQuery] SearchRealEsateAdminParam searchRealEstateParam)
        {
            int idAmin = GetIdAdmin(_adminRealEstateService.AccountRepository);
            if (idAmin != 0)
            {
                var reals = await _adminRealEstateService.GetAllRealEstatesPendingBySearch(searchRealEstateParam);
                if (reals != null)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(reals);
                }
                else
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "real-estate/pending/detail/{reasId}")]
        public async Task<IActionResult> GetRealEstatePendingDetail(int reasId)
        {
            int idAmin = GetIdAdmin(_adminRealEstateService.AccountRepository);
            if (idAmin != 0)
            {
                var realEstateDetailDto = await _adminRealEstateService.GetRealEstatePendingDetail(reasId);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(realEstateDetailDto);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "real-estate/all/detail/{reasId}")]
        public async Task<IActionResult> GetRealEstateAllDetail(int reasId)
        {
            int idAmin = GetIdAdmin(_adminRealEstateService.AccountRepository);
            if (idAmin != 0)
            {
                var realEstateDetailDto = await _adminRealEstateService.GetRealEstateAllDetail(reasId);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(realEstateDetailDto);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "real-estate/pending")]
        public async Task<IActionResult> GetRealEstateOnGoingByAdmin([FromQuery] PaginationParams paginationParams)
        {
            int idAmin = GetIdAdmin(_adminRealEstateService.AccountRepository);
            if (idAmin != 0)
            {
                var reals = await _adminRealEstateService.GetRealEstateOnGoingByAdmin();

                if (reals != null)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(reals);
                }
                else
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "real-estate/all")]
        public async Task<IActionResult> GetAllRealEstateExceptOnGoingByAdmin([FromQuery] PaginationParams paginationParams)
        {
            int idAmin = GetIdAdmin(_adminRealEstateService.AccountRepository);
            if (idAmin != 0)
            {
                var reals = await _adminRealEstateService.GetAllRealEstateExceptOnGoingByAdmin();

                if (reals != null)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(reals);
                }
                else
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost(BaseUri + "real-estate/change")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateStatusRealEstateByAdmin(ReasStatusParam reasStatusDto)
        {
            int idAdmin = GetIdAdmin(_adminRealEstateService.AccountRepository);
            if (idAdmin != 0)
            {
                var updateReal = await _adminRealEstateService.UpdateStatusRealEstateByAdmin(reasStatusDto);
                if (updateReal)
                {
                    return new ApiResponseMessage("MSG03");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
