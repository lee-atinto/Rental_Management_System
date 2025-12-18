using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Helpers;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;
using WindowsFormsApp1.Super_Admin_Account;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.Main_Form_Dashboards
{
    public partial class View_Reports : Form
    {
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private string UserName;
        private string UserRole;

        // -------------------- Button Style -------------------- //
        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        public View_Reports(string username, string userRole)
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

            SubscribeToCrashMonitor();
            ApplyRoleRestrictions();

            panelHeader.BackColor = Color.White;
            lbName.BackColor = Color.FromArgb(46, 51, 73);
            PicUserProfile.Image = Properties.Resources.profile;
            PicUserProfile.BackColor = Color.FromArgb(46, 51, 73);
            SideBarBakground.BackColor = Color.FromArgb(46, 51, 73);

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
            btnDashBoard.ForeColor = Color.Black;
            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
            btnContracts.ForeColor = Color.Black;
            btnMaintenance.ForeColor = Color.Black;
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

        private void View_Reports_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnViewReport, activeColor);
            LoadDashboardMetrics();
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

        private void LoadDashboardMetrics()
        {
            LoadTotalRevenue();
            LoadCollectionRate();
            LoadMaintenanceRequestsCount();
            LoadRevenueVsExpensesChart();
            LoadUnitAndMaintenanceCounts();
            LoadUnitsStatusPieChart();
        }

        private decimal GetTotalRevenue()
        {
            decimal totalRevenue = 0.00m;

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    string query = @"SELECT ISNULL(SUM(CASE WHEN paymentStatus = 'Paid' THEN paymentAmount ELSE 0 END), 0) 
                                     FROM Payment";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value && decimal.TryParse(result.ToString(), out decimal decValue))
                        {
                            totalRevenue = decValue;
                        }
                    }
                }
            }
            catch
            {
                totalRevenue = 0.00m;
            }

            return totalRevenue;
        }

        private decimal GetSixMonthsRevenue()
        {
            decimal totalRevenue = 0.00m;

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    string query = @"SELECT ISNULL(SUM(CASE WHEN paymentStatus = 'Paid' THEN paymentAmount ELSE 0 END), 0)
                                     FROM Payment
                                     WHERE paymentDate >= DATEADD(MONTH, -6, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        con.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value && decimal.TryParse(result.ToString(), out decimal decValue))
                        {
                            totalRevenue = decValue;
                        }
                    }
                }
            }
            catch
            {
                totalRevenue = 0.00m;
            }

            return totalRevenue;
        }

        private void LoadTotalRevenue()
        {
            try
            {
                decimal total = GetTotalRevenue();
                CultureInfo philippinesCulture = new CultureInfo("en-PH");
                label4.Text = total.ToString("C0", philippinesCulture);
                label4.ForeColor = Color.Green;
            }
            catch
            {
                label4.Text = "Error";
            }
        }

        private void LoadCollectionRate()
        {
            string totalRentQuery = @"SELECT ISNULL(SUM(MonthlyRent),0) FROM Unit";
            string collectedQuery = @"SELECT ISNULL(SUM(paymentAmount),0)
                                      FROM Payment
                                      WHERE paymentStatus = 'Completed'
                                      AND MONTH(paymentDate) = MONTH(GETDATE())
                                      AND YEAR(paymentDate) = YEAR(GETDATE())";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();
                    decimal totalRent = Convert.ToDecimal(new SqlCommand(totalRentQuery, con).ExecuteScalar());
                    decimal collected = Convert.ToDecimal(new SqlCommand(collectedQuery, con).ExecuteScalar());

                    double rate = totalRent > 0 ? (double)collected / (double)totalRent * 100 : 0;
                    if (rate > 100) rate = 100;

                    label6.Text = $"{rate:F2} %";
                    label6.ForeColor = rate >= 90 ? Color.Green : Color.Red;
                }
            }
            catch
            {
                label6.Text = "Error";
            }
        }

        private void LoadMaintenanceRequestsCount()
        {
            string query = @"SELECT COUNT(*)
                             FROM MaintenanceRequest
                             WHERE status <> 'Completed'";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    label8.Text = count.ToString();
                    label8.ForeColor = count > 0 ? Color.Red : Color.Green;
                }
            }
            catch
            {
                label8.Text = "Error";
            }
        }

        private void LoadRevenueVsExpensesChart()
        {
            string query = @"
                SELECT 
                    FORMAT(DateCol, 'MMM yyyy') AS MonthLabel,
                    SUM(Revenue) AS TotalRevenue,
                    SUM(Expense) AS TotalExpense
                FROM
                (
                    SELECT paymentDate AS DateCol, paymentAmount AS Revenue, 0 AS Expense
                    FROM Payment
                    WHERE paymentStatus = 'Paid'

                    UNION ALL

                    SELECT GETDATE(), 0, SUM(MonthlyRent)
                    FROM Unit
                    WHERE Status = 'Occupied'
                ) X
                GROUP BY FORMAT(DateCol,'MMM yyyy'), YEAR(DateCol), MONTH(DateCol)
                ORDER BY YEAR(DateCol), MONTH(DateCol)";

            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    da.Fill(dt);
                }

                chart1.Series.Clear();
                chart1.ChartAreas[0].AxisX.Title = "Month";
                chart1.ChartAreas[0].AxisY.Title = "Amount (₱)";
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "N0";

                Series rev = new Series("Revenue") { ChartType = SeriesChartType.Column };
                Series exp = new Series("Expenses") { ChartType = SeriesChartType.Column };

                foreach (DataRow row in dt.Rows)
                {
                    rev.Points.AddXY(row["MonthLabel"], row["TotalRevenue"]);
                    exp.Points.AddXY(row["MonthLabel"], row["TotalExpense"]);
                }

                chart1.Series.Add(rev);
                chart1.Series.Add(exp);
            }
            catch
            {
            }
        }

        private decimal GetSixMonthsExpenses()
        {
            string query = @"SELECT ISNULL(SUM(MonthlyRent),0)
                             FROM Unit
                             WHERE Status = 'Occupied'";

            using (SqlConnection con = new SqlConnection(DataConnection))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                decimal monthly = Convert.ToDecimal(cmd.ExecuteScalar());
                return monthly * 6;
            }
        }

        private void LoadUnitsStatusPieChart()
        {
            try
            {
                int occupied = 0, vacant = 0, maintenance = 0;

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();

                    occupied = Convert.ToInt32(new SqlCommand("SELECT COUNT(*) FROM Unit WHERE Status='Occupied'", con).ExecuteScalar());
                    vacant = Convert.ToInt32(new SqlCommand("SELECT COUNT(*) FROM Unit WHERE Status='Vacant'", con).ExecuteScalar());
                    maintenance = Convert.ToInt32(new SqlCommand("SELECT COUNT(*) FROM MaintenanceRequest WHERE status IN ('InProgress','Pending')", con).ExecuteScalar());
                }

                chartUnitsStatus.Series.Clear();
                chartUnitsStatus.Titles.Clear();
                chartUnitsStatus.Legends[0].Docking = Docking.Bottom;

                Title chartTitle = new Title
                {
                    Text = "Units Status Distribution",
                    Font = new Font("Calibri", 14, FontStyle.Bold),
                    Docking = Docking.Top
                };
                chartUnitsStatus.Titles.Add(chartTitle);

                Series series = new Series
                {
                    Name = "Units",
                    ChartType = SeriesChartType.Pie,
                    IsValueShownAsLabel = true
                };

                int total = occupied + vacant + maintenance;

                if (occupied > 0)
                {
                    series.Points.AddXY("Occupied", occupied);
                    series.Points[series.Points.Count - 1].Color = Color.Green;
                }

                if (vacant > 0)
                {
                    series.Points.AddXY("Vacant", vacant);
                    series.Points[series.Points.Count - 1].Color = Color.Orange;
                }

                if (maintenance > 0)
                {
                    series.Points.AddXY("Under Maintenance", maintenance);
                    series.Points[series.Points.Count - 1].Color = Color.Red;
                }

                foreach (DataPoint point in series.Points)
                {
                    point.Label = $"{point.AxisLabel}: {point.YValues[0]} ({point.YValues[0] / total * 100:F1}%)";
                    point.Font = new Font("Calibri", 12, FontStyle.Regular);
                }

                chartUnitsStatus.Series.Add(series);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading units status chart: " + ex.Message);
            }
        }

        private void LoadUnitAndMaintenanceCounts()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();

                    string occupiedQuery = @"SELECT COUNT(*) FROM Unit WHERE Status = 'Occupied'";
                    int occupied = Convert.ToInt32(new SqlCommand(occupiedQuery, con).ExecuteScalar());
                    label24.Text = occupied.ToString();

                    string vacantQuery = @"SELECT COUNT(*) FROM Unit WHERE Status = 'Vacant'";
                    int vacant = Convert.ToInt32(new SqlCommand(vacantQuery, con).ExecuteScalar());
                    label21.Text = vacant.ToString();

                    string maintenanceQuery = @"SELECT COUNT(*) 
                                                FROM MaintenanceRequest
                                                WHERE status IN ('InProgress','Pending')";
                    int maintenanceCount = Convert.ToInt32(new SqlCommand(maintenanceQuery, con).ExecuteScalar());
                    label18.Text = maintenanceCount.ToString();
                }
            }
            catch
            {
                label24.Text = "Error";
                label21.Text = "Error";
                label18.Text = "Error";
            }
        }

        private void btnOverView_Click(object sender, EventArgs e)
        {
            plOverView.Visible = true;

            plOccupancyReport.Visible = false;
        }


        private void btnOccupancyReport_Click(object sender, EventArgs e)
        {
            plOccupancyReport.Visible = true;

            plOverView.Visible = false;
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

        // --------------- Backup Button --------------- //
        private void btnBackUp_Click(object sender, EventArgs e)
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