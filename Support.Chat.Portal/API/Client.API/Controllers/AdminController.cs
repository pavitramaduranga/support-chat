using Microsoft.AspNetCore.Mvc;
using Support.Chat.Portal.Queue;

namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IQueueService _queueService;

        public AdminController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpPost("Stop")]
        public ActionResult StopShift()
        {
            _queueService.StopQueues();
            return Created("Queues cleared", null);
        }
    }
}
