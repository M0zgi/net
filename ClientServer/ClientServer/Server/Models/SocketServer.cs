using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class SocketServer
    {
        public int port { get; set; }
        public string ip { get; set; }

        private Task task;
        
        public SocketServer(int port, string ip)
        {
            this.port = port;
            this.ip = ip;
        }

        /// <summary>
        /// Запуск задачи на прослушивание порта
        /// </summary>
        public void Run()
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
                listenSocket.Listen(10);

                // получаем сообщение
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байтов

                byte[] data = new byte[256]; // буфер для получаемых данных

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                DateTime date = DateTime.Now;

                while (true)
                {
                    Socket handler = listenSocket.Accept();

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(date.ToShortTimeString() + " от " + handler.RemoteEndPoint + " получена строка: " + builder.ToString());

                    // отправляем ответ
                    string message = "Привет клиент!";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);

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
    }
}
