using Acr.Entities.Abstract;

namespace Acr.Entities.Concrete
{
    public class TestTable : BaseEntity, ISoftDeletable
    {
        public bool IsDeleted { get; set; }
    }
}