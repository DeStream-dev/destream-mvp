using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DeStream.Web.Entities.Data.Services
{
    public interface IDataServiceBase<T> where T:class
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

        IQueryable<T> Query(params Expression<Func<T, object>>[] includes);

    }

    public interface IDataServiceExtended<T, in TKey> :IDataServiceBase<T>
        where T:Entity<TKey>
    {
        T Get(TKey key, params Expression<Func<T, object>>[] includes);
    }
}
