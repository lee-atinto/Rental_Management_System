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
            LoadPaymentDetails();
        }

        private void LoadPaymentDetails()
        {
            string query = @"
        SELECT 
            P.paymentId,
            P.paymentDate AS DatePaid, 
            P.paymentAmount AS Amount, 
            M.methodName AS Method
            -- INALIS: P.receiptPath
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

        private void AddReceiptDownloadButton()
        {
            if (!dataGridViewHistory.Columns.Contains("ReceiptButton"))
            {
                DataGridViewButtonColumn downloadBtn = new DataGridViewButtonColumn
                {
                    Name = "ReceiptButton",
                    Text = "Download Receipt",
                    UseColumnTextForButtonValue = true,
                    HeaderText = "Receipt"
                };
                dataGridViewHistory.Columns.Add(downloadBtn);
            }
        }

        private void dataGridViewHistory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridViewHistory.Columns[e.ColumnIndex].Name != "ReceiptButton") return;

            DataGridViewRow row = dataGridViewHistory.Rows[e.RowIndex];
            string receiptPath = row.Cells["receiptPath"].Value?.ToString();
            string paymentId = row.Cells["paymentId"].Value?.ToString();

            if (string.IsNullOrEmpty(receiptPath))
            {
                MessageBox.Show("No receipt file available for this payment.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                MessageBox.Show($"Simulating download/view of receipt for Payment ID: {paymentId}. File path: {receiptPath}", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error accessing receipt file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}