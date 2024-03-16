namespace API.Interface.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task DeleteAsync(ICollection<T> entity);
    }
}
