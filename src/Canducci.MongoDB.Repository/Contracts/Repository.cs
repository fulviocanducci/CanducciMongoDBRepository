using Canducci.MongoDB.Connection;
using Canducci.MongoDB.Repository.MongoAttribute;
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
    public abstract class Repository<T> : IRepository<T>
        where T : class, new()
    {
        protected IConnect _connect { get; private set; }
        protected IMongoCollection<T> _collection { get; private set; }
        protected string _collectionName { get; private set; }

        public Repository(IConnect connect)
        {
            SetCollectionName();
            SetConnectAndCollection(connect);
        }

        #region add 
        
        public T Add(T model)
        {
            _collection.InsertOne(model);
            return model;
        }

        public IEnumerable<T> Add(IEnumerable<T> models)
        {
            _collection.InsertMany(models);
            return models;
        }

        public async Task<T> AddAsync(T model)
        {
            await _collection.InsertOneAsync(model);
            return model;
        }

        public async Task<IEnumerable<T>> AddAsync(IEnumerable<T> models)
        {
            await _collection.InsertManyAsync(models);
            return models;
        }

        #endregion

        #region edit 
        
        public bool Edit(Expression<Func<T, bool>> filter, T model)
        {
            return _collection
                .ReplaceOne(filter, model)
                .ModifiedCount > 0;
        }

        public async Task<bool> EditAsync(Expression<Func<T, bool>> filter, T model)
        {
            ReplaceOneResult result = 
                await _collection
                .ReplaceOneAsync(filter, model);
            return result
                .ModifiedCount > 0;
        }

        #endregion

        #region update
                
        public bool Update(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            return _collection
                .UpdateOne(filter, update)
                .ModifiedCount > 0;
        }
        public async Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update)
        {
            UpdateResult result = await _collection
                .UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        #endregion

        #region find
                
        public T Find(Expression<Func<T, bool>> filter)
        {
            return _collection
                .Find(filter)
                .FirstOrDefault();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> filter)
        {
            IAsyncCursor<T> result = await _collection
               .FindAsync(filter);
            return result
                .FirstOrDefault();
        }

        #endregion

        #region all

        
        public IEnumerable<T> All()
        {
            return _collection
                .AsQueryable()                
                .AsEnumerable();
        }

        public IEnumerable<T> All(Expression<Func<T, bool>> filter)
        {
            return _collection
                .AsQueryable()
                .Where(filter)
                .AsEnumerable();
        }

        public IEnumerable<T> All<Tkey>(Expression<Func<T, bool>> filter, Expression<Func<T, Tkey>> orderBy)
        {
            return _collection
                .AsQueryable()
                .Where(filter)
                .OrderBy(orderBy)
                .AsEnumerable();
        }

        public async Task<IList<T>> AllAsync()
        {
            return await _collection
              .AsQueryable()
              .ToListAsync();

        }

        public async Task<IList<T>> AllAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection
                .AsQueryable()
                .Where(filter)
                .ToListAsync();
        }

        public async Task<IList<T>> AllAsync<Tkey>(Expression<Func<T, bool>> filter, Expression<Func<T, Tkey>> orderBy)
        {               
            return await _collection    
                .AsQueryable()
                .Where(filter)
                .OrderBy(orderBy)
                .ToListAsync();
        }
        #endregion

        public bool Delete(Expression<Func<T, bool>> filter)
        {
            return _collection
                .DeleteOne(filter)
                .DeletedCount > 0;
        }

        public IQueryable<T> Queryable()
        {
            return _collection
                .AsQueryable();
        }

        public ObjectId CreateObjectId(string value)
        {
            return ObjectId.Parse(value);
        }

        #region Internal
        internal void SetCollectionName()
        {
            MongoCollectionName mongoCollectionName =
                (MongoCollectionName)
                System.Reflection.CustomAttributeExtensions.GetCustomAttribute(
                    System.Reflection.Assembly.GetEntryAssembly(), typeof(MongoCollectionName)); 
            _collectionName = mongoCollectionName != null ?
                mongoCollectionName.TableName :
                typeof(T).Name.ToLower();
            mongoCollectionName = null;
        }
        internal void SetConnectAndCollection(IConnect connect)
        {
            _connect = connect;
            _collection = _connect.Collection<T>(_collectionName);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _collection = null;
                    _connect = null;
                }
                disposed = true;
            }
        }

        

        ~Repository()
        {
            Dispose(false);
        }
        private bool disposed = false;
        #endregion Dispose            


        #region count

        public long Count()
        {
            return _collection
                .Count(Builders<T>.Filter.Empty);
        }

        public long Count(Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            return _collection
                .Count(filter, options);
        }

        public async Task<long> CountAsync()
        {
            return await _collection
                .CountAsync(Builders<T>.Filter.Empty);
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> filter, CountOptions options = null)
        {
            return await _collection
                .CountAsync(filter, options);
        }

        #endregion

    }
}