namespace WindowsFormsApp1.Super_Admin_Account
{
    partial class AddNewPayment
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.cbMethod = new System.Windows.Forms.ComboBox();
            this.lbRemarks = new System.Windows.Forms.Label();
            this.tbReference = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbPaymentMethod = new System.Windows.Forms.Label();
            this.lbPrice = new System.Windows.Forms.Label();
            this.lbAmountPaid = new System.Windows.Forms.Label();
            this.lbTenant = new System.Windows.Forms.Label();
            this.tbRemarks = new System.Windows.Forms.TextBox();
            this.tbUnit = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(498, 225);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(233, 27);
            this.dateTimePicker1.TabIndex = 228;
            // 
            // cbMethod
            // 
            this.cbMethod.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMethod.FormattingEnabled = true;
            this.cbMethod.Items.AddRange(new object[] {
            "Cash",
            "Gcash"});
            this.cbMethod.Location = new System.Drawing.Point(247, 225);
            this.cbMethod.Name = "cbMethod";
            this.cbMethod.Size = new System.Drawing.Size(233, 27);
            this.cbMethod.TabIndex = 227;
            this.cbMethod.SelectedIndexChanged += new System.EventHandler(this.cbMethod_SelectedIndexChanged);
            // 
            // lbRemarks
            // 
            this.lbRemarks.AutoSize = true;
            this.lbRemarks.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRemarks.Location = new System.Drawing.Point(448, 170);
            this.lbRemarks.Name = "lbRemarks";
            this.lbRemarks.Size = new System.Drawing.Size(47, 19);
            this.lbRemarks.TabIndex = 226;
            this.lbRemarks.Text = "Note:";
            // 
            // tbReference
            // 
            this.tbReference.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbReference.Location = new System.Drawing.Point(247, 285);
            this.tbReference.Name = "tbReference";
            this.tbReference.Size = new System.Drawing.Size(233, 27);
            this.tbReference.TabIndex = 225;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(447, 379);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(138, 35);
            this.btnCancel.TabIndex = 224;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(219, 379);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(138, 35);
            this.btnSave.TabIndex = 223;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(298, 36);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(182, 27);
            this.lbTitle.TabIndex = 222;
            this.lbTitle.Text = "Add New Payment";
            // 
            // lbPaymentMethod
            // 
            this.lbPaymentMethod.AutoSize = true;
            this.lbPaymentMethod.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPaymentMethod.Location = new System.Drawing.Point(69, 228);
            this.lbPaymentMethod.Name = "lbPaymentMethod";
            this.lbPaymentMethod.Size = new System.Drawing.Size(133, 19);
            this.lbPaymentMethod.TabIndex = 221;
            this.lbPaymentMethod.Text = "Payment Method:";
            // 
            // lbPrice
            // 
            this.lbPrice.AutoSize = true;
            this.lbPrice.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPrice.Location = new System.Drawing.Point(69, 288);
            this.lbPrice.Name = "lbPrice";
            this.lbPrice.Size = new System.Drawing.Size(141, 19);
            this.lbPrice.TabIndex = 220;
            this.lbPrice.Text = "Reference Number:";
            // 
            // lbAmountPaid
            // 
            this.lbAmountPaid.AutoSize = true;
            this.lbAmountPaid.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAmountPaid.Location = new System.Drawing.Point(69, 170);
            this.lbAmountPaid.Name = "lbAmountPaid";
            this.lbAmountPaid.Size = new System.Drawing.Size(103, 19);
            this.lbAmountPaid.TabIndex = 219;
            this.lbAmountPaid.Text = "Amount Paid:";
            // 
            // lbTenant
            // 
            this.lbTenant.AutoSize = true;
            this.lbTenant.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTenant.Location = new System.Drawing.Point(69, 103);
            this.lbTenant.Name = "lbTenant";
            this.lbTenant.Size = new System.Drawing.Size(60, 19);
            this.lbTenant.TabIndex = 218;
            this.lbTenant.Text = "Tenant:";
            // 
            // tbRemarks
            // 
            this.tbRemarks.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbRemarks.Location = new System.Drawing.Point(542, 167);
            this.tbRemarks.Name = "tbRemarks";
            this.tbRemarks.Size = new System.Drawing.Size(164, 27);
            this.tbRemarks.TabIndex = 217;
            // 
            // tbUnit
            // 
            this.tbUnit.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUnit.Location = new System.Drawing.Point(247, 167);
            this.tbUnit.Name = "tbUnit";
            this.tbUnit.Size = new System.Drawing.Size(156, 27);
            this.tbUnit.TabIndex = 216;
            // 
            // tbName
            // 
            this.tbName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbName.Location = new System.Drawing.Point(147, 102);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(559, 27);
            this.tbName.TabIndex = 215;
            // 
            // AddNewPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.cbMethod);
            this.Controls.Add(this.lbRemarks);
            this.Controls.Add(this.tbReference);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.lbPaymentMethod);
            this.Controls.Add(this.lbPrice);
            this.Controls.Add(this.lbAmountPaid);
            this.Controls.Add(this.lbTenant);
            this.Controls.Add(this.tbRemarks);
            this.Controls.Add(this.tbUnit);
            this.Controls.Add(this.tbName);
            this.Name = "AddNewPayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rental Management System";
            this.Load += new System.EventHandler(this.AddNewPayment_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox cbMethod;
        private System.Windows.Forms.Label lbRemarks;
        private System.Windows.Forms.TextBox tbReference;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbPaymentMethod;
        private System.Windows.Forms.Label lbPrice;
        private System.Windows.Forms.Label lbAmountPaid;
        private System.Windows.Forms.Label lbTenant;
        private System.Windows.Forms.TextBox tbRemarks;
        private System.Windows.Forms.TextBox tbUnit;
        private System.Windows.Forms.TextBox tbName;
    }
}