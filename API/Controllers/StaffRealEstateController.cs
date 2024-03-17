using API.MessageResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Paging;
using Repository.Param;
using Service.Interface;

namespace API.Controllers
{
    public class StaffRealEstateController : BaseApiController
    {
        private readonly IStaffRealEstateService _staffRealEstateService;
        private const string BaseUri = "/api/staff/";

        public StaffRealEstateController(IStaffRealEstateService staffRealEstateService)
        {
            _staffRealEstateService = staffRealEstateService;
        }

        [Authorize(policy: "Staff")]
        [HttpGet(BaseUri + "real-estate/pending")]
        public async Task<IActionResult> GetRealEstateOnGoingByStaff([FromQuery] PaginationParams paginationParams)
        {
                var reals = await _staffRealEstateService.GetRealEstateOnGoingByStaff();
                if (reals != null)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    return Ok();
                }
                else
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
        }


        [Authorize(policy: "Staff")]
        [HttpPost(BaseUri + "real-estate/pending/search")]
        public async Task<IActionResult> GetRealEstateOnGoingByStaffBySearch(SearchRealEsateAdminParam searchRealEstateDto)
        {
                var reals = await _staffRealEstateService.GetRealEstateOnGoingByStaffBySearch(searchRealEstateDto);
                if (reals != null)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(reals);
                    //var apiResponseMessage = new ApiResponseMessage("MSG01");
                    //return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
                else
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
        }

        [Authorize(policy: "Staff")]
        [HttpGet(BaseUri + "real-estate/pending/detail/{id}")]
        public async Task<IActionResult> GetRealEstateOnGoingDetailByStaff(int id)
        {
                var real_estate_detail = await _staffRealEstateService.GetRealEstateOnGoingDetailByStaff(id);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(real_estate_detail);
        }


        [Authorize(policy: "Staff")]
        [HttpGet(BaseUri + "real-estate/all")]
        public async Task<IActionResult> GetAllRealEstateExceptOnGoingByStaff([FromQuery] PaginationParams paginationParams)
        {
                var reals = await _staffRealEstateService.GetAllRealEstateExceptOnGoingByStaff();

                if (reals != null)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(reals);
                    //var apiResponseMessage = new ApiResponseMessage("MSG01");
                    //return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
                else
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
        }

        [Authorize(policy: "Staff")]
        [HttpPost(BaseUri + "real-estate/all/search")]
        public async Task<IActionResult> GetRealEstateExceptOnGoingByStaffBySearch(SearchRealEsateAdminParam searchRealEstateDto)
        {
                var reals = await _staffRealEstateService.GetRealEstateExceptOnGoingByStaffBySearch(searchRealEstateDto);
                if (reals != null)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(reals);
                    //var apiResponseMessage = new ApiResponseMessage("MSG01");
                    //return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
                else
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
        }

        [Authorize(policy: "Staff")]
        [HttpGet(BaseUri + "real-estate/all/detail/{id}")]
        public async Task<IActionResult> GetRealEstateExceptOnGoingDetailByStaff(int id)
        {
                var real_estate_detail = _staffRealEstateService.GetRealEstateExceptOnGoingDetailByStaff(id);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(real_estate_detail);
        }

        [Authorize(policy: "Staff")]
        [HttpPost(BaseUri + "real-estate/pending/change")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateStatusRealEstateByStaff(ReasStatusParam reasStatusDto)
        {
                bool check = await _staffRealEstateService.UpdateStatusRealEstateByStaff(reasStatusDto);
                if (check)
                {
                    return new ApiResponseMessage("MSG03");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
                }
        }
    }
}
