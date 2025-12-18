namespace WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Maintenance
{
    partial class AddRecquest
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
            this.cbTenant = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbTenantNasme = new System.Windows.Forms.Label();
            this.dtpRequestDate = new System.Windows.Forms.DateTimePicker();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.tbUnitNumber = new System.Windows.Forms.TextBox();
            this.lbUnitNumber = new System.Windows.Forms.Label();
            this.lbDescription = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbRequestType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // cbTenant
            // 
            this.cbTenant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTenant.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTenant.FormattingEnabled = true;
            this.cbTenant.Location = new System.Drawing.Point(39, 103);
            this.cbTenant.Name = "cbTenant";
            this.cbTenant.Size = new System.Drawing.Size(270, 27);
            this.cbTenant.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Add Request";
            // 
            // lbTenantNasme
            // 
            this.lbTenantNasme.AutoSize = true;
            this.lbTenantNasme.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTenantNasme.Location = new System.Drawing.Point(36, 81);
            this.lbTenantNasme.Name = "lbTenantNasme";
            this.lbTenantNasme.Size = new System.Drawing.Size(95, 19);
            this.lbTenantNasme.TabIndex = 2;
            this.lbTenantNasme.Text = "Tenant Name";
            // 
            // dtpRequestDate
            // 
            this.dtpRequestDate.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpRequestDate.Location = new System.Drawing.Point(40, 181);
            this.dtpRequestDate.Name = "dtpRequestDate";
            this.dtpRequestDate.Size = new System.Drawing.Size(269, 27);
            this.dtpRequestDate.TabIndex = 4;
            // 
            // cbStatus
            // 
            this.cbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStatus.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Location = new System.Drawing.Point(527, 103);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(200, 27);
            this.cbStatus.TabIndex = 3;
            // 
            // tbUnitNumber
            // 
            this.tbUnitNumber.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUnitNumber.Location = new System.Drawing.Point(346, 103);
            this.tbUnitNumber.Name = "tbUnitNumber";
            this.tbUnitNumber.Size = new System.Drawing.Size(152, 27);
            this.tbUnitNumber.TabIndex = 2;
            // 
            // lbUnitNumber
            // 
            this.lbUnitNumber.AutoSize = true;
            this.lbUnitNumber.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUnitNumber.Location = new System.Drawing.Point(342, 81);
            this.lbUnitNumber.Name = "lbUnitNumber";
            this.lbUnitNumber.Size = new System.Drawing.Size(91, 19);
            this.lbUnitNumber.TabIndex = 8;
            this.lbUnitNumber.Text = "Unit Number";
            // 
            // lbDescription
            // 
            this.lbDescription.AutoSize = true;
            this.lbDescription.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDescription.Location = new System.Drawing.Point(35, 242);
            this.lbDescription.Name = "lbDescription";
            this.lbDescription.Size = new System.Drawing.Size(83, 19);
            this.lbDescription.TabIndex = 9;
            this.lbDescription.Text = "Description";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(36, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 19);
            this.label2.TabIndex = 10;
            this.label2.Text = "Reqeust Date";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.Location = new System.Drawing.Point(523, 81);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(49, 19);
            this.lbStatus.TabIndex = 11;
            this.lbStatus.Text = "Status";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(260, 372);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(107, 32);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(396, 372);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 32);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cbRequestType
            // 
            this.cbRequestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRequestType.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRequestType.FormattingEnabled = true;
            this.cbRequestType.Location = new System.Drawing.Point(346, 181);
            this.cbRequestType.Name = "cbRequestType";
            this.cbRequestType.Size = new System.Drawing.Size(200, 27);
            this.cbRequestType.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(342, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 19);
            this.label3.TabIndex = 15;
            this.label3.Text = "Request Type";
            // 
            // tbDescription
            // 
            this.tbDescription.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDescription.Location = new System.Drawing.Point(39, 264);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(688, 102);
            this.tbDescription.TabIndex = 6;
            this.tbDescription.Text = "";
            // 
            // AddRecquest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 434);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbRequestType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbDescription);
            this.Controls.Add(this.lbUnitNumber);
            this.Controls.Add(this.tbUnitNumber);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.dtpRequestDate);
            this.Controls.Add(this.lbTenantNasme);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbTenant);
            this.Name = "AddRecquest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rental Management System";
            this.Load += new System.EventHandler(this.AddRecquest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTenant;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbTenantNasme;
        private System.Windows.Forms.DateTimePicker dtpRequestDate;
        private System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.TextBox tbUnitNumber;
        private System.Windows.Forms.Label lbUnitNumber;
        private System.Windows.Forms.Label lbDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbRequestType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox tbDescription;
    }
}