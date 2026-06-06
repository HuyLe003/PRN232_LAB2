using Microsoft.EntityFrameworkCore;
using PRN232.LMS.Repositories.Interfaces;
using PRN232.LMS.Repositories.Models;

namespace PRN232.LMS.Repositories.Implementations
{
    /// <summary>
    /// Generic repository base implementation for all entities
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DatabaseContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DatabaseContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<(List<T> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search = null, string? sort = null)
        {
            var query = _dbSet.AsQueryable();

            // Apply search if provided
            if (!string.IsNullOrEmpty(search))
            {
                query = ApplySearch(query, search);
            }

            // Get total count
            var total = await query.CountAsync();

            // Apply sorting if provided
            if (!string.IsNullOrEmpty(sort))
            {
                query = ApplySort(query, sort);
            }

            // Apply paging
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await SaveChangesAsync();
            return true;
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual IQueryable<T> ApplySearch(IQueryable<T> query, string search)
        {
            // Override in derived classes for specific search logic
            return query;
        }

        protected virtual IQueryable<T> ApplySort(IQueryable<T> query, string sort)
        {
            // Override in derived classes for specific sort logic
            return query;
        }
    }
}
