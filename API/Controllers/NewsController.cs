using API.Extensions;
using API.MessageResponse;
using Microsoft.AspNetCore.Mvc;
using Repository.Paging;
using Repository.Param;
using Service.Interface;

namespace API.Controllers
{
    public class NewsController : BaseApiController
    {
        private readonly INewsService _newsService;
        private const string BaseUri = "/api/home/";

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet(BaseUri + "news")]
        public async Task<IActionResult> GetAllNews([FromQuery] PaginationParams paginationParams)
        {
            var listNews = await _newsService.GetAllNews();
            Response.AddPaginationHeader(new PaginationHeader(listNews.CurrentPage, listNews.PageSize,
            listNews.TotalCount, listNews.TotalPages));
            if (listNews.PageSize == 0)
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(listNews);
            }
        }

        [HttpGet(BaseUri + "news/detail/{id}")]
        public async Task<IActionResult> GetNewsDetail(int id)
        {
            var newsDetail = await _newsService.GetNewsDetail(id);
            if (newsDetail != null)
            {
                return Ok(newsDetail);
            }
            else
            {
                return null;
            }
        }

        [HttpPost(BaseUri + "news/search")]
        public async Task<IActionResult> SearchNews(SearchNewsParam searchNews)
        {
            var reals = await _newsService.SearchNews(searchNews);
            Response.AddPaginationHeader(new PaginationHeader(reals.CurrentPage, reals.PageSize,
            reals.TotalCount, reals.TotalPages));
            if (reals.PageSize == 0)
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(reals);
            }
        }
    }
}
