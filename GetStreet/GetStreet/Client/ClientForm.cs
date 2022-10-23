using GetStreet.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetStreet.Client
{
    public partial class ClientForm : Form
    {
        ClientConnect client;
        public ClientForm()
        {
            InitializeComponent();
        }

        private void btn_testConnect_Click(object sender, EventArgs e)
        {
            client = new ClientConnect(Convert.ToInt32(numericPort.Value), tb_IP.Text);
            client.ConnectAsync();
        }

        private void btn_sign_in_Click(object sender, EventArgs e)
        {
            client = new ClientConnect(Convert.ToInt32(numericPort.Value), tb_IP.Text);
            client.AuthConnect(tb_login.Text, tb_pass.Text);
        }
    }
}
