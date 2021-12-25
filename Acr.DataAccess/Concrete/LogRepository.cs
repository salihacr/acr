using Acr.DataAccess.Abstract;
using Acr.Entities.Concrete;

namespace Acr.DataAccess.Concrete
{
    public class LogRepository : BaseRepository<Log>, ILogRepository
    {
        public LogRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}