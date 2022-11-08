using Lib.Enum;
using Lib.Entities;
using Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ClientForm.Entities
{
    public class ClientConnect
    {
        public int port { get; set; }
        public string ip { get; set; }

        private ManualResetEvent acceptEvent = new ManualResetEvent(false);

        private string? clientMesseage;

        private Request request;

        private string? _login;
        private string? _password;

        private bool isConnected = false;

        private List<string> chatMsg;

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

        public void ConnectAsync(string login, string pass, RequestCommands requestform, ref List<string> msg)
        {
            acceptEvent.Reset();
            _login = login;
            _password = pass;
            request = new Request();
            chatMsg = new List<string>();
            msg = chatMsg;
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
                        //Disconnect();
                        break;
                    case RequestCommands.Auth:
                        AuthConnect();
                        break;
                    //case RequestCommands.Zip:
                    //    ReauestZip();
                    //    break;
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

        private void ReauestPing()
        {
            TestServer ping = new TestServer();
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
                            TestServer pingResp = (TestServer)response.Body;
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

        public void AuthConnect()
        {
             request.Command = RequestCommands.Auth;

            // Создаем тело запроса
             Auth auth = new Auth();
             //auth.Email = _email;
             auth.Name = _login;
             auth.Password = _password;
             BinaryFormatter formatter = new BinaryFormatter();

             //byte[] salt = new byte[_email.Length];

             //PasswordHash passwordHash = new PasswordHash(_password, new SHA256CryptoServiceProvider(), salt);

             //auth.Password = passwordHash.password;

            try
            {
                request.Body = auth;
                
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
            //Auth authResponse;
            using (MemoryStream ms = new MemoryStream(data))
            {
                //Auth auth = new Auth();
                //BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    response = (Response)formatter.Deserialize(ms);
                    switch (response.Status)
                    {
                        case ResponseStatus.OK:

                            Auth authResponse = new Auth();

                            authResponse = (Auth)response.Body;

                            string str = authResponse.msg;

                            chatMsg.Add(str);
                            

                            //MessageBox.Show(authResponse.msg);
                            //isConnected = true;
                            //chatMsg = authResponse.msg;
                           //acceptEvent.WaitOne();
                            break;

                        case ResponseStatus.NOT_FOUND:
                            authResponse = (Auth)response.Body;
                            //MessageBox.Show(authResponse.msg);
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

        public bool ConnectStatus()
        {
            return isConnected;
        }

        //public string Msg()
        //{
        //    return chatMsg;
        //}
    }
}
