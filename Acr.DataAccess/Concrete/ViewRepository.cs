using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Acr.DataAccess.Abstract;
using Acr.Entities.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Acr.DataAccess.Concrete
{
    public class ViewRepository : IViewRepository
    {
        private readonly AppDbContext _dbContext;
        public ViewRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TView> GetListView<TView>(int pageNo = 1, int pageSize = 50, Expression<Func<TView, bool>> filter = null) where TView : BaseView
        {
            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 1;
            if (filter != null)
                return _dbContext.Set<TView>().Where(filter).Skip((pageNo - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
            else
                return _dbContext.Set<TView>().Skip((pageNo - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
        }
        public TView GetView<TView>(Expression<Func<TView, bool>> filter = null) where TView : BaseView
        {
            if (filter != null)
                return _dbContext.Set<TView>().AsNoTracking().FirstOrDefault(filter);
            else
                return _dbContext.Set<TView>().AsNoTracking().FirstOrDefault();
        }
        public TView GetView<TView>(object id) where TView : BaseView
        {
            return _dbContext.Set<TView>().Find(id);
        }
        public int GetTotalRowCount<TView>(Expression<Func<TView, bool>> filter = null) where TView : BaseView
        {
            if (filter != null)
                return _dbContext.Set<TView>().AsNoTracking().Count(filter);
            else
                return _dbContext.Set<TView>().AsNoTracking().Count();
        }
    }
}