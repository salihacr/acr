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
    public class TestController : BaseController
    {
        private readonly IManager<TestTable> _manager;
        public TestController(IManager<TestTable> manager)
        {
            _manager = manager;
        }

        [HttpPost("Get")]
        public IActionResult Get(int id)
        {
            var result = _manager.Get(id);
            return Ok(result);
        }
        [HttpPost("GetAll")]
        public TaskResult GetAll(PaginationRequest request)
        {
            //var result = _manager.GetAll(request);
            var result = _manager.GetAllFromView<ViewTestTable>(request);
            return result;
        }

        [HttpPost("Create")]
        public IActionResult Create(TestTable entity)
        {
            int userId = GetUserId();
            var result = _manager.Add(entity, userId);
            return Ok(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(TestTable entity)
        {
            int userId = GetUserId();
            var result = _manager.Update(entity, userId);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            int userId = GetUserId();
            var result = _manager.Delete(id, userId);
            return Ok(result);
        }


    }
}
