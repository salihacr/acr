using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Acr.Core.Models.Response;
using Acr.DataAccess.Abstract;
using Acr.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Acr.DataAccess.Concrete
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void AddRange(List<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            _dbContext.SaveChanges();
        }

        public TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }
        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.FirstOrDefault(filter);
        }

        public List<TEntity> GetAll(int pageNo = 1, int pageSize = 50, Expression<Func<TEntity, bool>> filter = null)
        {
            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 1;
            if (filter != null)
                return _dbSet.Where(filter).Skip((pageNo - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
            else
                return _dbSet.Skip((pageNo - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
        }
        public ListModel<TEntity> GetList(
            int pageNo = 1,
            int pageSize = 50,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null
            )
        {
            IQueryable<TEntity> query = _dbSet;
            query.AsNoTracking();

            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);

            var data = orderBy != null
                ? orderBy(query).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList()
                : query.Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            int totalRowCount = query.Count();
            double totalPage = (double)totalRowCount / pageSize;
            totalPage = Math.Ceiling(totalPage);

            var model = new ListModel<TEntity>
            {
                List = data,
                TotalPage = (int)totalPage,
                TotalRowCount = totalRowCount
            };
            return model;
        }

        public List<TResult> GetList<TResult>(
            int pageNo,
            int pageSize,
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool enableTracking = true) where TResult : class
        {
            IQueryable<TEntity> query = _dbSet;
            if (!enableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            if (predicate != null) query = query.Where(predicate);

            return orderBy != null
                ? orderBy(query).Select(selector).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList()
                : query.Select(selector).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }
        public void UpdateRange(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            _dbContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
        public void DeleteRange(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public int GetTotalRowCount(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
                return _dbSet.AsNoTracking().Count();
            else
                return _dbSet.AsNoTracking().Count(filter);
        }
    }
}