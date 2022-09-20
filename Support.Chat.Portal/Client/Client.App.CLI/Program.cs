// See https://aka.ms/new-console-template for more information
Console.WriteLine("Chat Client App");

Console.WriteLine("Kindly enter your name : ");

string clientName = Console.ReadLine();

//send client name to api 

Console.WriteLine($"Hi {clientName} Your support agent is {clientName}.");

//loop and call the poll end point once every 3 seconds
