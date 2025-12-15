namespace WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount
{
    partial class SuperAdmin_AdminAccounts
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lbTitle = new System.Windows.Forms.Label();
            this.adminData = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btnContracts = new System.Windows.Forms.Button();
            this.btnMaintenance = new System.Windows.Forms.Button();
            this.btnAdminAcc = new System.Windows.Forms.Button();
            this.PicUserProfile = new System.Windows.Forms.PictureBox();
            this.lbName = new System.Windows.Forms.Label();
            this.btnProperties = new System.Windows.Forms.Button();
            this.btnPaymentRec = new System.Windows.Forms.Button();
            this.btnBackUp = new System.Windows.Forms.Button();
            this.btnViewReport = new System.Windows.Forms.Button();
            this.btnTenant = new System.Windows.Forms.Button();
            this.btnDashBoard = new System.Windows.Forms.Button();
            this.btnlogout = new System.Windows.Forms.Button();
            this.SideBarBakground = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbCurrentPassword = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbUpdatePasswordMessage = new System.Windows.Forms.Label();
            this.pbSuccessMessage = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbShowPassword = new System.Windows.Forms.CheckBox();
            this.lbPasswordMessage = new System.Windows.Forms.Label();
            this.tbNewPassword = new System.Windows.Forms.TextBox();
            this.panelStrengthBar = new System.Windows.Forms.Panel();
            this.tbConfirmPassword = new System.Windows.Forms.TextBox();
            this.lbMatchPassword = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adminData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicUserProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SideBarBakground)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSuccessMessage)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelHeader.Controls.Add(this.lbTitle);
            this.panelHeader.Location = new System.Drawing.Point(302, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(2);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1244, 81);
            this.panelHeader.TabIndex = 184;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.Black;
            this.lbTitle.Location = new System.Drawing.Point(22, 26);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(174, 27);
            this.lbTitle.TabIndex = 143;
            this.lbTitle.Text = "ADMIN ACCOUNT";
            // 
            // adminData
            // 
            this.adminData.AllowUserToResizeColumns = false;
            this.adminData.AllowUserToResizeRows = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.adminData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.adminData.ColumnHeadersHeight = 45;
            this.adminData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.adminData.DefaultCellStyle = dataGridViewCellStyle4;
            this.adminData.Location = new System.Drawing.Point(14, 133);
            this.adminData.Margin = new System.Windows.Forms.Padding(2);
            this.adminData.Name = "adminData";
            this.adminData.RowHeadersWidth = 51;
            this.adminData.RowTemplate.Height = 24;
            this.adminData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.adminData.Size = new System.Drawing.Size(1165, 453);
            this.adminData.TabIndex = 185;
            this.adminData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.adminData_CellContentClick);
            this.adminData.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.adminData_RowPrePaint);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(988, 47);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(181, 50);
            this.btnAdd.TabIndex = 220;
            this.btnAdd.Text = "+   Add Users";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1135, 13);
            this.label1.TabIndex = 222;
            this.label1.Text = "_________________________________________________________________________________" +
    "________________________________________________________________________________" +
    "___________________________\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(190, 26);
            this.label2.TabIndex = 223;
            this.label2.Text = "Admin Management";
            // 
            // panel9
            // 
            this.panel9.Location = new System.Drawing.Point(0, 107);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(302, 2);
            this.panel9.TabIndex = 251;
            // 
            // btnContracts
            // 
            this.btnContracts.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnContracts.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContracts.ForeColor = System.Drawing.Color.White;
            this.btnContracts.Location = new System.Drawing.Point(24, 367);
            this.btnContracts.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.btnContracts.Name = "btnContracts";
            this.btnContracts.Size = new System.Drawing.Size(250, 50);
            this.btnContracts.TabIndex = 249;
            this.btnContracts.Text = "Contracts";
            this.btnContracts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnContracts.UseVisualStyleBackColor = false;
            this.btnContracts.Click += new System.EventHandler(this.btnContracts_Click);
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnMaintenance.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaintenance.ForeColor = System.Drawing.Color.White;
            this.btnMaintenance.Location = new System.Drawing.Point(24, 427);
            this.btnMaintenance.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Size = new System.Drawing.Size(250, 50);
            this.btnMaintenance.TabIndex = 250;
            this.btnMaintenance.Text = "Maintenance";
            this.btnMaintenance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMaintenance.UseVisualStyleBackColor = false;
            this.btnMaintenance.Click += new System.EventHandler(this.btnMaintenance_Click);
            // 
            // btnAdminAcc
            // 
            this.btnAdminAcc.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnAdminAcc.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdminAcc.ForeColor = System.Drawing.Color.White;
            this.btnAdminAcc.Location = new System.Drawing.Point(24, 487);
            this.btnAdminAcc.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.btnAdminAcc.Name = "btnAdminAcc";
            this.btnAdminAcc.Size = new System.Drawing.Size(250, 50);
            this.btnAdminAcc.TabIndex = 243;
            this.btnAdminAcc.Text = "Admin Account";
            this.btnAdminAcc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdminAcc.UseVisualStyleBackColor = false;
            // 
            // PicUserProfile
            // 
            this.PicUserProfile.Location = new System.Drawing.Point(41, 29);
            this.PicUserProfile.Margin = new System.Windows.Forms.Padding(2);
            this.PicUserProfile.Name = "PicUserProfile";
            this.PicUserProfile.Size = new System.Drawing.Size(69, 52);
            this.PicUserProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicUserProfile.TabIndex = 240;
            this.PicUserProfile.TabStop = false;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.ForeColor = System.Drawing.Color.White;
            this.lbName.Location = new System.Drawing.Point(114, 29);
            this.lbName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(60, 24);
            this.lbName.TabIndex = 241;
            this.lbName.Text = "label1";
            // 
            // btnProperties
            // 
            this.btnProperties.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnProperties.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProperties.ForeColor = System.Drawing.Color.White;
            this.btnProperties.Location = new System.Drawing.Point(24, 247);
            this.btnProperties.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(250, 50);
            this.btnProperties.TabIndex = 248;
            this.btnProperties.Text = "Properties";
            this.btnProperties.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProperties.UseVisualStyleBackColor = false;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // btnPaymentRec
            // 
            this.btnPaymentRec.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnPaymentRec.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPaymentRec.ForeColor = System.Drawing.Color.White;
            this.btnPaymentRec.Location = new System.Drawing.Point(24, 307);
            this.btnPaymentRec.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.btnPaymentRec.Name = "btnPaymentRec";
            this.btnPaymentRec.Size = new System.Drawing.Size(250, 50);
            this.btnPaymentRec.TabIndex = 247;
            this.btnPaymentRec.Text = "Payment Record";
            this.btnPaymentRec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaymentRec.UseVisualStyleBackColor = false;
            this.btnPaymentRec.Click += new System.EventHandler(this.btnPaymentRec_Click);
            // 
            // btnBackUp
            // 
            this.btnBackUp.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnBackUp.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackUp.ForeColor = System.Drawing.Color.White;
            this.btnBackUp.Location = new System.Drawing.Point(24, 607);
            this.btnBackUp.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.btnBackUp.Name = "btnBackUp";
            this.btnBackUp.Size = new System.Drawing.Size(250, 50);
            this.btnBackUp.TabIndex = 246;
            this.btnBackUp.Text = "Back up";
            this.btnBackUp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBackUp.UseVisualStyleBackColor = false;
            this.btnBackUp.Click += new System.EventHandler(this.btnBackUp_Click);
            // 
            // btnViewReport
            // 
            this.btnViewReport.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnViewReport.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewReport.ForeColor = System.Drawing.Color.White;
            this.btnViewReport.Location = new System.Drawing.Point(24, 547);
            this.btnViewReport.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.btnViewReport.Name = "btnViewReport";
            this.btnViewReport.Size = new System.Drawing.Size(250, 50);
            this.btnViewReport.TabIndex = 245;
            this.btnViewReport.Text = "View Report";
            this.btnViewReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnViewReport.UseVisualStyleBackColor = false;
            this.btnViewReport.Click += new System.EventHandler(this.btnViewReport_Click);
            // 
            // btnTenant
            // 
            this.btnTenant.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnTenant.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTenant.ForeColor = System.Drawing.Color.White;
            this.btnTenant.Location = new System.Drawing.Point(24, 187);
            this.btnTenant.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.btnTenant.Name = "btnTenant";
            this.btnTenant.Size = new System.Drawing.Size(250, 50);
            this.btnTenant.TabIndex = 244;
            this.btnTenant.Text = "Tenants";
            this.btnTenant.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTenant.UseVisualStyleBackColor = false;
            this.btnTenant.Click += new System.EventHandler(this.btnTenant_Click);
            // 
            // btnDashBoard
            // 
            this.btnDashBoard.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.btnDashBoard.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashBoard.ForeColor = System.Drawing.Color.White;
            this.btnDashBoard.Location = new System.Drawing.Point(24, 127);
            this.btnDashBoard.Margin = new System.Windows.Forms.Padding(15, 15, 15, 5);
            this.btnDashBoard.Name = "btnDashBoard";
            this.btnDashBoard.Size = new System.Drawing.Size(250, 50);
            this.btnDashBoard.TabIndex = 242;
            this.btnDashBoard.Text = "Dashboard";
            this.btnDashBoard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDashBoard.UseVisualStyleBackColor = false;
            this.btnDashBoard.Click += new System.EventHandler(this.btnDashBoard_Click);
            // 
            // btnlogout
            // 
            this.btnlogout.BackColor = System.Drawing.Color.White;
            this.btnlogout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnlogout.Location = new System.Drawing.Point(24, 726);
            this.btnlogout.Margin = new System.Windows.Forms.Padding(2);
            this.btnlogout.Name = "btnlogout";
            this.btnlogout.Size = new System.Drawing.Size(250, 50);
            this.btnlogout.TabIndex = 239;
            this.btnlogout.Text = "&Log out";
            this.btnlogout.UseVisualStyleBackColor = false;
            this.btnlogout.Click += new System.EventHandler(this.btnlogout_Click);
            // 
            // SideBarBakground
            // 
            this.SideBarBakground.BackColor = System.Drawing.Color.White;
            this.SideBarBakground.Location = new System.Drawing.Point(0, 0);
            this.SideBarBakground.Margin = new System.Windows.Forms.Padding(2);
            this.SideBarBakground.Name = "SideBarBakground";
            this.SideBarBakground.Size = new System.Drawing.Size(302, 797);
            this.SideBarBakground.TabIndex = 238;
            this.SideBarBakground.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.adminData);
            this.panel1.Location = new System.Drawing.Point(332, 82);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1196, 715);
            this.panel1.TabIndex = 252;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(33, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(298, 19);
            this.label3.TabIndex = 224;
            this.label3.Text = "Manage all admin accounts and permissions";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(72, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(206, 26);
            this.label4.TabIndex = 0;
            this.label4.Text = "Change Your Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft JhengHei UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(81, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(179, 24);
            this.label5.TabIndex = 1;
            this.label5.Text = "Current Password:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft JhengHei UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(81, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 24);
            this.label6.TabIndex = 2;
            this.label6.Text = "New Password:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft JhengHei UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(81, 183);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(178, 24);
            this.label7.TabIndex = 3;
            this.label7.Text = "Confirm Password";
            // 
            // tbCurrentPassword
            // 
            this.tbCurrentPassword.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCurrentPassword.Location = new System.Drawing.Point(336, 39);
            this.tbCurrentPassword.Name = "tbCurrentPassword";
            this.tbCurrentPassword.Size = new System.Drawing.Size(566, 31);
            this.tbCurrentPassword.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbUpdatePasswordMessage);
            this.panel2.Controls.Add(this.pbSuccessMessage);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.btnUpdate);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(332, 82);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1196, 715);
            this.panel2.TabIndex = 225;
            // 
            // lbUpdatePasswordMessage
            // 
            this.lbUpdatePasswordMessage.AutoSize = true;
            this.lbUpdatePasswordMessage.Font = new System.Drawing.Font("Calibri", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUpdatePasswordMessage.Location = new System.Drawing.Point(477, 510);
            this.lbUpdatePasswordMessage.Name = "lbUpdatePasswordMessage";
            this.lbUpdatePasswordMessage.Size = new System.Drawing.Size(54, 26);
            this.lbUpdatePasswordMessage.TabIndex = 15;
            this.lbUpdatePasswordMessage.Text = "label";
            // 
            // pbSuccessMessage
            // 
            this.pbSuccessMessage.Location = new System.Drawing.Point(103, 484);
            this.pbSuccessMessage.Name = "pbSuccessMessage";
            this.pbSuccessMessage.Size = new System.Drawing.Size(964, 74);
            this.pbSuccessMessage.TabIndex = 14;
            this.pbSuccessMessage.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.DimGray;
            this.label9.Location = new System.Drawing.Point(319, 623);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(573, 26);
            this.label9.TabIndex = 11;
            this.label9.Text = "Reminder: Please update Password Monthly for enhance secturity";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(437, 405);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(566, 42);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "UPDATE";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbMatchPassword);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbShowPassword);
            this.groupBox1.Controls.Add(this.tbCurrentPassword);
            this.groupBox1.Controls.Add(this.lbPasswordMessage);
            this.groupBox1.Controls.Add(this.tbNewPassword);
            this.groupBox1.Controls.Add(this.panelStrengthBar);
            this.groupBox1.Controls.Add(this.tbConfirmPassword);
            this.groupBox1.Location = new System.Drawing.Point(103, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(964, 299);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // cbShowPassword
            // 
            this.cbShowPassword.AutoSize = true;
            this.cbShowPassword.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowPassword.Location = new System.Drawing.Point(736, 242);
            this.cbShowPassword.Name = "cbShowPassword";
            this.cbShowPassword.Size = new System.Drawing.Size(164, 30);
            this.cbShowPassword.TabIndex = 9;
            this.cbShowPassword.Text = "Show Password";
            this.cbShowPassword.UseVisualStyleBackColor = true;
            this.cbShowPassword.CheckedChanged += new System.EventHandler(this.cbShowPassword_CheckedChanged);
            // 
            // lbPasswordMessage
            // 
            this.lbPasswordMessage.AutoSize = true;
            this.lbPasswordMessage.Location = new System.Drawing.Point(333, 157);
            this.lbPasswordMessage.Name = "lbPasswordMessage";
            this.lbPasswordMessage.Size = new System.Drawing.Size(35, 13);
            this.lbPasswordMessage.TabIndex = 8;
            this.lbPasswordMessage.Text = "label8";
            // 
            // tbNewPassword
            // 
            this.tbNewPassword.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNewPassword.Location = new System.Drawing.Point(336, 112);
            this.tbNewPassword.Name = "tbNewPassword";
            this.tbNewPassword.Size = new System.Drawing.Size(566, 31);
            this.tbNewPassword.TabIndex = 5;
            this.tbNewPassword.TextChanged += new System.EventHandler(this.tbNewPassword_TextChanged);
            // 
            // panelStrengthBar
            // 
            this.panelStrengthBar.Location = new System.Drawing.Point(336, 149);
            this.panelStrengthBar.Name = "panelStrengthBar";
            this.panelStrengthBar.Size = new System.Drawing.Size(566, 5);
            this.panelStrengthBar.TabIndex = 7;
            // 
            // tbConfirmPassword
            // 
            this.tbConfirmPassword.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConfirmPassword.Location = new System.Drawing.Point(336, 181);
            this.tbConfirmPassword.Name = "tbConfirmPassword";
            this.tbConfirmPassword.Size = new System.Drawing.Size(566, 31);
            this.tbConfirmPassword.TabIndex = 6;
            this.tbConfirmPassword.TextChanged += new System.EventHandler(this.tbConfirmPassword_TextChanged);
            // 
            // lbMatchPassword
            // 
            this.lbMatchPassword.AutoSize = true;
            this.lbMatchPassword.Location = new System.Drawing.Point(333, 215);
            this.lbMatchPassword.Name = "lbMatchPassword";
            this.lbMatchPassword.Size = new System.Drawing.Size(35, 13);
            this.lbMatchPassword.TabIndex = 10;
            this.lbMatchPassword.Text = "label8";
            // 
            // SuperAdmin_AdminAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1540, 799);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.btnContracts);
            this.Controls.Add(this.btnMaintenance);
            this.Controls.Add(this.btnAdminAcc);
            this.Controls.Add(this.PicUserProfile);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.btnProperties);
            this.Controls.Add(this.btnPaymentRec);
            this.Controls.Add(this.btnBackUp);
            this.Controls.Add(this.btnViewReport);
            this.Controls.Add(this.btnTenant);
            this.Controls.Add(this.btnDashBoard);
            this.Controls.Add(this.btnlogout);
            this.Controls.Add(this.SideBarBakground);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SuperAdmin_AdminAccounts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rental Management System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.SuperAdmin_AdminAccounts_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.adminData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicUserProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SideBarBakground)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSuccessMessage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.DataGridView adminData;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button btnContracts;
        private System.Windows.Forms.Button btnMaintenance;
        private System.Windows.Forms.Button btnAdminAcc;
        private System.Windows.Forms.PictureBox PicUserProfile;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button btnPaymentRec;
        private System.Windows.Forms.Button btnBackUp;
        private System.Windows.Forms.Button btnViewReport;
        private System.Windows.Forms.Button btnTenant;
        private System.Windows.Forms.Button btnDashBoard;
        private System.Windows.Forms.Button btnlogout;
        private System.Windows.Forms.PictureBox SideBarBakground;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbCurrentPassword;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.CheckBox cbShowPassword;
        private System.Windows.Forms.Label lbPasswordMessage;
        private System.Windows.Forms.Panel panelStrengthBar;
        private System.Windows.Forms.TextBox tbConfirmPassword;
        private System.Windows.Forms.TextBox tbNewPassword;
        private System.Windows.Forms.Label lbUpdatePasswordMessage;
        private System.Windows.Forms.PictureBox pbSuccessMessage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbMatchPassword;
    }
}