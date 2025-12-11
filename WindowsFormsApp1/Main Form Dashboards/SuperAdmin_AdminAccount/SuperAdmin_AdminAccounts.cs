using Rental_Management_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Super_Admin_Account;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount
{
    public partial class SuperAdmin_AdminAccounts : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        // -------------------- User Information -------------------- //
        private readonly string Username;
        private readonly string UserRole;

        // -------------------- Button Style -------------------- //
        private readonly Color activeColor = Color.FromArgb(46, 51, 73);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        private void InitializeButtonStyle(Button button)
        {
            if (button != null)
            {
                button.TabStop = false;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = Color.FromArgb(46, 51, 73);
                button.Padding = new Padding(50, 5, 5, 5);

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

        // -------------------- Delete Admin -------------------- //
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
                        adminCommand.ExecuteNonQuery();
                    }
                    using (SqlCommand userCommand = new SqlCommand(deleteAccounts, connection, transaction))
                    {
                        userCommand.Parameters.AddWithValue("@UserID", userID);
                        int rowsAffected = userCommand.ExecuteNonQuery();

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


        // -------------------- Load Admin Data -------------------- //
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

        public SuperAdmin_AdminAccounts(string username, string userRole)
        {
            InitializeComponent();

            // -------------------- Set User Information -------------------- //
            this.Username = username;
            this.UserRole = userRole;
            this.lbName.Text = $"{username} \n ({userRole})";

            // -------------------- Setup DataGridView Columns & Load Data -------------------- //
            LoadAdmins();

            // -------------------- Initialize Button Style -------------------- //
            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);
            InitializeButtonStyle(btnProperties);

            panelHeader.BackColor = Color.White;
            PanelBackGroundProfile.BackColor = Color.FromArgb(46, 51, 73);

            // -------------------- Set Padding Button -------------------- //
            btnDashBoard.Padding = new Padding(30, 0, 0, 0);
            btnAdminAcc.Padding = new Padding(30, 0, 0, 0);
            btnTenant.Padding = new Padding(30, 0, 0, 0);
            btnPaymentRec.Padding = new Padding(30, 0, 0, 0);
            btnViewReport.Padding = new Padding(30, 0, 0, 0);
            btnBackUp.Padding = new Padding(30, 0, 0, 0);
            btnProperties.Padding = new Padding(30, 0, 0, 0);

            // -------------------- Set Color Unactive Button -------------------- //
            btnDashBoard.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;

            // -------------------- DataGridView Styling -------------------- //
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
                if (string.Equals(adminName, this.Username, StringComparison.OrdinalIgnoreCase))
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

        // -------------------- Dashboard Buttons Click Event -------------------- //
        private void btnDashBoard_Click_1(object sender, EventArgs e)
        {
            DashBoard dashboard = new DashBoard(Username, UserRole);
            dashboard.Show();
            this.Hide();
        }

        // -------------------- Tenants Buttons Click Event -------------------- //
        private void btnTenant_Click_1(object sender, EventArgs e)
        {
            Tenants tenantsForm = new Tenants(Username, UserRole);
            tenantsForm.Show();
            this.Hide();
        }

        // -------------------- Properties Buttons Click Event -------------------- //
        private void btnProperties_Click(object sender, EventArgs e)
        {
            ProperTies properties = new ProperTies(Username, UserRole);
            properties.Show();
            this.Hide();
        }

        // -------------------- Payment Records Buttons Click Event -------------------- //
        private void btnPaymentRec_Click_1(object sender, EventArgs e)
        {
            Payment_Records payment_Records = new Payment_Records(Username, UserRole);
            payment_Records.Show();
            this.Hide();
        }

        // -------------------- Logout Buttons Click Event -------------------- //
        private void btnlogout_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = "UPDATE Account SET active = 0 WHERE username=@u";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", this.Username);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            this.Hide();
            new LoginPage().Show();
        }
    }
}
