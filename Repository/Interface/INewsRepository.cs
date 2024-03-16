using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Paging;
using Repository.Param;

namespace Repository.Interface
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
