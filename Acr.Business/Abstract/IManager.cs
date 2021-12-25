using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Transactions;
using Acr.Core.Helpers;
using Acr.Core.Models;
using Acr.Core.Models.Request;
using Acr.Core.Models.Response;
using Acr.Core.Result;
using Acr.DataAccess.Abstract;
using Acr.Entities.Abstract;
using Acr.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Acr.Business.Abstract
{
    public interface IManager<TEntity> where TEntity : BaseEntity
    {
        TaskResult Add(TEntity entity);
        TaskResult AddAndLog(TEntity entity, int userId = 0);
        TaskResult AddWithObject<TExtraObject>(TEntity entity, int userId = 0);

        TaskResult Update(TEntity entity);
        TaskResult UpdateAndLog(TEntity entity, int userId = 0);
        TaskResult UpdateWithObject<TExtraObject>(TEntity entity, int userId = 0);


        TaskResult Delete(GetRequest request);
        TaskResult DeleteSoft(GetRequest request);
        TaskResult DeleteAndLog(GetRequest request, int userId);


        TaskResult Get(GetRequest request, Expression<Func<TEntity, bool>> filter = null);
        TaskResult GetAll(PaginationRequest request, Expression<Func<TEntity, bool>> filter = null);

        TaskResult GetWithObject<TExtraObject>(GetRequest request);
        TaskResult GetAllWithObject<TExtraObject>(PaginationRequest request, Expression<Func<TEntity, bool>> filter = null);

        TaskResult GetFromViewById<TView>(GetRequest request) where TView : BaseView;
        TaskResult GetAllFromView<TView>(PaginationRequest request, Expression<Func<TView, bool>> filter = null) where TView : BaseView;

    }
}