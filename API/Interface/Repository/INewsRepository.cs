using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{
    public interface INewsRepository : IBaseRepository<News>
    {
        Task<PageList<NewsDto>> GetAllInNewsPage();
        Task<IEnumerable<NewsAdminDto>> GetAllInNewsAdmin();
        Task<PageList<NewsDto>> SearchNewByKey(SearchNewsParam searchNewsParam);
        Task<IEnumerable<NewsAdminDto>> SearchNewsAdminByKey(SearchNewsAdminParam searchNewsParam);
        Task<NewsDetailDto> GetDetailOfNews(int id);
        Task<bool> CreateNewNewsByAdmin(NewsCreate newCreate, int id, string name);
        Task<bool> UpdateNewsByAdmin(NewsDetailDto news);
    }
}
