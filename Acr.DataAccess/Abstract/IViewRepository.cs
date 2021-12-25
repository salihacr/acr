using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Acr.Entities.Abstract;

namespace Acr.DataAccess.Abstract
{
    public interface IViewRepository
    {
        List<TView> GetListView<TView>(int pageNo = 1, int pageSize = 50, Expression<Func<TView, bool>> filter = null) where TView : BaseView;
        TView GetView<TView>(Expression<Func<TView, bool>> filter = null) where TView : BaseView;
        TView GetView<TView>(object id) where TView : BaseView;
        int GetTotalRowCount<TView>(Expression<Func<TView, bool>> filter = null) where TView : BaseView;
    }
}