using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;
using WindowsFormsApp1.Super_Admin_Account;
using BCrypt.Net;
using Rental_Management_System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount
{
    public partial class SuperAdmin_AdminAccounts : Form
    {
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private readonly string UserName;
        private readonly string UserRole;

        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

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

        public SuperAdmin_AdminAccounts(string username, string userRole)
        {
            InitializeComponent();

            if (tbCurrentPassword != null) tbCurrentPassword.UseSystemPasswordChar = true;
            if (tbNewPassword != null) tbNewPassword.UseSystemPasswordChar = true;
            if (tbConfirmPassword != null) tbConfirmPassword.UseSystemPasswordChar = true;

            this.UserName = username;
            this.UserRole = userRole;
            this.lbName.Text = $"{username} \n ({userRole})";

            LoadAdmins();
            ApplyRoleRestrictions();

            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);
            InitializeButtonStyle(btnProperties);
            InitializeButtonStyle(btnContracts);
            InitializeButtonStyle(btnMaintenance);

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
            btnTenant.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
            btnContracts.ForeColor = Color.Black;
            btnMaintenance.ForeColor = Color.Black;

            adminData.BorderStyle = BorderStyle.None;
            adminData.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            adminData.RowHeadersVisible = false;
            adminData.EnableHeadersVisualStyles = false;
            adminData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.Navy;
            headerStyle.ForeColor = Color.White;
            headerStyle.Font = new Font("Arial", 18, FontStyle.Bold);
            adminData.ColumnHeadersDefaultCellStyle = headerStyle;

            Color defaultBG = Color.FromArgb(240, 240, 240);
            adminData.BackgroundColor = defaultBG;
            adminData.DefaultCellStyle.BackColor = Color.White;
            adminData.AlternatingRowsDefaultCellStyle.BackColor = defaultBG;

            adminData.RowTemplate.Height = 60;

            DataGridViewButtonColumn viewBtnCol = new DataGridViewButtonColumn();
            viewBtnCol.Name = "Edit";
            viewBtnCol.Text = "Edit";
            viewBtnCol.UseColumnTextForButtonValue = true;
            adminData.Columns.Add(viewBtnCol);

            DataGridViewButtonColumn editBtnCol = new DataGridViewButtonColumn();
            editBtnCol.Name = "Delete";
            editBtnCol.Text = "Delete";
            editBtnCol.UseColumnTextForButtonValue = true;
            adminData.Columns.Add(editBtnCol);

        }

        private void SuperAdmin_AdminAccounts_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnAdminAcc, activeColor);
        }

        private bool DeleteAdmin(int userID)
        {
            const string deleteAccounts = "DELETE FROM Account WHERE user_id = @UserID";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand adminCommand = new SqlCommand(deleteAccounts, connection, transaction))
                    {
                        adminCommand.Parameters.AddWithValue("@UserID", userID);
                        int rowsAffected = adminCommand.ExecuteNonQuery();

                        transaction.Commit();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Database Error during deletion: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void adminData_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = adminData.Rows[e.RowIndex];
            object value = row.Cells["Active"].Value;

            Color backColor = adminData.DefaultCellStyle.BackColor;

            if (value != null)
            {
                if (value is string status)
                {
                    if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                        backColor = Color.LightGreen;
                    else if (status.Equals("Offline", StringComparison.OrdinalIgnoreCase))
                        backColor = Color.LightCoral;
                }

                else if (value is bool isActive)
                {
                    backColor = isActive ? Color.LightGreen : Color.LightCoral;
                }

                else if (value is int numStatus)
                {
                    backColor = (numStatus != 0) ? Color.LightGreen : Color.LightCoral;
                }
            }

            row.DefaultCellStyle.BackColor = backColor;
        }

        public void LoadAdmins()
        {
            try
            {
                string query = @"SELECT user_id AS UserID, Username, email AS Email, role AS Role, password_Hash AS PasswordHash, active AS Active, last_login AS LastLogin FROM Account";

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        adminData.DataSource = dt;
                        adminData.ReadOnly = true;
                        adminData.RowHeadersVisible = false;
                        adminData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        adminData.AllowUserToAddRows = false;

                        if (adminData.Columns.Contains("UserID"))
                            adminData.Columns["UserID"].Visible = false;
                        foreach (DataGridViewRow row in adminData.Rows)
                        {
                            if (row.Cells["Active"].Value != null)
                            {
                                bool isActive = Convert.ToBoolean(row.Cells["Active"].Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading admin data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        private void adminData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridView dgv = (DataGridView)sender;
            string columnName = dgv.Columns[e.ColumnIndex].Name;

            string[] requiredCols =
            { "UserID", "Username", "Email", "Role","PasswordHash", "Active", "LastLogin"};

            foreach (string col in requiredCols)
            {
                if (!dgv.Columns.Contains(col))
                {
                    MessageBox.Show($"Missing column: {col}.",
                        "Column Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!int.TryParse(dgv.Rows[e.RowIndex].Cells["UserID"].Value?.ToString(), out int userId))
            {
                MessageBox.Show("Failed to retrieve UserID.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string adminName = dgv.Rows[e.RowIndex].Cells["Username"].Value?.ToString() ?? "Unknown";

            if (columnName == "Delete")
            {
                if (string.Equals(adminName, this.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("You cannot delete the account you are currently logged in with.",
                        "Security", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult confirm = MessageBox.Show(
                    $"Are you sure you want to delete user '{adminName}' (ID: {userId})?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.No) return;

                if (DeleteAdmin(userId))
                {
                    MessageBox.Show("User deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAdmins();
                }
                else
                {
                    MessageBox.Show("Delete failed.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            else if (columnName == "Edit")
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                if (!int.TryParse(row.Cells["UserID"].Value?.ToString(), out int userID))
                {
                    MessageBox.Show("Invalid UserID. Cannot edit this account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string username = row.Cells["Username"].Value?.ToString();
                string email = row.Cells["Email"].Value?.ToString();
                string role = row.Cells["Role"].Value?.ToString();
                string passwordHash = row.Cells["PasswordHash"].Value?.ToString();
                string active = row.Cells["Active"].Value?.ToString();

                AddAmin_Acccounts editForm = new AddAmin_Acccounts(this, userID, username, email, role, passwordHash, active);

                editForm.ShowDialog();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddAmin_Acccounts AddAcccount = new AddAmin_Acccounts(this);
            AddAcccount.ShowDialog();
        }

        private void ApplyRoleRestrictions()
        {
            if (UserRole == "Admin")
            {
                btnBackUp.Visible = false;
                btnViewReport.Visible = false;

                if (panel1 != null) panel1.Visible = false;
                if (adminData != null) adminData.Visible = false;
                if (btnAdd != null) btnAdd.Visible = false;

                if (panel2 != null) panel2.Visible = true;

                lbPasswordMessage.Visible = false;
                lbMatchPassword.Visible = false;
                lbUpdatePasswordMessage.Visible = false;
                panelHeader.BackColor = Color.LightBlue;
            }

            else if (UserRole == "SuperAdmin")
            {
                btnAdminAcc.Visible = true;
                btnBackUp.Visible = true;
                btnViewReport.Visible = true;

                if (panel1 != null) panel1.Visible = true;
                if (adminData != null) adminData.Visible = true;
                if (btnAdd != null) btnAdd.Visible = true;
                if (panel2 != null) panel2.Visible = false;

                panelHeader.BackColor = Color.White;
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }

        private string GetStoredHash(string username)
        {
            string query = "SELECT password_hash FROM Account WHERE username = @Username";

            using (SqlConnection con = new SqlConnection(DataConnection))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                con.Open();
                object result = cmd.ExecuteScalar();
                return result?.ToString() ?? string.Empty;
            }
        }

        private int GetUserId(string username)
        {
            string query = "SELECT user_id FROM Account WHERE username = @Username";
            using (SqlConnection con = new SqlConnection(DataConnection))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                con.Open();
                object result = cmd.ExecuteScalar();

                return (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : -1;
            }
        }

        private void InsertPasswordHistory(int userId, string oldHashedPassword, SqlConnection con, SqlTransaction transaction)
        {
            string historyQuery = "INSERT INTO PasswordHistory (account_id, old_password_hash, changed_date) VALUES (@AccountID, @OldHash, GETDATE())";
            using (SqlCommand cmd = new SqlCommand(historyQuery, con, transaction))
            {
                cmd.Parameters.AddWithValue("@AccountID", userId);
                cmd.Parameters.AddWithValue("@OldHash", oldHashedPassword);
                cmd.ExecuteNonQuery();
            }
        }

        private void UpdateAccountPassword(string newHashedPassword, SqlConnection con, SqlTransaction transaction)
        {
            string updateQuery = "UPDATE Account SET password_hash = @NewHash WHERE username = @Username";
            using (SqlCommand cmd = new SqlCommand(updateQuery, con, transaction))
            {
                cmd.Parameters.AddWithValue("@NewHash", newHashedPassword);
                cmd.Parameters.AddWithValue("@Username", this.UserName);
                cmd.ExecuteNonQuery();
            }
        }

        private bool IsNewPasswordInHistory(string newPlainPassword)
        {
            int userId = GetUserId(this.UserName);
            if (userId == -1) return false;

            string historyQuery = "SELECT old_password_hash FROM PasswordHistory WHERE account_id = @AccountID ORDER BY changed_date DESC";

            using (SqlConnection con = new SqlConnection(DataConnection))
            using (SqlCommand cmd = new SqlCommand(historyQuery, con))
            {
                cmd.Parameters.AddWithValue("@AccountID", userId);
                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string historicalHash = reader["old_password_hash"].ToString();
                        if (VerifyPassword(newPlainPassword, historicalHash))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private bool VerifyAndUpdatePassword(string currentPassword, string newPassword)
        {
            int userId = GetUserId(this.UserName);
            if (userId == -1)
            {
                MessageBox.Show("Error: Failed to retrieve user account ID for password update.", "Security Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string storedHashedPassword = GetStoredHash(this.UserName);

            if (!VerifyPassword(currentPassword, storedHashedPassword))
            {
                return false;
            }

            if (IsNewPasswordInHistory(newPassword))
            {
                MessageBox.Show("Password reuse detected. Please choose a new password that has not been used recently.",
                                 "Security Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string hashedNewPassword = HashPassword(newPassword);

            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    InsertPasswordHistory(userId, storedHashedPassword, con, transaction);
                    UpdateAccountPassword(hashedNewPassword, con, transaction);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Database Update Error: {ex.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string currentPassword = tbCurrentPassword.Text;
            string newPassword = tbNewPassword.Text;
            string confirmPassword = tbConfirmPassword.Text;

            lbUpdatePasswordMessage.Visible = false;

            if (string.IsNullOrWhiteSpace(currentPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill out all password fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New password and confirmation password do not match.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Security Requirement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (VerifyAndUpdatePassword(currentPassword, newPassword))
            {
                lbUpdatePasswordMessage.Text = "Password Successfully Updated";
                lbUpdatePasswordMessage.Visible = true;
                lbUpdatePasswordMessage.ForeColor = Color.Green;

                tbCurrentPassword.Clear();
                tbNewPassword.Clear();
                tbConfirmPassword.Clear();
            }
            else
            {
                if (VerifyPassword(currentPassword, GetStoredHash(this.UserName)) == false)
                {
                    lbUpdatePasswordMessage.Text = "Update Failed. Current password is incorrect.";
                }
                else
                {
                    lbUpdatePasswordMessage.Text = "Update Failed due to security policy or unknown error.";
                }

                lbUpdatePasswordMessage.Visible = true;
                lbUpdatePasswordMessage.ForeColor = Color.Red;
            }
        }

        private int GetPasswordStrengthScore(string password)
        {
            int score = 0;

            if (password.Length >= 8) score++;
            if (Regex.IsMatch(password, @"[A-Z]")) score++;
            if (Regex.IsMatch(password, @"[a-z]")) score++;
            if (Regex.IsMatch(password, @"\d")) score++;
            if (Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]")) score++;

            return score;
        }


        private void tbNewPassword_TextChanged(object sender, EventArgs e)
        {
            string password = tbNewPassword.Text;

            if (string.IsNullOrEmpty(password))
            {
                if (panelStrengthBar != null) panelStrengthBar.Visible = false;
                if (lbPasswordMessage != null) lbPasswordMessage.Visible = false;
                return;
            }

            if (panelStrengthBar != null) panelStrengthBar.Visible = true;
            if (lbPasswordMessage != null) lbPasswordMessage.Visible = true;

            int score = GetPasswordStrengthScore(password);

            Color indicatorColor;
            string strengthText;

            if (score <= 2)
            {
                indicatorColor = Color.Red;
                strengthText = "Password is weak";
            }
            else if (score <= 4)
            {
                indicatorColor = Color.Orange;
                strengthText = "Password is medium";
            }
            else
            {
                indicatorColor = Color.Green;
                strengthText = "Password is strong";
            }

            if (panelStrengthBar != null) panelStrengthBar.BackColor = indicatorColor;
            if (lbPasswordMessage != null)
            {
                lbPasswordMessage.Text = strengthText;
                lbPasswordMessage.ForeColor = indicatorColor;
            }
        }

        private void tbConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            string newPass = tbNewPassword.Text;
            string confirmPass = tbConfirmPassword.Text;

            if (lbMatchPassword == null) return;

            if (!string.IsNullOrEmpty(confirmPass) && newPass != confirmPass)
            {
                lbMatchPassword.Text = "Passwords do not match";
                lbMatchPassword.Visible = true;

                this.tbConfirmPassword.BackColor = Color.LightPink;
            }
            else
            {
                lbMatchPassword.Visible = false;
                this.tbConfirmPassword.BackColor = SystemColors.Window;
            }
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (tbCurrentPassword != null)
            {
                tbCurrentPassword.UseSystemPasswordChar = !cbShowPassword.Checked;
            }

            if (tbNewPassword != null)
            {
                tbNewPassword.UseSystemPasswordChar = !cbShowPassword.Checked;
            }
            if (tbConfirmPassword != null)
            {
                tbConfirmPassword.UseSystemPasswordChar = !cbShowPassword.Checked;
            }
        }

        // -------------------- Button Side Bar -------------------- //

        // --------------- Dashboard Button --------------- //
        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard dashboard = new DashBoard(UserName, UserRole);
            dashboard.Show();
            this.Close();
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

        // --------------- View Reports Button --------------- //
        private void btnViewReport_Click(object sender, EventArgs e)
        {
            if (UserRole == "SuperAdmin")
            {

            }
            else
            {
                MessageBox.Show("Access Denied: Admin cannot view full reports.");
            }
        }

        // --------------- Backup Button --------------- //
        private void btnBackUp_Click(object sender, EventArgs e)
        {
            if (UserRole == "SuperAdmin")
            {

            }
            else
            {
                MessageBox.Show("Access Denied: Admin cannot access backups.");
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