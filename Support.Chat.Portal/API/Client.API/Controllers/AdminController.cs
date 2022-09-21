using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Client.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private ConnectionFactory _factory;
        private readonly IConnection _connection;
        private IModel _channel;
        public AdminController()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        [HttpPost("Stop")]
        public ActionResult StopShift()
        {
            _channel.QueueDelete("TASK_QUEUE", false, false);
            _channel.QueueDelete("JUNIOR", false, false);
            _channel.QueueDelete("MIDLEVEL", false, false);
            _channel.QueueDelete("SENIOR", false, false);

            return Created("", null);
        }
    }
}
