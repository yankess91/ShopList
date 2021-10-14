using Microsoft.EntityFrameworkCore;
using ShopList.Infrastructure.Database;
using ShopList.Infrastructure.Model;
using ShopList.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShopList.DataAccess
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query)
                    .AsNoTracking();
            }
            else
            {
                return query
                    .AsNoTracking();
            }
        }

        public async virtual Task<TEntity> GetById(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public virtual async Task<IEnumerable<TEntity>> Insert(IEnumerable<TEntity> entity)
        {
            var entityList = entity.ToList();
            _dbSet.AddRange(entityList);
            await _context.SaveChangesAsync();

            return entityList;
        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<TEntity> Delete(int id)
        {
            var entityToDelete = await GetById(id);
            return await Delete(entityToDelete);
        }

        public virtual async Task<TEntity> Delete(TEntity entityToDelete)
        {
            _dbSet.Remove(entityToDelete);
            await _context.SaveChangesAsync();

            return entityToDelete;
        }

        public virtual async Task<IEnumerable<TEntity>> Update(IEnumerable<TEntity> entityToUpdate)
        {
            _dbSet.UpdateRange(entityToUpdate);

            await _context.SaveChangesAsync();

            return entityToUpdate;
        }
    }
}