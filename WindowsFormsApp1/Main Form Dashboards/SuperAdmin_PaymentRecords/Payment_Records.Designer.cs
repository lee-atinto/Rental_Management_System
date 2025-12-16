namespace WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords
{
    partial class Payment_Records
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.PaymentTenantData = new System.Windows.Forms.DataGridView();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.cbMethod = new System.Windows.Forms.ComboBox();
            this.btnRecordPayment = new System.Windows.Forms.Button();
            this.lbTotalCollected = new System.Windows.Forms.Label();
            this.lbPendingPayments = new System.Windows.Forms.Label();
            this.lbThisMonth = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.pbCalendar = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.pbPayment = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.pbProfit = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lbTitle = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.SideBarBakground = new System.Windows.Forms.PictureBox();
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
            this.panel9 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentTenantData)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCalendar)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPayment)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfit)).BeginInit();
            this.panel2.SuspendLayout();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SideBarBakground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicUserProfile)).BeginInit();
            this.SuspendLayout();
            // 
            // PaymentTenantData
            // 
            this.PaymentTenantData.AllowUserToAddRows = false;
            this.PaymentTenantData.AllowUserToDeleteRows = false;
            this.PaymentTenantData.AllowUserToResizeColumns = false;
            this.PaymentTenantData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 20, 0, 20);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PaymentTenantData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.PaymentTenantData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PaymentTenantData.Location = new System.Drawing.Point(14, 367);
            this.PaymentTenantData.Name = "PaymentTenantData";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.PaymentTenantData.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.PaymentTenantData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PaymentTenantData.Size = new System.Drawing.Size(1165, 327);
            this.PaymentTenantData.TabIndex = 0;
            this.PaymentTenantData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PaymentTenantData_CellContentClick);
            // 
            // tbSearch
            // 
            this.tbSearch.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSearch.Location = new System.Drawing.Point(112, 27);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(708, 33);
            this.tbSearch.TabIndex = 1;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(843, 27);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(142, 34);
            this.cbStatus.TabIndex = 2;
            this.cbStatus.SelectedIndexChanged += new System.EventHandler(this.cbStatus_SelectedIndexChanged);
            // 
            // cbMethod
            // 
            this.cbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMethod.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMethod.FormattingEnabled = true;
            this.cbMethod.Location = new System.Drawing.Point(1000, 27);
            this.cbMethod.Name = "cbMethod";
            this.cbMethod.Size = new System.Drawing.Size(142, 34);
            this.cbMethod.TabIndex = 3;
            this.cbMethod.SelectedIndexChanged += new System.EventHandler(this.cbMethod_SelectedIndexChanged);
            // 
            // btnRecordPayment
            // 
            this.btnRecordPayment.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecordPayment.Location = new System.Drawing.Point(942, 47);
            this.btnRecordPayment.Name = "btnRecordPayment";
            this.btnRecordPayment.Size = new System.Drawing.Size(227, 50);
            this.btnRecordPayment.TabIndex = 4;
            this.btnRecordPayment.Text = "+ Add Payment";
            this.btnRecordPayment.UseVisualStyleBackColor = true;
            this.btnRecordPayment.Click += new System.EventHandler(this.btnRecordPayment_Click);
            // 
            // lbTotalCollected
            // 
            this.lbTotalCollected.AutoSize = true;
            this.lbTotalCollected.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTotalCollected.Location = new System.Drawing.Point(109, 60);
            this.lbTotalCollected.Name = "lbTotalCollected";
            this.lbTotalCollected.Size = new System.Drawing.Size(65, 26);
            this.lbTotalCollected.TabIndex = 5;
            this.lbTotalCollected.Text = "label1";
            // 
            // lbPendingPayments
            // 
            this.lbPendingPayments.AutoSize = true;
            this.lbPendingPayments.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPendingPayments.Location = new System.Drawing.Point(109, 60);
            this.lbPendingPayments.Name = "lbPendingPayments";
            this.lbPendingPayments.Size = new System.Drawing.Size(65, 26);
            this.lbPendingPayments.TabIndex = 6;
            this.lbPendingPayments.Text = "label2";
            // 
            // lbThisMonth
            // 
            this.lbThisMonth.AutoSize = true;
            this.lbThisMonth.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbThisMonth.Location = new System.Drawing.Point(109, 60);
            this.lbThisMonth.Name = "lbThisMonth";
            this.lbThisMonth.Size = new System.Drawing.Size(65, 26);
            this.lbThisMonth.TabIndex = 7;
            this.lbThisMonth.Text = "label3";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.PaymentTenantData);
            this.panel1.Controls.Add(this.btnRecordPayment);
            this.panel1.Location = new System.Drawing.Point(319, 82);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1196, 715);
            this.panel1.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.lbThisMonth);
            this.panel5.Controls.Add(this.pbCalendar);
            this.panel5.Location = new System.Drawing.Point(851, 118);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(328, 115);
            this.panel5.TabIndex = 253;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.DimGray;
            this.label7.Location = new System.Drawing.Point(107, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 23);
            this.label7.TabIndex = 8;
            this.label7.Text = "This Month";
            // 
            // pbCalendar
            // 
            this.pbCalendar.Location = new System.Drawing.Point(24, 23);
            this.pbCalendar.Name = "pbCalendar";
            this.pbCalendar.Size = new System.Drawing.Size(77, 69);
            this.pbCalendar.TabIndex = 2;
            this.pbCalendar.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.pbPayment);
            this.panel4.Controls.Add(this.lbPendingPayments);
            this.panel4.Location = new System.Drawing.Point(437, 118);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(328, 115);
            this.panel4.TabIndex = 252;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DimGray;
            this.label6.Location = new System.Drawing.Point(107, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(152, 23);
            this.label6.TabIndex = 7;
            this.label6.Text = "Pending Payments";
            // 
            // pbPayment
            // 
            this.pbPayment.Location = new System.Drawing.Point(24, 23);
            this.pbPayment.Name = "pbPayment";
            this.pbPayment.Size = new System.Drawing.Size(77, 69);
            this.pbPayment.TabIndex = 1;
            this.pbPayment.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.pbProfit);
            this.panel3.Controls.Add(this.lbTotalCollected);
            this.panel3.Location = new System.Drawing.Point(14, 118);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(328, 115);
            this.panel3.TabIndex = 251;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(107, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "Total Collected";
            // 
            // pbProfit
            // 
            this.pbProfit.Location = new System.Drawing.Point(24, 23);
            this.pbProfit.Name = "pbProfit";
            this.pbProfit.Size = new System.Drawing.Size(77, 69);
            this.pbProfit.TabIndex = 0;
            this.pbProfit.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(33, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(240, 19);
            this.label3.TabIndex = 250;
            this.label3.Text = "Track and manage tenant payments";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(32, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 26);
            this.label2.TabIndex = 249;
            this.label2.Text = "Payments Management";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1135, 13);
            this.label1.TabIndex = 248;
            this.label1.Text = "_________________________________________________________________________________" +
    "________________________________________________________________________________" +
    "___________________________\r\n";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.tbSearch);
            this.panel2.Controls.Add(this.cbMethod);
            this.panel2.Controls.Add(this.cbStatus);
            this.panel2.Location = new System.Drawing.Point(14, 258);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1165, 93);
            this.panel2.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(30, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 26);
            this.label4.TabIndex = 251;
            this.label4.Text = "Search:";
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.Black;
            this.lbTitle.Location = new System.Drawing.Point(22, 26);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(181, 27);
            this.lbTitle.TabIndex = 143;
            this.lbTitle.Text = "PAYMENT RECORD";
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelHeader.Controls.Add(this.lbTitle);
            this.panelHeader.Location = new System.Drawing.Point(302, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(2);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1244, 81);
            this.panelHeader.TabIndex = 194;
            // 
            // SideBarBakground
            // 
            this.SideBarBakground.BackColor = System.Drawing.Color.White;
            this.SideBarBakground.Location = new System.Drawing.Point(0, 0);
            this.SideBarBakground.Margin = new System.Windows.Forms.Padding(2);
            this.SideBarBakground.Name = "SideBarBakground";
            this.SideBarBakground.Size = new System.Drawing.Size(302, 797);
            this.SideBarBakground.TabIndex = 251;
            this.SideBarBakground.TabStop = false;
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
            this.btnContracts.TabIndex = 262;
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
            this.btnMaintenance.TabIndex = 263;
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
            this.btnAdminAcc.TabIndex = 256;
            this.btnAdminAcc.Text = "Admin Account";
            this.btnAdminAcc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdminAcc.UseVisualStyleBackColor = false;
            this.btnAdminAcc.Click += new System.EventHandler(this.btnAdminAcc_Click);
            // 
            // PicUserProfile
            // 
            this.PicUserProfile.Location = new System.Drawing.Point(41, 29);
            this.PicUserProfile.Margin = new System.Windows.Forms.Padding(2);
            this.PicUserProfile.Name = "PicUserProfile";
            this.PicUserProfile.Size = new System.Drawing.Size(69, 52);
            this.PicUserProfile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicUserProfile.TabIndex = 253;
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
            this.lbName.TabIndex = 254;
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
            this.btnProperties.TabIndex = 261;
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
            this.btnPaymentRec.TabIndex = 260;
            this.btnPaymentRec.Text = "Payment Record";
            this.btnPaymentRec.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPaymentRec.UseVisualStyleBackColor = false;
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
            this.btnBackUp.TabIndex = 259;
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
            this.btnViewReport.TabIndex = 258;
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
            this.btnTenant.TabIndex = 257;
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
            this.btnDashBoard.TabIndex = 255;
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
            this.btnlogout.TabIndex = 252;
            this.btnlogout.Text = "&Log out";
            this.btnlogout.UseVisualStyleBackColor = false;
            this.btnlogout.Click += new System.EventHandler(this.btnlogout_Click);
            // 
            // panel9
            // 
            this.panel9.Location = new System.Drawing.Point(0, 107);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(302, 2);
            this.panel9.TabIndex = 264;
            // 
            // Payment_Records
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
            this.Controls.Add(this.panel1);
            this.Name = "Payment_Records";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rental Management System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Payment_Records_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PaymentTenantData)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCalendar)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPayment)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbProfit)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SideBarBakground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicUserProfile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView PaymentTenantData;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.ComboBox cbMethod;
        private System.Windows.Forms.Button btnRecordPayment;
        private System.Windows.Forms.Label lbTotalCollected;
        private System.Windows.Forms.Label lbPendingPayments;
        private System.Windows.Forms.Label lbThisMonth;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox SideBarBakground;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.PictureBox pbCalendar;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox pbPayment;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pbProfit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
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
        private System.Windows.Forms.Panel panel9;
    }
}