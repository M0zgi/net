using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Server.Models
{
    public class SocketServer
    {
        public int port { get; set; }
        public string ip { get; set; }

        private Task task;

        private int max_conn = 10;

        private ManualResetEvent acceptEvent = new ManualResetEvent(false);
        
        public SocketServer(int port, string ip)
        {
            this.port = port;
            this.ip = ip;
        }

        /// <summary>
        /// Запуск задачи на прослушивание порта
        /// </summary>
        private void Run()
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipEndPoint);

                // начинаем прослушивание
                listenSocket.Listen(max_conn);

                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов

                byte[] data = new byte[256]; // буфер для получаемых данных

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                //DateTime date = DateTime.Now;

                while (true)
                {
                    Socket handler = listenSocket.Accept();

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + " от " + handler.RemoteEndPoint + " получена строка: " + builder.ToString());

                    // отправляем ответ
                    string message = "Привет клиент!";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    builder.Clear();
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Задача слушать порт
        /// </summary>
        public void Start()
        {
            if (task != null)
                Console.WriteLine("Сервер запущен");

            this.task = new Task(this.Run);
            task.Start();
        }

        private void RunData()
        {
            // получаем адреса для запуска сокета
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);

            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipEndPoint);

                // начинаем прослушивание
                listenSocket.Listen(max_conn);

                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов

                byte[] data = new byte[256]; // буфер для получаемых данных

                Console.WriteLine("Сервер запущен. Ожидание запроса от клиента...");

                //DateTime date = DateTime.Now;

                while (true)
                {
                    Socket handler = listenSocket.Accept();

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine("От " + handler.RemoteEndPoint + " получен запрос: " + builder.ToString());

                    string message = "";

                    if (builder.ToString() == "1")
                    {
                        // отправляем ответ
                        message = DateTime.Now.ToShortTimeString();
                    }

                    else if (builder.ToString() == "2")
                    {
                        // отправляем ответ
                        message = DateTime.Now.ToShortDateString();
                    }

                    else
                    {
                        // отправляем ответ
                        message = "Неверный запрос!";
                    }

                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);

                    builder.Clear();
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void StartData()
        {
            if (task != null)
                Console.WriteLine("Сервер запущен");

            this.task = new Task(this.RunData);
            task.Start();
        }

        public void StartAsync()
        {
            if (task != null)
                Console.WriteLine("Сервер запущен");

            this.task = new Task(this.RunAsync);
            task.Start();
        }

        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private void RunAsync()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);

            try
            {
                socket.Bind(ipEndPoint);
                socket.Listen(this.max_conn);
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    acceptEvent.Reset();
                    socket.BeginAccept(new AsyncCallback(AcceptCallBack), socket);
                    acceptEvent.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                Socket accept_socket = socket.EndAccept(ar);
            
                acceptEvent.Set();
                //Console.WriteLine("A new connection. IP:port = " + accept_socket.RemoteEndPoint.ToString());

                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов

                byte[] data = new byte[256]; // буфер для получаемых данных

               // while (true)
               // {
                    //Socket handler = socket.Accept();
                    //acceptEvent.Reset();
                    do
                    {
                        bytes = accept_socket.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (accept_socket.Available > 0);

                    if (accept_socket.Connected)
                    {
                        Console.WriteLine(DateTime.Now.ToShortTimeString() + " от " + accept_socket.RemoteEndPoint +
                                          " получена строка: " + builder.ToString());
                    }

                    // отправляем ответ
                    string message = "Привет клиент!";
                    data = Encoding.Unicode.GetBytes(message);

                    if (accept_socket.Connected)
                    {
                        accept_socket.Send(data);
                        accept_socket.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), accept_socket);
                    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DisconnectCallBack(IAsyncResult ar)
        {
            //acceptEvent.WaitOne();
            Socket handler = ar.AsyncState as Socket;
            handler.EndDisconnect(ar);
            Console.WriteLine("Connection closed");
        }

        public void StartAsyncData()
        {
            if (task != null)
                Console.WriteLine("Сервер запущен");

            this.task = new Task(this.RunAsyncData);
            task.Start();
        }

        private void RunAsyncData()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);

            try
            {
                socket.Bind(ipEndPoint);
                socket.Listen(this.max_conn);
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    acceptEvent.Reset();
                    socket.BeginAccept(new AsyncCallback(AcceptCallBackData), socket);
                    acceptEvent.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AcceptCallBackData(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                Socket accept_socket = socket.EndAccept(ar);
            
                acceptEvent.Set();
                //Console.WriteLine("A new connection. IP:port = " + accept_socket.RemoteEndPoint.ToString());

                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов

                byte[] data = new byte[256]; // буфер для получаемых данных

                do
                {
                    bytes = accept_socket.Receive(data);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (accept_socket.Available > 0);

                if (accept_socket.Connected)
                {
                    Console.WriteLine("От " + accept_socket.RemoteEndPoint + " получен запрос: " + builder.ToString());

                    string message = "";

                    if (builder.ToString() == "1")
                    {
                        // отправляем ответ
                        message = DateTime.Now.ToShortTimeString();
                    }

                    else if (builder.ToString() == "2")
                    {
                        // отправляем ответ
                        message = DateTime.Now.ToShortDateString();
                    }

                    else
                    {
                        // отправляем ответ
                        message = "Неверный запрос!";
                    }

                    data = Encoding.Unicode.GetBytes(message);
                    //accept_socket.Send(data);

                    builder.Clear();
                    // закрываем сокет
                }

                if (accept_socket.Connected)
                {
                    accept_socket.Send(data);
                    accept_socket.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), accept_socket);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
