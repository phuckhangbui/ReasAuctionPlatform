using API.DTOs;
using API.Helper;
using API.Interface.Repository;
using API.Param;

namespace API.Interface.Service
{
    public interface IAdminNewsService
    {
        IAccountRepository AccountRepository { get; }
        Task<IEnumerable<NewsAdminDto>> GetAllNewsByAdmin();
        Task<NewsDetailDto> GetNewsDetailByAdmin(int id);
        Task<IEnumerable<NewsAdminDto>> SearchNewsByAdmin(SearchNewsAdminParam searchNews);
        Task<bool> AddNewNews(NewsCreate newCreate, int idAdmin);
        Task<bool> UpdateNewNews(NewsDetailDto newsDetailDto);  
    }
}
