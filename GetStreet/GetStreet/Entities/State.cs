using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GetStreet.Entities
{
    public class State
    {
        public Socket senderSocket;
        public int data;
        private string message;
        public static int BUFFER_SIZE = 256;
        public byte[] buffer = new byte[BUFFER_SIZE];
        public StringBuilder sb = new StringBuilder();

        //public State()
        //{
        //    message = "Привет сервер!";               
        //    buffer = Encoding.Unicode.GetBytes(message);
        //    //data = 0; // количество полученных байт
        //}
    }
}
