using Microsoft.AspNetCore.Mvc;

using Acr.Business.Abstract;
using Acr.Core.Models.Request;
using Acr.Entities.Concrete;

namespace Acr.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestJsonController : BaseController
    {
        private readonly IManager<TestJsonTable> _manager;
        public TestJsonController(IManager<TestJsonTable> manager)
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
        public IActionResult GetAll(PaginationRequest request)
        {
            //var result = _manager.GetAll(request);
            //var result = _manager.GetAllFromView<ViewTestJsonTable>(request);4

            var result = _manager.GetAllWithObject<TestJson>(request);
            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create(TestJsonTable entity)
        {
            int userId = GetUserId();
            var result = _manager.Add(entity, userId);
            return Ok(result);
        }

        [HttpPost("Update")]
        public IActionResult Update(TestJsonTable entity)
        {
            int userId = GetUserId();
            var result = _manager.Update(entity, userId);
            return Ok(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int id)
        {
            int userId = GetUserId();
            var result = _manager.Delete(id, userId);
            return Ok(result);
        }
    }
}
