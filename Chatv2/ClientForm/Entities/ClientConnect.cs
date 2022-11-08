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
using Lib;
using Lib.Entityes;
using Lib.Enum;

namespace ClientForm.Entities
{
    public class ClientConnect
    {
        public int port { get; set; }
        public string ip { get; set; }

        private string? _name;
        private string? _password;

        private Request request;

        private ManualResetEvent acceptEvent = new ManualResetEvent(false);
        private string? clientMesseage;


        public ClientConnect(int port, string ip)
        {
            this.port = port;
            this.ip = ip;
        }

        Socket client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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

        public void ConnectCallBack(IAsyncResult ar)
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
                    //case RequestCommands.Auth:
                    //    AuthConnect();
                    //    break;
                    ////case RequestCommands.Zip:
                    //   // ReauestZip();
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
                //acceptEvent.Set();
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

            Response responseping;

            using (MemoryStream ms = new MemoryStream(data))
            {
                try
                {
                    responseping = (Response)formatter.Deserialize(ms);
                    switch (responseping.Status)
                    {
                        case ResponseStatus.OK:
                            TestServer pingResp = (TestServer)responseping.Body;
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

    }
}
