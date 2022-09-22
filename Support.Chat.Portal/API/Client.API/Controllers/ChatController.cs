using Client.API.Services;
using Microsoft.AspNetCore.Mvc;


namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ISessionQueueService _sessionQueueService;

        public ChatController(ISessionQueueService sessionQueueService)
        {
            _sessionQueueService = sessionQueueService;
        }

        [HttpGet]
        public string Get(string useremail)
        {
            _sessionQueueService.PublishMessageToSessionQueue(useremail);
            return "Chat Client connected";
        }
    }
}
