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
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

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
            ORDER BY paymentDate DESC -- Kinukuha ang pinaka-latest na date
        )
        SELECT 
            ISNULL((SELECT SUM(paymentAmount) FROM Payment WHERE contractId = @ContractId), 0) AS TotalPaid,
            ISNULL((SELECT paymentAmount FROM LatestPayment), 0) AS LatestPaymentAmount
        FROM Contract C
        WHERE C.contractID = @ContractId;
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
                            decimal totalPaid = reader.GetDecimal(reader.GetOrdinal("TotalPaid"));
                            decimal latestPayment = reader.GetDecimal(reader.GetOrdinal("LatestPaymentAmount"));

                            if (label1 != null)
                                label1.Text = latestPayment.ToString("C", philippineCulture);

                            if (label3 != null)
                                label3.Text = totalPaid.ToString("C", philippineCulture);
                        }
                        else
                        {
                            if (label1 != null) label1.Text = (0m).ToString("C", philippineCulture);
                            if (label3 != null) label3.Text = (0m).ToString("C", philippineCulture);
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