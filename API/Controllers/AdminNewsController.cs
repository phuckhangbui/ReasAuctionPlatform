using API.DTOs;
using API.Errors;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminNewsController : BaseApiController
    {
        private readonly IAdminNewsService _adminNewsService;
        private const string BaseUri = "/api/admin/";

        public AdminNewsController(IAdminNewsService adminNewsService)
        {
            _adminNewsService = adminNewsService;
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet(BaseUri + "news")]
        public async Task<IActionResult> GetAllNewsByAdmin()
        {
                var listNews = await _adminNewsService.GetAllNewsByAdmin();
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(listNews);
        }


        [Authorize(policy: "AdminAndStaff")]
        [HttpGet(BaseUri + "news/detail/{id}")]
        public async Task<IActionResult> GetNewsDetailByAdmin(int id)
        {
                var newsDetail = await _adminNewsService.GetNewsDetailByAdmin(id);
                if (newsDetail != null)
                {
                    return Ok(newsDetail);
                }
                else
                {
                    return null;
                }
        }


        [Authorize(policy: "AdminAndStaff")]
        [HttpGet(BaseUri + "news/search")]
        public async Task<IActionResult> SearchNewsByAdmin([FromQuery] SearchNewsAdminParam searchNews)
        {
                var reals = await _adminNewsService.SearchNewsByAdmin(searchNews);
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
        [HttpPost(BaseUri + "news/add")]
        public async Task<ActionResult<ApiResponseMessage>> AddNewNews(NewsCreate newCreate)
        {
                bool check = await _adminNewsService.AddNewNews(newCreate, GetLoginAccountId());
                if (check)
                {
                    return new ApiResponseMessage("MSG21");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation"));
                }
        }


        [Authorize(policy: "AdminAndStaff")]
        [HttpPost(BaseUri + "news/update")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateNewNews(NewsDetailDto newsDetailDto)
        {
                bool check = await _adminNewsService.UpdateNewNews(newsDetailDto);
                if (check)
                {
                    return new ApiResponseMessage("MSG03");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation"));
                }
        }
    }
}
