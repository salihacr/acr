using System;
using System.Reflection;
using System.Transactions;
using System.Linq.Expressions;
using Acr.Business.Abstract;
using Acr.Core.Helpers;
using Acr.Core.Models;
using Acr.Core.Models.Request;
using Acr.Core.Models.Response;
using Acr.Core.Result;
using Acr.DataAccess.Abstract;
using Acr.DataAccess.Concrete;
using Acr.Entities.Abstract;
using Acr.Entities.Concrete;

namespace Acr.Business.Concrete
{
    public class BaseManager<TEntity> : IManager<TEntity> where TEntity : BaseEntity
    {
        private readonly IRepository<TEntity> _repository;
        private readonly ILogRepository _logRepository;
        private readonly IViewRepository _viewRepository;
        public BaseManager(IRepository<TEntity> repository, ILogRepository logRepository)
        {
            _repository = repository;
            _logRepository = logRepository;
            _viewRepository = new ViewRepository(new DataAccess.AppDbContext()); // örnek, değişelim
        }

        #region Get/GetAll
        public TaskResult Get(GetRequest request, Expression<Func<TEntity, bool>> filter = null)
        {
            try
            {
                var result = _repository.GetById(request.Id);
                if (result != null)
                    return DataResult<TEntity>.Successful(result);
                return TaskResult.NotFound();
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }

        public TaskResult GetAll(PaginationRequest request, Expression<Func<TEntity, bool>> filter = null)
        {
            try
            {
                var data = _repository.GetAll(request.PageNo, request.PageSize, filter);
                int totalRowCount = _repository.GetTotalRowCount();
                double totalPage = (double)totalRowCount / request.PageSize;
                totalPage = Math.Ceiling(totalPage);
                var model = new ListModel<TEntity>
                {
                    List = data,
                    TotalPage = (int)totalPage,
                    TotalRowCount = totalRowCount
                };
                return DataResult<ListModel<TEntity>>.Successful(model);
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }
        #endregion

        #region Create
        public virtual TaskResult Add(TEntity entity)
        {
            try
            {
                _repository.Add(entity);
                return DataResult<int>.Successful(entity.Id);
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }

        public virtual TaskResult AddAndLog(TEntity entity, int userId = 0)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _repository.Add(entity);
                    CreateLog(entity, TransactionType.Create, userId);
                    scope.Complete();

                    return DataResult<int>.Successful(entity.Id);
                }
            }
            catch (Exception ex)
            {
                // buraya log4net veya kendi loglama mekanizmanı entegre et
                return TaskResult.Fail(message: ex.ToString());
            }
        }

        public virtual TaskResult AddWithObject<TExtraObject>(TEntity entity, int userId = 0)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    if (entity is IExtraJsonObject<TExtraObject>)
                    {
                        Type type = entity.GetType();
                        PropertyInfo extraObjectProp = type.GetProperty(nameof(IExtraJsonObject<TExtraObject>.ExtraObject));  // Extra Object Prop
                        var extraObjectValue = extraObjectProp.GetValue(entity); // Extra Obj Value

                        PropertyInfo jsonStringProp = type.GetProperty(nameof(IExtraJsonObject<TExtraObject>.JsonString)); // Json String Prop
                        jsonStringProp.SetValue(entity, Extensions.Serialize(extraObjectValue));
                    }
                    _repository.Add(entity);
                    CreateLog(entity, TransactionType.Create, userId); // logla

                    scope.Complete();
                    return DataResult<int>.Successful(entity.Id);
                }
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }
        #endregion
        
        #region Update
        public TaskResult Update(TEntity entity)
        {
            try
            {
                _repository.Update(entity);
                return TaskResult.Successful();
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }

        public virtual TaskResult UpdateAndLog(TEntity entity, int userId = 0)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _repository.Update(entity);
                    CreateLog(entity, TransactionType.Update, userId);
                    
                    scope.Complete();
                    return TaskResult.Successful();
                }
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }
        
        public virtual TaskResult UpdateWithObject<TExtraObject>(TEntity entity, int userId = 0)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (entity is IExtraJsonObject<TExtraObject>)
                    {
                        Type type = entity.GetType();
                        PropertyInfo extraObjectProp = type.GetProperty(nameof(IExtraJsonObject<TExtraObject>.ExtraObject));  // Extra Object Prop
                        var extraObjectValue = extraObjectProp.GetValue(entity); // Extra Obj Value

                        PropertyInfo jsonStringProp = type.GetProperty(nameof(IExtraJsonObject<TExtraObject>.JsonString)); // Json String Prop
                        jsonStringProp.SetValue(entity, Extensions.Serialize(extraObjectValue));
                    }

                    _repository.Update(entity);
                    CreateLog(entity, TransactionType.Update, userId); // logla

                    scope.Complete();
                    return TaskResult.Successful();
                }
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }
        #endregion

        #region Delete
        public TaskResult Delete(GetRequest request)
        {
            try
            {
                var entity = _repository.GetById(request.Id);
                if (entity != null)
                {
                    _repository.Delete(entity);
                    return TaskResult.Successful();
                };
                return TaskResult.NotFound();
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }

        public TaskResult DeleteSoft(GetRequest request)
        {
            try
            {
                var entity = _repository.GetById(request.Id);
                if (entity != null)
                {
                    if (entity is ISoftDeletable)
                    {
                        Type entityType = entity.GetType();
                        PropertyInfo propertyInfo = entityType.GetProperty(nameof(ISoftDeletable.IsDeleted));
                        propertyInfo.SetValue(entity, true);
                        _repository.Update(entity);
                        return TaskResult.Successful();
                    }
                };
                return TaskResult.NotFound();
            }
            catch (Exception ex)
            {
                // buraya log4net veya kendi loglama mekanizmanı entegre et
                return TaskResult.Fail(message: ex.ToString());
            }
        }

        public TaskResult DeleteAndLog(GetRequest request, int userId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var entity = _repository.GetById(request.Id);
                    if (entity != null)
                    {
                        if (entity is ISoftDeletable)
                        {
                            Type type = entity.GetType();
                            PropertyInfo isDeletedProp = type.GetProperty(nameof(ISoftDeletable.IsDeleted));
                            isDeletedProp.SetValue(entity, true);
                            _repository.Update(entity);
                        }
                        else
                            _repository.Delete(entity);

                        CreateLog(entity, TransactionType.Delete, userId);
                        return TaskResult.Successful();
                    };
                    return TaskResult.NotFound();
                }
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }
        #endregion

        #region ViewRepository
        public TaskResult GetFromViewById<TView>(GetRequest request) where TView : BaseView
        {
            try
            {
                var result = _viewRepository.GetView<TView>(request.Id);
                if (result == null)
                    return TaskResult.NotFound();
                return DataResult<TView>.Successful(result);
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }

        public TaskResult GetAllFromView<TView>(PaginationRequest request, Expression<Func<TView, bool>> filter = null) where TView : BaseView
        {
            try
            {
                var data = _viewRepository.GetListView<TView>(request.PageNo, request.PageSize, filter);
                int totalRowCount = _viewRepository.GetTotalRowCount<TView>(filter);
                double totalPage = (double)totalRowCount / request.PageSize;
                totalPage = Math.Ceiling(totalPage);
                var model = new ListModel<TView>
                {
                    List = data,
                    TotalPage = (int)totalPage,
                    TotalRowCount = totalRowCount
                };
                return DataResult<ListModel<TView>>.Successful(model);
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }
        #endregion

        #region Get/GetAll With Extra Objects
        public virtual TaskResult GetWithObject<TExtraObject>(GetRequest request)
        {
            try
            {
                var entity = _repository.GetById(request.Id);
                if (entity == null)
                    return TaskResult.NotFound();
                else
                {
                    if (entity is IExtraJsonObject<TExtraObject>)
                    {
                        Type entityType = entity.GetType();
                        PropertyInfo objProperty = entityType.GetProperty(nameof(IExtraJsonObject<TExtraObject>.JsonString));
                        var jsonString = objProperty.GetValue(entity).ToString();

                        PropertyInfo jsonObj = entityType.GetProperty(nameof(IExtraJsonObject<TExtraObject>.ExtraObject));
                        jsonObj.SetValue(entity, Extensions.Deserialize<TExtraObject>(jsonString));
                    }
                    return DataResult<TEntity>.Successful(entity);
                }
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }

        public virtual TaskResult GetAllWithObject<TExtraObject>(PaginationRequest request, Expression<Func<TEntity, bool>> filter = null)
        {
            try
            {
                var data = _repository.GetAll(request.PageNo, request.PageSize, filter);
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        if (item is IExtraJsonObject<TExtraObject>)
                        {
                            Type entityType = item.GetType();
                            PropertyInfo objProperty = entityType.GetProperty(nameof(IExtraJsonObject<TExtraObject>.JsonString));
                            var jsonString = objProperty.GetValue(item).ToString();

                            PropertyInfo jsonObj = entityType.GetProperty(nameof(IExtraJsonObject<TExtraObject>.ExtraObject));
                            jsonObj.SetValue(item, Extensions.Deserialize<TExtraObject>(jsonString));
                        }
                    }
                }

                int totalRowCount = _repository.GetTotalRowCount();
                double totalPage = (double)totalRowCount / request.PageSize;
                totalPage = Math.Ceiling(totalPage);
                var model = new ListModel<TEntity>
                {
                    List = data,
                    TotalPage = (int)totalPage,
                    TotalRowCount = totalRowCount
                };
                return DataResult<ListModel<TEntity>>.Successful(model);
            }
            catch (Exception ex)
            {
                return TaskResult.Fail(message: ex.ToString());
            }
        }
        #endregion

        #region Log Function
        private void CreateLog(TEntity entity, TransactionType transactionType, int userId)
        {
            var log = new Log
            {
                UserId = userId.ToString(),
                AffectedRecordId = entity.Id > 0 ? entity.Id : -1,
                AffectedTableName = typeof(TEntity).Name,
                TransactionType = (int)transactionType,
                TransactionDateTime = DateTime.Now,
                Name = "",
            };
            _logRepository.Add(log);
        }
        #endregion
    }
}