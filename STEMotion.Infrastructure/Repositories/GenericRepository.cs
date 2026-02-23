using Microsoft.EntityFrameworkCore;
using STEMotion.Application.Interfaces.RepositoryInterfaces;
using STEMotion.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly StemotionContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(StemotionContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Creates a new entity in the database asynchronously.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity"></param>
        //public void Delete(T entity) => _dbSet.Remove(entity);
        public void Delete(T entity)
        {
            var statusProperty = typeof(T).GetProperty("Status",
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.IgnoreCase);

            if (statusProperty != null && statusProperty.CanWrite)
            {
                var propertyType = statusProperty.PropertyType;

                if (propertyType == typeof(string))
                {
                    statusProperty.SetValue(entity, "InActive");
                    _dbSet.Update(entity);
                }
                else if (propertyType == typeof(bool))
                {
                    statusProperty.SetValue(entity, false);
                    _dbSet.Update(entity);
                }
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }



        /// <summary>
        /// Checks if any entity exists in the database that matches the given predicate asynchronously. Predicate is a condition to filter entities.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => await _dbSet.AnyAsync(predicate);

        /// <summary>
        ///  Find All not include
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns></returns>

        /// <summary>
        /// Find By Conditon (Ko can await/async)
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) => !trackChanges ? _context.Set<T>().Where(expression).AsNoTracking()
        : _context.Set<T>().Where(expression);

        /// <summary>
        /// Gets all entities from the database asynchronously.
        /// </summary>
        /// <returns></returns>
        //public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task<IEnumerable<T>> FindAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            // Kiểm tra xem includes có giá trị và có phần tử nào không
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Find All (Ko include => ko await/async)
        /// </summary>
        /// <param name="trackChanges"></param>
        /// <returns></returns>
        public IQueryable<T> FindAll(bool trackChanges = false) => !trackChanges ? _context.Set<T>().AsNoTracking()
             : _context.Set<T>();

        /// <summary>
        /// Gets an entity by its unique identifier asynchronously. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity) => _dbSet.Update(entity);
        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (predicate != null)
                query = query.Where(predicate);
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Check if T has a property named "OrderIndex"
            var orderIndexProperty = typeof(T).GetProperty("OrderIndex");
            if (orderIndexProperty != null && orderIndexProperty.PropertyType == typeof(int))
            {
                query = query.OrderBy(x => EF.Property<int>(x, "OrderIndex"));
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
