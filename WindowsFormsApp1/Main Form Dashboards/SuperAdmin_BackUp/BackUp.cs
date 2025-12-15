using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;
using WindowsFormsApp1.Super_Admin_Account;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp
{
    public partial class BackUp : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private string UserName;
        private string UserRole;

        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        public BackUp(string username, string userRole)
        {
            InitializeComponent();

            this.UserName = username;
            this.UserRole = userRole;
            lbName.Text = $"{username} \n{userRole}";

            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnProperties);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);
            InitializeButtonStyle(btnContracts);
            InitializeButtonStyle(btnMaintenance);

            LoadData();
            ApplyRoleRestrictions();

            panelHeader.BackColor = Color.White;
            lbName.BackColor = Color.FromArgb(46, 51, 73);
            PicUserProfile.Image = Properties.Resources.profile;
            PicUserProfile.BackColor = Color.FromArgb(46, 51, 73);
            SideBarBakground.BackColor = Color.FromArgb(46, 51, 73);

            int padding = 30;
            btnDashBoard.Padding = new Padding(padding, 0, 0, 0);
            btnAdminAcc.Padding = new Padding(padding, 0, 0, 0);
            btnTenant.Padding = new Padding(padding, 0, 0, 0);
            btnProperties.Padding = new Padding(padding, 0, 0, 0);
            btnViewReport.Padding = new Padding(padding, 0, 0, 0);
            btnBackUp.Padding = new Padding(padding, 0, 0, 0);
            btnPaymentRec.Padding = new Padding(padding, 0, 0, 0);
            btnContracts.Padding = new Padding(padding, 0, 0, 0);
            btnMaintenance.Padding = new Padding(padding, 0, 0, 0);

            btnDashBoard.ForeColor = Color.Black;
            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
            btnContracts.ForeColor = Color.Black;
            btnMaintenance.ForeColor = Color.Black;

            tbBackupPath.TextChanged += (s, e) => UpdateButtonStates();
            tbRestoreFile.TextChanged += (s, e) => UpdateButtonStates();
        }

        private void ApplyRoleRestrictions()
        {
            if (UserRole == "Admin")
            {
                btnBackUp.Visible = false;
                btnViewReport.Visible = false;
                panelHeader.BackColor = Color.LightBlue;
            }
            else if (UserRole == "SuperAdmin")
            {
                btnAdminAcc.Visible = true;
                btnBackUp.Visible = true;
                btnViewReport.Visible = true;
                panelHeader.BackColor = Color.White;
            }
        }

        private void InitializeButtonStyle(Button button)
        {
            if (button != null)
            {
                button.TabStop = false;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = Color.FromArgb(46, 51, 73);
                button.BackColor = defaultBackColor;
                button.FlatAppearance.MouseDownBackColor = defaultBackColor;
                button.FlatAppearance.MouseOverBackColor = defaultBackColor;
                button.TextAlign = ContentAlignment.MiddleLeft;
                button.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void SetButtonActiveStyle(Button button, Color backColor)
        {
            if (button != null)
            {
                button.BackColor = backColor;
                button.FlatAppearance.MouseDownBackColor = backColor;
                button.FlatAppearance.MouseOverBackColor = backColor;
            }
        }

        private void BackUp_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnBackUp, activeColor);
        }

        private void LoadData()
        {
            DataTable dt = new DataTable();

            string query = @"
SELECT 
    t.name AS [Table Name],
    ISNULL(SUM(p.rows),0) AS [Records],
    CASE 
        WHEN ISNULL(SUM(a.total_pages),0) * 8.0 >= 1048576 THEN CAST(ROUND((SUM(a.total_pages) * 8.0)/1048576,2) AS VARCHAR)+' GB'
        WHEN ISNULL(SUM(a.total_pages),0) * 8.0 >= 1024 THEN CAST(ROUND((SUM(a.total_pages) * 8.0)/1024,2) AS VARCHAR)+' MB'
        ELSE CAST(ROUND(ISNULL(SUM(a.total_pages),0) * 8.0,2) AS VARCHAR)+' KB'
    END AS [Size],
    'N/A' AS [Last Updated]
FROM sys.tables t
LEFT JOIN sys.indexes i ON t.object_id = i.object_id AND i.index_id IN (0,1)
LEFT JOIN sys.partitions p ON i.object_id = p.object_id AND i.index_id = p.index_id
LEFT JOIN sys.allocation_units a ON p.partition_id = a.container_id
WHERE t.is_ms_shipped = 0
GROUP BY t.name
ORDER BY t.name;
";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    da.SelectCommand.CommandTimeout = 300;
                    da.Fill(dt);
                }

                dtDatabase.DataSource = dt;

                dtDatabase.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dtDatabase.ReadOnly = true;
                dtDatabase.AllowUserToAddRows = false;
                dtDatabase.RowHeadersVisible = false;

                dtDatabase.Columns["Table Name"].HeaderText = "Table Name";
                dtDatabase.Columns["Records"].HeaderText = "Records";
                dtDatabase.Columns["Size"].HeaderText = "Size";
                dtDatabase.Columns["Last Updated"].HeaderText = "Last Updated";

                dtDatabase.Columns["Records"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtDatabase.Columns["Size"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dtDatabase.Columns["Last Updated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred while loading database data:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void OptimizeDatabase()
        {
            string optimizeQuery = @"EXEC sp_MSforeachtable @command1='ALTER INDEX ALL ON ? REORGANIZE;'; EXEC sp_updatestats;";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                using (SqlCommand command = new SqlCommand(optimizeQuery, connection))
                {
                    connection.Open();
                    command.CommandTimeout = 300;
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Database optimization complete! Reorganized indexes and updated statistics.", "Optimization Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during database optimization: " + ex.Message, "Optimization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSystemOverview_Click(object sender, EventArgs e)
        {
            plSystemOverView.Visible = true;

            plDataBase.Visible = false;
            plBackupRecovery.Visible = false;
        }

        private void btnDatabse_Click(object sender, EventArgs e)
        {
            LoadData();
            plDataBase.Visible = true;

            plSystemOverView.Visible = false;
            plBackupRecovery.Visible = false;
        }

        private void btnBackupRecovery_Click(object sender, EventArgs e)
        {
            plBackupRecovery.Visible = true;

            plDataBase.Visible = false;
            plSystemOverView.Visible = false;
        }
        private void btnOptimizeData_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Optimization can take a few minutes and temporarily increase CPU usage. Do you want to continue?",
                "Confirm Optimization",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                OptimizeDatabase();
            }
        }

        private void btnBrowseBackup_Click_1(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDlg = new FolderBrowserDialog())
            {
                folderDlg.Description = "Select folder to save database backup";
                if (folderDlg.ShowDialog() == DialogResult.OK)
                {
                    tbBackupPath.Text = folderDlg.SelectedPath;
                }
            }
        }

        private void btnBrowseRestore_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDlg = new OpenFileDialog())
            {
                fileDlg.Title = "Select database backup file to restore";
                fileDlg.Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*";
                if (fileDlg.ShowDialog() == DialogResult.OK)
                {
                    tbRestoreFile.Text = fileDlg.FileName;
                }
            }
        }

        private void UpdateButtonStates()
        {
            if (!string.IsNullOrWhiteSpace(tbBackupPath.Text))
            {
                btnBackups.Enabled = true;
                btnBrowseBackup.Enabled = true;
                btnRestore.Enabled = false;
                btnBrowseRestore.Enabled = false;
            }
            else if (!string.IsNullOrWhiteSpace(tbRestoreFile.Text))
            {
                btnRestore.Enabled = true;
                btnBrowseRestore.Enabled = true;
                btnBackups.Enabled = false;
                btnBrowseBackup.Enabled = false;
            }
            else
            {
                btnBackups.Enabled = true;
                btnBrowseBackup.Enabled = true;
                btnRestore.Enabled = true;
                btnBrowseRestore.Enabled = true;
            }
        }

        private void SetBackupMode(bool enableBackup)
        {
            btnBackups.Enabled = enableBackup;
            btnBrowseBackup.Enabled = enableBackup;
            btnRestore.Enabled = !enableBackup;
            btnBrowseRestore.Enabled = !enableBackup;
        }

        private void btnBackups_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbBackupPath.Text))
            {
                MessageBox.Show("Please select a folder to save the backup.", "Backup Path Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetBackupMode(false);

            try
            {
                string backupFile = System.IO.Path.Combine(tbBackupPath.Text,
                    $"RentalManagementSystem_Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = $"BACKUP DATABASE [RentalManagementSystem] TO DISK = '{backupFile}' WITH INIT, NAME = 'RentalManagementSystem-Full Backup'";
                    cmd.CommandTimeout = 600;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show($"Database backup completed successfully!\nSaved at: {backupFile}", "Backup Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during backup: " + ex.Message, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBackupMode(true);
                UpdateButtonStates();
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbRestoreFile.Text) || !System.IO.File.Exists(tbRestoreFile.Text))
            {
                MessageBox.Show("Please select a valid backup file to restore.", "Restore File Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetBackupMode(false);

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 600;
                    con.Open();

                    cmd.CommandText = @"ALTER DATABASE [RentalManagementSystem] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"RESTORE DATABASE [RentalManagementSystem] FROM DISK = '{tbRestoreFile.Text}' WITH REPLACE";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"ALTER DATABASE [RentalManagementSystem] SET MULTI_USER";
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Database restored successfully!", "Restore Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during restore: " + ex.Message, "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBackupMode(true);
                UpdateButtonStates();
            }
        }


        // -------------------- Button Side Bar -------------------- //

        // --------------- Dashboard Button --------------- //
        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard dashboard = new DashBoard(UserName, UserRole);
            dashboard.Show();
            this.Hide();
        }

        // --------------- Tenant Button --------------- //
        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants tenants = new Tenants(UserName, UserRole);
            tenants.Show();
            this.Hide();
        }

        // --------------- Properties Button --------------- //
        private void btnProperties_Click(object sender, EventArgs e)
        {
            ProperTies properties = new ProperTies(UserName, UserRole);
            properties.Show();
            this.Hide();
        }

        // --------------- Payment Records Button --------------- //
        private void btnPaymentRec_Click(object sender, EventArgs e)
        {
            Payment_Records paymentRec = new Payment_Records(UserName, UserRole);
            paymentRec.Show();
            this.Hide();
        }

        // --------------- Contracts Button --------------- //
        private void btnContracts_Click(object sender, EventArgs e)
        {
            Contracts contract = new Contracts(UserName, UserRole);
            contract.Show();
            this.Hide();
        }

        // --------------- Maintenance Button --------------- //
        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            Maintenance maintenance = new Maintenance(UserName, UserRole);
            maintenance.Show();
            this.Hide();
        }

        // --------------- Admin Accounts Button --------------- //
        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts adminAcc = new SuperAdmin_AdminAccounts(UserName, UserRole);
            adminAcc.Show();
            this.Hide();
        }

        // --------------- View Reports Button --------------- //
        private void btnViewReport_Click(object sender, EventArgs e)
        {
            if (UserRole == "SuperAdmin")
            {
                View_Reports view = new View_Reports(UserName, UserRole);
                view.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Access Denied: Admin cannot view full reports.");
            }
        }

        // --------------- Logout Button --------------- //
        private void btnlogout_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = "UPDATE Account SET active = 0 WHERE username=@u";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", this.UserName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            this.Hide();
            new LoginPage().Show();
        }
    }
}
