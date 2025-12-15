using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_PaymentRecords
{
    public partial class ViewPaymentHistory : Form
    {
        private readonly string DataConnection;
        private readonly int ContractID;
        private readonly CultureInfo philippineCulture = new CultureInfo("en-PH");

        public ViewPaymentHistory(string connectionString, int contractId, string tenantName)
        {
            InitializeComponent();
            this.DataConnection = connectionString;
            this.ContractID = contractId;
            this.Text = $"Payment History: {tenantName}";

            InitializeDataGridViewStyle();
        }

        private void InitializeDataGridViewStyle()
        {
            if (dataGridViewHistory != null)
            {
                dataGridViewHistory.BorderStyle = BorderStyle.None;
                dataGridViewHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dataGridViewHistory.RowHeadersVisible = false;
                dataGridViewHistory.EnableHeadersVisualStyles = false;
                dataGridViewHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dataGridViewHistory.RowTemplate.Height = 35;
                dataGridViewHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewHistory.ReadOnly = true;
                dataGridViewHistory.AllowUserToAddRows = false;
            }
        }

        private void ViewPaymentHistory_Load(object sender, EventArgs e)
        {
            LoadSummary();
            LoadPaymentDetails();
        }

        private void LoadSummary()
        {
            string summaryQuery = @"
                ;WITH LatestPayment AS (
                    SELECT TOP 1 
                        paymentAmount 
                    FROM Payment 
                    WHERE contractId = @ContractId 
                    ORDER BY paymentDate DESC
                )
                SELECT 
                    C.startDate,
                    MAX(R.dueDate) AS LatestDueDate,
                    -- Total Paid: Sum ng lahat ng paymentAmount
                    ISNULL((SELECT SUM(paymentAmount) FROM Payment WHERE contractId = @ContractId), 0) AS TotalPaid,
                    -- Months Paid: Count ng payments
                    ISNULL((SELECT COUNT(paymentId) FROM Payment WHERE contractId = @ContractId), 0) AS MonthsPaid,
                    -- Latest Payment Amount (para sa label1)
                    ISNULL((SELECT paymentAmount FROM LatestPayment), 0) AS LatestPaymentAmount
                FROM Contract C
                INNER JOIN Rent R ON C.contractID = R.contractID
                WHERE C.contractID = @ContractId
                GROUP BY C.startDate;
            ";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            using (SqlCommand command = new SqlCommand(summaryQuery, connection))
            {
                command.Parameters.AddWithValue("@ContractId", ContractID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            decimal latestPaymentAmount = reader.GetDecimal(reader.GetOrdinal("LatestPaymentAmount"));
                            DateTime startDate = reader.GetDateTime(reader.GetOrdinal("startDate"));
                            DateTime latestDueDate = reader.GetDateTime(reader.GetOrdinal("LatestDueDate"));
                            decimal totalPaid = reader.GetDecimal(reader.GetOrdinal("TotalPaid"));
                            int monthsPaid = reader.GetInt32(reader.GetOrdinal("MonthsPaid"));

                            if (label1 != null)
                                label1.Text = latestPaymentAmount.ToString("C", philippineCulture);

                            if (label2 != null)
                                label2.Text = monthsPaid.ToString();

                            if (label3 != null)
                                label3.Text = totalPaid.ToString("C", philippineCulture);

                            if (label4 != null)
                                label4.Text = startDate.ToShortDateString();

                            if (label5 != null)
                                label5.Text = latestDueDate.ToShortDateString();
                        }
                        else
                        {
                            if (label1 != null) label1.Text = (0m).ToString("C", philippineCulture);
                            if (label2 != null) label2.Text = "0";
                            if (label3 != null) label3.Text = (0m).ToString("C", philippineCulture);
                            if (label4 != null) label4.Text = "N/A";
                            if (label5 != null) label5.Text = "N/A";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading summary: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadPaymentDetails()
        {
            string query = @"
                SELECT 
                    P.paymentId,
                    P.paymentDate AS DatePaid, 
                    P.paymentAmount AS Amount, 
                    M.methodName AS Method
                FROM Payment P
                INNER JOIN PaymentMethod M ON P.paymentMethodId = M.paymentMethodId
                WHERE P.contractId = @ContractId
                ORDER BY P.paymentDate DESC";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@ContractId", ContractID);
                DataTable dtHistory = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dtHistory);

                    if (dataGridViewHistory != null)
                    {
                        dataGridViewHistory.DataSource = dtHistory;
                        FormatHistoryColumns();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading history: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormatHistoryColumns()
        {
            if (dataGridViewHistory.Columns.Contains("Amount"))
            {
                dataGridViewHistory.Columns["Amount"].DefaultCellStyle.Format = "C";
                dataGridViewHistory.Columns["Amount"].DefaultCellStyle.FormatProvider = philippineCulture;
            }

            if (dataGridViewHistory.Columns.Contains("paymentId"))
            {
                dataGridViewHistory.Columns["paymentId"].Visible = false;
            }
        }
    }
}