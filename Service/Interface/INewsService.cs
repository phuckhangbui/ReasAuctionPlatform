using Repository.DTOs;
using Repository.Paging;
using Repository.Param;

namespace Service.Interface
{
    public interface INewsService
    {
        Task<PageList<NewsDto>> GetAllNews();
        Task<NewsDetailDto> GetNewsDetail(int id);
        Task<PageList<NewsDto>> SearchNews(SearchNewsParam searchNews);
    }
}
