using System.ComponentModel;

namespace Acr.Entities.Abstract
{
    public interface ISoftDeletable
    {
        [DefaultValue(false)]
        bool IsDeleted { get; set; }
    }
}