using RabbitMQ.Client;
using Support.Chat.Portal.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void InitializeQue(string queName,int queLength) {
            _channel.QueueDeclare(
                queue: queName.ToUpper(), 
                durable: true, exclusive: false,
                autoDelete: false, 
                arguments: 
                new Dictionary<string, object>() { 
                    ["x-max-length"] = queLength,
                    ["x-overflow"] = "reject-publish" }
                );
        }

    }
}
