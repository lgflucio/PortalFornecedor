using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Create(TEntity model);
        void CreateRange(List<TEntity> model);

        bool Update(TEntity model);

        bool Delete(TEntity model);
        public void InsertInContext(TEntity entity);
        public void InsertListInContext(List<TEntity> entity);
        TEntity Find(Expression<Func<TEntity, bool>> where);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, object> includes);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where);
        int Save();

    }
}
