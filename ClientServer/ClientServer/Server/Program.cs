using Server.Models;

SocketServer server = new SocketServer(4000, "127.0.0.1");
server.Start();

// Delay.
Console.ReadKey();