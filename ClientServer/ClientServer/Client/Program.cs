using Client.Models;

ClientConnect client = new ClientConnect(4000, "127.0.0.1");

client.Connect();

// Delay.
Console.ReadKey();