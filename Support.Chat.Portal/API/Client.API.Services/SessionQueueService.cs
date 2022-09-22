using System.Text;
using RabbitMQ.Client;

namespace Client.API.Services
{
    public class SessionQueueService : ISessionQueueService
    {
        public void PublishMessageToSessionQueue(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "", routingKey: "TASK_QUEUE", basicProperties: properties, body: body);
            }
        }
    }
}