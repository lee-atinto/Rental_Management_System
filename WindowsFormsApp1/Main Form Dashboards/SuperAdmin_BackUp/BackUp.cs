using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Helpers;
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
        private readonly string DataConnection = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
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

            ApplyRoleRestrictions();
            SubscribeToCrashMonitor();

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

            progressRestore.Visible = false;
            lblRestoreStatus.Visible = false;

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
                button.ForeColor = Color.Black;
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

        private void SubscribeToCrashMonitor()
        {
            GlobalCrashMonitor.Instance.OnCriticalDataMissing += ShowCriticalAlert;
        }

        private void ShowCriticalAlert(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ShowCriticalAlert(message)));
                return;
            }

            MessageBox.Show(
                $"System Alert: {message}",
                "Critical Data Missing / Crash Detected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
        private void BackUp_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnBackUp, activeColor);

            CheckCriticalTablesDirectly();

            var recoveryManager = new Helpers.CrashRecoveryManager(
                ConfigurationManager.ConnectionStrings["DB"].ConnectionString,
                @"C:\RentalSystem_Backups",
                new string[] {
            "Account", "Address", "Contract", "LoginLogs",
            "MaintenanceRequest", "PasswordHistory", "Payment",
            "PaymentMethod", "PaymentType", "PersonalInformation",
            "Property", "PropertyOwner", "Rent", "RequestType",
            "Requirements", "Tenant", "Unit"
                }
            );
            _ = recoveryManager.CheckAndRecoverCriticalTablesAsync();
        }


        private void CheckCriticalTablesDirectly()
        {
            string[] criticalTables = {
        "Account", "Address", "Contract", "LoginLogs",
        "MaintenanceRequest", "PasswordHistory", "Payment",
        "PaymentMethod", "PaymentType", "PersonalInformation",
        "Property", "PropertyOwner", "Rent", "RequestType",
        "Requirements", "Tenant", "Unit"
    };

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection))
                {
                    conn.Open();
                    foreach (string table in criticalTables)
                    {
                        string query = $"IF OBJECT_ID('{table}', 'U') IS NULL SELECT 0 ELSE SELECT 1";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            int exists = (int)cmd.ExecuteScalar();
                            if (exists == 0)
                            {
                                MessageBox.Show(
                                    $"Critical table missing: {table}. The system may not function correctly.",
                                    "Missing Data Alert",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error detected!\n" + ex.Message, "System Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error detected!\n" + ex.Message, "System Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogError(Exception ex)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BackupErrors.log");
                File.AppendAllText(logPath, $"{DateTime.Now}: {ex}\n\n");
            }
            catch { }
        }

        private void btnBackupRecovery_Click(object sender, EventArgs e)
        {
            plBackupRecovery.Visible = true;
        }

        private void btnOptimizeData_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Optimization feature is currently disabled.", "Information");
        }

        private void btnBrowseBackup_Click_1(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDlg = new FolderBrowserDialog())
            {
                folderDlg.Description = "Select folder to save backup files";
                if (folderDlg.ShowDialog() == DialogResult.OK)
                    tbBackupPath.Text = folderDlg.SelectedPath;
            }
        }

        private void btnBrowseRestore_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDlg = new OpenFileDialog())
            {
                fileDlg.Title = "Select Backup File";
                fileDlg.Filter = "System Backup (*.zip)|*.zip|Database Backup (*.bak)|*.bak|All Files (*.*)|*.*";
                if (fileDlg.ShowDialog() == DialogResult.OK)
                    tbRestoreFile.Text = fileDlg.FileName;
            }
        }

        private void UpdateButtonStates()
        {
            btnBackups.Enabled = !string.IsNullOrWhiteSpace(tbBackupPath.Text);
            btnRestore.Enabled = !string.IsNullOrWhiteSpace(tbRestoreFile.Text) && File.Exists(tbRestoreFile.Text);
        }

        private async void btnBackups_Click(object sender, EventArgs e)
        {
            string backupDir = string.IsNullOrWhiteSpace(tbBackupPath.Text)
                ? @"C:\RentalSystem_Backups"
                : tbBackupPath.Text.Trim();

            if (!Directory.Exists(backupDir)) Directory.CreateDirectory(backupDir);

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string bakFileName = $"Database_{timestamp}.bak";
            string tempBakPath = Path.Combine(backupDir, bakFileName);
            string finalZipPath = Path.Combine(backupDir, $"RentalSystem_FullBackup_{timestamp}.zip");

            try
            {
                lblRestoreStatus.Visible = true;
                lblRestoreStatus.Text = "Step 1: Backing up Database...";

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString))
                {
                    string query = "BACKUP DATABASE [RentalManagementSystem] TO DISK = @path WITH INIT";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@path", tempBakPath);
                        await con.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                lblRestoreStatus.Text = "Step 2: Preparing Files for ZIP...";
                string tempZipFolder = Path.Combine(Path.GetTempPath(), "CombineBackup_" + timestamp);
                Directory.CreateDirectory(tempZipFolder);

                string appFiles = AppDomain.CurrentDomain.BaseDirectory;
                foreach (string file in Directory.GetFiles(appFiles, "*.*", SearchOption.TopDirectoryOnly))
                {
                    File.Copy(file, Path.Combine(tempZipFolder, Path.GetFileName(file)), true);
                }

                File.Move(tempBakPath, Path.Combine(tempZipFolder, bakFileName));

                lblRestoreStatus.Text = "Step 3: Creating Final Zip File...";
                await Task.Run(() => ZipFile.CreateFromDirectory(tempZipFolder, finalZipPath));

                Directory.Delete(tempZipFolder, true);

                MessageBox.Show($"Full Backup Successful!\nSaved at: {finalZipPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                tbBackupPath.Text = string.Empty;
            }
            catch (Exception ex)
            {
                LogError(ex);
                MessageBox.Show("Backup failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lblRestoreStatus.Visible = false;
            }
        }

        private async void btnRestore_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbRestoreFile.Text) || !File.Exists(tbRestoreFile.Text)) return;

            DialogResult confirm = MessageBox.Show("BABALA: Mapapalitan ang lahat ng data. Ituloy?", "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            string extractPath = @"C:\RentalSystem_RestoreTemp";

            SqlConnectionStringBuilder scb = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            string dbName = scb.InitialCatalog;
            scb.InitialCatalog = "master";
            string masterConn = scb.ConnectionString;

            try
            {
                lblRestoreStatus.Visible = true;
                lblRestoreStatus.Text = "Extracting Backup Package...";

                if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);
                Directory.CreateDirectory(extractPath);

                await Task.Run(() => ZipFile.ExtractToDirectory(tbRestoreFile.Text, extractPath));

                string[] bakFiles = Directory.GetFiles(extractPath, "*.bak", SearchOption.AllDirectories);
                if (bakFiles.Length == 0) throw new Exception("Walang .bak file na nahanap sa loob ng ZIP.");
                string bakFileToRestore = bakFiles[0];

                lblRestoreStatus.Text = "Restoring Database (Closing connections)...";
                using (SqlConnection con = new SqlConnection(masterConn))
                {
                    await con.OpenAsync();

                    string sql = $@"
                        ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        RESTORE DATABASE [{dbName}] FROM DISK = @path WITH REPLACE;
                        ALTER DATABASE [{dbName}] SET MULTI_USER;";

                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@path", bakFileToRestore);
                        cmd.CommandTimeout = 0;
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                MessageBox.Show("Restore successful! Mag-re-restart ang application.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tbRestoreFile.Text = string.Empty;
                Application.Restart();
            }
            catch (Exception ex)
            {
                LogError(ex);
                MessageBox.Show("Restore failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lblRestoreStatus.Visible = false;
                try { if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true); } catch { }
            }
        }

        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard dashboard = new DashBoard(UserName, UserRole);
            dashboard.Show();
            this.Hide();
        }

        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants tenants = new Tenants(UserName, UserRole);
            tenants.Show();
            this.Hide();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            ProperTies properties = new ProperTies(UserName, UserRole);
            properties.Show();
            this.Hide();
        }

        private void btnPaymentRec_Click(object sender, EventArgs e)
        {
            Payment_Records paymentRec = new Payment_Records(UserName, UserRole);
            paymentRec.Show();
            this.Hide();
        }

        private void btnContracts_Click(object sender, EventArgs e)
        {
            Contracts contract = new Contracts(UserName, UserRole);
            contract.Show();
            this.Hide();
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            Maintenance maintenance = new Maintenance(UserName, UserRole);
            maintenance.Show();
            this.Hide();
        }

        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts adminAcc = new SuperAdmin_AdminAccounts(UserName, UserRole);
            adminAcc.Show();
            this.Hide();
        }

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
                MessageBox.Show("Access Denied.");
            }
        }

        private void btnlogout_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                SqlCommand cmd = new SqlCommand("UPDATE Account SET active = 0 WHERE username=@u", conn);
                cmd.Parameters.AddWithValue("@u", this.UserName);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            this.Hide();
            new LoginPage().Show();
        }
    }
}