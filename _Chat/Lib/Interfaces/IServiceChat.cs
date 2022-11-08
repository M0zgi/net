using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Interfaces
{
    public interface IServiceChat
    {
        int ClientConnect(string name);

        void ClientDisconnect(int id);

        void SendMesseage(string messeage, int id);

        //void StartAsync();
        //void RunAsync();
        //void AcceptCallBack(IAsyncResult ar);

        //void DisconnectCallBack(IAsyncResult ar);

    }

    public interface IserverChatCallback
    {
        void MsgCallback(string messeage);
    }
}
