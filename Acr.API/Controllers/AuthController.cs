using Microsoft.AspNetCore.Mvc;
using Acr.Business.Abstract;
using Acr.Core.Models.Request;
using Acr.Entities.Concrete;
using Acr.Entities.Views;
using Acr.Core.Result;

namespace Acr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthManager _authManager;
        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var result = _authManager.Login(loginRequest);
            return Ok(result);
        }
        [HttpPost("Create")]
        public IActionResult Create(User entity)
        {
            int userId = GetUserId();
            var result = _authManager.CreateUser(entity, userId);
            return Ok(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(User entity)
        {
            int userId = GetUserId();
            var result = _authManager.UpdateUser(entity, userId);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            int userId = GetUserId();
            var result = _authManager.Delete(id, userId);
            return Ok(result);
        }
    }
}
