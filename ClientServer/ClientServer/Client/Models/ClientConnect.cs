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
        
        private ManualResetEvent acceptEvent = new ManualResetEvent(false);

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

               //DateTime date = DateTime.Now;

               data = new byte[256]; // буфер для ответа
               StringBuilder builder = new StringBuilder();
               int bytes = 0; // количество полученных байт
 
               do
               {
                   bytes = socket.Receive(data, data.Length, 0);
                   builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
               }
               while (socket.Available > 0);

               Console.WriteLine(DateTime.Now.ToShortTimeString() + " от " + socket.RemoteEndPoint + " получена строка: " + builder.ToString());
               builder.Clear();
               // закрываем сокет
               socket.Shutdown(SocketShutdown.Both);
               socket.Close();
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
        }

        public void ConnectData(string message)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);          
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);
                
                //DateTime date = DateTime.Now;

                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт
 
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);

                
                Console.WriteLine("Ответ сервера: " + builder.ToString());


                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Socket client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public void ConnectAsync()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);
            this.client_socket.BeginConnect(ipPoint, new AsyncCallback(ConnectCallBack), this.client_socket);
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                this.client_socket.EndConnect(ar);
                string message = "Привет сервер!";               
                byte[] data = Encoding.Unicode.GetBytes(message);
                client_socket.Send(data);

                //DateTime date = DateTime.Now;

                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт
 
                do
                {
                    bytes = client_socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (client_socket.Available > 0);

                Console.WriteLine(DateTime.Now.ToShortTimeString() + " от " + client_socket.RemoteEndPoint + " получена строка: " + builder.ToString());
            
                // закрываем сокет
                Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Disconnect()
        {
            this.client_socket.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), this.client_socket);

        }
        private void DisconnectCallBack(IAsyncResult ar)
        {
            Socket handler = ar.AsyncState as Socket;
            handler.EndDisconnect(ar);
            Console.WriteLine("Connection closed");
        }

        public void ConnectAsyncData()
        {
            acceptEvent.Reset();
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);
            this.client_socket.BeginConnect(ipPoint, new AsyncCallback(ConnectCallBackData), this.client_socket);
            acceptEvent.WaitOne();
        }

        public void ConnectCallBackData(IAsyncResult ar)
        {
            Console.WriteLine("1 - для получения текущего времени\n" +
                              "2 - для получения даты\n" +
                              "Введите цифру: ");
            string? mes = Console.ReadLine();
            try
            {
                // подключаемся к удаленному хосту
                Socket handler = (Socket)ar.AsyncState;
                this.client_socket.EndConnect(ar);
                byte[] data = Encoding.Unicode.GetBytes(mes);
                client_socket.Send(data);
               
                //DateTime date = DateTime.Now;

                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = client_socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                } while (client_socket.Available > 0);
                
                Console.WriteLine("Ответ сервера: " + builder.ToString());

                // закрываем сокет
                Disconnect();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
