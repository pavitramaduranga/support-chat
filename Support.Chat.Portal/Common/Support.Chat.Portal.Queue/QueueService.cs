using RabbitMQ.Client;

namespace Support.Chat.Portal.Queue
{
    public class QueueService : IQueueService
    {
        private ConnectionFactory _factory;
        private readonly IConnection _connection;
        private IModel _channel;
        public QueueService()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void StopQueues()
        {
            _channel.QueueDelete("TASK_QUEUE", false, false);
            _channel.QueueDelete("JUNIOR", false, false);
            _channel.QueueDelete("MIDLEVEL", false, false);
            _channel.QueueDelete("SENIOR", false, false);
        }
    }
}