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
using WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.Super_Admin_Account;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords
{
    public partial class Payment_Records : Form
    {
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        private readonly string UserName;
        private readonly string UserRole;
        private readonly CultureInfo philippineCulture = new CultureInfo("en-PH");
        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);
        private DataTable dtCombinedRecords = new DataTable();

        private Label lblNoRecords;

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
            SetupNoRecordsLabel();
            pbProfit.SizeMode = PictureBoxSizeMode.Zoom;
            pbPayment.SizeMode = PictureBoxSizeMode.Zoom;
            pbCalendar.SizeMode = PictureBoxSizeMode.Zoom;
            pbProfit.Image = Properties.Resources.profit;
            pbPayment.Image = Properties.Resources.payment;
            pbCalendar.Image = Properties.Resources.calendar;
            this.UserName = username;
            this.UserRole = userRole;
            this.lbName.Text = $"{UserName} \n ({UserRole})";
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
            btnDashBoard.ForeColor = Color.Black;
            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnContracts.ForeColor = Color.Black;
            btnMaintenance.ForeColor = Color.Black;
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
            PaymentTenantData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            PaymentTenantData.ReadOnly = true;
            PaymentTenantData.AllowUserToAddRows = false;
        }

        private void SetupNoRecordsLabel()
        {
            lblNoRecords = new Label();
            lblNoRecords.Text = "No Payment found based on your filter and search criteria.";
            lblNoRecords.AutoSize = true;
            lblNoRecords.Font = new Font("Calibri", 14, FontStyle.Italic);
            lblNoRecords.ForeColor = Color.Gray; 
            lblNoRecords.Padding = new Padding(20);
            lblNoRecords.Visible = false;

            this.Controls.Add(lblNoRecords);
            lblNoRecords.BringToFront();
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
            string sumPendingQuery = @"
        SELECT ISNULL(SUM(Balance), 0) 
        FROM (
            SELECT 
                U.MonthlyRent - ISNULL(Payments.TotalPaid, 0) AS Balance
            FROM Contract C
            INNER JOIN Unit U ON C.unitID = U.unitID
            LEFT JOIN (
                SELECT contractId, SUM(paymentAmount) AS TotalPaid
                FROM Payment
                GROUP BY contractId
            ) Payments ON C.contractID = Payments.contractId
            WHERE LOWER(C.contractStatus) = 'active'
        ) AS ContractBalances
        WHERE Balance > 0";

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

        public void CalculateMonthlyPayments()
        {
            string countQuery = @"
        SELECT COUNT(paymentId) 
        FROM Payment 
        WHERE MONTH(paymentDate) = MONTH(GETDATE()) 
          AND YEAR(paymentDate) = YEAR(GETDATE())";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            using (SqlCommand command = new SqlCommand(countQuery, connection))
            {
                try
                {
                    connection.Open();
                    int totalPayments = (int)command.ExecuteScalar();

                    lbThisMonth.Text = totalPayments.ToString();
                }
                catch (Exception ex)
                {
                    lbThisMonth.Text = "0";
                    MessageBox.Show($"Error counting monthly payments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                cbMethod.Items.Add("All Payment");
                cbMethod.Items.Add("Cash");
                cbMethod.Items.Add("Online Payment");
                cbMethod.Items.Add("Bank Account");
                cbMethod.SelectedIndex = 0;
            }
        }

        private void Payment_Records_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnPaymentRec, activeColor);

            lblNoRecords.Location = new Point(
                PaymentTenantData.Location.X + (PaymentTenantData.Width - lblNoRecords.Width) / 2,
                PaymentTenantData.Location.Y + (PaymentTenantData.Height - lblNoRecords.Height) / 2
            );

            GetPaymentData();
            CalculateTotalCollected();
            CalculateTotalPendingPayments();
            CalculateMonthlyPayments();
        }

        public void GetPaymentData()
        {
            string consolidatedQuery = @"
        SELECT 
    C.contractID,
    TI.firstName + ' ' + TI.lastName AS Tenant,
    ISNULL(PR.propertyName, 'N/A') AS Property,
    ISNULL(P.paymentAmount, 0) AS Amount,
    R.dueDate AS DueDate,
    P.paymentDate AS PaymentDate,
    ISNULL(PT.typeName, 'N/A') AS TypeName,
    CASE
        WHEN P.paymentId IS NOT NULL THEN 'Paid'
        WHEN R.dueDate < CAST(GETDATE() AS DATE) THEN 'Overdue'
        ELSE 'Pending'
    END AS Status,
    P.RowVersion
FROM Contract C
INNER JOIN PersonalInformation TI ON C.tenantId = TI.tenantId
LEFT JOIN Property PR ON C.propertyID = PR.propertyID
LEFT JOIN Unit U ON C.unitID = U.unitID
LEFT JOIN Rent R ON C.contractID = R.contractID

-- ✅ CRITICAL FIX
LEFT JOIN Payment P
    ON C.contractID = P.contractId
    AND P.paymentDate <= R.dueDate

LEFT JOIN PaymentType PT ON P.paymentTypeID = PT.paymentTypeID
WHERE LOWER(C.contractStatus) = 'active'
ORDER BY R.dueDate DESC;";

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
                    ApplyFilters();
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
                PT.typeName AS PaymentMethod
            FROM Contract C
            INNER JOIN PersonalInformation TI ON C.tenantId = TI.tenantId
            INNER JOIN Property PR ON C.propertyID = PR.propertyID
            INNER JOIN Unit U ON C.unitID = U.unitID 
            LEFT JOIN Rent R ON C.contractID = R.contractID
            LEFT JOIN Payment P ON C.contractID = P.contractId
            LEFT JOIN PaymentType PT ON P.paymentTypeID = PT.paymentTypeID
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

        public Image ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(image, new Size(width, height));
            return resizedImage;
        }

        private void AddActionButtons()
        {
            if (PaymentTenantData.Columns.Contains("View")) PaymentTenantData.Columns.Remove("View");
            if (PaymentTenantData.Columns.Contains("Download")) PaymentTenantData.Columns.Remove("Download");
            if (PaymentTenantData.Columns.Contains("Update")) PaymentTenantData.Columns.Remove("Update");
            if (PaymentTenantData.Columns.Contains("Delete")) PaymentTenantData.Columns.Remove("Delete");

            int iconSize = 16;

            PaymentTenantData.Columns.Add(new DataGridViewImageColumn
            {
                Name = "View",
                HeaderText = "",
                Image = ResizeImage(Properties.Resources.view, iconSize, iconSize),
                Width = 40,
                ToolTipText = "View Payment History"
            });

            PaymentTenantData.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Download",
                HeaderText = "",
                Image = ResizeImage(Properties.Resources.downlaod, iconSize, iconSize),
                Width = 40,
                ToolTipText = "Download SOA"
            });

            PaymentTenantData.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Update",
                HeaderText = "",
                Image = ResizeImage(Properties.Resources.edit, iconSize, iconSize),
                Width = 40,
                ToolTipText = "Update Payment Record"
            });

            if (UserRole == "SuperAdmin")
            {
                PaymentTenantData.Columns.Add(new DataGridViewImageColumn
                {
                    Name = "Delete",
                    HeaderText = "",
                    Image = ResizeImage(Properties.Resources.delete, iconSize, iconSize),
                    Width = 40,
                    ToolTipText = "Delete Payment Record"
                });
            }
        }

        private void FormatDataGridColumns()
        {
            foreach (DataGridViewColumn column in PaymentTenantData.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if (PaymentTenantData.Columns.Contains("Tenant")) PaymentTenantData.Columns["Tenant"].Width = 150;
            if (PaymentTenantData.Columns.Contains("Property")) PaymentTenantData.Columns["Property"].Width = 150;
            if (PaymentTenantData.Columns.Contains("Amount"))
            {
                PaymentTenantData.Columns["Amount"].DefaultCellStyle.Format = "C";
                PaymentTenantData.Columns["Amount"].DefaultCellStyle.FormatProvider = philippineCulture;
                PaymentTenantData.Columns["Amount"].Width = 150;
            }
            if (PaymentTenantData.Columns.Contains("DueDate")) PaymentTenantData.Columns["DueDate"].Width = 150;
            if (PaymentTenantData.Columns.Contains("PaymentDate")) PaymentTenantData.Columns["PaymentDate"].Width = 150;
            if (PaymentTenantData.Columns.Contains("TypeName"))
            {
                PaymentTenantData.Columns["TypeName"].Width = 130;
                PaymentTenantData.Columns["TypeName"].HeaderText = "Payment Type";
            }
            if (PaymentTenantData.Columns.Contains("Status")) PaymentTenantData.Columns["Status"].Width = 100;
            if (PaymentTenantData.Columns.Contains("contractID")) PaymentTenantData.Columns["contractID"].Visible = false;
            if (PaymentTenantData.Columns.Contains("RowVersion")) PaymentTenantData.Columns["RowVersion"].Visible = false;
        }

        private void ShowPaymentHistory(int contractId, string tenantName)
        {
            ViewPaymentHistory historyForm = new ViewPaymentHistory(DataConnection, contractId, tenantName);
            historyForm.ShowDialog();
        }

        private void ApplyFilters()
        {
            if (dtCombinedRecords == null || dtCombinedRecords.DefaultView == null) return;

            string statusFilter = cbStatus.SelectedItem?.ToString() ?? "All Statuses";
            string methodFilter = cbMethod.SelectedItem?.ToString() ?? "All Payment";
            string searchFilter = tbSearch.Text.Trim();

            List<string> filters = new List<string>();

            if (statusFilter != "All Statuses")
                filters.Add($"Status = '{statusFilter}'");

            if (methodFilter != "All Payment")
                filters.Add($"TypeName = '{methodFilter}'");

            if (!string.IsNullOrEmpty(searchFilter))
            {
                filters.Add($"(Tenant LIKE '%{searchFilter}%' OR Property LIKE '%{searchFilter}%')");
            }

            dtCombinedRecords.DefaultView.RowFilter = string.Join(" AND ", filters);
                
            if (dtCombinedRecords.DefaultView.Count == 0)
            {
                lblNoRecords.Location = new Point(
                    PaymentTenantData.Location.X + (PaymentTenantData.Width - lblNoRecords.Width) / 2,
                    PaymentTenantData.Location.Y + (PaymentTenantData.Height - lblNoRecords.Height) / 2
                );

                lblNoRecords.Visible = true;
                PaymentTenantData.Visible = false;
            }
            else
            {
                lblNoRecords.Visible = false;
                PaymentTenantData.Visible = true;
            }
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

        private async void PaymentTenantData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow row = PaymentTenantData.Rows[e.RowIndex];
            string columnName = PaymentTenantData.Columns[e.ColumnIndex].Name;
            int contractId = Convert.ToInt32(row.Cells["contractID"].Value);
            byte[] rowVersion = row.Cells["RowVersion"].Value as byte[];

            switch (columnName)
            {
                case "Update":
                    using (AddPayment editForm = new AddPayment(contractId))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            decimal updatedAmount = editForm.NewAmount;
                            await ExecuteUpdateTransaction(contractId, updatedAmount, rowVersion);
                        }
                    }
                    break;

                case "View":
                    new ViewPaymentHistory(DataConnection, contractId, row.Cells["Tenant"].Value.ToString()).ShowDialog();
                    break;

                case "Download":
                    DataTable soa = GetStatementOfAccountData(contractId);
                    if (soa.Rows.Count > 0) ExportDataToExcel(soa, row.Cells["Tenant"].Value.ToString(), row.Cells["Property"].Value.ToString());
                    break;
            }
        }

        private async Task ExecuteUpdateTransaction(int contractId, decimal amount, byte[] originalVersion)
        {

            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                await conn.OpenAsync();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    await Task.Delay(2000);

                    string updateSql = @"UPDATE Payment SET paymentAmount = @amount 
                                        WHERE contractId = @cid AND RowVersion = @rv";

                    using (SqlCommand cmd = new SqlCommand(updateSql, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@cid", contractId);
                        cmd.Parameters.AddWithValue("@rv", originalVersion);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            MessageBox.Show("Update successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            transaction.Rollback();
                            MessageBox.Show("Conflict Detected: May nag-edit na ng record na ito. I-refresh ang listahan.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Transaction Failed: " + ex.Message);
                }
            }
            RefreshDashboardData();
        }

        private void DeletePaymentRecord(int contractId)
        {
            string deleteQuery = "DELETE FROM Payment WHERE contractId = @contractId";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            {
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@contractId", contractId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record successfully deleted from the database.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No record was found to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Database Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RefreshDashboardData()
        {
            GetPaymentData();
            CalculateTotalCollected();
            CalculateTotalPendingPayments();
            CalculateMonthlyPayments();
        }

        private void btnRecordPayment_Click(object sender, EventArgs e)
        {
            using (AddPayment addPaymentForm = new AddPayment())
            {
                if (addPaymentForm.ShowDialog() == DialogResult.OK)
                {
                    GetPaymentData();
                    CalculateTotalCollected();
                    CalculateTotalPendingPayments();
                    CalculateMonthlyPayments();
                }
            }
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

        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard dashboard = new DashBoard(UserName, UserRole);
            dashboard.Show();
            this.Hide();
        }

        private void btnTenant_Click(object sender, EventArgs e)
        {
            Type tenantsType = Type.GetType("WindowsFormsApp1.DashBoard1.SuperAdmin_Tenants.Tenants");
            if (tenantsType != null)
            {
                Form tenants = (Form)Activator.CreateInstance(tenantsType, UserName, UserRole);
                tenants.Show();
                this.Hide();
            }
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            ProperTies properties = new ProperTies(UserName, UserRole);
            properties.Show();
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
            Type maintenanceType = Type.GetType("WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Maintenance.Maintenance");
            if (maintenanceType != null)
            {
                Form maintenance = (Form)Activator.CreateInstance(maintenanceType, UserName, UserRole);
                maintenance.Show();
                this.Hide();
            }
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
                MessageBox.Show("Access Denied: Admin cannot view full reports.");
            }
        }

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