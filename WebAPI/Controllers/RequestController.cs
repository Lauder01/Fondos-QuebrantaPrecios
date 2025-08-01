using Microsoft.AspNetCore.Mvc;
using FQP.Entities;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetRequests()
        {
            // Mock: return empty list
            return Ok(new List<Request>());
        }
    }
}
