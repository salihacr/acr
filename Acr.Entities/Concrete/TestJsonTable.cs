using Acr.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Acr.Entities.Concrete
{
    public class TestJsonTable : BaseEntity, ISoftDeletable, IParentChild, IExtraJsonObject<TestJson>
    {
        [NotMapped]
        public TestJson ExtraObject { get; set; }
        [JsonIgnore]
        public string JsonValue { get; set; }

        public int ParentId { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class TestJson
    {
        public int Num { get; set; }
        public string Str { get; set; }
    }
}