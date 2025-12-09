namespace WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp
{
    partial class BackUp
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOverView = new System.Windows.Forms.Button();
            this.btnDataBase = new System.Windows.Forms.Button();
            this.plSystemOverView = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSystemUsers = new System.Windows.Forms.Button();
            this.plDataBase = new System.Windows.Forms.Panel();
            this.DataTables = new System.Windows.Forms.DataGridView();
            this.btnOptimize = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.plSystemUsers = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.plBackUpRec = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lbTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.plSystemOverView.SuspendLayout();
            this.plDataBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataTables)).BeginInit();
            this.plSystemUsers.SuspendLayout();
            this.panel3.SuspendLayout();
            this.plBackUpRec.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(28, 28);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(25);
            this.panel1.Size = new System.Drawing.Size(1140, 148);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(227, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "SuperAdmin Access Only";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(928, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "This page contains sensitive system information and administrative controls. All " +
    "actions performed here are logged and \r\nmonitored. Please exercise caution when " +
    "making changes.";
            // 
            // btnOverView
            // 
            this.btnOverView.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOverView.Location = new System.Drawing.Point(28, 28);
            this.btnOverView.Name = "btnOverView";
            this.btnOverView.Size = new System.Drawing.Size(157, 35);
            this.btnOverView.TabIndex = 1;
            this.btnOverView.Text = "System Overview";
            this.btnOverView.UseVisualStyleBackColor = true;
            this.btnOverView.Click += new System.EventHandler(this.btnOverView_Click);
            // 
            // btnDataBase
            // 
            this.btnDataBase.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDataBase.Location = new System.Drawing.Point(221, 28);
            this.btnDataBase.Name = "btnDataBase";
            this.btnDataBase.Size = new System.Drawing.Size(104, 35);
            this.btnDataBase.TabIndex = 2;
            this.btnDataBase.Text = "Database";
            this.btnDataBase.UseVisualStyleBackColor = true;
            this.btnDataBase.Click += new System.EventHandler(this.btnDataBase_Click);
            // 
            // plSystemOverView
            // 
            this.plSystemOverView.Controls.Add(this.label3);
            this.plSystemOverView.Location = new System.Drawing.Point(0, 85);
            this.plSystemOverView.Name = "plSystemOverView";
            this.plSystemOverView.Size = new System.Drawing.Size(1140, 447);
            this.plSystemOverView.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(28, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 26);
            this.label3.TabIndex = 0;
            this.label3.Text = "Recent System Activities";
            // 
            // btnSystemUsers
            // 
            this.btnSystemUsers.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSystemUsers.Location = new System.Drawing.Point(356, 28);
            this.btnSystemUsers.Name = "btnSystemUsers";
            this.btnSystemUsers.Size = new System.Drawing.Size(157, 35);
            this.btnSystemUsers.TabIndex = 4;
            this.btnSystemUsers.Text = "System User\'s";
            this.btnSystemUsers.UseVisualStyleBackColor = true;
            this.btnSystemUsers.Click += new System.EventHandler(this.btnSystemUsers_Click);
            // 
            // plDataBase
            // 
            this.plDataBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plDataBase.Controls.Add(this.DataTables);
            this.plDataBase.Controls.Add(this.btnOptimize);
            this.plDataBase.Controls.Add(this.label4);
            this.plDataBase.Location = new System.Drawing.Point(0, 85);
            this.plDataBase.Name = "plDataBase";
            this.plDataBase.Padding = new System.Windows.Forms.Padding(25);
            this.plDataBase.Size = new System.Drawing.Size(1140, 475);
            this.plDataBase.TabIndex = 4;
            // 
            // DataTables
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.Padding = new System.Windows.Forms.Padding(20);
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataTables.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.DataTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataTables.Location = new System.Drawing.Point(28, 95);
            this.DataTables.Name = "DataTables";
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.DataTables.RowsDefaultCellStyle = dataGridViewCellStyle16;
            this.DataTables.Size = new System.Drawing.Size(1083, 320);
            this.DataTables.TabIndex = 2;
            this.DataTables.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataTables_CellContentClick);
            // 
            // btnOptimize
            // 
            this.btnOptimize.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOptimize.Location = new System.Drawing.Point(905, 28);
            this.btnOptimize.Name = "btnOptimize";
            this.btnOptimize.Size = new System.Drawing.Size(206, 43);
            this.btnOptimize.TabIndex = 1;
            this.btnOptimize.Text = "Optimize Database";
            this.btnOptimize.UseVisualStyleBackColor = true;
            this.btnOptimize.Click += new System.EventHandler(this.btnOptimize_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(28, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 26);
            this.label4.TabIndex = 0;
            this.label4.Text = "Database Tables";
            // 
            // plSystemUsers
            // 
            this.plSystemUsers.Controls.Add(this.button2);
            this.plSystemUsers.Controls.Add(this.label5);
            this.plSystemUsers.Location = new System.Drawing.Point(0, 85);
            this.plSystemUsers.Name = "plSystemUsers";
            this.plSystemUsers.Size = new System.Drawing.Size(1140, 447);
            this.plSystemUsers.TabIndex = 4;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(618, 27);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Add User";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "System Users";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.plSystemOverView);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.btnSystemUsers);
            this.panel3.Controls.Add(this.btnOverView);
            this.panel3.Controls.Add(this.btnDataBase);
            this.panel3.Controls.Add(this.plDataBase);
            this.panel3.Controls.Add(this.plSystemUsers);
            this.panel3.Controls.Add(this.plBackUpRec);
            this.panel3.Location = new System.Drawing.Point(28, 191);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(25);
            this.panel3.Size = new System.Drawing.Size(1140, 560);
            this.panel3.TabIndex = 5;
            // 
            // plBackUpRec
            // 
            this.plBackUpRec.Controls.Add(this.groupBox1);
            this.plBackUpRec.Controls.Add(this.label6);
            this.plBackUpRec.Location = new System.Drawing.Point(0, 85);
            this.plBackUpRec.Name = "plBackUpRec";
            this.plBackUpRec.Size = new System.Drawing.Size(1140, 446);
            this.plBackUpRec.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(29, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1083, 351);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::WindowsFormsApp1.Properties.Resources.sync;
            this.pictureBox1.Location = new System.Drawing.Point(475, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(122, 102);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(437, 143);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(193, 26);
            this.label7.TabIndex = 2;
            this.label7.Text = "Backup and Recovery";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(406, 172);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(248, 36);
            this.button4.TabIndex = 1;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(28, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(193, 26);
            this.label6.TabIndex = 0;
            this.label6.Text = "Backup and Recovery";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(536, 28);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(214, 35);
            this.button3.TabIndex = 5;
            this.button3.Text = "Backup and Recovery";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panelHeader.Controls.Add(this.lbTitle);
            this.panelHeader.Location = new System.Drawing.Point(302, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(2);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1224, 81);
            this.panelHeader.TabIndex = 175;
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Calibri", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.ForeColor = System.Drawing.Color.Black;
            this.lbTitle.Location = new System.Drawing.Point(22, 26);
            this.lbTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(87, 27);
            this.lbTitle.TabIndex = 143;
            this.lbTitle.Text = "BACKUP";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Location = new System.Drawing.Point(319, 82);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(25);
            this.panel2.Size = new System.Drawing.Size(1196, 869);
            this.panel2.TabIndex = 176;
            // 
            // BackUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1526, 845);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelHeader);
            this.Name = "BackUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BackUp";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.BackUp_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.plSystemOverView.ResumeLayout(false);
            this.plSystemOverView.PerformLayout();
            this.plDataBase.ResumeLayout(false);
            this.plDataBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataTables)).EndInit();
            this.plSystemUsers.ResumeLayout(false);
            this.plSystemUsers.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.plBackUpRec.ResumeLayout(false);
            this.plBackUpRec.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOverView;
        private System.Windows.Forms.Button btnDataBase;
        private System.Windows.Forms.Panel plSystemOverView;
        private System.Windows.Forms.Button btnSystemUsers;
        private System.Windows.Forms.Panel plDataBase;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOptimize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel plSystemUsers;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView DataTables;
        private System.Windows.Forms.Panel plBackUpRec;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button4;
    }
}