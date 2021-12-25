using System.ComponentModel.DataAnnotations.Schema;

namespace Acr.Entities.Abstract
{
    public interface IExtraJsonObject<TObject> // where TObject : BaseExtraObject
    {
        string JsonString { get; set; }

        [NotMapped]
        TObject ExtraObject { get; set; }
    }
}