using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsLiveController : ControllerBase
    {
        [HttpGet]
        public string Get(string useremail)
        {
            return "Chat session is live";

        }
    }
}
