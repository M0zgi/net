using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Lib;
using Lib.Data;
using Lib.Entities;
using Lib.Enum;

namespace ServerConsole.Entities
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

        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public void StartAsync()
        {
            if (task != null)
                Console.WriteLine("Сервер запущен");

            this.task = new Task(this.RunAsync);
            task.Start();
        }
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

                int bytes = 0; // количество полученных байтов
                byte[] data = new byte[1024]; // буфер для получаемых данных

                do
                {
                    bytes = accept_socket.Receive(data);
                }
                while (accept_socket.Available > 0);

                BinaryFormatter formatter = new BinaryFormatter();
                Request request;
               

                using (MemoryStream ms = new MemoryStream(data))
                {
                    try
                    {
                        request = (Request)formatter.Deserialize(ms);
                        switch (request.Command)
                        {
                            case RequestCommands.Auth:
                                Auth auth = (Auth) request.Body;
                                Console.WriteLine(auth.Email);
                                Console.WriteLine(auth.Password);
                                bool check;
                                User user = new User();
                                user.Email = auth.Email;
                                user.Password = auth.Password;
                                check = Login(user);
                                if (check)
                                {
                                    Console.WriteLine("Ok");
                                    socketOk = accept_socket;
                                    LoginOkDeny(check);
                                }

                                else
                                {
                                    Console.WriteLine("No");
                                    socketOk = accept_socket;
                                    LoginOkDeny(check);
                                }

                                break;
                            case RequestCommands.Ping:
                                Ping ping = (Ping) request.Body;
                                Console.WriteLine(ping.msg);
                                if (accept_socket.Connected)
                                {
                                    Console.WriteLine(DateTime.Now.ToShortTimeString() + " от " + accept_socket.RemoteEndPoint +
                                                      " получена строка: " + ping.msg);

                                    socketOk = accept_socket;
                                    SendOk();
                                }
                                break;
                            case RequestCommands.Zip:
                                ZipCode zipCode = (ZipCode) request.Body;
                                Console.WriteLine(zipCode.Zip);
                                socketOk = accept_socket;
                                SendZip(zipCode.Zip);
                                break;
                            default:
                                Console.WriteLine(" No Command ");
                                break;
                        }
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DisconnectCallBack(IAsyncResult ar)
        {
            Socket handler = ar.AsyncState as Socket;
            handler.EndDisconnect(ar);
            Console.WriteLine("Connection closed");
        }

        private Socket socketOk;
        private void SendOk()
        {
            Response response = new Response();

            response.Status = ResponseStatus.OK;
            string message = "Привет клиент!";
            Ping pingResp = new Ping();
                                    
            //Ping pingResp = (Ping) response.Body;
            pingResp.msg = message;
            response.Body = pingResp;
            BinaryFormatter formatter = new BinaryFormatter();
                                    
            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, response);
                    byte[] r = ms.ToArray();

                    // Отправка сущности на сервер
                    socketOk.Send(r);
                    socketOk.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), socketOk);
                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void SendZip(string zip)
        {
            Response response = new Response();

            response.Status = ResponseStatus.ZIP;

            using var dbContext = new ApplicationDbContext();

            var street = dbContext
                .Streets
                .Where(x => x.ZipCode.Zip == zip)
                .OrderByDescending(x => x.Name);

            response.Body = street.ToList();
            BinaryFormatter formatter = new BinaryFormatter();
                                    
            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, response);
                    byte[] r = ms.ToArray();

                    // Отправка сущности на сервер
                    socketOk.Send(r);
                    socketOk.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), socketOk);
                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private bool Login(User user)
        {
            using var dbContext = new ApplicationDbContext();

            var login = dbContext.Users
                .FirstOrDefault(x => x.Email == user.Email);

            bool b = user.Equals(login);

            return b;
        }

        private void LoginOkDeny(bool check)
        {
            Response response = new Response();
            string message = "";
            if (check)
            {
                response.Status = ResponseStatus.OK;
                message = "Вы успешно авторизовались! Клиент найден в базе";
            }

            else
            {
                response.Status = ResponseStatus.NOT_FOUND;
                message = "Клиент не найден в базе. Проверьте введенные данные";
            }
            
            Auth auth = new Auth();
            auth.msg = message;
            response.Body = auth;
            BinaryFormatter formatter = new BinaryFormatter();
                                    
            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, response);
                    byte[] r = ms.ToArray();

                    // Отправка сущности на сервер
                    socketOk.Send(r);
                    socketOk.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), socketOk);
                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
