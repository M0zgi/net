using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Lib;
using Lib.Entityes;
using Lib.Enum;

namespace ServerChat.Entities
{
    public class ServerUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }

        private Socket _userHandle;

        private Task _userThread;

        private ManualResetEvent acceptEvent = new ManualResetEvent(false);
        public ServerUser(Socket handle)
        {
            _userHandle = handle;
            _userThread = new Task(listner);
            _userThread.Start();
        }
        private void listner()
        {
            try
            {
                while (_userHandle.Connected)
                {
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[1024]; // буфер для получаемых данных

                    do
                    {
                        bytes = _userHandle.Receive(data);
                    }
                    while (_userHandle.Available > 0);

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
                                    Auth auth = (Auth)request.Body;
                                    if (auth.msg != "сервер я отключаюсь" && auth.msg != "Connection closed!")
                                    {
                                        Name = auth.Username;
                                        Server.NewUser(this);
                                    }

                                    else
                                    {
                                        Server.EndUser(this);
                                    }
                                    break;

                                case RequestCommands.SendMsg:
                                    SendMessage msg = (SendMessage)request.Body;
                                    Message = msg.Message;
                                    Server.UserConnectedSend(this);
                                    break;

                                case RequestCommands.Ping:
                                    TestServer ping = (TestServer)request.Body;
                                    Console.WriteLine(ping.msg);
                                    SendOk();
                                    break;

                                default:
                                    Console.WriteLine(" No Command ");
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
            }
            catch { Server.EndUser(this); }
        }

        public void End()
        {
            Response response = new Response();

            response.Status = ResponseStatus.OK;
            string message = "Пока :)!";
            Auth Resp = new Auth();
            Resp.msg = message;
            response.Body = Resp;
            BinaryFormatter formatter = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, response);
                    byte[] r = ms.ToArray();

                    // Отправка сущности на сервер
                    _userHandle.Send(r);
                    _userHandle.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), _userHandle);
               
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void SendMessage(string content)
        {
            Response response = new Response();
            response.Status = ResponseStatus.OK;
            Auth auth = new Auth();
            auth.msg = content;
            response.Body = auth;
            BinaryFormatter formatter = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, response);
                    byte[] r = ms.ToArray();

                    // Отправка сущности на сервер
                    _userHandle.Send(r);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void SendMessageChat(string content)
        {
            Response responseMsg = new Response();
            responseMsg.Status = ResponseStatus.MSG;
            SendMessage msg = new SendMessage();
            msg.Message = content;
            responseMsg.Body = msg;
            BinaryFormatter formatter = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, responseMsg);
                    byte[] r = ms.ToArray();

                    // Отправка сущности на сервер
                    _userHandle.Send(r);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void SendOk()
        {
            Response response = new Response();

            response.Status = ResponseStatus.OK;
            string message = "Привет клиент!";
            TestServer pingResp = new TestServer();
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
                    _userHandle.Send(r);
                    _userHandle.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), _userHandle);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void DisconnectCallBack(IAsyncResult ar)
        {
            Socket handler = ar.AsyncState as Socket;
            handler.EndDisconnect(ar);
            acceptEvent.Set();
        }

        public void Send(string Buffer)
        {
            Response responseMsg = new Response();
            responseMsg.Status = ResponseStatus.NewUser;
            SendMessage msg = new SendMessage();
            msg.Message = Buffer;
            responseMsg.Body = msg;
            BinaryFormatter formatter = new BinaryFormatter();

            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, responseMsg);
                    byte[] r = ms.ToArray();

                    // Отправка сущности на сервер
                    _userHandle.Send(r);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
