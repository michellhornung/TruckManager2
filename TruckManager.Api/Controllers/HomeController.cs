using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TruckManager.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return base.Ok("Truck Manager is Running...");
        }
    }
}
