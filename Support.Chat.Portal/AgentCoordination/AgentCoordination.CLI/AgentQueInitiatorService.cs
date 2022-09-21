using RabbitMQ.Client;
using Support.Chat.Portal.Common.DTO;

namespace AgentCoordination.CLI
{
    public class AgentQueInitiatorService
    {
        private ConnectionFactory _factory;
        private readonly IConnection _connection;
        private IModel _channel;

        public AgentQueInitiatorService()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void InitializeAgentQueues(AgentCapacityPerShift agentCapacityPerShift)
        {
            if (agentCapacityPerShift.JuniorCapacity > 0)
            {
                InitializeQueues("JUNIOR", agentCapacityPerShift.JuniorCapacity);
            }
            if (agentCapacityPerShift.MidLevelCapacity > 0)
            {
                InitializeQueues("MIDLEVEL", agentCapacityPerShift.MidLevelCapacity);
            }
            if (agentCapacityPerShift.SeniorCapacity > 0)
            {
                InitializeQueues("SENIOR", agentCapacityPerShift.SeniorCapacity);
            }
            if (agentCapacityPerShift.TeamLeadCapacity > 0)
            {
                InitializeQueues("TEAMLEAD", agentCapacityPerShift.TeamLeadCapacity);
            }
            if (agentCapacityPerShift.OverFlowCapacity > 0)
            {
                InitializeQueues("OVERFLOW", agentCapacityPerShift.OverFlowCapacity);
            }
        }

        public void InitializeQueues(string queName, int queLength)
        {
            _channel.QueueDeclare(
                queue: queName.ToUpper(),
                durable: true, exclusive: false,
                autoDelete: false,
                arguments:
                new Dictionary<string, object>()
                {
                    ["x-max-length"] = queLength,
                    ["x-overflow"] = "reject-publish"
                }
            );
        }
    }
}
