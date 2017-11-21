using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities.Data.Services.Implementations
{
    internal abstract class DataServiceBase<T> : IDataServiceBase<T> where T:class
    {
        protected readonly DeStreamWebDbContext _dbContext;

        protected DbSet<T> Set
        {
            get
            {
                return _dbContext.Set<T>();
            }
        }


        protected DataServiceBase(DeStreamWebDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual void Create(T entity)
        {
            Set.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Set.Remove(entity);
        }

        public IQueryable<T> Query(params Expression<Func<T, object>>[] includes)
        {
            return includes.Aggregate(Set.AsQueryable(),(q,i)=>q.Include(i));
        }

        public virtual void Update(T entity)
        {
            
        }
    }

    internal abstract class DataServiceExtended<T, TKey> : DataServiceBase<T>, IDataServiceExtended<T, TKey> where T : Entity<TKey>
    {
        protected DataServiceExtended(DeStreamWebDbContext dbContext) : base(dbContext)
        {
        }

        public T Get(TKey key, params Expression<Func<T, object>>[] includes)
        {
            return Query(includes).FirstOrDefault(x => ((object)x.Id == (object)key));
        }
    }
}
