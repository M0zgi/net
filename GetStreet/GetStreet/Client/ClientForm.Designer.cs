namespace GetStreet.Client
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_result = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gb_pass = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.tb_login = new System.Windows.Forms.TextBox();
            this.gb_login = new System.Windows.Forms.GroupBox();
            this.btn_sign_in = new System.Windows.Forms.Button();
            this.btn_sign_up = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gb_pass.SuspendLayout();
            this.gb_login.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.Location = new System.Drawing.Point(12, 231);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(210, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Введите почтовый индекс";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(188, 25);
            this.textBox1.TabIndex = 0;
            // 
            // btn_search
            // 
            this.btn_search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btn_search.Location = new System.Drawing.Point(12, 302);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(210, 35);
            this.btn_search.TabIndex = 1;
            this.btn_search.Text = "Искать";
            this.btn_search.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lb_result);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox2.Location = new System.Drawing.Point(236, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(552, 419);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Результаты поиска";
            // 
            // lb_result
            // 
            this.lb_result.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_result.FormattingEnabled = true;
            this.lb_result.ItemHeight = 17;
            this.lb_result.Location = new System.Drawing.Point(8, 27);
            this.lb_result.Name = "lb_result";
            this.lb_result.Size = new System.Drawing.Size(538, 378);
            this.lb_result.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gb_pass);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox3.Location = new System.Drawing.Point(12, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(210, 149);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Авторизация";
            // 
            // gb_pass
            // 
            this.gb_pass.Controls.Add(this.textBox2);
            this.gb_pass.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.gb_pass.Location = new System.Drawing.Point(5, 84);
            this.gb_pass.Name = "gb_pass";
            this.gb_pass.Size = new System.Drawing.Size(198, 57);
            this.gb_pass.TabIndex = 3;
            this.gb_pass.TabStop = false;
            this.gb_pass.Text = "Пароль";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(4, 20);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(188, 23);
            this.textBox2.TabIndex = 0;
            // 
            // tb_login
            // 
            this.tb_login.Location = new System.Drawing.Point(4, 20);
            this.tb_login.Name = "tb_login";
            this.tb_login.Size = new System.Drawing.Size(188, 23);
            this.tb_login.TabIndex = 0;
            // 
            // gb_login
            // 
            this.gb_login.Controls.Add(this.tb_login);
            this.gb_login.Location = new System.Drawing.Point(18, 28);
            this.gb_login.Name = "gb_login";
            this.gb_login.Size = new System.Drawing.Size(198, 57);
            this.gb_login.TabIndex = 2;
            this.gb_login.TabStop = false;
            this.gb_login.Text = "Логин";
            // 
            // btn_sign_in
            // 
            this.btn_sign_in.Location = new System.Drawing.Point(11, 164);
            this.btn_sign_in.Name = "btn_sign_in";
            this.btn_sign_in.Size = new System.Drawing.Size(94, 35);
            this.btn_sign_in.TabIndex = 4;
            this.btn_sign_in.Text = "Вход";
            this.btn_sign_in.UseVisualStyleBackColor = true;
            // 
            // btn_sign_up
            // 
            this.btn_sign_up.Location = new System.Drawing.Point(126, 164);
            this.btn_sign_up.Name = "btn_sign_up";
            this.btn_sign_up.Size = new System.Drawing.Size(94, 35);
            this.btn_sign_up.TabIndex = 5;
            this.btn_sign_up.Text = "Регистрация";
            this.btn_sign_up.UseVisualStyleBackColor = true;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_sign_up);
            this.Controls.Add(this.btn_sign_in);
            this.Controls.Add(this.gb_login);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.groupBox1);
            this.Name = "ClientForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClientForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.gb_pass.ResumeLayout(false);
            this.gb_pass.PerformLayout();
            this.gb_login.ResumeLayout(false);
            this.gb_login.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private TextBox textBox1;
        private Button btn_search;
        private GroupBox groupBox2;
        private ListBox lb_result;
        private GroupBox groupBox3;
        private GroupBox gb_pass;
        private TextBox textBox2;
        private TextBox tb_login;
        private GroupBox gb_login;
        private Button btn_sign_in;
        private Button btn_sign_up;
    }
}