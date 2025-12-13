using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.Super_Admin_Account;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords
{
    public partial class Payment_Records : Form
    {
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        private readonly string UserName;
        private readonly string UserRole;
        private readonly CultureInfo philippineCulture = new CultureInfo("en-PH");

        // -------------------- Button Style -------------------- //
        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        private DataTable dtCombinedRecords = new DataTable();

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

        public Payment_Records(string username, string userRole)
        {
            InitializeComponent();

            // <<< Re-add the DataGridView CellContentClick event hookup that must not be removed >>>
            this.PaymentTenantData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PaymentTenantData_CellContentClick);

            // -------------------- PictureBox Setup -------------------- //
            pbProfit.SizeMode = PictureBoxSizeMode.Zoom;
            pbPayment.SizeMode = PictureBoxSizeMode.Zoom;
            pbCalendar.SizeMode = PictureBoxSizeMode.Zoom;

            pbProfit.Image = Properties.Resources.profit;
            pbPayment.Image = Properties.Resources.payment;
            pbCalendar.Image = Properties.Resources.calendar;

            this.UserName = username;
            this.UserRole = userRole;
            this.lbName.Text = $"{UserName} \n ({UserRole})";

            // -------------------- Button Initialization -------------------- //
            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);
            InitializeButtonStyle(btnProperties);
            InitializeButtonStyle(btnContracts);
            InitializeButtonStyle(btnMaintenance);

            // -------------------- UI Styling -------------------- //
            panelHeader.BackColor = Color.White;
            lbName.BackColor = Color.FromArgb(46, 51, 73);
            PicUserProfile.Image = Properties.Resources.profile;
            PicUserProfile.BackColor = Color.FromArgb(46, 51, 73);
            SideBarBakground.BackColor = Color.FromArgb(46, 51, 73);

            // -------------------- Set Color Unactive Button -------------------- //
            btnDashBoard.ForeColor = Color.Black;
            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnContracts.ForeColor = Color.Black;
            btnMaintenance.ForeColor = Color.Black;

            // -------------------- DataGridView Styling -------------------- //
            PaymentTenantData.BorderStyle = BorderStyle.None;
            PaymentTenantData.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            PaymentTenantData.RowHeadersVisible = false;
            PaymentTenantData.EnableHeadersVisualStyles = false;
            PaymentTenantData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            Color defaultBG = Color.FromArgb(240, 240, 240);
            PaymentTenantData.BackgroundColor = defaultBG;
            PaymentTenantData.DefaultCellStyle.BackColor = Color.White;
            PaymentTenantData.AlternatingRowsDefaultCellStyle.BackColor = defaultBG;
            PaymentTenantData.RowTemplate.Height = 60;
            PaymentTenantData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            PaymentTenantData.ReadOnly = true;
            PaymentTenantData.AllowUserToAddRows = false;
        }

        public void CalculateTotalCollected()
        {
            string sumQuery = "SELECT ISNULL(SUM(paymentAmount), 0) FROM Payment";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            using (SqlCommand command = new SqlCommand(sumQuery, connection))
            {
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    decimal totalCollected = (result != null && result != DBNull.Value) ? Convert.ToDecimal(result) : 0m;
                    lbTotalCollected.Text = totalCollected.ToString("C", philippineCulture);
                }
                catch (Exception ex)
                {
                    lbTotalCollected.Text = "Error";
                    MessageBox.Show($"Error calculating total: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void CalculateTotalPendingPayments()
        {
            string sumPendingQuery = @"; WITH CalculatedBalance AS ( 
                SELECT 
                    R.contractID, 
                    R.dueDate, 
                    U.MonthlyRent - ISNULL(SUM(PY.paymentAmount), 0) AS CurrentBalance 
                FROM Rent R 
                INNER JOIN Contract C ON R.contractID = C.contractID 
                INNER JOIN Unit U ON C.propertyID = U.propertyID 
                LEFT JOIN Payment PY ON R.contractID = PY.contractId 
                GROUP BY R.contractID, U.MonthlyRent, R.dueDate 
            ) 
            SELECT ISNULL(SUM(CB.CurrentBalance), 0) 
            FROM CalculatedBalance CB 
            WHERE CB.CurrentBalance > 0;";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            using (SqlCommand command = new SqlCommand(sumPendingQuery, connection))
            {
                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    decimal totalPending = (result != null && result != DBNull.Value) ? Convert.ToDecimal(result) : 0m;
                    lbPendingPayments.Text = totalPending.ToString("C", philippineCulture);
                }
                catch (Exception ex)
                {
                    lbPendingPayments.Text = "Error";
                    MessageBox.Show($"Error calculating pending total: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ComboBoxStatus()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("All Statuses");
            cbStatus.Items.Add("Paid");
            cbStatus.Items.Add("Pending");
            cbStatus.Items.Add("Overdue");
            cbStatus.SelectedIndex = 0;
        }

        private void ComboBoxMethod()
        {
            if (cbMethod != null)
            {
                cbMethod.Items.Clear();
                var methods = new[] { "All Methods", "Bank Transfer", "Credit Card", "Debit Card", "Over-the-Counter",
                                      "Online Portal", "PayPal", "Check", "Maya", "GCash", "Cash" };
                cbMethod.Items.AddRange(methods);
                cbMethod.SelectedIndex = 0;
            }
        }

        private void Payment_Records_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnPaymentRec, activeColor);
            GetPaymentData();
            CalculateTotalCollected();
            CalculateTotalPendingPayments();
        }

        public void GetPaymentData()
        {
            string consolidatedQuery = @" WITH TotalCalculatedBalance AS ( 
                SELECT 
                    C.contractID,
                    TI.firstName + ' ' + TI.lastName AS Tenant, 
                    PR.propertyName AS Property, 
                    U.MonthlyRent, 
                    MAX(R.dueDate) AS LatestDueDate,
                    MAX(P.paymentDate) AS LastPaymentDate, 
                    ISNULL(SUM(P.paymentAmount), 0) AS TotalPaid, 
                    U.MonthlyRent - ISNULL(SUM(P.paymentAmount), 0) AS CurrentBalance 
                FROM Contract C 
                INNER JOIN PersonalInformation TI ON C.tenantId = TI.tenantId
                INNER JOIN Property PR ON C.propertyID = PR.propertyID 
                INNER JOIN Unit U ON C.propertyID = U.propertyID 
                INNER JOIN Rent R ON C.contractID = R.contractID
                LEFT JOIN Payment P ON C.contractID = P.contractId
                GROUP BY C.contractID, TI.firstName, TI.lastName, PR.propertyName, U.MonthlyRent
            ) 
            SELECT 
                contractID,
                Tenant, 
                Property, 
                TotalPaid AS Amount, 
                LatestDueDate AS DueDate, 
                LastPaymentDate AS PaymentDate, 
                CASE 
                    WHEN LastPaymentDate IS NULL THEN 'N/A' 
                    ELSE 'Multiple'
                END AS DateMethod,
                CASE 
                    WHEN CurrentBalance <= 0 THEN 'Paid' 
                    WHEN CurrentBalance > 0 AND LatestDueDate < GETDATE() THEN 'Overdue' 
                    ELSE 'Pending' 
                END AS Status 
            FROM TotalCalculatedBalance
            ORDER BY Tenant DESC";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            using (SqlCommand command = new SqlCommand(consolidatedQuery, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                try
                {
                    connection.Open();
                    dtCombinedRecords.Clear();
                    PaymentTenantData.DataSource = null;
                    PaymentTenantData.Columns.Clear();

                    adapter.Fill(dtCombinedRecords);
                    PaymentTenantData.DataSource = dtCombinedRecords.DefaultView;

                    FormatDataGridColumns();
                    AddActionButtons();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error fetching payment data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            ComboBoxStatus();
            ComboBoxMethod();
        }

        private DataTable GetStatementOfAccountData(int contractId)
        {
            string query = @"
    SELECT 
        TI.firstName + ' ' + TI.lastName AS TenantName, 
        PR.propertyName AS PropertyName,
        U.unitNumber AS UnitNumber,
        U.MonthlyRent,
        R.dueDate AS DueDate,
        P.paymentDate AS DatePaid,
        P.paymentAmount AS AmountPaid,
        M.methodName AS PaymentMethod
    FROM Contract C
    INNER JOIN PersonalInformation TI ON C.tenantId = TI.tenantId
    INNER JOIN Property PR ON C.propertyID = PR.propertyID
    INNER JOIN Unit U ON C.propertyID = U.propertyID   -- FIXED HERE
    LEFT JOIN Rent R ON C.contractID = R.contractID
    LEFT JOIN Payment P ON C.contractID = P.contractId
    LEFT JOIN PaymentMethod M ON P.paymentMethodId = M.paymentMethodId
    WHERE C.contractID = @ContractId
    ORDER BY P.paymentDate ASC, R.dueDate ASC";


            DataTable dtSOA = new DataTable();

            using (SqlConnection connection = new SqlConnection(DataConnection))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@ContractId", contractId);

                try
                {
                    connection.Open();
                    adapter.Fill(dtSOA);
                    return dtSOA;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving SOA data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }

        private void FormatDataGridColumns()
        {
            if (PaymentTenantData.Columns.Contains("Amount"))
            {
                PaymentTenantData.Columns["Amount"].DefaultCellStyle.Format = "C";
                PaymentTenantData.Columns["Amount"].DefaultCellStyle.FormatProvider = philippineCulture;
            }

            if (PaymentTenantData.Columns.Contains("contractID"))
            {
                PaymentTenantData.Columns["contractID"].Visible = false;
            }
        }

        private void ShowPaymentHistory(int contractId, string tenantName)
        {
            ViewPaymentHistory historyForm = new ViewPaymentHistory(DataConnection, contractId, tenantName);
            historyForm.ShowDialog();
        }

        private void AddActionButtons()
        {
            if (!PaymentTenantData.Columns.Contains("ViewColumn"))
            {
                DataGridViewImageColumn viewBtn = new DataGridViewImageColumn
                {
                    Name = "ViewColumn",
                    HeaderText = "View",
                    Image = Properties.Resources.view,
                    ImageLayout = DataGridViewImageCellLayout.Normal
                };
                PaymentTenantData.Columns.Add(viewBtn);
            }

            if (!PaymentTenantData.Columns.Contains("DownloadColumn"))
            {
                DataGridViewImageColumn downloadBtn = new DataGridViewImageColumn
                {
                    Name = "DownloadColumn",
                    HeaderText = "Download",
                    Image = Properties.Resources.downlaod,
                    ImageLayout = DataGridViewImageCellLayout.Normal
                };
                PaymentTenantData.Columns.Add(downloadBtn);
            }

            PaymentTenantData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void ApplyFilters()
        {
            if (dtCombinedRecords == null || dtCombinedRecords.DefaultView == null) return;

            string statusFilter = cbStatus.SelectedItem?.ToString() ?? "All Statuses";
            string methodFilter = cbMethod.SelectedItem?.ToString() ?? "All Methods";
            string searchFilter = tbSearch.Text.Trim();
            string rowFilter = "";

            if (statusFilter != "All Statuses") rowFilter += $"Status = '{statusFilter}'";
            if (methodFilter != "All Methods") rowFilter += (string.IsNullOrEmpty(rowFilter) ? "" : " AND ") + $"DateMethod = '{methodFilter}'";
            if (!string.IsNullOrEmpty(searchFilter))
            {
                string searchClause = $"Tenant LIKE '%{searchFilter}%' OR Property LIKE '%{searchFilter}%'";
                rowFilter += (string.IsNullOrEmpty(rowFilter) ? "" : " AND ") + $"({searchClause})";
            }

            dtCombinedRecords.DefaultView.RowFilter = rowFilter;
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void cbMethod_SelectedIndexChanged(object sender, EventArgs e) => ApplyFilters();
        private void tbSearch_TextChanged(object sender, EventArgs e) => ApplyFilters();

        private void ExportDataToExcel(DataTable data, string tenantName, string propertyName)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV File (*.csv)|*.csv|Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = $"SOA_{tenantName.Replace(" ", "_")}_{propertyName.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.csv";
                sfd.Title = "I-save ang Statement of Account";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (sfd.FilterIndex == 1)
                    {
                        try
                        {
                            StringBuilder sb = new StringBuilder();

                            IEnumerable<string> columnNames = data.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                            sb.AppendLine(string.Join(",", columnNames));

                            foreach (DataRow row in data.Rows)
                            {
                                IEnumerable<string> fields = row.ItemArray.Select(field =>
                                    string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                                sb.AppendLine(string.Join(",", fields));
                            }

                            File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);

                            MessageBox.Show($"SOA for {tenantName} successfully generated to: {sfd.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error saving file: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Para sa Excel (.xlsx) format, kailangan mong mag-install ng library tulad ng EPPlus.", "Export Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void PaymentTenantData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = PaymentTenantData.Rows[e.RowIndex];

            bool columnExists = PaymentTenantData.Columns.Contains("contractID");

            if (!columnExists || row.Cells["contractID"].Value == DBNull.Value)
            {
                MessageBox.Show("Contract ID not found for this row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int contractId = Convert.ToInt32(row.Cells["contractID"].Value);
            string tenantName = row.Cells["Tenant"].Value?.ToString();
            string propertyName = row.Cells["Property"].Value?.ToString();

            if (PaymentTenantData.Columns[e.ColumnIndex].Name == "ViewColumn")
            {
                ShowPaymentHistory(contractId, tenantName);
            }
            else if (PaymentTenantData.Columns[e.ColumnIndex].Name == "DownloadColumn")
            {
                DataTable soaData = GetStatementOfAccountData(contractId);

                if (soaData != null && soaData.Rows.Count > 0)
                {
                    ExportDataToExcel(soaData, tenantName, propertyName);
                }
                else
                {
                    MessageBox.Show("Walang mahanap na payment record para sa contract na ito.", "Walang Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnRecordPayment_Click(object sender, EventArgs e)
        {
            AddPayment addPaymentForm = new AddPayment();

            if (addPaymentForm.ShowDialog() == DialogResult.OK)
            {
                GetPaymentData();
                CalculateTotalCollected();
                CalculateTotalPendingPayments();
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

        // --------------- Admin Account Button --------------- //
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
