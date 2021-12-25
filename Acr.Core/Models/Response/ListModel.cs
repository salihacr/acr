using System.Collections.Generic;

namespace Acr.Core.Models.Response
{
    public class ListModel<TEntity>
    {
        public List<TEntity> List { get; set; }
        public int TotalPage { get; set; }
        public int TotalRowCount { get; set; }
        //public bool More { get; set; }
    }
}