using Acr.Core.Models.Request;
using Acr.Core.Result;
using Acr.Entities.Concrete;

namespace Acr.Business.Abstract
{
    public interface IAuthManager : IManager<User>
    {
        TaskResult Login(LoginRequest loginRequest);
        TaskResult CreateUser(User user, int userId);
        TaskResult UpdateUser(User user, int userId);
    }
}