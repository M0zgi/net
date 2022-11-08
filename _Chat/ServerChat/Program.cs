using ServerChat.Entities;

SocketServer server = new SocketServer(4041, "127.0.0.1");

server.StartAsync();

Console.WriteLine("IP: 127.0.0.1 \nPort: 4041");
// Delay.
Console.ReadKey();
