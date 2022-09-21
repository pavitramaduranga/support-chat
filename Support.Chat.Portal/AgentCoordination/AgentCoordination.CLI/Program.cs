// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Linq;
using AgentCoordination.CLI;

Console.WriteLine("Agent Coordination CLI");


#region Agent Que Initiator

AgentQueInitiatorService agentQueInitiator = new AgentQueInitiatorService();
agentQueInitiator.InitializeAgentQues();

#endregion

#region Listner to the session que

agentQueInitiator.InitializeQue("TASK_QUEUE", 1000);

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
        PublishToChatAgents(message);
        Console.WriteLine(" [x] Connected");

        // Note: it is possible to access the channel via
        //       ((EventingBasicConsumer)sender).Model here
        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };
    channel.BasicConsume(queue: "TASK_QUEUE",
                         autoAck: false,
                         consumer: consumer);
    #endregion
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