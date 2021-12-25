using Acr.Entities.Concrete;

namespace Acr.DataAccess.Abstract
{
    public interface IExampleRepository : IRepository<Example>
    {
        void ExampleFunction(string a);
    }
}