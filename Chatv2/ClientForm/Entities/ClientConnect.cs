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

        public ClientConnect(int port, string ip)
        {
            this.port = port;
            this.ip = ip;
        }

        Socket client_socket;

        public void ConnectAsync(string name, string pass, RequestCommands requestform)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);

            client_socket = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, ProtocolType.Tcp);

            request = new Request();
            request.Command = requestform;
            _name = name;
            _password = pass;

            try
            {
                client_socket.BeginConnect(ipPoint, new AsyncCallback(ConnectCallBack), client_socket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                        //ReauestPing();
                        break;
                    case RequestCommands.Auth:
                        AuthConnect();
                        break;
                    //case RequestCommands.Zip:
                       // ReauestZip();
                        break;
                    default:
                        MessageBox.Show(" No Command ");
                        break;
                }

                // закрываем сокет
               // Disconnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //acceptEvent.Set();
            }
        }


        public void AuthConnect()
        {
            Auth auth = new Auth();
            auth.Username = _name;
            auth.Password = _password;

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



        }

    }
}
