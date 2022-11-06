namespace FtpLoad
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
            this.tb_srvUrl = new System.Windows.Forms.TextBox();
            this.btn_connect = new System.Windows.Forms.Button();
            this.tb_login = new System.Windows.Forms.TextBox();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.btn_sendFile = new System.Windows.Forms.Button();
            this.pictureBoxAvatar = new System.Windows.Forms.PictureBox();
            this.btn_Info = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_srvUrl
            // 
            this.tb_srvUrl.Location = new System.Drawing.Point(41, 15);
            this.tb_srvUrl.Name = "tb_srvUrl";
            this.tb_srvUrl.Size = new System.Drawing.Size(100, 23);
            this.tb_srvUrl.TabIndex = 0;
            this.tb_srvUrl.Text = "127.0.0.1";
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(176, 16);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(129, 23);
            this.btn_connect.TabIndex = 2;
            this.btn_connect.Text = "Connect";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // tb_login
            // 
            this.tb_login.Location = new System.Drawing.Point(41, 51);
            this.tb_login.Name = "tb_login";
            this.tb_login.Size = new System.Drawing.Size(100, 23);
            this.tb_login.TabIndex = 3;
            this.tb_login.Text = "adm";
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(176, 51);
            this.tb_password.Name = "tb_password";
            this.tb_password.Size = new System.Drawing.Size(129, 23);
            this.tb_password.TabIndex = 4;
            this.tb_password.Text = "11111";
            // 
            // btn_sendFile
            // 
            this.btn_sendFile.Location = new System.Drawing.Point(24, 411);
            this.btn_sendFile.Name = "btn_sendFile";
            this.btn_sendFile.Size = new System.Drawing.Size(100, 23);
            this.btn_sendFile.TabIndex = 5;
            this.btn_sendFile.Text = "Send File";
            this.btn_sendFile.UseVisualStyleBackColor = true;
            this.btn_sendFile.Click += new System.EventHandler(this.btn_sendFile_Click);
            // 
            // pictureBoxAvatar
            // 
            this.pictureBoxAvatar.Location = new System.Drawing.Point(24, 95);
            this.pictureBoxAvatar.Name = "pictureBoxAvatar";
            this.pictureBoxAvatar.Size = new System.Drawing.Size(300, 300);
            this.pictureBoxAvatar.TabIndex = 6;
            this.pictureBoxAvatar.TabStop = false;
            // 
            // btn_Info
            // 
            this.btn_Info.Location = new System.Drawing.Point(224, 411);
            this.btn_Info.Name = "btn_Info";
            this.btn_Info.Size = new System.Drawing.Size(100, 23);
            this.btn_Info.TabIndex = 7;
            this.btn_Info.Text = "Avatar Info";
            this.btn_Info.UseVisualStyleBackColor = true;
            this.btn_Info.Click += new System.EventHandler(this.btn_Info_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 444);
            this.Controls.Add(this.btn_Info);
            this.Controls.Add(this.pictureBoxAvatar);
            this.Controls.Add(this.btn_sendFile);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.tb_login);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.tb_srvUrl);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAvatar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox tb_srvUrl;
        private Button btn_connect;
        private TextBox tb_login;
        private TextBox tb_password;
        private Button btn_sendFile;
        private PictureBox pictureBoxAvatar;
        private Button btn_Info;
    }
}