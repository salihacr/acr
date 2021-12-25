using System.ComponentModel;

namespace Acr.Entities.Abstract
{
    public interface IParentChild
    {
        [DefaultValue(-1)]
        int ParentId { get; set; }
    }
}