using System.Net;
using System.Net.Sockets;
using ClientForm.Entities;
using Lib.Enum;
using System.Windows.Forms;
using Lib;
using Lib.Entityes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ClientForm
{
    public partial class Form1 : Form
    {
        ClientConnect client;

        private bool isConnected = false;

        private string _host;
        private int _port;
        Socket client_socket;
        private Request request;
        private Request requestmsg;
        private Request requestexit;
        private string? _name;
        private string? _password;

        private IPEndPoint ipPoint;


        //Auth auth;

        private ManualResetEvent acceptEvent = new ManualResetEvent(false);
        private delegate void ChatEvent(string content);
        private ChatEvent _addMessage;
        private ChatEvent _addUser;
        private bool IsConnected = false;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            _addMessage = new ChatEvent(AddMessage);
            _addUser = new ChatEvent(AddUserList);
            boxMeseage.Enabled = false;
            btn_send.Enabled = false;
            btn_SigiIn.Enabled = false;
        }

        private void btnTestServer_Click(object sender, EventArgs e)
        {
            client = new ClientConnect(Convert.ToInt32(num_Port.Value), tb_IP.Text);
            client.ConnectAsync("Привет сервер", RequestCommands.Ping);

        }

        private void btn_SigiIn_Click(object sender, EventArgs e)
        {
            if (!IsConnected)
            {
                ConnectAsync(RequestCommands.Auth);
                btn_SigiIn.Text = "Выход";
                IsConnected = true;
                tb_login.Enabled = false;
                tb_pass.Enabled = false;
                boxMeseage.Enabled = true;
                btn_send.Enabled = true;
            }

            else
            {
                //ConnectAsync(RequestCommands.Exit);
                IsConnected = false;
                auth.msg = "сервер я отключаюсь";
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
                            Disconnect();
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

                //Disconnect();

                btn_SigiIn.Text = "Войти";
                IsConnected = false;
                tb_login.Enabled = true;
                tb_pass.Enabled = true;

                //btn_SigiIn.Enabled = false;
            }
        }

        private void Disconnect()
        {
            Response response = new Response();

            response.Status = ResponseStatus.AUTH;
            string message = "Connection closed!";
            Auth Resp = new Auth();

            //Ping pingResp = (Ping) response.Body;
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
                    client_socket.Send(r);
                    client_socket.Shutdown(SocketShutdown.Both);
                    client_socket.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), client_socket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            //client_socket.Shutdown(SocketShutdown.Both);
            //client_socket.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), client_socket);
        }

        private void DisconnectCallback(IAsyncResult ar)
        {
            try
            {
                acceptEvent.WaitOne();
                Socket client = (Socket)ar.AsyncState;
                client.EndDisconnect(ar);

                // Signal that the disconnect is complete.
                acceptEvent.Set();
            }
            catch (Exception e)
            {

            }


        }

        private void ConnectAsync(RequestCommands requestform)
        {
            //acceptEvent.Reset();

            ipPoint = new IPEndPoint(IPAddress.Parse(tb_IP.Text), (int)num_Port.Value);

            client_socket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            request = new Request();
            request.Command = requestform;

            try
            {
                client_socket.BeginConnect(ipPoint, new AsyncCallback(ConnectCallBack), client_socket);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //acceptEvent.WaitOne();
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
                        //ReauestPing();
                        break;
                    case RequestCommands.Auth:
                        AuthConnect();
                        break;
                    case RequestCommands.Exit:
                        // AuthExit();
                        break;
                    case RequestCommands.SendMsg:
                        //ReauestZip();
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

        private Auth auth;
        private void AuthConnect()
        {
            auth = new Auth();
            //auth = new Auth();
            auth.Username = tb_login.Text;
            auth.Password = tb_pass.Text;

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

            while (client_socket.Connected)
            {
                int bytes = 0; // количество полученных байтов
                byte[] data = new byte[1024]; // буфер для получаемых данных

                do
                {
                    bytes = client_socket.Receive(data);
                }
                while (client_socket.Available > 0);

                Response response;
                Auth authResponse;
                SendMessage sendMessage;
                using (MemoryStream ms = new MemoryStream(data))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        response = (Response)formatter.Deserialize(ms);
                        switch (response.Status)
                        {
                            case ResponseStatus.OK:
                                authResponse = (Auth)response.Body;
                                AddUserList(authResponse.msg);
                                AddMessage(authResponse.msg);
                                
                                //btn_SigiIn.Enabled = false;
                                break;

                            case ResponseStatus.MSG:
                                sendMessage = (SendMessage)response.Body;
                                AddMessage(sendMessage.Message);
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


        }

        private void AddMessage(string Content)
        {
            if (InvokeRequired)
            {
                Invoke(_addMessage, Content);
                return;
            }
            boxMeseage.Enabled = true;
            chatBox.SelectionStart = chatBox.TextLength;
            chatBox.SelectionLength = Content.Length;
            //chatBox.AppendText(Content + Environment.NewLine);

            char ch = ':';

            string nicname = " ";

            int indexLastOfChar = Content.LastIndexOf(ch);

            if (indexLastOfChar > 1)
            {
                nicname = Content.Remove(indexLastOfChar);
            }

            int indexOfChar = Content.IndexOf(ch);

            if (indexOfChar > 1)
            {
                Content = Content.Substring(indexOfChar);
            }


            string str = DateTime.Now.ToShortTimeString();
            chatBox.AppendText(str + " " + nicname + Content + Environment.NewLine);

            int i = 0;
            while (i <= chatBox.Text.Length - nicname.Length)
            {
                i = chatBox.Text.IndexOf(nicname, i);
                if (i < 0) break;
                chatBox.SelectionStart = i;
                chatBox.SelectionLength = nicname.Length;
                chatBox.SelectionFont = new Font(chatBox.SelectionFont, FontStyle.Bold | chatBox.SelectionFont.Style);
                i += nicname.Length;
            }


            //i = 0;
            //while (i <= chatBox.Text.Length - nicname.Length)
            //{
            //    i = chatBox.Text.IndexOf(nicname, i);
            //    if (i < 0) break;
            //    chatBox.SelectionStart = i;
            //    chatBox.SelectionLength = nicname.Length;
            //    chatBox.SelectionColor = Color.Yellow;
                

            //    i += nicname.Length;
            //}

            if (!IsConnected)
            {
                boxMeseage.Enabled = false;
                btn_send.Enabled = false;
            }
        }

        private void Send(string Buffer)
        {
            if (IsConnected)
            {
                requestmsg = new Request();
                requestmsg.Command = RequestCommands.SendMsg;
                SendMessage msg = new SendMessage();
                msg.Message = Buffer;
                requestmsg.Body = msg;
            }

            try
            {

                BinaryFormatter formatter = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    try
                    {
                        formatter.Serialize(ms, requestmsg);
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
        }
        private void btn_send_Click(object sender, EventArgs e)
        {
            string msgData = boxMeseage.Text;
            if (string.IsNullOrEmpty(msgData))
                return;

            Send($": {msgData}");
            boxMeseage.Text = string.Empty;
        }

        private void boxMeseage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string msgData = boxMeseage.Text;
                msgData = Regex.Replace(msgData, @"[\n]", "");

                if (string.IsNullOrEmpty(msgData))
                    return;

                Send($": {msgData}");
                boxMeseage.Text = string.Empty;
            }
        }

       

        private void chatBox_TextChanged(object sender, EventArgs e)
        {
            chatBox.SelectionStart = chatBox.Text.Length;
            chatBox.ScrollToCaret();
        }

        private void tb_login_TextChanged(object sender, EventArgs e)
        {
            if (tb_login.Text.Length > 3)
            {
                btn_SigiIn.Enabled = true;
            }
        }

        private void AddUserList(string name)
        {
            if (InvokeRequired)
            {
                Invoke(_addUser, name);
                return;
            }
            char ch = ':';

            string nicname = " ";

            int indexLastOfChar = name.LastIndexOf(ch);

            if (indexLastOfChar > 1)
            {
                nicname = name.Remove(indexLastOfChar);
                lb_userList.Items.Add(nicname);
            }
        }
    }

   
}