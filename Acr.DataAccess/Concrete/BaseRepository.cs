using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Acr.DataAccess.Abstract;
using Acr.Entities.Abstract;
using Microsoft.EntityFrameworkCore;

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
            return _dbSet.Find(filter);
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