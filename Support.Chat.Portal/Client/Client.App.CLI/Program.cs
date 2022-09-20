// See https://aka.ms/new-console-template for more information
Console.WriteLine("Chat Client App");

Console.WriteLine("Kindly enter your name : ");

string clientName = Console.ReadLine();

//send client name to api 
//https://localhost:7078/api/IsLive?useremail=pmd%40gmail.com


Console.WriteLine($"Hi {clientName} Your support agent is {clientName}.");

//loop and call the poll end point once every 3 seconds
//https://localhost:7078/api/Chat?useremail=pmd%40gmail.com