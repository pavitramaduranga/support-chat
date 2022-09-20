// See https://aka.ms/new-console-template for more information
using AgentCoordination.CLI.Data;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Agent Coordination CLI");

#region DB Test

using (var context = new CoordinatorDbContext())
{
    foreach (var seniority in context.Seniorities)
    {
        Console.WriteLine($"seniority {seniority.Name}");

        //foreach (var post in blog.Posts)
        //{
        //    Console.WriteLine($"\t{post.Title}: {post.Content} by {post.AuthorName.First} {post.AuthorName.Last}");
        //}
    }
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
        PublishToChatAgents("message");

        int dots = message.Split('.').Length - 1;
        Thread.Sleep(dots * 1000);

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
    //Get Capacity for each seniority level (shift)
    //get active sessions for each level
    //each level active session < capacity
        //add to que
    //save session request


    var factory = new ConnectionFactory() { HostName = "localhost" };
    using (var connection = factory.CreateConnection())
    using (var channel = connection.CreateModel())
    {

        channel.QueueDeclare(queue: "task_queue_agent", durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(v);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: "", routingKey: "task_queue_agent", basicProperties: properties, body: body);
    }
}