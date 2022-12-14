namespace ClientForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.num_Port = new System.Windows.Forms.NumericUpDown();
            this.tb_IP = new System.Windows.Forms.TextBox();
            this.boxMeseage = new System.Windows.Forms.RichTextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.pictureBoxAvatar = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_btn_SigiUp = new System.Windows.Forms.Button();
            this.btn_SigiIn = new System.Windows.Forms.Button();
            this.tb_pass = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_login = new System.Windows.Forms.TextBox();
            this.btnTestServer = new System.Windows.Forms.Button();
            this.btn_loadAvatar = new System.Windows.Forms.Button();
            this.chatBox = new System.Windows.Forms.RichTextBox();
            this.lb_userList = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Port)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.num_Port);
            this.groupBox1.Controls.Add(this.tb_IP);
            this.groupBox1.Location = new System.Drawing.Point(12, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(204, 89);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Сетевые настройки";
            // 
            // num_Port
            // 
            this.num_Port.Location = new System.Drawing.Point(127, 23);
            this.num_Port.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.num_Port.Minimum = new decimal(new int[] {
            1500,
            0,
            0,
            0});
            this.num_Port.Name = "num_Port";
            this.num_Port.Size = new System.Drawing.Size(67, 23);
            this.num_Port.TabIndex = 1;
            this.num_Port.Value = new decimal(new int[] {
            4041,
            0,
            0,
            0});
            // 
            // tb_IP
            // 
            this.tb_IP.Location = new System.Drawing.Point(10, 23);
            this.tb_IP.Name = "tb_IP";
            this.tb_IP.Size = new System.Drawing.Size(112, 23);
            this.tb_IP.TabIndex = 0;
            this.tb_IP.Text = "127.0.0.1";
            // 
            // boxMeseage
            // 
            this.boxMeseage.BackColor = System.Drawing.Color.Yellow;
            this.boxMeseage.Location = new System.Drawing.Point(9, 590);
            this.boxMeseage.Name = "boxMeseage";
            this.boxMeseage.Size = new System.Drawing.Size(509, 96);
            this.boxMeseage.TabIndex = 2;
            this.boxMeseage.Text = "";
            this.boxMeseage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.boxMeseage_KeyUp);
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(524, 590);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(101, 95);
            this.btn_send.TabIndex = 3;
            this.btn_send.Text = "Отправить";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // pictureBoxAvatar
            // 
            this.pictureBoxAvatar.Image = global::ClientForm.Properties.Resources.max_kukurudziak_qbc3Zmxw0G8_unsplash;
            this.pictureBoxAvatar.InitialImage = global::ClientForm.Properties.Resources.max_kukurudziak_qbc3Zmxw0G8_unsplash;
            this.pictureBoxAvatar.Location = new System.Drawing.Point(477, 29);
            this.pictureBoxAvatar.Name = "pictureBoxAvatar";
            this.pictureBoxAvatar.Size = new System.Drawing.Size(135, 135);
            this.pictureBoxAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxAvatar.TabIndex = 4;
            this.pictureBoxAvatar.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_btn_SigiUp);
            this.groupBox2.Controls.Add(this.btn_SigiIn);
            this.groupBox2.Controls.Add(this.tb_pass);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tb_login);
            this.groupBox2.Location = new System.Drawing.Point(222, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(403, 213);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Пользователь";
            // 
            // btn_btn_SigiUp
            // 
            this.btn_btn_SigiUp.Location = new System.Drawing.Point(156, 84);
            this.btn_btn_SigiUp.Name = "btn_btn_SigiUp";
            this.btn_btn_SigiUp.Size = new System.Drawing.Size(87, 38);
            this.btn_btn_SigiUp.TabIndex = 7;
            this.btn_btn_SigiUp.Text = "Регистрация";
            this.btn_btn_SigiUp.UseVisualStyleBackColor = true;
            // 
            // btn_SigiIn
            // 
            this.btn_SigiIn.Location = new System.Drawing.Point(66, 84);
            this.btn_SigiIn.Name = "btn_SigiIn";
            this.btn_SigiIn.Size = new System.Drawing.Size(87, 38);
            this.btn_SigiIn.TabIndex = 6;
            this.btn_SigiIn.Text = "Войти";
            this.btn_SigiIn.UseVisualStyleBackColor = true;
            this.btn_SigiIn.Click += new System.EventHandler(this.btn_SigiIn_Click);
            // 
            // tb_pass
            // 
            this.tb_pass.Location = new System.Drawing.Point(66, 52);
            this.tb_pass.Name = "tb_pass";
            this.tb_pass.Size = new System.Drawing.Size(177, 23);
            this.tb_pass.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Пароль";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Логин";
            // 
            // tb_login
            // 
            this.tb_login.Location = new System.Drawing.Point(66, 19);
            this.tb_login.Name = "tb_login";
            this.tb_login.Size = new System.Drawing.Size(177, 23);
            this.tb_login.TabIndex = 0;
            this.tb_login.TextChanged += new System.EventHandler(this.tb_login_TextChanged);
            // 
            // btnTestServer
            // 
            this.btnTestServer.Location = new System.Drawing.Point(22, 64);
            this.btnTestServer.Name = "btnTestServer";
            this.btnTestServer.Size = new System.Drawing.Size(184, 23);
            this.btnTestServer.TabIndex = 6;
            this.btnTestServer.Text = "Проверить доступность сервера";
            this.btnTestServer.UseVisualStyleBackColor = true;
            this.btnTestServer.Click += new System.EventHandler(this.btnTestServer_Click);
            // 
            // btn_loadAvatar
            // 
            this.btn_loadAvatar.Location = new System.Drawing.Point(477, 187);
            this.btn_loadAvatar.Name = "btn_loadAvatar";
            this.btn_loadAvatar.Size = new System.Drawing.Size(135, 23);
            this.btn_loadAvatar.TabIndex = 7;
            this.btn_loadAvatar.Text = "Загрузить аватар";
            this.btn_loadAvatar.UseVisualStyleBackColor = true;
            this.btn_loadAvatar.Click += new System.EventHandler(this.btn_loadAvatar_Click);
            // 
            // chatBox
            // 
            this.chatBox.BackColor = System.Drawing.Color.DodgerBlue;
            this.chatBox.Location = new System.Drawing.Point(12, 233);
            this.chatBox.Name = "chatBox";
            this.chatBox.ReadOnly = true;
            this.chatBox.Size = new System.Drawing.Size(506, 351);
            this.chatBox.TabIndex = 8;
            this.chatBox.Text = "";
            this.chatBox.TextChanged += new System.EventHandler(this.chatBox_TextChanged);
            // 
            // lb_userList
            // 
            this.lb_userList.FormattingEnabled = true;
            this.lb_userList.ItemHeight = 15;
            this.lb_userList.Location = new System.Drawing.Point(525, 234);
            this.lb_userList.Name = "lb_userList";
            this.lb_userList.Size = new System.Drawing.Size(100, 349);
            this.lb_userList.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 715);
            this.Controls.Add(this.lb_userList);
            this.Controls.Add(this.chatBox);
            this.Controls.Add(this.btn_loadAvatar);
            this.Controls.Add(this.btnTestServer);
            this.Controls.Add(this.pictureBoxAvatar);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.boxMeseage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MyChat";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Port)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private NumericUpDown num_Port;
        private TextBox tb_IP;
        private RichTextBox boxMeseage;
        private Button btn_send;
        private PictureBox pictureBoxAvatar;
        private GroupBox groupBox2;
        private Button btn_btn_SigiUp;
        private Button btn_SigiIn;
        private TextBox tb_pass;
        private Label label2;
        private Label label1;
        private TextBox tb_login;
        private Button btnTestServer;
        private Button btn_loadAvatar;
        private RichTextBox chatBox;
        private ListBox lb_userList;
    }
}