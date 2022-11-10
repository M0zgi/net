using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Lib.Entityes;
using Lib.Interface;

namespace ServerChat.Entities
{
    public static class Server
    {
        public static List<ServerUser> UserList = new List<ServerUser>();

        private static ManualResetEvent acceptEvent = new ManualResetEvent(false);


        public static int UserId = 1;
        public static int CountUsers;

        public const string Host = "127.0.0.1";
        public const int Port = 4041;

       // public static int port { get; set; }
       // public static string ip { get; set; }

       //private static Task task;
//
        public static Socket ServerSocket;

        public static bool Work = true;

        public delegate void UserEvent(string Name);

        public delegate void UserEventSend(string Name, string msg);

        public static event UserEvent UserConnected = (Username) =>
        {
            Console.WriteLine($"Пользователь {Username}: вошел в чат.");
            CountUsers++;
            SendGlobalMessage($"{Username}: подключился к чату.");
            SendUserList();
        };
        public static event UserEvent UserDisconnected = (Username) =>
        {
            Console.WriteLine($"Пользователь {Username}: покинул чат.");
            CountUsers--;
            SendGlobalMessage($"Пользователь {Username}: отключился от чата.");
            SendUserList();
        };

        public static event UserEventSend SendMsg = (Username, Messege) =>
        {
            //Console.WriteLine($"User {Username} connected.");
            //CountUsers++;
            SendMessageAll($"{Username} {Messege}");
            //SendUserList();
        };

        public static void EndUser(ServerUser usr)
        {
            if (!UserList.Contains(usr))
                return;
            UserList.Remove(usr);
            usr.End();
            UserDisconnected(usr.Name);

        }

        public static void NewUser(ServerUser usr)
        {
            if (UserList.Contains(usr))
                return;
            UserList.Add(usr);
            UserConnected(usr.Name);
        }

        public static void UserConnectedSend(ServerUser usr)
        {
            if (usr.Message == "вышел из чата")
            {
                EndUser(usr);
            }

            SendMsg(usr.Name, usr.Message);
        }

        public static void SendGlobalMessage(string content)
        {
            for(int i = 0;i < CountUsers;i++)
            {
                UserList[i].SendMessage(content);
            }
        }

        public static void SendMessageAll(string content)
        {
            for(int i = 0;i < CountUsers;i++)
            {
                UserList[i].SendMessageChat(content);
            }
        }

        public static void SendUserList()
        {
            string userList = "";

            for(int i = 0;i < CountUsers;i++)
            {
                userList += UserList[i].Name + ",";
            }

            SendAllUsers(userList);
        }

        public static void SendAllUsers(string data)
        {
            for (int i = 0; i < CountUsers; i++)
            {
                UserList[i].Send(data);
            }
        }
    }
}
