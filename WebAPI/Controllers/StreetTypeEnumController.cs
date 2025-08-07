using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Enums;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StreetTypeEnumController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAll()
        {
            var values = System.Enum.GetNames(typeof(StreetTypeEnum)).ToList();
            return Ok(values);
        }
    }
}
