using Acr.Entities.Abstract;

namespace Acr.Entities.Concrete
{
    public class Example : BaseEntity, ISoftDeletable
    {
        public string TestString { get; set; }
        public int TestNumber { get; set; }

        public bool IsDeleted { get; set; }
    }
}