using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
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
            LoadOccupancyRate();
            LoadCollectionRate();
            LoadMaintenanceRequestsCount();
            LoadRevenueVsExpensesChart();
            LoadSixMonthsRevenue();
            LoadSixMonthsExpenses();
            LoadSixMonthsNetProfit();

            LoadAverageRevenue();
            LoadAverageExpenses();
            LoadProfitMargin();

            LoadTopPerformingProperties();
            LoadUnitAndMaintenanceCounts();
            LoadUnitsStatusPieChart();
            LoadPaymentAmountSummary();
            LoadPaymentStatusDistributionChart();
            LoadPaymentMethodBreakdown();
            LoadPaymentMethodPercentLabels();

        }

        private void LoadPaymentAmountSummary()
        {
            try
            {
                decimal totalRevenue = 0;
                decimal collected = 0;
                decimal pending = 0;
                decimal overdue = 0;

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();

                    totalRevenue = Convert.ToDecimal(
                        new SqlCommand(@"SELECT ISNULL(SUM(paymentAmount),0) FROM Payment", con)
                        .ExecuteScalar()
                    );

                    collected = Convert.ToDecimal(
                        new SqlCommand(@"SELECT ISNULL(SUM(paymentAmount),0) 
                                 FROM Payment 
                                 WHERE paymentStatus='Paid'", con)
                        .ExecuteScalar()
                    );

                    pending = Convert.ToDecimal(
                        new SqlCommand(@"SELECT ISNULL(SUM(paymentAmount),0) 
                                 FROM Payment 
                                 WHERE paymentStatus='Pending'", con)
                        .ExecuteScalar()
                    );

                    overdue = Convert.ToDecimal(
                        new SqlCommand(@"SELECT ISNULL(SUM(paymentAmount),0) 
                                 FROM Payment 
                                 WHERE paymentStatus='Overdue'", con)
                        .ExecuteScalar()
                    );
                }

                CultureInfo ph = new CultureInfo("en-PH");

                label27.Text = totalRevenue.ToString("C0", ph);
                label27.ForeColor = Color.Green;

                label17.Text = collected.ToString("C0", ph);
                label17.ForeColor = Color.Green;

                label23.Text = pending.ToString("C0", ph);
                label23.ForeColor = Color.Orange;

                label29.Text = overdue.ToString("C0", ph);
                label29.ForeColor = Color.Red;
            }
            catch
            {
                label27.Text = "Error";
                label17.Text = "Error";
                label23.Text = "Error";
                label29.Text = "Error";
            }
        }

        private void LoadPaymentMethodPercentLabels()
        {
            try
            {
                int total =
                    pbBankTransfer.Value +
                    pbGCash.Value +
                    pbCash.Value +
                    pbOthers.Value;

                if (total == 0)
                {
                    label36.Text = "0%";
                    label37.Text = "0%";
                    label38.Text = "0%";
                    label39.Text = "0%";
                    return;
                }

                double bankPct = pbBankTransfer.Value * 100.0 / total;
                double gcashPct = pbGCash.Value * 100.0 / total;
                double cashPct = pbCash.Value * 100.0 / total;
                double otherPct = pbOthers.Value * 100.0 / total;

                label36.Text = bankPct.ToString("F1") + "%";
                label37.Text = gcashPct.ToString("F1") + "%";
                label38.Text = cashPct.ToString("F1") + "%";
                label39.Text = otherPct.ToString("F1") + "%";

                label36.ForeColor = Color.SteelBlue;
                label37.ForeColor = Color.DodgerBlue;
                label38.ForeColor = Color.Green;
                label39.ForeColor = Color.Gray;
            }
            catch
            {
                label36.Text = "Error";
                label37.Text = "Error";
                label38.Text = "Error";
                label39.Text = "Error";
            }
        }

        private void LoadPaymentStatusDistributionChart()
        {
            try
            {
                decimal expected = 0, collected = 0, pending = 0, overdue = 0;

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();

                    expected = Convert.ToDecimal(
                        new SqlCommand("SELECT ISNULL(SUM(paymentAmount),0) FROM Payment", con)
                        .ExecuteScalar());

                    collected = Convert.ToDecimal(
                        new SqlCommand("SELECT ISNULL(SUM(paymentAmount),0) FROM Payment WHERE paymentStatus='Paid'", con)
                        .ExecuteScalar());

                    pending = Convert.ToDecimal(
                        new SqlCommand("SELECT ISNULL(SUM(paymentAmount),0) FROM Payment WHERE paymentStatus='Pending'", con)
                        .ExecuteScalar());

                    overdue = Convert.ToDecimal(
                        new SqlCommand("SELECT ISNULL(SUM(paymentAmount),0) FROM Payment WHERE paymentStatus='Overdue'", con)
                        .ExecuteScalar());
                }

                chartPaymentStatus.Series.Clear();
                chartPaymentStatus.Titles.Clear();
                chartPaymentStatus.Legends[0].Docking = Docking.Bottom;

                chartPaymentStatus.Titles.Add(new Title(
                    "Payment Status Distribution",
                    Docking.Top,
                    new Font("Calibri", 14, FontStyle.Bold),
                    Color.Black));

                Series series = new Series("Payments");
                series.ChartType = SeriesChartType.Pie;
                series.IsValueShownAsLabel = true;

                decimal total = expected;

                if (expected > 0)
                {
                    int i = series.Points.AddXY("Total Expected", expected);
                    series.Points[i].Color = Color.SteelBlue;
                }

                if (collected > 0)
                {
                    int i = series.Points.AddXY("Collected", collected);
                    series.Points[i].Color = Color.Green;
                }

                if (pending > 0)
                {
                    int i = series.Points.AddXY("Pending", pending);
                    series.Points[i].Color = Color.Orange;
                }

                if (overdue > 0)
                {
                    int i = series.Points.AddXY("Overdue", overdue);
                    series.Points[i].Color = Color.Red;
                }

                foreach (DataPoint p in series.Points)
                {
                    p.Label = p.AxisLabel + ": " + ((p.YValues[0] / (double)total) * 100).ToString("F1") + "%";
                    p.Font = new Font("Calibri", 12, FontStyle.Regular);
                }

                chartPaymentStatus.Series.Add(series);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Payment Status Distribution chart: " + ex.Message);
            }
        }


        private void LoadPaymentMethodBreakdown()
        {
            try
            {
                int bank = 0, gcash = 0, cash = 0, others = 0, total = 0;

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();

                    total = Convert.ToInt32(
                        new SqlCommand(@"SELECT COUNT(*) FROM Payment", con)
                        .ExecuteScalar());

                    bank = Convert.ToInt32(
                        new SqlCommand(@"SELECT COUNT(*) FROM Payment 
                                 WHERE paymentMethodID = 
                                 (SELECT paymentMethodID FROM PaymentMethod WHERE methodName='Bank Transfer')", con)
                        .ExecuteScalar());

                    gcash = Convert.ToInt32(
                        new SqlCommand(@"SELECT COUNT(*) FROM Payment 
                                 WHERE paymentMethodID = 
                                 (SELECT paymentMethodID FROM PaymentMethod WHERE methodName='GCash')", con)
                        .ExecuteScalar());

                    cash = Convert.ToInt32(
                        new SqlCommand(@"SELECT COUNT(*) FROM Payment 
                                 WHERE paymentMethodID = 
                                 (SELECT paymentMethodID FROM PaymentMethod WHERE methodName='Cash')", con)
                        .ExecuteScalar());

                    others = total - (bank + gcash + cash);
                }

                pbBankTransfer.Maximum = 100;
                pbGCash.Maximum = 100;
                pbCash.Maximum = 100;
                pbOthers.Maximum = 100;

                pbBankTransfer.Value = total > 0 ? (int)(bank * 100.0 / total) : 0;
                pbGCash.Value = total > 0 ? (int)(gcash * 100.0 / total) : 0;
                pbCash.Value = total > 0 ? (int)(cash * 100.0 / total) : 0;
                pbOthers.Value = total > 0 ? (int)(others * 100.0 / total) : 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payment method breakdown: " + ex.Message);
            }
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

        private void LoadSixMonthsRevenue()
        {
            try
            {
                decimal revenue = GetSixMonthsRevenue();
                CultureInfo philippinesCulture = new CultureInfo("en-PH");
                label7.Text = revenue.ToString("C0", philippinesCulture);
                label7.ForeColor = Color.Green;
            }
            catch
            {
                label7.Text = "Error";
            }
        }

        private void LoadOccupancyRate()
        {
            string occupiedQuery = @"SELECT COUNT(*) FROM Unit WHERE Status = 'Occupied'";
            string totalQuery = @"SELECT COUNT(*) FROM Unit";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();
                    int occupied = Convert.ToInt32(new SqlCommand(occupiedQuery, con).ExecuteScalar());
                    int total = Convert.ToInt32(new SqlCommand(totalQuery, con).ExecuteScalar());

                    double rate = total > 0 ? (double)occupied / total * 100 : 0;
                    label5.Text = $"{rate:F2} %";
                    label5.ForeColor = rate >= 80 ? Color.Green : Color.Orange;
                }
            }
            catch
            {
                label5.Text = "Error";
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

        private void LoadSixMonthsExpenses()
        {
            try
            {
                decimal expenses = GetSixMonthsExpenses();
                label12.Text = $"₱ {expenses:N2}";
                label12.ForeColor = Color.Red;
            }
            catch
            {
                label12.Text = "Error";
            }
        }

        private void LoadSixMonthsNetProfit()
        {
            try
            {
                decimal revenue = GetSixMonthsRevenue();
                decimal expenses = GetSixMonthsExpenses();
                decimal profit = revenue - expenses;

                label15.Text = $"₱ {profit:N2}";
                label15.ForeColor = profit >= 0 ? Color.Green : Color.Red;
            }
            catch
            {
                label15.Text = "Error";
            }
        }

        private void LoadAverageRevenue()
        {
            try
            {
                decimal sixMonthRevenue = GetSixMonthsRevenue();
                decimal avgRevenue = sixMonthRevenue / 6;

                CultureInfo philippinesCulture = new CultureInfo("en-PH");
                label10.Text = $"Average: {avgRevenue.ToString("C0", philippinesCulture)}/month";
                label10.ForeColor = Color.Green;
            }
            catch
            {
                label10.Text = "Average: Error";
            }
        }

        private void LoadAverageExpenses()
        {
            try
            {
                decimal sixMonthExpenses = GetSixMonthsExpenses();
                decimal avgExpenses = sixMonthExpenses / 6;

                label11.Text = $"Average: ₱ {avgExpenses:N0}/month";
                label11.ForeColor = Color.Red;
            }
            catch
            {
                label11.Text = "Average: Error";
            }
        }

        private void LoadProfitMargin()
        {
            try
            {
                decimal revenue = GetSixMonthsRevenue();
                decimal expenses = GetSixMonthsExpenses();
                decimal profit = revenue - expenses;

                double margin = revenue != 0 ? (double)(profit / revenue) * 100 : 0;

                label14.Text = $"Margin: {margin:F2} %";
                label14.ForeColor = margin >= 0 ? Color.Green : Color.Red;
            }
            catch
            {
                label14.Text = "Margin: Error";
            }
        }

        private void LoadTopPerformingProperties()
        {
            try
            {
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    string query = @"
        SELECT 
            U.unitNumber AS UnitNumber,
            ISNULL(SUM(CASE WHEN Pay.paymentStatus = 'Paid' THEN Pay.paymentAmount ELSE 0 END),0) AS Revenue,
            CAST(CASE WHEN COUNT(U.unitID) = 0 THEN 0 
                 ELSE SUM(CASE WHEN U.Status = 'Occupied' THEN 1 ELSE 0 END) * 100.0 / COUNT(U.unitID) END AS DECIMAL(5,2)) AS OccupancyPercent
        FROM Unit U
        LEFT JOIN Payment Pay ON U.UnitID = Pay.UnitID
        GROUP BY U.unitNumber
        ORDER BY Revenue DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.Fill(dt);
                }

                dt.Columns.Add("Rank", typeof(int));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Rank"] = i + 1;
                }
                dt.Columns["Rank"].SetOrdinal(0);

                TopPropertiesGrid.RowTemplate.Height = 35;

                TopPropertiesGrid.DataSource = dt;

                TopPropertiesGrid.Columns["Rank"].HeaderText = "Rank";
                TopPropertiesGrid.Columns["UnitNumber"].HeaderText = "Unit";
                TopPropertiesGrid.Columns["Revenue"].HeaderText = "Revenue";
                TopPropertiesGrid.Columns["OccupancyPercent"].HeaderText = "Occupancy";

                TopPropertiesGrid.Columns["Revenue"].DefaultCellStyle.Format = "C0";
                TopPropertiesGrid.Columns["Revenue"].DefaultCellStyle.FormatProvider = new CultureInfo("en-PH");
                TopPropertiesGrid.Columns["OccupancyPercent"].DefaultCellStyle.Format = "N2";

                TopPropertiesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                TopPropertiesGrid.ReadOnly = true;
                TopPropertiesGrid.RowHeadersVisible = false;
                TopPropertiesGrid.AllowUserToAddRows = false;

                foreach (DataGridViewRow row in TopPropertiesGrid.Rows)
                {
                    int rank = Convert.ToInt32(row.Cells["Rank"].Value);
                    if (rank == 1) row.DefaultCellStyle.BackColor = Color.Gold;
                    else if (rank == 2) row.DefaultCellStyle.BackColor = Color.Silver;
                    else if (rank == 3) row.DefaultCellStyle.BackColor = Color.Peru;
                    else row.DefaultCellStyle.BackColor = Color.White;

                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading top performing properties: " + ex.Message);
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

            plRevenueAnalysis.Visible = false;
            plOccupancyReport.Visible = false;
            plPayementAnalysis.Visible = false;
        }

        private void btnRevenueAnalysis_Click(object sender, EventArgs e)
        {
            plRevenueAnalysis.Visible = true;

            plOverView.Visible = false;
            plOccupancyReport.Visible = false;
            plPayementAnalysis.Visible = false;
        }

        private void btnOccupancyReport_Click(object sender, EventArgs e)
        {
            plOccupancyReport.Visible = true;

            plOverView.Visible = false;
            plRevenueAnalysis.Visible = false;
            plPayementAnalysis.Visible = false;
        }

        private void btnPayementAnalysis_Click(object sender, EventArgs e)
        {
            plPayementAnalysis.Visible = true;

            plOverView.Visible = false;
            plRevenueAnalysis.Visible = false;
            plOccupancyReport.Visible = false;
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