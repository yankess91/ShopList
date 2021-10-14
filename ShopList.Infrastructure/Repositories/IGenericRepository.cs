using ShopList.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ShopList.Infrastructure.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity> Delete(int id);

        Task<TEntity> Delete(TEntity entityToDelete);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        Task<TEntity> GetById(int id);

        Task<IEnumerable<TEntity>> Insert(IEnumerable<TEntity> entity);

        Task<TEntity> Insert(TEntity entity);

        Task<IEnumerable<TEntity>> Update(IEnumerable<TEntity> entityToUpdate);
    }
}