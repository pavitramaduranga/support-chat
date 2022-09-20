using System.Text;
using RabbitMQ.Client;

namespace Client.API.Services
{
    public class SessionQueService
    {
        public void PublishMessage(string message)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())

            using (var channel = connection.CreateModel())
            {
                //var queueCount = channel.MessageCount("task_queue");

                //supportRequest.User = i.ToString();
                //supportRequest.Id = Guid.NewGuid();
                //var supportCreatedMessage = new SupportRequestCreatedMessage()
                //{
                //    MessageId = Guid.NewGuid(),
                //    SupportRequest = supportRequest,
                //    CreatedDate = DateTime.UtcNow
                //};

                //var json = JsonConvert.SerializeObject(supportCreatedMessage);
                //var body = Encoding.UTF8.GetBytes(json);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: null, body: body);

            }

        }

    }
}