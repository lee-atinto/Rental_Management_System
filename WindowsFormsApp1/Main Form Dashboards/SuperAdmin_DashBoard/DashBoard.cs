using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;

namespace WindowsFormsApp1.Super_Admin_Account
{
    public partial class DashBoard : Form
    {
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private string UserName;
        private string UserRole;

        // -------------------- Button Style -------------------- //
        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        public DashBoard(string username, string userRole)
        {
            InitializeComponent();
            LoadMaintenanceData();

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
            GetRecentMaintenance();
            LoadRecentPayments();

            DataRecentMaintenanceRequests.CellFormatting += new DataGridViewCellFormattingEventHandler(dgv_CellFormatting);

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
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel4.BorderStyle = BorderStyle.FixedSingle;

            // -------------------- Data Recent Maintenance Request Borderline -------------------- //
            DataRecentMaintenanceRequests.ReadOnly = true;
            DataRecentMaintenanceRequests.Dock = DockStyle.Fill;
            DataRecentMaintenanceRequests.RowHeadersVisible = false;
            DataRecentMaintenanceRequests.ColumnHeadersVisible = false;
            DataRecentMaintenanceRequests.BorderStyle = BorderStyle.None;
            DataRecentMaintenanceRequests.CellBorderStyle = DataGridViewCellBorderStyle.None;
            DataRecentMaintenanceRequests.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DataRecentMaintenanceRequests.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DataRecentMaintenanceRequests.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataRecentMaintenanceRequests.DefaultCellStyle.Font = new Font("Segoe UI", 10F);

            DataPaymentRecent.ReadOnly = true;
            DataPaymentRecent.Dock = DockStyle.Fill;
            DataPaymentRecent.RowHeadersVisible = false;
            DataPaymentRecent.ColumnHeadersVisible = false;
            DataPaymentRecent.BorderStyle = BorderStyle.None;
            DataPaymentRecent.CellBorderStyle = DataGridViewCellBorderStyle.None;
            DataPaymentRecent.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            DataPaymentRecent.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            DataPaymentRecent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataPaymentRecent.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
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

        private DataTable GetRecentMaintenance()
        {
            DataTable dt = new DataTable();
            string query = @"
        SELECT TOP 5
            MR.maintenanceReqID, 
            MR.description, 
            MR.requestDate, 
            MR.Status AS MaintenanceStatus,
            U.UnitNumber,
            PI.firstName, 
            PI.middleName, 
            PI.lastName,
            MR.propertyID, 
            RT.typeName
        FROM MaintenanceRequest AS MR 
        LEFT JOIN Tenant AS T ON MR.TenantID = T.TenantID
        LEFT JOIN PersonalInformation AS PI ON T.TenantID = PI.TenantID 
        LEFT JOIN Contract AS C ON T.TenantID = C.TenantID 
        LEFT JOIN Unit AS U ON C.UnitID = U.UnitID
        LEFT JOIN RequestType AS RT ON MR.requestTypeID = RT.requestTypeID
        WHERE MR.requestDate >= DATEADD(day, -7, CAST(GETDATE() AS DATE))
        ORDER BY MR.requestDate DESC;";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching recent maintenance: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
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

                        if (DataPaymentRecent.Columns.Contains("TenantName"))
                            DataPaymentRecent.Columns["TenantName"].Width = 150;
                        if (DataPaymentRecent.Columns.Contains("Property"))
                            DataPaymentRecent.Columns["Property"].Width = 150;
                        if (DataPaymentRecent.Columns.Contains("paymentAmount"))
                        {
                            DataPaymentRecent.Columns["paymentAmount"].DefaultCellStyle.Format = "C0";
                            DataPaymentRecent.Columns["paymentAmount"].Width = 100;
                        }
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

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (DataRecentMaintenanceRequests == null || DataRecentMaintenanceRequests.Rows.Count == 0 || e.RowIndex < 0)
            {
                return;
            }

            if (!DataRecentMaintenanceRequests.Columns.Contains("Status") || !DataRecentMaintenanceRequests.Columns.Contains("Info"))
            {
                return;
            }

            int statusColumnIndex = DataRecentMaintenanceRequests.Columns["Status"].Index;
            int infoColumnIndex = DataRecentMaintenanceRequests.Columns["Info"].Index;

            if (e.ColumnIndex != statusColumnIndex && e.ColumnIndex != infoColumnIndex)
            {
                return;
            }

            if (e.ColumnIndex == statusColumnIndex)
            {
                object cellValue = DataRecentMaintenanceRequests.Rows[e.RowIndex].Cells[statusColumnIndex].Value;
                Color statusColor = Color.White;

                if (cellValue != null && cellValue != DBNull.Value)
                {
                    string status = cellValue.ToString().ToLower().Replace("-", "");

                    switch (status)
                    {
                        case "pending":
                            break;
                        case "inprogress":
                            break;
                        case "completed":
                            break;
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

        private void DashBoard_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnDashBoard, activeColor);
            LoadDashboardData();
        }

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

                    int allRequests = GetScalarResult(connection, "SELECT COUNT(maintenanceReqID) FROM MaintenanceRequest");
                    label5.Text = allRequests.ToString();

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

        // -------------------- Button Side Bar -------------------- //

        private void btnTenant_Click_1(object sender, EventArgs e)
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

        private void btnViewReport_Click_1(object sender, EventArgs e)
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

        private void btnBackUp_Click_1(object sender, EventArgs e)
        {
            if (UserRole == "SuperAdmin")
            {
                BackUp backup = new BackUp(UserName, UserRole);
                backup.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Access Denied: Admin cannot access backups.");
            }
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