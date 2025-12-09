namespace WindowsFormsApp1.Login_ResetPassword
{
    partial class Reset_Password
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reset_Password));
            this.lbMatchPassword = new System.Windows.Forms.Label();
            this.lblStrengthMessage = new System.Windows.Forms.Label();
            this.panelStrengthBar = new System.Windows.Forms.Panel();
            this.TbConfirmPassword = new System.Windows.Forms.TextBox();
            this.TbEmail = new System.Windows.Forms.TextBox();
            this.BtnLogin = new System.Windows.Forms.Button();
            this.LinkSignIn = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TbNewPassword = new System.Windows.Forms.TextBox();
            this.ShowPassword = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbMatchPassword
            // 
            this.lbMatchPassword.AutoSize = true;
            this.lbMatchPassword.Location = new System.Drawing.Point(517, 404);
            this.lbMatchPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbMatchPassword.Name = "lbMatchPassword";
            this.lbMatchPassword.Size = new System.Drawing.Size(35, 13);
            this.lbMatchPassword.TabIndex = 57;
            this.lbMatchPassword.Text = "label5";
            // 
            // lblStrengthMessage
            // 
            this.lblStrengthMessage.AutoSize = true;
            this.lblStrengthMessage.Location = new System.Drawing.Point(517, 321);
            this.lblStrengthMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStrengthMessage.Name = "lblStrengthMessage";
            this.lblStrengthMessage.Size = new System.Drawing.Size(35, 13);
            this.lblStrengthMessage.TabIndex = 56;
            this.lblStrengthMessage.Text = "label5";
            // 
            // panelStrengthBar
            // 
            this.panelStrengthBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 4.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelStrengthBar.Location = new System.Drawing.Point(514, 315);
            this.panelStrengthBar.Margin = new System.Windows.Forms.Padding(2);
            this.panelStrengthBar.Name = "panelStrengthBar";
            this.panelStrengthBar.Size = new System.Drawing.Size(248, 4);
            this.panelStrengthBar.TabIndex = 55;
            // 
            // TbConfirmPassword
            // 
            this.TbConfirmPassword.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbConfirmPassword.ForeColor = System.Drawing.Color.Black;
            this.TbConfirmPassword.Location = new System.Drawing.Point(514, 373);
            this.TbConfirmPassword.Margin = new System.Windows.Forms.Padding(2);
            this.TbConfirmPassword.Name = "TbConfirmPassword";
            this.TbConfirmPassword.Size = new System.Drawing.Size(249, 30);
            this.TbConfirmPassword.TabIndex = 3;
            this.TbConfirmPassword.UseSystemPasswordChar = true;
            this.TbConfirmPassword.TextChanged += new System.EventHandler(this.TbConfirmPassword_TextChanged);
            // 
            // TbEmail
            // 
            this.TbEmail.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbEmail.ForeColor = System.Drawing.Color.Black;
            this.TbEmail.Location = new System.Drawing.Point(514, 210);
            this.TbEmail.Margin = new System.Windows.Forms.Padding(2);
            this.TbEmail.Name = "TbEmail";
            this.TbEmail.Size = new System.Drawing.Size(249, 30);
            this.TbEmail.TabIndex = 1;
            // 
            // BtnLogin
            // 
            this.BtnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLogin.Location = new System.Drawing.Point(527, 468);
            this.BtnLogin.Margin = new System.Windows.Forms.Padding(2);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(219, 38);
            this.BtnLogin.TabIndex = 45;
            this.BtnLogin.Text = "&Log in";
            this.BtnLogin.UseVisualStyleBackColor = true;
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // LinkSignIn
            // 
            this.LinkSignIn.ActiveLinkColor = System.Drawing.Color.Black;
            this.LinkSignIn.AutoSize = true;
            this.LinkSignIn.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LinkSignIn.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.LinkSignIn.LinkColor = System.Drawing.Color.DarkGray;
            this.LinkSignIn.Location = new System.Drawing.Point(613, 526);
            this.LinkSignIn.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LinkSignIn.Name = "LinkSignIn";
            this.LinkSignIn.Size = new System.Drawing.Size(52, 19);
            this.LinkSignIn.TabIndex = 4;
            this.LinkSignIn.TabStop = true;
            this.LinkSignIn.Text = "Sign in";
            this.LinkSignIn.VisitedLinkColor = System.Drawing.Color.Black;
            this.LinkSignIn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkSignIn_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(108, 336);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 23);
            this.label2.TabIndex = 54;
            this.label2.Text = "Confirm Password";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(604, 48);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(75, 41);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 53;
            this.pictureBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(542, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 37);
            this.label1.TabIndex = 51;
            this.label1.Text = "Reset Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(108, 250);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 23);
            this.label4.TabIndex = 48;
            this.label4.Text = "New Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(108, 173);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 23);
            this.label3.TabIndex = 47;
            this.label3.Text = "Email";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ShowPassword);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(408, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(422, 588);
            this.panel1.TabIndex = 58;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-2, -2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(846, 617);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 59;
            this.pictureBox1.TabStop = false;
            // 
            // TbNewPassword
            // 
            this.TbNewPassword.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TbNewPassword.ForeColor = System.Drawing.Color.Black;
            this.TbNewPassword.Location = new System.Drawing.Point(514, 287);
            this.TbNewPassword.Margin = new System.Windows.Forms.Padding(2);
            this.TbNewPassword.Name = "TbNewPassword";
            this.TbNewPassword.Size = new System.Drawing.Size(249, 30);
            this.TbNewPassword.TabIndex = 2;
            this.TbNewPassword.UseSystemPasswordChar = true;
            this.TbNewPassword.TextChanged += new System.EventHandler(this.TbNewPassword_TextChanged);
            // 
            // ShowPassword
            // 
            this.ShowPassword.AutoSize = true;
            this.ShowPassword.BackColor = System.Drawing.Color.White;
            this.ShowPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowPassword.Location = new System.Drawing.Point(228, 410);
            this.ShowPassword.Margin = new System.Windows.Forms.Padding(2);
            this.ShowPassword.Name = "ShowPassword";
            this.ShowPassword.Size = new System.Drawing.Size(126, 21);
            this.ShowPassword.TabIndex = 55;
            this.ShowPassword.Text = "Show Password";
            this.ShowPassword.UseVisualStyleBackColor = false;
            this.ShowPassword.CheckedChanged += new System.EventHandler(this.ShowPassword_CheckedChanged);
            // 
            // Reset_Password
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(842, 612);
            this.Controls.Add(this.lbMatchPassword);
            this.Controls.Add(this.lblStrengthMessage);
            this.Controls.Add(this.panelStrengthBar);
            this.Controls.Add(this.TbConfirmPassword);
            this.Controls.Add(this.TbEmail);
            this.Controls.Add(this.BtnLogin);
            this.Controls.Add(this.LinkSignIn);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TbNewPassword);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Reset_Password";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Renatal Mangement System";
            this.Load += new System.EventHandler(this.Reset_Password_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbMatchPassword;
        private System.Windows.Forms.Label lblStrengthMessage;
        private System.Windows.Forms.Panel panelStrengthBar;
        private System.Windows.Forms.TextBox TbConfirmPassword;
        private System.Windows.Forms.TextBox TbEmail;
        private System.Windows.Forms.Button BtnLogin;
        private System.Windows.Forms.LinkLabel LinkSignIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox TbNewPassword;
        private System.Windows.Forms.CheckBox ShowPassword;
    }
}