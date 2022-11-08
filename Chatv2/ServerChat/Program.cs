using System.Net;
using System.Net.Sockets;
using Lib.Entityes;
using ServerChat.Entities;

IPAddress address = IPAddress.Parse(Server.Host);
Server.ServerSocket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
Server.ServerSocket.Bind(new IPEndPoint(address, Server.Port));
Server.ServerSocket.Listen(100);

while(Server.Work)
{
    Socket handle = Server.ServerSocket.Accept();
    Console.WriteLine($"New connection: {handle.RemoteEndPoint.ToString()}");
    new ServerUser(handle);

}
Console.WriteLine("Server closeing...");