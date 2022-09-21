using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using AgentCoordination.CLI;
using Support.Chat.Portal.Common.DTO;
using Support.Chat.Portal.Common.Enums;

Console.WriteLine("Agent Coordination CLI");


#region Agent Que Initiator

AgentService agentService = new AgentService();
AgentCapacityPerShift agentCapacityPerShift = agentService.GetAgentCapacityPerShift(Shift.OfficeTime);
int totalCapacityOfTheShift = agentService.GetAgentCapacityPerShift(agentCapacityPerShift);

AgentQueInitiatorService agentQueInitiator = new();
agentQueInitiator.InitializeAgentQueues(agentCapacityPerShift);

#endregion

#region Listner to the session queue

agentQueInitiator.InitializeQueues("TASK_QUEUE", totalCapacityOfTheShift);

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{

    Console.WriteLine(" [*] Waiting for messages.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Received {0}", message);

        //Publish to agents
        PublishToChatAgents(agentCapacityPerShift, message);
        Console.WriteLine(" [x] Connected");

        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };
    channel.BasicConsume(queue: "TASK_QUEUE", autoAck: false, consumer: consumer);
}
#endregion

Console.WriteLine(" Press enter to exit.");
Console.ReadLine();


void PublishToChatAgents(AgentCapacityPerShift agentCapacityPerShift, string message)
{
    var factory = new ConnectionFactory() { HostName = "localhost" };
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {

        var body = Encoding.UTF8.GetBytes(message);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        if (agentCapacityPerShift.JuniorCapacity != 0 && (int)channel.MessageCount("JUNIOR") < agentCapacityPerShift.JuniorCapacity)
        {
            channel.BasicPublish(exchange: "", routingKey: "JUNIOR", basicProperties: properties, body: body);
        }
        else if (agentCapacityPerShift.MidLevelCapacity != 0 && (int)channel.MessageCount("MIDLEVEL") < agentCapacityPerShift.MidLevelCapacity)
        {
            channel.BasicPublish(exchange: "", routingKey: "MIDLEVEL", basicProperties: properties, body: body);
        }
        else if (agentCapacityPerShift.SeniorCapacity != 0 && (int)channel.MessageCount("SENIOR") < agentCapacityPerShift.SeniorCapacity)
        {
            channel.BasicPublish(exchange: "", routingKey: "SENIOR", basicProperties: properties, body: body);
        }
        else if (agentCapacityPerShift.TeamLeadCapacity != 0 && (int)channel.MessageCount("TEAMLEAD") < agentCapacityPerShift.TeamLeadCapacity)
        {
            channel.BasicPublish(exchange: "", routingKey: "TEAMLEAD", basicProperties: properties, body: body);
        }
        //TODO :: only applicable to office hours shift
        else if (agentCapacityPerShift.OverFlowCapacity != 0 && (int)channel.MessageCount("OVERFLOW") < agentCapacityPerShift.OverFlowCapacity)
        {
            channel.BasicPublish(exchange: "", routingKey: "OVERFLOW", basicProperties: properties, body: body);
        }
    }
}