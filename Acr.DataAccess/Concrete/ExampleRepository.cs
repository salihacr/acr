using System;
using Acr.DataAccess.Abstract;
using Acr.Entities.Concrete;

namespace Acr.DataAccess.Concrete
{
    public class ExampleRepository : BaseRepository<Example>, IExampleRepository
    {
        public ExampleRepository(AppDbContext dbContext) : base(dbContext) { }

        public void ExampleFunction(string a)
        {
            throw new NotImplementedException();
        }
    }
}