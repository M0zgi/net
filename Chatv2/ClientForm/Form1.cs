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
using Microsoft.VisualBasic.ApplicationServices;
using System.Drawing.Imaging;

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

        private static List<string> users;

        string _fullPath = "";
        string _safeFileName = "";


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
            users   = new List<string>();
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

                //lb_userList.Items.Clear();
                ////nicname = name.Remove(indexLastOfChar);
                ////lb_userList.Items.Add(nicname);

                //foreach (var user in users)
                //{
                //    lb_userList.Items.Add(user);
                //}
            }

            else
            {
                //lb_userList.Items.Clear();
                //string username = tb_login.Text;

                //users.Remove(username);

                //foreach (var user in users)
                //{
                //    lb_userList.Items.Add(user);
                //}
                
                //ConnectAsync(RequestCommands.Exit);
                IsConnected = false;
                auth.msg = "сервер я отключаюсь";
                //
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
                lb_userList.Items.Clear();
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
                    Thread.Sleep(500);
                    
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
                                //AddUserList(authResponse.msg);
                                AddMessage(authResponse.msg);
                                
                                //btn_SigiIn.Enabled = false;
                                break;

                            case ResponseStatus.MSG:
                                sendMessage = (SendMessage)response.Body;
                                AddMessage(sendMessage.Message);
                                break;

                            case ResponseStatus.NewUser:
                                sendMessage = (SendMessage)response.Body;
                                AddUserList(sendMessage.Message);
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
            string[] Users = name.Split(',');
            int countUsers = Users.Length;
            lb_userList.Invoke((MethodInvoker)delegate { lb_userList.Items.Clear(); });
            for(int j = 0;j < countUsers;j++)
            {
                lb_userList.Invoke((MethodInvoker)delegate { lb_userList.Items.Add(Users[j]) ; });
            }
        }

        private void btn_loadAvatar_Click(object sender, EventArgs e)
        {
            LoadImage();
            ConvertWebp();

            try
            {
                string fullPath = _fullPath;
                string fileName = _safeFileName + ".webp";

                // Создаем объект FtpWebRequest - он указывает на файл, который будет создан
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + "127.0.0.1" + ":21" + "/uploads/" + fileName);
                request.Credentials = new NetworkCredential("adm", "11111");

                // устанавливаем метод на загрузку файлов

                request.Method = WebRequestMethods.Ftp.UploadFile;

                // создаем поток для загрузки файла
                FileStream fs = new FileStream(fullPath, FileMode.Open);
                byte[] fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, fileContents.Length);
                fs.Close();
                request.ContentLength = fileContents.Length;

                // пишем считанный в массив байтов файл в выходной поток
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                // получаем ответ от сервера в виде объекта FtpWebResponse
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();


                response.Close();

                if (Path.GetExtension("http://127.0.0.1/uploads/" + fileName) == ".webp")
                {
                    using (WebP webp = new WebP())
                        pictureBoxAvatar.Image = webp.Load("http://127.0.0.1/uploads/" + fileName);
                }
                else
                {

                    pictureBoxAvatar.Image = Image.FromFile("http://127.0.0.1/uploads/" + fileName);
                }
                pictureBoxAvatar.SizeMode = PictureBoxSizeMode.Zoom; 

                //pictureBoxAvatar.Load("http://127.0.0.1/uploads/" + fileName);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadImage()
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image files (*.webp, *.png, *.tif, *.tiff, *.jpg)|*.webp;*.png;*.tif;*.tiff;*.jpg";
                    openFileDialog.FileName = "";
                    if (openFileDialog.ShowDialog() == DialogResult.OK) {
                        
                        string pathFileName = openFileDialog.FileName;
                        string s = DateTime.Now.ToString("yyyyMMddhhmmss");

                        //_onlyFilePath =
                        //    openFileDialog.FileName.Remove(
                        //        openFileDialog.FileName.IndexOf(openFileDialog.SafeFileName));

                        _safeFileName = Path.GetFileName(openFileDialog.FileName);


                        resizeImage(600, 600, pathFileName);

                       //_safeFileName = "Avatar_" + s;

                        if (Path.GetExtension(pathFileName) == ".webp")
                        {
                            using (WebP webp = new WebP())
                                pictureBoxAvatar.Image = webp.Load(pathFileName);
                        }
                        else
                        {
                            pictureBoxAvatar.Image = Image.FromFile(Environment.CurrentDirectory + "\\Temp\\" + _safeFileName);
                        }
                            
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nIn WebPExample.buttonLoad_Click", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConvertWebp()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            byte[] rawWebP;
            
            try
            {
                if (this.pictureBoxAvatar.Image == null)
                    MessageBox.Show("Please, load an image first");

                //get the picture box image
                Bitmap bmp = (Bitmap)pictureBoxAvatar.Image;

                //Test simple encode in lossly mode in memory with quality 75
                string lossyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _safeFileName + ".webp");
                _fullPath = lossyFileName;
                using (WebP webp = new WebP())
                    rawWebP = webp.EncodeLossy(bmp, 75);


                File.WriteAllBytes(lossyFileName, rawWebP);
                //MessageBox.Show("Made " + lossyFileName, "Simple lossy");

                ////Test simple encode in lossless mode in memory
                //string simpleLosslessFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SimpleLossless.webp");
                //using (WebP webp = new WebP())
                //    rawWebP = webp.EncodeLossless(bmp);
                //File.WriteAllBytes(simpleLosslessFileName, rawWebP);
                //MessageBox.Show("Made " + simpleLosslessFileName, "Simple lossless");

                ////Test encode in lossly mode in memory with quality 75 and speed 9
                //string advanceLossyFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AdvanceLossy.webp");
                //using (WebP webp = new WebP())
                //    rawWebP = webp.EncodeLossy(bmp, 71, 9, true);
                //File.WriteAllBytes(advanceLossyFileName, rawWebP);
                //MessageBox.Show("Made " + advanceLossyFileName, "Advance lossy");

                ////Test advance encode lossless mode in memory with speed 9
                //string losslessFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AdvanceLossless.webp");
                //using (WebP webp = new WebP())
                //    rawWebP = webp.EncodeLossless(bmp, 9);
                //File.WriteAllBytes(losslessFileName, rawWebP);
                //MessageBox.Show("Made " + losslessFileName, "Advance lossless");

                //Test encode near lossless mode in memory with quality 40 and speed 9
                // quality 100: No-loss (bit-stream same as -lossless).
                // quality 80: Very very high PSNR (around 54dB) and gets an additional 5-10% size reduction over WebP-lossless image.
                // quality 60: Very high PSNR (around 48dB) and gets an additional 20%-25% size reduction over WebP-lossless image.
                // quality 40: High PSNR (around 42dB) and gets an additional 30-35% size reduction over WebP-lossless image.
                // quality 20 (and below): Moderate PSNR (around 36dB) and gets an additional 40-50% size reduction over WebP-lossless image.
                //string nearLosslessFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NearLossless.webp");
                //using (WebP webp = new WebP())
                //    rawWebP = webp.EncodeNearLossless(bmp, 40, 9);
                //File.WriteAllBytes(nearLosslessFileName, rawWebP);
                //MessageBox.Show("Made " + nearLosslessFileName, "Near lossless");

                //MessageBox.Show("End of Test");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nIn WebPExample.buttonSave_Click", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void resizeImage(int newWidth, int newHeight, string stPhotoPath)
        {
            //string _safeFileName = "";
            //string _onlyFilePath = "";
            //string lossyFileName = Environment.CurrentDirectory + "\\Temp\\" + _safeFileName;
            //System.IO.File.Copy(stPhotoPath, lossyFileName, true);
            
            Image imgPhoto = Image.FromFile(stPhotoPath); 

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                                                (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                                                (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                PixelFormat.Format24bppRgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);
            bmPhoto.Save(Environment.CurrentDirectory + "\\Temp\\" + _safeFileName);

            grPhoto.Dispose();
            imgPhoto.Dispose();

            // return bmPhoto;
        }
    }

   
}