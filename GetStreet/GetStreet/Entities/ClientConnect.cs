using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Lib;
using Lib.Entities;
using LIB.Entities;
using Lib.Enum;
using Lib.Helpers;

namespace GetStreet.Entities
{
    public class ClientConnect
    {
        public int port { get; set; }
        public string ip { get; set; }

        private Task task;

        private ManualResetEvent acceptEvent = new ManualResetEvent(false);

        private string? clientMesseage;

        private RequestCommands _request;

        private Request request;

        private List<string> streetList;

        private string? _email;
        private string? _password;

        public ClientConnect(int port, string ip)
        {
            this.port = port;
            this.ip = ip;
        }

        private Socket client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public void ConnectAsync(string msg, RequestCommands requestform)
        {
            acceptEvent.Reset();
            clientMesseage = msg;
            request = new Request();
            request.Command = requestform;
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);
            this.client_socket.BeginConnect(ipPoint, new AsyncCallback(ConnectCallBack), this.client_socket);
            acceptEvent.WaitOne();
        }

        public void ConnectAsync(string msg, RequestCommands requestform, ref List<string> ls)
        {
            acceptEvent.Reset();
            clientMesseage = msg;
            request = new Request();
            streetList = new List<string>();
            ls = streetList;
            request.Command = requestform;
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);
            this.client_socket.BeginConnect(ipPoint, new AsyncCallback(ConnectCallBack), this.client_socket);
            acceptEvent.WaitOne();
        }

        public void ConnectAsync(string email, string pass, RequestCommands requestform)
        {
            acceptEvent.Reset();
            _email = email;
            _password = pass;
            request = new Request();
            request.Command = requestform;
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);
            this.client_socket.BeginConnect(ipPoint, new AsyncCallback(ConnectCallBack), this.client_socket);
            acceptEvent.WaitOne();
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                this.client_socket.EndConnect(ar);

                switch (request.Command)
                {
                    case RequestCommands.Ping:
                        ReauestPing();
                        break;
                    case RequestCommands.Auth:
                        AuthConnect();
                        break;
                    case RequestCommands.Zip:
                        ReauestZip();
                        break;
                    default:
                        MessageBox.Show(" No Command ");
                        break;
                }

                // закрываем сокет
                Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                acceptEvent.Set();
            }
        }

        public void Disconnect()
        {
            client_socket.Shutdown(SocketShutdown.Both);
            this.client_socket.BeginDisconnect(true, new AsyncCallback(DisconnectCallBack), this.client_socket);
            acceptEvent.WaitOne();
        }
        private void DisconnectCallBack(IAsyncResult ar)
        {
            Socket handler = ar.AsyncState as Socket;
            handler = client_socket;
            handler.EndDisconnect(ar);
            acceptEvent.Set();
            //MessageBox.Show("Connection closed");
        }

        Socket _socket;
        public void AuthConnect()
        {
             request.Command = RequestCommands.Auth;

             // Создаем тело запроса
             Auth auth = new Auth();
             auth.Email = _email;

             byte[] salt = new byte[_email.Length];

             PasswordHash passwordHash = new PasswordHash(_password, new SHA256CryptoServiceProvider(), salt);

             auth.Password = passwordHash.password;

            try
            {
                request.Body = auth;
                BinaryFormatter formatter = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    try
                    {
                        formatter.Serialize(ms, request);
                        byte[] r = ms.ToArray();

                        // Отправка сущности на сервер
                        client_socket.Send(r);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            int bytes = 0; // количество полученных байтов
            byte[] data = new byte[1024]; // буфер для получаемых данных

            do
            {
                bytes = client_socket.Receive(data);
            }
            while (client_socket.Available > 0);

            Response response;
            Auth authResponse;
            using (MemoryStream ms = new MemoryStream(data))
            {
                //Auth auth = new Auth();
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    response = (Response)formatter.Deserialize(ms);
                    switch (response.Status)
                    {
                        case ResponseStatus.OK:
                            authResponse = (Auth)response.Body;
                            MessageBox.Show(authResponse.msg);
                            break;

                        case ResponseStatus.NOT_FOUND:
                            authResponse = (Auth)response.Body;
                            MessageBox.Show(authResponse.msg);
                            break;

                        default:
                            MessageBox.Show("Ваш запрос не может быть обработан \nОбратитесь к разрабочтикам.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ReauestPing()
        {
            Ping ping = new Ping();
            ping.msg = clientMesseage;
            request.Body = ping;
            BinaryFormatter formatter = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, request);
                    byte[] r = ms.ToArray();

                    // Отправка сущности на сервер
                    client_socket.Send(r);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            int bytes = 0; // количество полученных байтов
            byte[] data = new byte[1024]; // буфер для получаемых данных

            do
            {
                bytes = client_socket.Receive(data);
            }
            while (client_socket.Available > 0);

            Response response;

            using (MemoryStream ms = new MemoryStream(data))
            {
                try
                {
                    response = (Response)formatter.Deserialize(ms);
                    switch (response.Status)
                    {
                        case ResponseStatus.OK:
                            Ping pingResp = (Ping)response.Body;
                            //Console.WriteLine(pingResp.msg);
                            MessageBox.Show(DateTime.Now.ToShortTimeString() + " от " +
                                            client_socket.RemoteEndPoint + " получена строка: " + pingResp.msg);
                            break;

                        default:
                            MessageBox.Show("Ваш запрос не может быть обработан \nОбратитесь к разрабочтикам.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ReauestZip()
        {
            ZipCode zipCode = new ZipCode();
            zipCode.Zip = clientMesseage;
            request.Body = zipCode;
            BinaryFormatter formatter = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, request);
                    byte[] r = ms.ToArray();

                    // Отправка сущности на сервер
                    client_socket.Send(r);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            int bytes = 0; // количество полученных байтов
            byte[] data = new byte[1024]; // буфер для получаемых данных

            do
            {
                bytes = client_socket.Receive(data);
            }
            while (client_socket.Available > 0);

            Response response;

            using (MemoryStream ms = new MemoryStream(data))
            {
                try
                {
                    response = (Response)formatter.Deserialize(ms);
                    switch (response.Status)
                    {
                        case ResponseStatus.ZIP:

                            List<Street> ls1 = new List<Street>();

                            foreach (var str in (List<Street>)response.Body)
                            {
                                ls1.Add(str);
                            }

                            foreach (var finishList in ls1)
                            {
                                streetList.Add(finishList.Name);
                            }

                            break;

                        default:
                            MessageBox.Show("Ваш запрос не может быть обработан \nОбратитесь к разрабочтикам.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
