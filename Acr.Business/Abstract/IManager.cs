using System;
using System.Linq.Expressions;
using Acr.Core.Result;
using Acr.Core.Models.Request;
using Acr.Entities.Abstract;

namespace Acr.Business.Abstract
{
    public interface IManager<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TaskResult Add(TEntity entity);
        TaskResult Add(TEntity entity, int userId = 0);
        TaskResult AddWithObject<TExtraObject>(TEntity entity, int userId = 0);

        TaskResult Update(TEntity entity);
        TaskResult Update(TEntity entity, int userId = 0);
        TaskResult UpdateWithObject<TExtraObject>(TEntity entity, int userId = 0);


        TaskResult Delete(int id);
        TaskResult Delete(int id, int userId);
        TaskResult DeleteSoft(int id);


        TaskResult Get(int id, Expression<Func<TEntity, bool>> filter = null);
        TaskResult GetAll(PaginationRequest request, Expression<Func<TEntity, bool>> filter = null);

        TaskResult GetWithObject<TExtraObject>(int id);
        TaskResult GetAllWithObject<TExtraObject>(PaginationRequest request, Expression<Func<TEntity, bool>> filter = null);

        TaskResult GetFromViewById<TView>(int id) where TView : BaseView;
        TaskResult GetAllFromView<TView>(PaginationRequest request, Expression<Func<TView, bool>> filter = null) where TView : BaseView;

    }
}