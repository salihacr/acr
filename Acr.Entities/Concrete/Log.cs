using System;
using Acr.Entities.Abstract;

namespace Acr.Entities.Concrete
{
    public class Log : BaseEntity
    {
        public Log()
        {
            TransactionDateTime = DateTime.Now;
            TransactionType = -1;
            ModuleId = -1;
            AffectedRecordId = -1;
        }
        public string UserId { get; set; }
        public int ModuleId { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public int TransactionType { get; set; }
        public long AffectedRecordId { get; set; }
        public string AffectedTableName { get; set; }
    }
}