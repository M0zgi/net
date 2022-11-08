using ClientForm.Entities;
using Lib.Enum;
using System.Windows.Forms;

namespace ClientForm
{
    public partial class Form1 : Form
    {
        ClientConnect client;

        private bool isConnected = false;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void btnTestServer_Click(object sender, EventArgs e)
        {
            client = new ClientConnect(Convert.ToInt32(num_Port.Value), tb_IP.Text);
            client.ConnectAsync("Привет сервер", RequestCommands.Ping);
        }

        private void btn_SigiIn_Click(object sender, EventArgs e)
        {
            List<string> msg = new List<string>();

            client = new ClientConnect(Convert.ToInt32(num_Port.Value), tb_IP.Text);

            client.ConnectAsync(tb_login.Text, tb_pass.Text, RequestCommands.Auth, ref msg);

            // MessageBox.Show(_msg);

            //isConnected = client.ConnectStatus();

            //while (isConnected)
            // {

            //}

            foreach (var item in msg)
            {
                lbChat.Items.Add(item);
            }
        }
    }
}