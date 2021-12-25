using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Acr.Entities.Abstract;

namespace Acr.DataAccess.Abstract
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity Get(Expression<Func<TEntity, bool>> filter);
        TEntity GetById(object id);
        List<TEntity> GetAll(int pageNo = 1, int pageSize = 50, Expression<Func<TEntity, bool>> filter = null);

        void Add(TEntity entity);
        void AddRange(List<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> entities);
        int GetTotalRowCount(Expression<Func<TEntity, bool>> filter = null);
    }
}