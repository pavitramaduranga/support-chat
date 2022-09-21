// See https://aka.ms/new-console-template for more information
using AgentCoordination.CLI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Support.Chat.Portal.Common.Models;
using System.Text;
using System.Linq;
using System.Threading.Channels;
using AgentCoordination.CLI;

Console.WriteLine("Agent Coordination CLI");


#region AgentQueInitiator

AgentQueInitiatorService agentQueInitiator = new AgentQueInitiatorService();
const int MaxConcurrentCount = 10;
// get team
// get team members
using (var db = new CoordinatorDbContext())
{
    var team = db.Teams.Include(o => o.Agents).ThenInclude(o => o.Seniority).FirstOrDefault(o => o.Shift == Shift.OfficeTime);
    var seniorities = db.Seniorities.ToList();

    if (team == null)
    {
        //return NotFound("Not found team");
    }

    var availableAgents = team?.Agents.ToList();

    var juniorAgents = availableAgents?.Select(x => x.SeniorityId == 1);
    if (juniorAgents?.Count() > 0)
    {
        var juniorLevelData = seniorities.FirstOrDefault(x=>x.Name == "Junior");

        int juniorAgentCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(juniorLevelData?.Efficiency * MaxConcurrentCount)));

        agentQueInitiator.InitializeQue(juniorLevelData?.Name, juniorAgentCapacity);
    }

    var midLevelAgents = availableAgents?.Select(x => x.SeniorityId == 2);
    if (midLevelAgents?.Count() > 0)
    {
        var midLevelData = seniorities.FirstOrDefault(x => x.Name == "MidLevel");
        int midLevelAgentCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(midLevelData?.Efficiency * MaxConcurrentCount)));

        agentQueInitiator.InitializeQue(midLevelData?.Name, midLevelAgentCapacity);
    }

    var seniorAgents = availableAgents?.Select(x => x.SeniorityId == 3);
    if (seniorAgents?.Count() > 0)
    {
        var seniorLevelData = seniorities.FirstOrDefault(x => x.Name == "Senior");
        int seniorLevelAgentCapacity = Convert.ToInt32(Math.Floor(Convert.ToDouble(seniorLevelData?.Efficiency * MaxConcurrentCount)));

        agentQueInitiator.InitializeQue(seniorLevelData?.Name, seniorLevelAgentCapacity);
    }


    // update database with data

    //// update max count in shared database (redis)
    //var capacity = Math.Floor(concurrentChatCount * MaxQueueTheshold);
    //await _distributedCache.SetStringAsync(QueueMaxCountCacheKey, capacity.ToString());
    //var a = await _distributedCache.GetStringAsync(QueueMaxCountCacheKey);

}

#endregion

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "task_queue",
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null
                         );

    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

    Console.WriteLine(" [*] Waiting for messages.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Received {0}", message);

        //Publish to agents
        PublishToChatAgents(message);



        Console.WriteLine(" [x] Connected");

        // Note: it is possible to access the channel via
        //       ((EventingBasicConsumer)sender).Model here
        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };
    channel.BasicConsume(queue: "task_queue",
                         autoAck: false,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}

void PublishToChatAgents(string v)
{

    //Get shift
    //Get Capacity for each seniority level (shift)//
    //get active sessions for each level//
    //each level active session < capacity
    //add to que
    //save session request


    var factory = new ConnectionFactory() { HostName = "localhost" };
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {

        var body = Encoding.UTF8.GetBytes(v);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: "", routingKey: "JUNIOR", basicProperties: properties, body: body);
    }
}