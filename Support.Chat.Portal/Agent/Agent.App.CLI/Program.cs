// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Agent Client App");

Console.WriteLine("Press 1 for Junior Level");
Console.WriteLine("Press 2 for Mid Level");
Console.WriteLine("Press 3 for Senior Level");
Console.WriteLine("Press 4 for TeamLead Level");

string agentLevel = Console.ReadLine();
string queName = string.Empty;
switch (agentLevel)
{
    case "1":
        queName = "Junior";
        Console.WriteLine("Logged in as Junior Level");
        break;
    case "2":
        queName = "MidLevel";
        Console.WriteLine("Logged in as Mid Level");
        break;
    case "3":
        queName = "Senior";
        Console.WriteLine("Logged in as Senior Level");
        break;
    case "4":
        queName = "TeamLead";
        Console.WriteLine("Logged in as TeamLead Level");
        break;

    default:
        Console.WriteLine("Invalid agent level");
        break;
}

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    Console.WriteLine(" [-] Waiting for messages.");

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Connected {0}", message);
        Thread.Sleep(10 * 1000);

        Console.WriteLine(" [x] Agent closed the chat");

        // Note: it is possible to access the channel via
        //       ((EventingBasicConsumer)sender).Model here
        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };
    channel.BasicConsume(queue: queName.ToUpper(),
                         autoAck: false,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}