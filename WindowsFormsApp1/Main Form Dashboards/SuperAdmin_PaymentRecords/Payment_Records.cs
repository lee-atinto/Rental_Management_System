using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Super_Admin_Account;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords
{
    public partial class Payment_Records : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private readonly string Username;
        private readonly string UserRole;

        private readonly Color activeColor = Color.FromArgb(46, 51, 73);
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

            pbProfit.SizeMode = PictureBoxSizeMode.Zoom;
            pbPayment.SizeMode = PictureBoxSizeMode.Zoom;
            pbCalendar.SizeMode = PictureBoxSizeMode.Zoom;

            pbProfit.Image = Properties.Resources.profit;
            pbPayment.Image = Properties.Resources.payment;
            pbCalendar.Image = Properties.Resources.calendar;

            this.Username = username;
            this.UserRole = userRole;
            this.lbName.Text = $"{Username} \n ({UserRole})";

            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);
            InitializeButtonStyle(btnProperties);

            panelHeader.BackColor = Color.White;
            PanelBackGroundProfile.BackColor = Color.FromArgb(46, 51, 73);

            btnDashBoard.Padding = new Padding(30, 0, 0, 0);
            btnAdminAcc.Padding = new Padding(30, 0, 0, 0);
            btnTenant.Padding = new Padding(30, 0, 0, 0);
            btnPaymentRec.Padding = new Padding(30, 0, 0, 0);
            btnViewReport.Padding = new Padding(30, 0, 0, 0);
            btnBackUp.Padding = new Padding(30, 0, 0, 0);
            btnProperties.Padding = new Padding(30, 0, 0, 0);

            btnDashBoard.ForeColor = Color.Black;
            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;

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
            CultureInfo philippineCulture = new CultureInfo("en-PH");

            using (SqlConnection connection = new SqlConnection(DataConnection))
            {
                using (SqlCommand command = new SqlCommand(sumQuery, connection))
                {
                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        decimal totalCollected = 0m;

                        if (result != null && result != DBNull.Value)
                        {
                            totalCollected = Convert.ToDecimal(result);
                        }

                        lbTotalCollected.Text = totalCollected.ToString("C", philippineCulture);
                    }

                    catch (SqlException ex)
                    {
                        lbTotalCollected.Text = "DB Error";
                        MessageBox.Show($"Database Error calculating total: {ex.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    catch (Exception ex)
                    {
                        lbTotalCollected.Text = "Error";
                        MessageBox.Show($"General Error calculating total: {ex.Message}", "General Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void CalculateTotalPendingPayments()
        {
            string sumPendingQuery = @" WITH CalculatedBalance AS ( SELECT R.contractID, R.dueDate, R.rentAmount - ISNULL(SUM(PY.paymentAmount), 0) AS CurrentBalance FROM Rent R
                                        LEFT JOIN Payment PY ON R.contractID = PY.contractId GROUP BY R.contractID, R.rentAmount, R.dueDate) SELECT ISNULL(SUM(CB.CurrentBalance), 0)
                                        FROM CalculatedBalance CB WHERE CB.CurrentBalance > 0 AND CB.dueDate >= GETDATE();";
            using (SqlConnection connection = new SqlConnection(DataConnection))
            {
                using (SqlCommand command = new SqlCommand(sumPendingQuery, connection))
                {
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        decimal totalPending = 0m;

                        if (result != null && result != DBNull.Value)
                        {
                            totalPending = Convert.ToDecimal(result);
                        }

                        CultureInfo philippineCulture = new CultureInfo("en-PH");
                        lbPendingPayments.Text = totalPending.ToString("C", philippineCulture);
                    }
                    catch (SqlException ex)
                    {
                        lbPendingPayments.Text = "DB Error";
                        MessageBox.Show($"Database Error calculating pending total: {ex.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        lbPendingPayments.Text = "Error";
                        MessageBox.Show($"General Error calculating pending total: {ex.Message}", "General Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
                cbMethod.Items.Add("All Methods");
                cbMethod.Items.Add("Bank Transfer");
                cbMethod.Items.Add("Credit Card");
                cbMethod.Items.Add("Debit Card");
                cbMethod.Items.Add("Over-the-Counter");
                cbMethod.Items.Add("Online Portal");
                cbMethod.Items.Add("PayPal");
                cbMethod.Items.Add("Check");
                cbMethod.Items.Add("Maya");
                cbMethod.Items.Add("GCash");
                cbMethod.Items.Add("Cash");

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
            string combinedQuery = @" WITH CalculatedBalance AS ( SELECT R.contractID, R.rentAmount, R.dueDate, ISNULL(SUM(PY.paymentAmount), 0) AS TotalPaid, R.rentAmount - ISNULL(SUM(PY.paymentAmount), 0) AS CurrentBalance FROM Rent R
                                   LEFT JOIN Payment PY ON R.contractID = PY.contractId GROUP BY R.contractID, R.rentAmount, R.dueDate) SELECT TI.firstName + ' ' + TI.lastName AS Tenant, PR.propertyName AS Property, P.paymentAmount AS Amount, CB.dueDate AS DueDate, P.paymentDate AS PaymentDate, M.methodName AS DateMethod,
                                   CASE WHEN CB.CurrentBalance <= 0 THEN 'Paid' WHEN CB.CurrentBalance > 0 AND CB.dueDate < GETDATE() THEN 'Overdue' ELSE 'Pending' END AS Status 
                                   FROM Payment P INNER JOIN PaymentMethod M ON P.paymentMethodId = M.paymentMethodId INNER JOIN PaymentType T ON P.paymentTypeId = T.paymentTypeId INNER JOIN PersonalInformation TI ON P.tenantId = TI.tenantId
                                   INNER JOIN Rent R_Details ON P.contractId = R_Details.contractID INNER JOIN Property PR ON R_Details.propertyID = PR.propertyID INNER JOIN CalculatedBalance CB ON P.contractId = CB.contractID
                                   ORDER BY P.paymentDate DESC";
            using (SqlConnection connection = new SqlConnection(DataConnection))
            {
                using (SqlCommand command = new SqlCommand(combinedQuery, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        try
                        {
                            connection.Open();
                            dtCombinedRecords.Clear();
                            adapter.Fill(dtCombinedRecords);

                            PaymentTenantData.Columns.Clear();
                            PaymentTenantData.DataSource = dtCombinedRecords.DefaultView;

                            CultureInfo philippineCulture = new CultureInfo("en-PH");

                            if (PaymentTenantData.Columns.Contains("Amount"))
                            {
                                PaymentTenantData.Columns["Amount"].DefaultCellStyle.Format = "C";
                                PaymentTenantData.Columns["Amount"].DefaultCellStyle.FormatProvider = philippineCulture;
                            }

                            DataGridViewImageColumn viewBtn = new DataGridViewImageColumn();
                            viewBtn.HeaderText = "View";
                            viewBtn.Image = Properties.Resources.view;
                            viewBtn.ImageLayout = DataGridViewImageCellLayout.Normal;
                            PaymentTenantData.Columns.Add(viewBtn);

                            DataGridViewImageColumn downloadBtn = new DataGridViewImageColumn();
                            downloadBtn.HeaderText = "Download";
                            downloadBtn.Image = Properties.Resources.downlaod;
                            downloadBtn.ImageLayout = DataGridViewImageCellLayout.Normal;
                            PaymentTenantData.Columns.Add(downloadBtn);

                            PaymentTenantData.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show($"Database Error: {ex.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "General Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            ComboBoxStatus();
            ComboBoxMethod();
        }

        private void PaymentTenantData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStatus = cbStatus.SelectedItem.ToString();

            if (selectedStatus == "All Statuses")
            {
                dtCombinedRecords.DefaultView.RowFilter = string.Empty;
            }

            else
            {
                dtCombinedRecords.DefaultView.RowFilter = string.Format("Status = '{0}'", selectedStatus);
            }
        }

        private void cbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedMethod = cbMethod.SelectedItem.ToString();

            if (dtCombinedRecords == null || dtCombinedRecords.DefaultView == null)
            {
                return;
            }

            if (selectedMethod == "All Methods")
            {
                dtCombinedRecords.DefaultView.RowFilter = string.Empty;
            }

            else
            {
                dtCombinedRecords.DefaultView.RowFilter = string.Format("DateMethod = '{0}'", selectedMethod);
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (dtCombinedRecords == null || dtCombinedRecords.DefaultView == null)
            {
                return;
            }

            string searchText = tbSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                dtCombinedRecords.DefaultView.RowFilter = string.Empty;
            }
            else
            {
                string filter = string.Format("Tenant LIKE '%{0}%'", searchText);

                dtCombinedRecords.DefaultView.RowFilter = filter;
            }
        }

        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard dashBoard = new DashBoard(Username, UserRole);
            dashBoard.Show();
            this.Hide();
        }

        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts admin_AdminAccounts = new SuperAdmin_AdminAccounts(Username, UserRole);
            admin_AdminAccounts.Show();
            this.Hide();
        }

        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants tenant = new Tenants(Username, UserRole);
            tenant.Show();
            this.Hide();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            ProperTies properties = new ProperTies(Username, UserRole);
            properties.Show();
            this.Hide();
        }
    }
}