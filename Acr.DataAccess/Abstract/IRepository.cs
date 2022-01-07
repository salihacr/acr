using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Acr.Core.Models.Response;
using Acr.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Query;

namespace Acr.DataAccess.Abstract
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity Get(Expression<Func<TEntity, bool>> filter);
        TEntity GetById(object id);
        List<TEntity> GetAll(int pageNo = 1, int pageSize = 50, Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        ListModel<TEntity> GetList(
            int pageNo = 1,
            int pageSize = 50,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        void Add(TEntity entity);
        void AddRange(List<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> entities);
        int GetTotalRowCount(Expression<Func<TEntity, bool>> filter = null);
    }
}