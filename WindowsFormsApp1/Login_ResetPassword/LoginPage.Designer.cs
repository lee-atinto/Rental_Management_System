namespace WindowsFormsApp1.Login_ResetPassword
{
    partial class LoginPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginPage));
            this.LinkLabel1 = new System.Windows.Forms.LinkLabel();
            this.TbUsername = new System.Windows.Forms.TextBox();
            this.TbPassword = new System.Windows.Forms.TextBox();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.ShowPassword = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbTitle = new System.Windows.Forms.Label();
            this.plLoginForm = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.btnSuperAdmin = new System.Windows.Forms.Button();
            this.pbPasswordIcon = new System.Windows.Forms.PictureBox();
            this.pbUsernameIcon = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.plLoginForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPasswordIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUsernameIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // LinkLabel1
            // 
            this.LinkLabel1.ActiveLinkColor = System.Drawing.Color.Black;
            this.LinkLabel1.AutoSize = true;
            this.LinkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.LinkLabel1.LinkColor = System.Drawing.Color.DarkGray;
            this.LinkLabel1.Location = new System.Drawing.Point(264, 402);
            this.LinkLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LinkLabel1.Name = "LinkLabel1";
            this.LinkLabel1.Size = new System.Drawing.Size(122, 17);
            this.LinkLabel1.TabIndex = 41;
            this.LinkLabel1.TabStop = true;
            this.LinkLabel1.Text = "Forgot Password?";
            this.LinkLabel1.VisitedLinkColor = System.Drawing.Color.Black;
            this.LinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // TbUsername
            // 
            this.TbUsername.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbUsername.ForeColor = System.Drawing.Color.Black;
            this.TbUsername.Location = new System.Drawing.Point(86, 263);
            this.TbUsername.Margin = new System.Windows.Forms.Padding(2);
            this.TbUsername.Name = "TbUsername";
            this.TbUsername.Size = new System.Drawing.Size(323, 30);
            this.TbUsername.TabIndex = 39;
            // 
            // TbPassword
            // 
            this.TbPassword.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbPassword.ForeColor = System.Drawing.Color.Black;
            this.TbPassword.Location = new System.Drawing.Point(86, 352);
            this.TbPassword.Margin = new System.Windows.Forms.Padding(2);
            this.TbPassword.Name = "TbPassword";
            this.TbPassword.Size = new System.Drawing.Size(323, 30);
            this.TbPassword.TabIndex = 40;
            // 
            // BtnLogin
            // 
            this.BtnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLogin.Location = new System.Drawing.Point(50, 444);
            this.BtnLogin.Margin = new System.Windows.Forms.Padding(2);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(359, 38);
            this.BtnLogin.TabIndex = 38;
            this.BtnLogin.Text = "&Log in";
            this.BtnLogin.UseVisualStyleBackColor = true;
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // ShowPassword
            // 
            this.ShowPassword.AutoSize = true;
            this.ShowPassword.BackColor = System.Drawing.Color.White;
            this.ShowPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowPassword.Location = new System.Drawing.Point(72, 398);
            this.ShowPassword.Margin = new System.Windows.Forms.Padding(2);
            this.ShowPassword.Name = "ShowPassword";
            this.ShowPassword.Size = new System.Drawing.Size(126, 21);
            this.ShowPassword.TabIndex = 33;
            this.ShowPassword.Text = "Show Password";
            this.ShowPassword.UseVisualStyleBackColor = false;
            this.ShowPassword.CheckedChanged += new System.EventHandler(this.ShowPassword_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(82, 327);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 23);
            this.label4.TabIndex = 35;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(82, 238);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 34;
            this.label3.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkGray;
            this.label2.Location = new System.Drawing.Point(46, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(233, 19);
            this.label2.TabIndex = 32;
            this.label2.Text = "Sign in to your account to continue\r\n";
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Calibri", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(43, 24);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(199, 37);
            this.lbTitle.TabIndex = 31;
            this.lbTitle.Text = "Welcome Back";
            // 
            // plLoginForm
            // 
            this.plLoginForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plLoginForm.Controls.Add(this.button2);
            this.plLoginForm.Controls.Add(this.button1);
            this.plLoginForm.Controls.Add(this.label1);
            this.plLoginForm.Controls.Add(this.panel2);
            this.plLoginForm.Controls.Add(this.btnAdmin);
            this.plLoginForm.Controls.Add(this.BtnLogin);
            this.plLoginForm.Controls.Add(this.LinkLabel1);
            this.plLoginForm.Controls.Add(this.btnSuperAdmin);
            this.plLoginForm.Controls.Add(this.label3);
            this.plLoginForm.Controls.Add(this.ShowPassword);
            this.plLoginForm.Controls.Add(this.lbTitle);
            this.plLoginForm.Controls.Add(this.pbPasswordIcon);
            this.plLoginForm.Controls.Add(this.pbUsernameIcon);
            this.plLoginForm.Controls.Add(this.label4);
            this.plLoginForm.Controls.Add(this.TbPassword);
            this.plLoginForm.Controls.Add(this.label2);
            this.plLoginForm.Controls.Add(this.TbUsername);
            this.plLoginForm.Location = new System.Drawing.Point(381, 12);
            this.plLoginForm.Name = "plLoginForm";
            this.plLoginForm.Size = new System.Drawing.Size(449, 608);
            this.plLoginForm.TabIndex = 44;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LightBlue;
            this.button2.ForeColor = System.Drawing.Color.DarkBlue;
            this.button2.Location = new System.Drawing.Point(244, 554);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(165, 30);
            this.button2.TabIndex = 45;
            this.button2.Text = "Login as Admin";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Thistle;
            this.button1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.DarkViolet;
            this.button1.Location = new System.Drawing.Point(50, 554);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 30);
            this.button1.TabIndex = 44;
            this.button1.Text = "Login as Super Admin";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(166, 518);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 19);
            this.label1.TabIndex = 43;
            this.label1.Text = "Quick Demo Login:";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(50, 503);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(359, 2);
            this.panel2.TabIndex = 42;
            // 
            // btnAdmin
            // 
            this.btnAdmin.Image = global::WindowsFormsApp1.Properties.Resources.setting;
            this.btnAdmin.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdmin.Location = new System.Drawing.Point(244, 109);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(165, 93);
            this.btnAdmin.TabIndex = 34;
            this.btnAdmin.Text = "Admin";
            this.btnAdmin.UseVisualStyleBackColor = true;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // btnSuperAdmin
            // 
            this.btnSuperAdmin.Image = global::WindowsFormsApp1.Properties.Resources.shield;
            this.btnSuperAdmin.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSuperAdmin.Location = new System.Drawing.Point(50, 108);
            this.btnSuperAdmin.Name = "btnSuperAdmin";
            this.btnSuperAdmin.Size = new System.Drawing.Size(165, 93);
            this.btnSuperAdmin.TabIndex = 33;
            this.btnSuperAdmin.Text = "SuperAdmin";
            this.btnSuperAdmin.UseVisualStyleBackColor = true;
            this.btnSuperAdmin.Click += new System.EventHandler(this.btnSuperAdmin_Click);
            // 
            // pbPasswordIcon
            // 
            this.pbPasswordIcon.Image = ((System.Drawing.Image)(resources.GetObject("pbPasswordIcon.Image")));
            this.pbPasswordIcon.Location = new System.Drawing.Point(50, 354);
            this.pbPasswordIcon.Margin = new System.Windows.Forms.Padding(2);
            this.pbPasswordIcon.Name = "pbPasswordIcon";
            this.pbPasswordIcon.Size = new System.Drawing.Size(40, 29);
            this.pbPasswordIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbPasswordIcon.TabIndex = 37;
            this.pbPasswordIcon.TabStop = false;
            // 
            // pbUsernameIcon
            // 
            this.pbUsernameIcon.Image = ((System.Drawing.Image)(resources.GetObject("pbUsernameIcon.Image")));
            this.pbUsernameIcon.Location = new System.Drawing.Point(50, 264);
            this.pbUsernameIcon.Margin = new System.Windows.Forms.Padding(2);
            this.pbUsernameIcon.Name = "pbUsernameIcon";
            this.pbUsernameIcon.Size = new System.Drawing.Size(40, 29);
            this.pbUsernameIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbUsernameIcon.TabIndex = 36;
            this.pbUsernameIcon.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-4, -6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(846, 641);
            this.pictureBox1.TabIndex = 43;
            this.pictureBox1.TabStop = false;
            // 
            // LoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(842, 632);
            this.Controls.Add(this.plLoginForm);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LoginPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rental Management System";
            this.Load += new System.EventHandler(this.LoginPage_Load);
            this.plLoginForm.ResumeLayout(false);
            this.plLoginForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPasswordIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbUsernameIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel LinkLabel1;
        private System.Windows.Forms.TextBox TbUsername;
        private System.Windows.Forms.TextBox TbPassword;
        private System.Windows.Forms.Button BtnLogin;
        private System.Windows.Forms.CheckBox ShowPassword;
        private System.Windows.Forms.PictureBox pbPasswordIcon;
        private System.Windows.Forms.PictureBox pbUsernameIcon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Panel plLoginForm;
        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.Button btnSuperAdmin;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
    }
}