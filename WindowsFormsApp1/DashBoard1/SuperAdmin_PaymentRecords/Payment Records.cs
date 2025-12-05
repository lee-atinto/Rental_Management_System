using Rental_Management_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.Super_Admin_Account;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.Login_ResetPassword
{
    public partial class Payment_Records : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS;Initial Catalog=RentalManagementSystem;Integrated Security=True";

        // -------------------- User Information -------------------- //
        private readonly string Username;
        private readonly string UserRole;

        // -------------------- Admin Account Form -------------------- //
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

        private void DataPaymentRecords_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if(DataPaymentRecords.Columns[e.ColumnIndex].Name == "cStatus")
            {
                if (e.Value != null && e.Value is string status)
                {
                    if (status.Equals("Fully Paid", StringComparison.OrdinalIgnoreCase))
                    {
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.SelectionBackColor = Color.Green;
                    }
                    else if (status.Equals("Balanced", StringComparison.OrdinalIgnoreCase))
                    {
                        e.CellStyle.BackColor = Color.LightYellow;
                        e.CellStyle.SelectionBackColor = Color.Gold;
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.LightCoral;
                        e.CellStyle.SelectionBackColor = Color.Red;
                    }
                }
            }
        }

        private void DataPaymentRecords_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        }

        private void LoadPaymentRecords(string searchTerm = "")
        {
            try
            {
                string query = "SELECT P.PaymentID, P.TenantID, T.TenantName, P.AmountPaid, P.cStatus, P.DateOfPayment FROM Payments AS P INNER JOIN Tenants AS T ON P.TenantID = T.TenantID";

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (DataPaymentRecords != null)
                        {
                            DataPaymentRecords.DataSource = dt;
                            DataPaymentRecords.ReadOnly = true;
                            DataPaymentRecords.RowHeadersVisible = false;
                            DataPaymentRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                            DataPaymentRecords.AllowUserToAddRows = false;

                            if (DataPaymentRecords.Columns.Contains("TenantID"))
                            {
                                DataPaymentRecords.Columns["TenantID"].Visible = false;
                            }

                            if (DataPaymentRecords.Columns.Contains("TenantName"))
                            {
                                DataPaymentRecords.Columns["TenantName"].HeaderText = "TENANT";
                                DataPaymentRecords.Columns["TenantName"].DisplayIndex = 0;
                            }
                            if (DataPaymentRecords.Columns.Contains("AmmountPaid"))
                            {
                                DataPaymentRecords.Columns["AmmountPaid"].DisplayIndex = 1;
                            }
                            if (DataPaymentRecords.Columns.Contains("cStatus"))
                            {
                                DataPaymentRecords.Columns["cStatus"].DisplayIndex = 2;
                            }
                            if (DataPaymentRecords.Columns.Contains("RoDateOfPaymentle"))
                            {
                                DataPaymentRecords.Columns["DateOfPayment"].DisplayIndex = 3;
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

        public Payment_Records(string username, string userRole)
        {
            InitializeComponent();

            this.DataPaymentRecords.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataPaymentRecords_CellFormatting);

            // -------------------- Set User Information -------------------- //
            Username = username;
            UserRole = userRole;
            this.lbName.Text = $"{Username} \n ({UserRole})";

            // -------------------- Load Payment Data -------------------- //
            LoadPaymentRecords();

            // -------------------- Initialize Button Style (Sidebar) -------------------- //
            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);

            panelHeader.BackColor = Color.FromArgb(46, 51, 73);
            PanelBackGroundProfile.BackColor = Color.FromArgb(46, 51, 73);

            // -------------------- Set Padding Button -------------------- //
            btnDashBoard.Padding = new Padding(30, 0, 0, 0);
            btnAdminAcc.Padding = new Padding(30, 0, 0, 0);
            btnTenant.Padding = new Padding(30, 0, 0, 0);
            btnPaymentRec.Padding = new Padding(30, 0, 0, 0);
            btnViewReport.Padding = new Padding(30, 0, 0, 0);
            btnBackUp.Padding = new Padding(30, 0, 0, 0);

            // -------------------- Set Color Unactive Button -------------------- //
            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnDashBoard.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;

            // -------------------- DataGridView Styling -------------------- //
            DataPaymentRecords.BorderStyle = BorderStyle.None;
            DataPaymentRecords.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            DataPaymentRecords.RowHeadersVisible = false;
            DataPaymentRecords.EnableHeadersVisualStyles = false;
            DataPaymentRecords.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = Color.Navy;
            headerStyle.ForeColor = Color.White;
            headerStyle.Font = new Font("Arial", 18, FontStyle.Bold);
            DataPaymentRecords.ColumnHeadersDefaultCellStyle = headerStyle;

            Color defaultBG = Color.FromArgb(240, 240, 240);
            DataPaymentRecords.BackgroundColor = defaultBG;
            DataPaymentRecords.DefaultCellStyle.BackColor = Color.White;
            DataPaymentRecords.AlternatingRowsDefaultCellStyle.BackColor = defaultBG;

            DataPaymentRecords.RowTemplate.Height = 60;
        }

        private void Payment_Records_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnPaymentRec, activeColor);
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = tbSearch.Text;
            LoadPaymentRecords(searchTerm);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Filter logic goes here, using ApplyAdvancedFilter(statusFilter, startDate, endDate)", "Filter", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // -------------------- Dashboard Buttons Click Event -------------------- //
        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            string currentUsername = this.Username;
            string currentUserRole = this.UserRole;

            if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrEmpty(currentUserRole))
            {
                currentUsername = SessionManager.LoggedInUsername;
                currentUserRole = SessionManager.LoggedInUserRole;
                if (string.IsNullOrEmpty(currentUsername))
                {
                    MessageBox.Show("Session expired or user data is missing. Please log in again.", "Session Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Hide();
                    return;
                }
            }

            if (currentUserRole == "Super Admin")
            {
                MessageBox.Show("Navigate to Super Admin Dashboard", "Navigation", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else if (currentUserRole == "Admin")
            {
                MessageBox.Show("Navigate to Admin Dashboard", "Navigation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Hide();
        }

        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts adminAcc = new SuperAdmin_AdminAccounts(Username, UserRole);
            adminAcc.Show();
            this.Hide();
        }

        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants Tenant = new Tenants(Username, UserRole);
            Tenant.Show();
            this.Hide();
        }
    }
}