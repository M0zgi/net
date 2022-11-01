using GetStreet.Entities;
using Lib.Data;
using Lib.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Entities;
using Lib.Helpers;

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
            client.ConnectAsync("Привет сервер", RequestCommands.Ping);
        }

        private void btn_sign_in_Click(object sender, EventArgs e)
        {
            client = new ClientConnect(Convert.ToInt32(numericPort.Value), tb_IP.Text);
            client.ConnectAsync(tb_login.Text, tb_pass.Text, RequestCommands.Auth);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            List<string> ls = new List<string>();
            client = new ClientConnect(Convert.ToInt32(numericPort.Value), tb_IP.Text);
            client.ConnectAsync(tb_Zip.Text, RequestCommands.Zip, ref ls);
            lb_result.Items.Clear();

            foreach (var zip in ls)
            {
                lb_result.Items.Add(zip);
            }
        }

        private void btn_sign_up_Click(object sender, EventArgs e)
        {
            using var dbContext = new ApplicationDbContext();
            User user = new User();
            user.Email = tb_login.Text;

            byte[] salt = new byte[user.Email.Length];

            var passwordHash = new PasswordHash(tb_pass.Text, new SHA256CryptoServiceProvider(), salt);

            user.Password = passwordHash.password;
            
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

        }
    }
}
