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

               string message = "Привет сервер!";               
               byte[] data = Encoding.Unicode.GetBytes(message);
               socket.Send(data);

               DateTime date = DateTime.Now;

               data = new byte[256]; // буфер для ответа
               StringBuilder builder = new StringBuilder();
               int bytes = 0; // количество полученных байт
 
               do
               {
                   bytes = socket.Receive(data, data.Length, 0);
                   builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
               }
               while (socket.Available > 0);

               Console.WriteLine(date.ToShortTimeString() + " от " + socket.RemoteEndPoint + " получена строка: " + builder.ToString());


               // закрываем сокет
               socket.Shutdown(SocketShutdown.Both);
               socket.Close();
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
            
            
        }
    }
}
