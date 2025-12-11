using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Login_ResetPassword;

namespace WindowsFormsApp1.Super_Admin_Account
{
    public partial class DashBoard : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private string UserName;
        private string UserRole;

        // -------------------- Button Style -------------------- //
        private readonly Color activeColor = Color.FromArgb(46, 51, 73);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        public DashBoard(string username, string userRole)
        {
            InitializeComponent();

            this.UserName = username;
            this.UserRole = userRole;
            lbName.Text = $"{username} \n ({userRole})";

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

            panelHeader.BackColor = Color.White;
            PanelBackGroundProfile.BackColor = Color.FromArgb(46, 51, 73);

            // -------------------- Set Padding -------------------- //
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

            // -------------------- Set Color Unactive Button -------------------- //
            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
            btnContracts.ForeColor = Color.Black;
            btnMaintenance.ForeColor = Color.Black;

            // -------------------- Set Borderline -------------------- //
            plRecentPayments.BorderStyle = BorderStyle.FixedSingle;
            plUpcomingRenewals.BorderStyle = BorderStyle.FixedSingle;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel4.BorderStyle = BorderStyle.FixedSingle;

            // -------------------- Set Borderline color -------------------- //
            plRecentPayments.BackColor = Color.FromArgb(240, 240, 240);
            plUpcomingRenewals.BackColor = Color.FromArgb(240, 240, 240);
            panel1.BackColor = Color.White; panel2.BackColor = Color.White;
            panel3.BackColor = Color.White; panel4.BackColor = Color.White;
        }

        // -------------------- Role-Based Control -------------------- //
        private void ApplyRoleRestrictions()
        {
            if (UserRole == "Admin")
            {
                btnAdminAcc.Visible = false;
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

        // -------------------- Button Style -------------------- //
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

        protected override bool ShowFocusCues => false;

        private void DashBoard_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnDashBoard, activeColor);
            LoadDashboardData();
        }

        // -------------------- Load Dashboard Data -------------------- //
        private void LoadDashboardData()
        {
            CultureInfo philippinesCulture = new CultureInfo("en-PH");

            using (SqlConnection connection = new SqlConnection(DataConnection))
            {
                try
                {
                    connection.Open();

                    int totalProperties = GetScalarResult(connection, "SELECT COUNT(propertyID) FROM Property");
                    label2.Text = totalProperties.ToString();

                    int activeTenants = GetScalarResult(connection, "SELECT COUNT(tenantID) FROM Tenant WHERE tenantStatus = 'Active'");
                    label3.Text = activeTenants.ToString();

                    decimal totalRevenue = GetScalarDecimalResult(connection, "SELECT ISNULL(SUM(CASE WHEN paymentStatus = 'Paid' THEN paymentAmount ELSE 0 END), 0) FROM Payment");
                    label4.Text = totalRevenue.ToString("C0", philippinesCulture);

                    int openRequests = GetScalarResult(connection, "SELECT COUNT(maintenanceReqID) FROM MaintenanceRequest WHERE status = 'Open'");
                    label5.Text = openRequests.ToString();

                    if (UserRole == "Admin")
                        label4.Text = "Confidential";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while loading dashboard data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int GetScalarResult(SqlConnection connection, string query)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    if (int.TryParse(result.ToString(), out int intValue)) return intValue;
                    if (decimal.TryParse(result.ToString(), out decimal decValue)) return (int)decValue;
                }
                return 0;
            }
        }

        private decimal GetScalarDecimalResult(SqlConnection connection, string query)
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value && decimal.TryParse(result.ToString(), out decimal decValue))
                    return decValue;
                return 0.00m;
            }
        }

        // -------------------- Buttons Click Events -------------------- //
        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts adminAccount = new SuperAdmin_AdminAccounts(UserName, UserRole);
            adminAccount.Show();
            this.Hide();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            ProperTies properties = new ProperTies(UserName, UserRole);
            properties.Show();
            this.Hide();
        }

        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants tenants = new Tenants(UserName, UserRole);
            tenants.Show();
            this.Hide();
        }

        private void btnPaymentRec_Click_1(object sender, EventArgs e)
        {
            Payment_Records payment = new Payment_Records(UserName, UserRole);
            payment.Show();
            this.Hide();
        }

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

        private void btnContracts_Click(object sender, EventArgs e)
        {
            
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            
        }

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