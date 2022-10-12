using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class ClientConnect
    {
        public int port { get; set; }
        public string ip { get; set; }

        private Task task;
        
        public ClientConnect(int port, string ip)
        {
            this.port = port;
            this.ip = ip;
        }

        public void Connect()
        {
           try
           {
               IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);

               Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

               // подключаемся к удаленному хосту
               socket.Connect(ipPoint);
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message);
           }
            
            
        }
    }
}
