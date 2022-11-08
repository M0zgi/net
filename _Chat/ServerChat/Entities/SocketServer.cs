﻿using Lib.Entities;
using Lib.Enum;
using Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Lib.Interfaces;
using System.Xml.Linq;

namespace ServerChat.Entities
{
    public class SocketServer: IServiceChat
    {

        public int port { get; set; }
        public string ip { get; set; }

        private Task task;

        private int max_conn = 10;

        private ManualResetEvent acceptEvent = new ManualResetEvent(false);

        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        List<ServerUser> users = new List<ServerUser>();

        List<string> messages = new List<string>();

        private int nextId = 1;

        public SocketServer(int port, string ip)
        {
            this.port = port;
            this.ip = ip;
        }

        public void StartAsync()
        {
            if (task != null)
                Console.WriteLine("Сервер запущен");

            this.task = new Task(this.RunAsync);
            task.Start();
        }

        public void RunAsync()
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

        public void AcceptCallBack(IAsyncResult ar)
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
                                Auth auth = (Auth)request.Body;
                                Console.WriteLine(auth.Name);
                                socketOk = accept_socket;
                                ClientConnect(auth.Name);
                                //    Console.WriteLine(auth.Password);
                                //    bool check;
                                //    User user = new User();
                                //    user.Email = auth.Email;
                                //    user.Password = auth.Password;
                                //    check = Login(user);
                                //    if (check)
                                //    {
                                //        Console.WriteLine("Ok");
                                //        socketOk = accept_socket;
                                //        LoginOkDeny(check);
                                //    }

                                //    else
                                //    {
                                //        Console.WriteLine("No");
                                //        socketOk = accept_socket;
                                //        LoginOkDeny(check);
                                //    }

                                break;
                            case RequestCommands.Ping:
                                TestServer ping = (TestServer) request.Body;
                                Console.WriteLine(ping.msg);
                                if (accept_socket.Connected)
                                {
                                    Console.WriteLine(DateTime.Now.ToShortTimeString() + " от " + accept_socket.RemoteEndPoint +
                                                      " получена строка: " + ping.msg);

                                    socketOk = accept_socket;
                                    SendOk();
                                }
                                break;
                            //case RequestCommands.Zip:
                            //    ZipCode zipCode = (ZipCode) request.Body;
                            //    Console.WriteLine(zipCode.Zip);
                            //    socketOk = accept_socket;
                            //    SendZip(zipCode.Zip);
                            //    break;
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

        public void DisconnectCallBack(IAsyncResult ar)
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
            TestServer pingResp = new TestServer();
                                    
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

        private bool Login(User user)
        {
            //using var dbContext = new ApplicationDbContext();

            //var login = dbContext.Users
            //    .FirstOrDefault(x => x.Email == user.Email);

            //bool b = user.Equals(login);
            bool b = false;
            return b;
        }

        private void LoginOkDeny(bool check)
        {
            
        }

        public int ClientConnect(string name)
        {
            ServerUser user = new ServerUser()
            {
                UserId = nextId,
                Name = name
            };

            nextId++;

            Response response = new Response();

            response.Status = ResponseStatus.OK;

            string message = user.Name + " Вы успешно авторизовались в чате! " + user.UserId;
            Auth auth = new Auth();
                                    
            //Ping pingResp = (Ping) response.Body;
            auth.msg = message;
            response.Body = auth;
            BinaryFormatter formatter = new BinaryFormatter();

           // users.Add(user);

            using (var ms = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(ms, response);
                    byte[] r = ms.ToArray();

                    // Отправка сущности на сервер
                    socketOk.Send(r);
                    //socketOk.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), socketOk);
                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //SendMesseage(user.Name + " подключился к чату", user.UserId);

            //users.Add(user);

            return user.UserId;
        }

        public void ClientDisconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.UserId == id);

            if (user != null)
            {
                users.Remove(user);
                SendMesseage(user.Name + " вышел из чата", 0);
            }
        }

        public void SendMesseage(string messeage , int id)
        {
            foreach (var iten in users)
            {
                string answer = DateTime.Now.ToShortTimeString();

                var user = users.FirstOrDefault(i => i.UserId == id);

                if (user != null)
                {
                    answer += ": " + user.Name + " ";
                }

                answer += messeage;


            }
        }

        //public int port { get; set; }
        //public string ip { get; set; }

        //private Task task;

        //private int max_conn = 10;

        //private ManualResetEvent acceptEvent = new ManualResetEvent(false);

        //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //List<ServerUser> users = new List<ServerUser>();

        //private int nextId = 1;

        //public SocketServer(int port, string ip)
        //{
        //    this.port = port;
        //    this.ip = ip;
        //}

        //public void StartAsync()
        //{
        //    if (task != null)
        //        Console.WriteLine("Сервер запущен");

        //    this.task = new Task(this.RunAsync);
        //    task.Start();
        //}

        //public void RunAsync()
        //{
        //    IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), this.port);

        //    try
        //    {
        //        socket.Bind(ipEndPoint);
        //        socket.Listen(this.max_conn);
        //        Console.WriteLine("Сервер запущен. Ожидание подключений...");

        //        while (true)
        //        {
        //            acceptEvent.Reset();
        //            socket.BeginAccept(new AsyncCallback(AcceptCallBack), socket);
        //            acceptEvent.WaitOne();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //public void AcceptCallBack(IAsyncResult ar)
        //{
        //    try
        //    {
        //        Socket socket = (Socket)ar.AsyncState;
        //        Socket accept_socket = socket.EndAccept(ar);

        //        acceptEvent.Set();

        //        int bytes = 0; // количество полученных байтов
        //        byte[] data = new byte[1024]; // буфер для получаемых данных

        //        do
        //        {
        //            bytes = accept_socket.Receive(data);
        //        }
        //        while (accept_socket.Available > 0);

        //        BinaryFormatter formatter = new BinaryFormatter();
        //        Request request;
               

        //        using (MemoryStream ms = new MemoryStream(data))
        //        {
        //            try
        //            {
        //                request = (Request)formatter.Deserialize(ms);
        //                switch (request.Command)
        //                {
        //                    case RequestCommands.Auth:
        //                        Auth auth = (Auth)request.Body;
        //                        Console.WriteLine(auth.Name);
        //                        socketOk = accept_socket;
        //                        ClientConnect(auth.Name);
        //                        //    Console.WriteLine(auth.Password);
        //                        //    bool check;
        //                        //    User user = new User();
        //                        //    user.Email = auth.Email;
        //                        //    user.Password = auth.Password;
        //                        //    check = Login(user);
        //                        //    if (check)
        //                        //    {
        //                        //        Console.WriteLine("Ok");
        //                        //        socketOk = accept_socket;
        //                        //        LoginOkDeny(check);
        //                        //    }

        //                        //    else
        //                        //    {
        //                        //        Console.WriteLine("No");
        //                        //        socketOk = accept_socket;
        //                        //        LoginOkDeny(check);
        //                        //    }

        //                        break;
        //                    case RequestCommands.Ping:
        //                        TestServer ping = (TestServer) request.Body;
        //                        Console.WriteLine(ping.msg);
        //                        if (accept_socket.Connected)
        //                        {
        //                            Console.WriteLine(DateTime.Now.ToShortTimeString() + " от " + accept_socket.RemoteEndPoint +
        //                                              " получена строка: " + ping.msg);

        //                            socketOk = accept_socket;
        //                            SendOk();
        //                        }
        //                        break;
        //                    //case RequestCommands.Zip:
        //                    //    ZipCode zipCode = (ZipCode) request.Body;
        //                    //    Console.WriteLine(zipCode.Zip);
        //                    //    socketOk = accept_socket;
        //                    //    SendZip(zipCode.Zip);
        //                    //    break;
        //                    default:
        //                        Console.WriteLine(" No Command ");
        //                        break;
        //                }
        //            } catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //            }
                    
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //public void DisconnectCallBack(IAsyncResult ar)
        //{
        //    Socket handler = ar.AsyncState as Socket;
        //    handler.EndDisconnect(ar);
        //    Console.WriteLine("Connection closed");
        //}

        //private Socket socketOk;
        //private void SendOk()
        //{
        //    Response response = new Response();

        //    response.Status = ResponseStatus.OK;
        //    string message = "Привет клиент!";
        //    TestServer pingResp = new TestServer();
                                    
        //    //Ping pingResp = (Ping) response.Body;
        //    pingResp.msg = message;
        //    response.Body = pingResp;
        //    BinaryFormatter formatter = new BinaryFormatter();
                                    
        //    using (var ms = new MemoryStream())
        //    {
        //        try
        //        {
        //            formatter.Serialize(ms, response);
        //            byte[] r = ms.ToArray();

        //            // Отправка сущности на сервер
        //            socketOk.Send(r);
        //            socketOk.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), socketOk);
        //        } 
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }
        //}

        //private bool Login(User user)
        //{
        //    //using var dbContext = new ApplicationDbContext();

        //    //var login = dbContext.Users
        //    //    .FirstOrDefault(x => x.Email == user.Email);

        //    //bool b = user.Equals(login);
        //    bool b = false;
        //    return b;
        //}

        //private void LoginOkDeny(bool check)
        //{
            
        //}

        //public int ClientConnect(string name)
        //{
        //    ServerUser user = new ServerUser()
        //    {
        //        UserId = nextId,
        //        Name = name
        //    };

        //    nextId++;

        //    Response response = new Response();

        //    response.Status = ResponseStatus.OK;

        //    string message = user.Name + " Вы успешно авторизовались в чате! " + user.UserId;
        //    Auth auth = new Auth();
                                    
        //    //Ping pingResp = (Ping) response.Body;
        //    auth.msg = message;
        //    response.Body = auth;
        //    BinaryFormatter formatter = new BinaryFormatter();

        //   // users.Add(user);

        //    using (var ms = new MemoryStream())
        //    {
        //        try
        //        {
        //            formatter.Serialize(ms, response);
        //            byte[] r = ms.ToArray();

        //            // Отправка сущности на сервер
        //            socketOk.Send(r);
        //            //socketOk.BeginDisconnect(false, new AsyncCallback(DisconnectCallBack), socketOk);
        //        } 
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }

        //    //SendMesseage(user.Name + " подключился к чату", user.UserId);

        //    //users.Add(user);

        //    return user.UserId;
        //}

        //public void ClientDisconnect(int id)
        //{
        //    var user = users.FirstOrDefault(i => i.UserId == id);

        //    if (user != null)
        //    {
        //        users.Remove(user);
        //        SendMesseage(user.Name + " вышел из чата", 0);
        //    }
        //}

        //public void SendMesseage(string messeage , int id)
        //{
        //    foreach (var iten in users)
        //    {
        //        string answer = DateTime.Now.ToShortTimeString();

        //        var user = users.FirstOrDefault(i => i.UserId == id);

        //        if (user != null)
        //        {
        //            answer += ": " + user.Name + " ";
        //        }

        //        answer += messeage;


        //    }
        //}
    }
}