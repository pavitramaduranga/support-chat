﻿// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Agent Client App");
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
    channel.BasicConsume(queue: "JUNIOR",
                         autoAck: false,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}