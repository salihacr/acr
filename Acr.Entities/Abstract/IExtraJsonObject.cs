using System.ComponentModel.DataAnnotations.Schema;

namespace Acr.Entities.Abstract
{
    public interface IExtraJsonObject<TObject> // where TObject : BaseExtraObject
    {
        string JsonValue { get; set; }

        [NotMapped]
        TObject ExtraObject { get; set; }
    }
}