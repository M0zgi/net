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
        private string? _name;
        private string? _password;



        //Auth auth;

        private ManualResetEvent acceptEvent = new ManualResetEvent(false);
        private delegate void ChatEvent(string content);
        private ChatEvent _addMessage;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            _addMessage = new ChatEvent(AddMessage);
        }

        private void btnTestServer_Click(object sender, EventArgs e)
        {
            client = new ClientConnect(Convert.ToInt32(num_Port.Value), tb_IP.Text);
            client.ConnectAsync("Привет сервер", RequestCommands.Ping);
        }

        private void btn_SigiIn_Click(object sender, EventArgs e)
        {
            ConnectAsync(RequestCommands.Auth);
        }

        public void ConnectAsync(RequestCommands requestform)
        {
            //acceptEvent.Reset();

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(tb_IP.Text), (int)num_Port.Value);

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

        public void AuthConnect()
        {
            Auth auth = new Auth();
            auth = new Auth();
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
                                AddMessage(authResponse.msg);
                                btn_SigiIn.Enabled = false;
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
            chatBox.SelectionStart = chatBox.TextLength;
            chatBox.SelectionLength = Content.Length;
            chatBox.AppendText(Content + Environment.NewLine);
        }

        public void Send(string Buffer)
        {
            requestmsg = new Request();

            requestmsg.Command = RequestCommands.SendMsg;
            SendMessage msg = new SendMessage();
            msg.Message = Buffer;

            try
            {
                requestmsg.Body = msg;
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

            Send($"{DateTime.Now.ToShortTimeString()} : {msgData}");
            boxMeseage.Text = string.Empty;
        }

        private void boxMeseage_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                string msgData = boxMeseage.Text;
                msgData= Regex.Replace(msgData, @"[\n]", "");

                if (string.IsNullOrEmpty(msgData))
                    return;

                Send($"{DateTime.Now.ToShortTimeString()} : {msgData}");
                boxMeseage.Text = string.Empty;
            }
        }
    }
}