using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository : IDisposable
    {
        TEntity Create<TEntity>(TEntity ToCreate) where TEntity : class;

        bool Delete<TEntity>(TEntity ToDelete) where TEntity : class;

        bool Update<TEntity>(TEntity ToUpdate) where TEntity : class;

        TEntity Retrieve<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;

        List<TEntity> Filter<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : class;
    }
}
