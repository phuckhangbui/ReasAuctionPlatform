using API.Data;
using API.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DataContext _context;

        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                _context.Set<T>().Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public async Task DeleteAsync(ICollection<T> entity)
        {
            try
            {
                foreach (T t in entity)
                {
                    _context.Set<T>().Remove(t);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return;
            }

        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                var tracker = _context.Attach(entity);
                tracker.State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }
    }
}
