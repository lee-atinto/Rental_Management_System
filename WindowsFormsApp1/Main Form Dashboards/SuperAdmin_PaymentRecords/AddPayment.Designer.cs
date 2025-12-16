namespace WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_PaymentRecords
{
    partial class AddPayment
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
            this.cbTenantName = new System.Windows.Forms.ComboBox();
            this.tbUnitNumber = new System.Windows.Forms.TextBox();
            this.dtpPaymentDate = new System.Windows.Forms.DateTimePicker();
            this.cbPaymentType = new System.Windows.Forms.ComboBox();
            this.cbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.tbAmountPaid = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbTenantName = new System.Windows.Forms.Label();
            this.lbUnitNumber = new System.Windows.Forms.Label();
            this.lbPaymentMethod = new System.Windows.Forms.Label();
            this.lbPaymentType = new System.Windows.Forms.Label();
            this.lbAmountPaid = new System.Windows.Forms.Label();
            this.lbDatePaid = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbReferenceNumber = new System.Windows.Forms.Label();
            this.tbReferenceID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbTenantName
            // 
            this.cbTenantName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTenantName.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTenantName.FormattingEnabled = true;
            this.cbTenantName.Location = new System.Drawing.Point(55, 97);
            this.cbTenantName.Name = "cbTenantName";
            this.cbTenantName.Size = new System.Drawing.Size(286, 31);
            this.cbTenantName.TabIndex = 0;
            this.cbTenantName.SelectedIndexChanged += new System.EventHandler(this.cbTenantName_SelectedIndexChanged_1);
            // 
            // tbUnitNumber
            // 
            this.tbUnitNumber.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUnitNumber.Location = new System.Drawing.Point(360, 97);
            this.tbUnitNumber.Name = "tbUnitNumber";
            this.tbUnitNumber.Size = new System.Drawing.Size(226, 31);
            this.tbUnitNumber.TabIndex = 1;
            // 
            // dtpPaymentDate
            // 
            this.dtpPaymentDate.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpPaymentDate.Location = new System.Drawing.Point(399, 253);
            this.dtpPaymentDate.Name = "dtpPaymentDate";
            this.dtpPaymentDate.Size = new System.Drawing.Size(187, 31);
            this.dtpPaymentDate.TabIndex = 2;
            // 
            // cbPaymentType
            // 
            this.cbPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaymentType.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPaymentType.FormattingEnabled = true;
            this.cbPaymentType.Location = new System.Drawing.Point(332, 179);
            this.cbPaymentType.Name = "cbPaymentType";
            this.cbPaymentType.Size = new System.Drawing.Size(254, 31);
            this.cbPaymentType.TabIndex = 3;
            // 
            // cbPaymentMethod
            // 
            this.cbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaymentMethod.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPaymentMethod.FormattingEnabled = true;
            this.cbPaymentMethod.Location = new System.Drawing.Point(55, 179);
            this.cbPaymentMethod.Name = "cbPaymentMethod";
            this.cbPaymentMethod.Size = new System.Drawing.Size(249, 31);
            this.cbPaymentMethod.TabIndex = 4;
            // 
            // tbAmountPaid
            // 
            this.tbAmountPaid.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAmountPaid.Location = new System.Drawing.Point(55, 253);
            this.tbAmountPaid.Name = "tbAmountPaid";
            this.tbAmountPaid.Size = new System.Drawing.Size(131, 31);
            this.tbAmountPaid.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(50, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 26);
            this.label1.TabIndex = 6;
            this.label1.Text = "Payment";
            // 
            // lbTenantName
            // 
            this.lbTenantName.AutoSize = true;
            this.lbTenantName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTenantName.Location = new System.Drawing.Point(51, 75);
            this.lbTenantName.Name = "lbTenantName";
            this.lbTenantName.Size = new System.Drawing.Size(95, 19);
            this.lbTenantName.TabIndex = 7;
            this.lbTenantName.Text = "Tenant Name";
            // 
            // lbUnitNumber
            // 
            this.lbUnitNumber.AutoSize = true;
            this.lbUnitNumber.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUnitNumber.Location = new System.Drawing.Point(356, 75);
            this.lbUnitNumber.Name = "lbUnitNumber";
            this.lbUnitNumber.Size = new System.Drawing.Size(91, 19);
            this.lbUnitNumber.TabIndex = 8;
            this.lbUnitNumber.Text = "Unit Number";
            // 
            // lbPaymentMethod
            // 
            this.lbPaymentMethod.AutoSize = true;
            this.lbPaymentMethod.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPaymentMethod.Location = new System.Drawing.Point(51, 157);
            this.lbPaymentMethod.Name = "lbPaymentMethod";
            this.lbPaymentMethod.Size = new System.Drawing.Size(171, 19);
            this.lbPaymentMethod.TabIndex = 9;
            this.lbPaymentMethod.Text = "Choose Payment Method";
            // 
            // lbPaymentType
            // 
            this.lbPaymentType.AutoSize = true;
            this.lbPaymentType.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPaymentType.Location = new System.Drawing.Point(328, 157);
            this.lbPaymentType.Name = "lbPaymentType";
            this.lbPaymentType.Size = new System.Drawing.Size(151, 19);
            this.lbPaymentType.TabIndex = 10;
            this.lbPaymentType.Text = "Choose Payment Type";
            // 
            // lbAmountPaid
            // 
            this.lbAmountPaid.AutoSize = true;
            this.lbAmountPaid.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAmountPaid.Location = new System.Drawing.Point(51, 231);
            this.lbAmountPaid.Name = "lbAmountPaid";
            this.lbAmountPaid.Size = new System.Drawing.Size(59, 19);
            this.lbAmountPaid.TabIndex = 11;
            this.lbAmountPaid.Text = "Amount";
            // 
            // lbDatePaid
            // 
            this.lbDatePaid.AutoSize = true;
            this.lbDatePaid.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDatePaid.Location = new System.Drawing.Point(395, 231);
            this.lbDatePaid.Name = "lbDatePaid";
            this.lbDatePaid.Size = new System.Drawing.Size(40, 19);
            this.lbDatePaid.TabIndex = 12;
            this.lbDatePaid.Text = "Date";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(228, 330);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(141, 35);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lbReferenceNumber
            // 
            this.lbReferenceNumber.AutoSize = true;
            this.lbReferenceNumber.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbReferenceNumber.Location = new System.Drawing.Point(211, 231);
            this.lbReferenceNumber.Name = "lbReferenceNumber";
            this.lbReferenceNumber.Size = new System.Drawing.Size(130, 19);
            this.lbReferenceNumber.TabIndex = 15;
            this.lbReferenceNumber.Text = "Reference Number";
            // 
            // tbReferenceID
            // 
            this.tbReferenceID.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbReferenceID.Location = new System.Drawing.Point(215, 253);
            this.tbReferenceID.Name = "tbReferenceID";
            this.tbReferenceID.Size = new System.Drawing.Size(154, 31);
            this.tbReferenceID.TabIndex = 14;
            // 
            // AddPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 389);
            this.Controls.Add(this.lbReferenceNumber);
            this.Controls.Add(this.tbReferenceID);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lbDatePaid);
            this.Controls.Add(this.lbAmountPaid);
            this.Controls.Add(this.lbPaymentType);
            this.Controls.Add(this.lbPaymentMethod);
            this.Controls.Add(this.lbUnitNumber);
            this.Controls.Add(this.lbTenantName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbAmountPaid);
            this.Controls.Add(this.cbPaymentMethod);
            this.Controls.Add(this.cbPaymentType);
            this.Controls.Add(this.dtpPaymentDate);
            this.Controls.Add(this.tbUnitNumber);
            this.Controls.Add(this.cbTenantName);
            this.Name = "AddPayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rental Mangement System";
            this.Load += new System.EventHandler(this.AddPayment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTenantName;
        private System.Windows.Forms.TextBox tbUnitNumber;
        private System.Windows.Forms.DateTimePicker dtpPaymentDate;
        private System.Windows.Forms.ComboBox cbPaymentType;
        private System.Windows.Forms.ComboBox cbPaymentMethod;
        private System.Windows.Forms.TextBox tbAmountPaid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbTenantName;
        private System.Windows.Forms.Label lbUnitNumber;
        private System.Windows.Forms.Label lbPaymentMethod;
        private System.Windows.Forms.Label lbPaymentType;
        private System.Windows.Forms.Label lbAmountPaid;
        private System.Windows.Forms.Label lbDatePaid;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lbReferenceNumber;
        private System.Windows.Forms.TextBox tbReferenceID;
    }
}