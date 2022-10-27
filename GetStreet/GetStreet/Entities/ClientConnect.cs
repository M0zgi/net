using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Lib;
using Lib.Entities;
using LIB.Entities;
using Lib.Enum;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

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

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                this.client_socket.EndConnect(ar);
                //string message = "Привет сервер!"; 

                //Request request = new Request();
                //request.Command = _request;

                switch (request.Command)
                {
                    case RequestCommands.Ping:
                        ReauestPing();
                        break;
                    case RequestCommands.Zip:
                        ReauestZip();
                        break;
                    default:
                        MessageBox.Show(" No Command ");
                        break;
                }


                //request.Command = Lib.Enum.RequestCommands.Ping;
                // Создаем тело запроса

                // auth.Password = password;

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
            MessageBox.Show("Connection closed");
        }

        Socket _socket;
        public void AuthConnect(string email, string password)
        {
            if (_socket != null)
            {
                MessageBox.Show(" Вы подсоеденены ");
                return;
            }

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);

            _socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Установка соединения
                _socket.Connect(ipPoint);

                // Создаем запрос на операцию на сервере
                Request request = new Request();
                request.Command = Lib.Enum.RequestCommands.Auth;

                // Создаем тело запроса
                Lib.Entities.Auth auth = new Lib.Entities.Auth();
                auth.Email = email;
                auth.Password = password;

                request.Body = auth;

                BinaryFormatter formatter = new BinaryFormatter();

                using (var ms = new MemoryStream())
                {
                    try
                    {
                        formatter.Serialize(ms, request);
                        byte[] r = ms.ToArray();

                        // Отправка сущности на сервер
                        _socket.Send(r);
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
                            MessageBox.Show(" No Command ");
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


                            //Street street = (Street)response.Body;
                            //streetList = (List<string>)response.Body;
                            //Console.WriteLine(pingResp.msg);
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
                            MessageBox.Show(" No Command ");
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
