using Microsoft.AspNetCore.Mvc;

namespace ShopPC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("hello")]
        public ActionResult<string> Hello()
        {
            return Ok("Hello World");
        }
    }
}
