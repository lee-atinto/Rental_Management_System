using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Helpers;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;

namespace WindowsFormsApp1.Super_Admin_Account
{
    public partial class DashBoard : BaseForm
    {
        private readonly string DataConnection = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private string UserName;
        private string UserRole;

        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        public DashBoard(string username, string userRole)
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

            LoadMaintenanceData();
            LoadRecentPayments();

            DataRecentMaintenanceRequests.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);

            // UI styling
            panelHeader.BackColor = Color.White;
            lbName.BackColor = Color.FromArgb(46, 51, 73);
            PicUserProfile.Image = Properties.Resources.profile;
            PicUserProfile.BackColor = Color.FromArgb(46, 51, 73);
            SideBarBakground.BackColor = Color.FromArgb(46, 51, 73);

            Size iconSize = new Size(64, 64);
            pictureBox1.Size = iconSize;
            pictureBox2.Size = iconSize;
            pictureBox3.Size = iconSize;
            pictureBox4.Size = iconSize;

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;

            pictureBox1.Image = Properties.Resources.property;
            pictureBox2.Image = Properties.Resources.lender;
            pictureBox3.Image = Properties.Resources.revenue;
            pictureBox4.Image = Properties.Resources.application;

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

            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
            btnContracts.ForeColor = Color.Black;
            btnMaintenance.ForeColor = Color.Black;

            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel4.BorderStyle = BorderStyle.FixedSingle;

            SubscribeToCrashMonitor();
            SetupDataGridView(DataRecentMaintenanceRequests);
            SetupDataGridView(DataPaymentRecent);
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

        private void SetupDataGridView(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.Dock = DockStyle.Fill;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersVisible = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
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

        private void DashBoard_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnDashBoard, activeColor);
            SubscribeToCrashMonitor();
            LoadDashboardData();
        }

        public void LoadMaintenanceData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection))
                {
                    string query = @"SELECT CONCAT(
                        PI.firstName, ' ', PI.middleName, ' ', PI.lastName, CHAR(13), CHAR(10),
                        U.unitNumber, ' ', MR.description, CHAR(13), CHAR(10),
                        MR.requestDate) AS [Info],
                        MR.Status
                        FROM PersonalInformation PI
                        INNER JOIN Property P ON PI.personalInfoID = P.propertyID
                        INNER JOIN Unit U ON P.PropertyID = U.PropertyID 
                        INNER JOIN MaintenanceRequest MR ON P.PropertyID = MR.PropertyID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    conn.Open();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        DataTable emptyDt = new DataTable();
                        emptyDt.Columns.Add("Info");
                        emptyDt.Rows.Add("There is no recent maintenance request.");
                        DataRecentMaintenanceRequests.DataSource = emptyDt;
                    }
                    else
                    {
                        DataRecentMaintenanceRequests.DataSource = dt;
                        if (DataRecentMaintenanceRequests.Columns.Contains("Status"))
                        {
                            DataRecentMaintenanceRequests.Columns["Status"].Width = 125;
                            DataRecentMaintenanceRequests.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }

                    DataRecentMaintenanceRequests.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
        }

        public void LoadRecentPayments()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection))
                {
                    string query = @"
                        SELECT 
                            P.paymentId,
                            ISNULL(TI.firstName,'') + ' ' + ISNULL(TI.lastName,'') AS TenantName,
                            ISNULL(PR.propertyName,'N/A') AS Property,
                            P.paymentAmount,
                            P.paymentDate,
                            P.paymentStatus
                        FROM Payment P
                        INNER JOIN Contract C ON P.contractId = C.contractID
                        INNER JOIN PersonalInformation TI ON C.tenantId = TI.tenantId
                        LEFT JOIN Property PR ON C.propertyID = PR.propertyID
                        WHERE DATEPART(week, P.paymentDate) = DATEPART(week, GETDATE())
                          AND YEAR(P.paymentDate) = YEAR(GETDATE())
                        ORDER BY P.paymentDate DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    conn.Open();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        DataTable emptyDt = new DataTable();
                        emptyDt.Columns.Add("Message");
                        emptyDt.Rows.Add("There is no recent payment.");
                        DataPaymentRecent.DataSource = emptyDt;
                        DataPaymentRecent.Columns["Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        DataPaymentRecent.CellBorderStyle = DataGridViewCellBorderStyle.None;
                    }
                    else
                    {
                        DataPaymentRecent.DataSource = dt;
                        DataPaymentRecent.CellBorderStyle = DataGridViewCellBorderStyle.Single;

                        CultureInfo phCulture = new CultureInfo("en-PH");

                        if (DataPaymentRecent.Columns.Contains("paymentAmount"))
                        {
                            DataPaymentRecent.Columns["paymentAmount"].DefaultCellStyle.Format = "C0";
                            DataPaymentRecent.Columns["paymentAmount"].DefaultCellStyle.FormatProvider = phCulture;
                            DataPaymentRecent.Columns["paymentAmount"].Width = 100;
                        }
                        if (DataPaymentRecent.Columns.Contains("TenantName"))
                            DataPaymentRecent.Columns["TenantName"].Width = 150;
                        if (DataPaymentRecent.Columns.Contains("Property"))
                            DataPaymentRecent.Columns["Property"].Width = 150;
                        if (DataPaymentRecent.Columns.Contains("paymentDate"))
                            DataPaymentRecent.Columns["paymentDate"].Width = 120;
                        if (DataPaymentRecent.Columns.Contains("paymentStatus"))
                            DataPaymentRecent.Columns["paymentStatus"].Width = 100;
                        if (DataPaymentRecent.Columns.Contains("paymentId"))
                            DataPaymentRecent.Columns["paymentId"].Visible = false;
                    }

                    DataPaymentRecent.ColumnHeadersVisible = false;
                    DataPaymentRecent.RowHeadersVisible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading recent payments: " + ex.Message);
            }
        }

        private void LoadDashboardData()
        {
            CultureInfo philippinesCulture = new CultureInfo("en-PH");

            using (SqlConnection connection = new SqlConnection(DataConnection))
            {
                try
                {
                    connection.Open();

                    int totalProperties = Convert.ToInt32(new SqlCommand("SELECT COUNT(propertyID) FROM Property", connection).ExecuteScalar());
                    int activeTenants = Convert.ToInt32(new SqlCommand("SELECT COUNT(tenantID) FROM Tenant WHERE tenantStatus = 'Active'", connection).ExecuteScalar());
                    decimal totalRevenue = Convert.ToDecimal(new SqlCommand("SELECT ISNULL(SUM(CASE WHEN paymentStatus = 'Paid' THEN paymentAmount ELSE 0 END),0) FROM Payment", connection).ExecuteScalar());
                    int allRequests = Convert.ToInt32(new SqlCommand("SELECT COUNT(maintenanceReqID) FROM MaintenanceRequest", connection).ExecuteScalar());

                    label2.Text = totalProperties.ToString();
                    label3.Text = activeTenants.ToString();
                    label4.Text = UserRole == "Admin" ? "Confidential" : totalRevenue.ToString("C0", philippinesCulture);
                    label5.Text = allRequests.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading dashboard data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- Remaining code for buttons and formatting remains unchanged ---
        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DataRecentMaintenanceRequests == null || DataRecentMaintenanceRequests.Rows.Count == 0 || e.RowIndex < 0)
                return;

            if (!DataRecentMaintenanceRequests.Columns.Contains("Status") || !DataRecentMaintenanceRequests.Columns.Contains("Info"))
                return;

            int statusColumnIndex = DataRecentMaintenanceRequests.Columns["Status"].Index;
            int infoColumnIndex = DataRecentMaintenanceRequests.Columns["Info"].Index;

            if (e.ColumnIndex != statusColumnIndex && e.ColumnIndex != infoColumnIndex)
                return;

            if (e.ColumnIndex == statusColumnIndex)
            {
                object cellValue = DataRecentMaintenanceRequests.Rows[e.RowIndex].Cells[statusColumnIndex].Value;
                Color statusColor = Color.White;

                if (cellValue != null && cellValue != DBNull.Value)
                {
                    string status = cellValue.ToString().ToLower().Replace("-", "");
                    switch (status)
                    {
                        case "pending": break;
                        case "inprogress": break;
                        case "completed": break;
                    }
                }

                e.CellStyle.BackColor = statusColor;
                e.CellStyle.SelectionBackColor = statusColor;
            }
            else if (e.ColumnIndex == infoColumnIndex)
            {
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.SelectionBackColor = Color.White;
            }

            e.CellStyle.ForeColor = Color.Black;
            e.CellStyle.SelectionForeColor = Color.Black;
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

        private void btnBackUp_Click(object sender, EventArgs e)
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
