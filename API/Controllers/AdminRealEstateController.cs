using API.MessageResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Paging;
using Repository.Param;
using Service.Interface;

namespace API.Controllers
{
    public class AdminRealEstateController : BaseApiController
    {
        private readonly IAdminRealEstateService _adminRealEstateService;
        private readonly IRealEstateService _realEstateService;
        private readonly INotificatonService _notificatonService;

        private const string BaseUri = "/api/admin/";

        public AdminRealEstateController(IAdminRealEstateService adminRealEstateService, IRealEstateService realEstateService, INotificatonService notificatonService)
        {
            _adminRealEstateService = adminRealEstateService;
            _realEstateService = realEstateService;
            _notificatonService = notificatonService;
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet(BaseUri + "real-estate/all/search")]
        public async Task<IActionResult> GetAllRealEstatesBySearch([FromQuery] SearchRealEsateAdminParam searchRealEstateParam)
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


        [Authorize(policy: "AdminAndStaff")]
        [HttpGet(BaseUri + "real-estate/pending/search")]
        public async Task<IActionResult> GetAllRealEstatesPendingBySearch([FromQuery] SearchRealEsateAdminParam searchRealEstateParam)
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


        [Authorize(policy: "AdminAndStaff")]
        [HttpGet(BaseUri + "real-estate/pending/detail/{reasId}")]
        public async Task<IActionResult> GetRealEstatePendingDetail(int reasId)
        {
            var realEstateDetailDto = await _adminRealEstateService.GetRealEstatePendingDetail(reasId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(realEstateDetailDto);
        }


        [Authorize(policy: "AdminAndStaff")]
        [HttpGet(BaseUri + "real-estate/all/detail/{reasId}")]
        public async Task<IActionResult> GetRealEstateAllDetail(int reasId)
        {
            var realEstateDetailDto = await _adminRealEstateService.GetRealEstateAllDetail(reasId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(realEstateDetailDto);
        }


        [Authorize(policy: "AdminAndStaff")]
        [HttpGet(BaseUri + "real-estate/pending")]
        public async Task<IActionResult> GetRealEstateOnGoingByAdmin([FromQuery] PaginationParams paginationParams)
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


        [Authorize(policy: "AdminAndStaff")]
        [HttpGet(BaseUri + "real-estate/all")]
        public async Task<IActionResult> GetAllRealEstateExceptOnGoingByAdmin([FromQuery] PaginationParams paginationParams)
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


        [Authorize(policy: "AdminAndStaff")]
        [HttpPost(BaseUri + "real-estate/change")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateStatusRealEstateByAdmin(ReasStatusParam reasStatusDto)
        {
            var updateReal = await _adminRealEstateService.UpdateStatusRealEstateByAdmin(reasStatusDto);
            if (updateReal)
            {
                var realEstate = _realEstateService.GetRealEstate(reasStatusDto.reasId);
                await _notificatonService.SendNotificationWhenApproveRealEstate(reasStatusDto, realEstate);
                return new ApiResponseMessage("MSG03");
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
            }
        }


        [Authorize(policy: "AdminAndStaff")]
        [HttpGet(BaseUri + "real-estate/name/{reasId}")]
        public async Task<IActionResult> GetNameRealEstate(int reasId)
        {
            var reasName = await _adminRealEstateService.GetRealEstateName(reasId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reasName);
        }
    }
}
