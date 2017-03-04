using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Canducci.MongoDB.Contracts
{
    public interface IRepository<T> : IDisposable
        where T : class, new()
    {          

        T Add(T model);
        IEnumerable<T> Add(IEnumerable<T> models);
        Task<T> AddAsync(T model);
        Task<IEnumerable<T>> AddAsync(IEnumerable<T> models);

        bool Edit(Expression<Func<T, bool>> filter, T model);
        Task<bool> EditAsync(Expression<Func<T, bool>> filter, T model);

        bool Update(Expression<Func<T, bool>> filter, UpdateDefinition<T> update);
        Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update);

        T Find(Expression<Func<T, bool>> filter);
        Task<T> FindAsync(Expression<Func<T, bool>> filter);

        IEnumerable<T> All();
        IEnumerable<T> All(Expression<Func<T, bool>> filter);
        IEnumerable<T> All<Tkey>(Expression<Func<T, bool>> filter, Expression<Func<T, Tkey>> orderBy);
        Task<IList<T>> AllAsync();
        Task<IList<T>> AllAsync(Expression<Func<T, bool>> filter);
        Task<IList<T>> AllAsync<Tkey>(Expression<Func<T, bool>> filter, Expression<Func<T, Tkey>> orderBy);

        bool Delete(Expression<Func<T, bool>> filter);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> filter);

        IQueryable<T> Queryable();
         
        ObjectId CreateObjectId(string value);

        long Count();
        long Count(Expression<Func<T, bool>> filter, CountOptions options = null);
        Task<long> CountAsync();
        Task<long> CountAsync(Expression<Func<T, bool>> filter, CountOptions options = null);
    }
}
