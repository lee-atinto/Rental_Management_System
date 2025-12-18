using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_PaymentRecords
{
    public partial class AddPayment : Form
    {
        private readonly string DataConnection = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        private int contractId = -1;
        private int paymentIdToUpdate = -1;

        public decimal NewAmount { get; private set; }
        private byte[] originalRowVersion;

        public AddPayment()
        {
            InitializeComponent();
        }

        public AddPayment(int contractId)
        {
            InitializeComponent();
            this.contractId = contractId;
            cbTenantName.Enabled = false;

            if (contractId > 0)
            {
                SetupFormForEdit(contractId);
            }
        }

        public AddPayment(int contractId, byte[] rowVersion) : this(contractId)
        {
            this.originalRowVersion = rowVersion;
        }

        private void SetupFormForEdit(int contractId)
        {
            this.Text = "Update Payment Record (Contract ID: " + contractId + ")";
            LoadExistingPaymentData(contractId);
        }

        private void LoadExistingPaymentData(int contractId)
        {
            string query = @"
                SELECT TOP 1 
                    P.paymentId, P.paymentAmount, P.paymentDate, P.paymentMethodId, P.paymentTypeId, T.tenantId,
                    P.referenceID, P.RowVersion
                FROM Payment P
                INNER JOIN Contract C ON P.contractId = C.contractId
                INNER JOIN PersonalInformation T ON C.tenantID = T.tenantId
                WHERE P.contractId = @ContractId
                ORDER BY P.paymentDate DESC, P.paymentId DESC";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ContractId", contractId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            paymentIdToUpdate = Convert.ToInt32(reader["paymentId"]);
                            tbAmountPaid.Text = reader["paymentAmount"].ToString();
                            dtpPaymentDate.Value = Convert.ToDateTime(reader["paymentDate"]);
                            tbReferenceID.Text = reader["referenceID"] != DBNull.Value ? reader["referenceID"].ToString() : string.Empty;
                            cbPaymentMethod.SelectedValue = reader["paymentMethodId"];
                            cbPaymentType.SelectedValue = reader["paymentTypeId"];
                            cbTenantName.SelectedValue = reader["tenantId"];

                            if (originalRowVersion == null && reader["RowVersion"] != DBNull.Value)
                            {
                                originalRowVersion = (byte[])reader["RowVersion"];
                            }
                        }
                        else
                        {
                            MessageBox.Show("No payment record found. You can only add a NEW one.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            paymentIdToUpdate = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task<bool> UpdatePaymentWithTransaction(int contractId, decimal newAmount, byte[] originalRowVersion)
        {
            using (SqlConnection connection = new SqlConnection(DataConnection))
            {
                await connection.OpenAsync();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    await Task.Delay(3000);

                    string updateQuery = @"
                UPDATE Payment 
                SET paymentAmount = @amount 
                WHERE contractId = @contractId AND RowVersion = @originalRowVersion";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@amount", newAmount);
                        command.Parameters.AddWithValue("@contractId", contractId);
                        command.Parameters.AddWithValue("@originalRowVersion", originalRowVersion);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            transaction.Commit();
                            return true;
                        }
                        else
                        {
                            transaction.Rollback();
                            MessageBox.Show("Conflict Detected: May ibang admin na naunang mag-update sa record na ito. Paki-refresh ang data.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Transaction Rolled Back: {ex.Message}");
                    return false;
                }
            }
        }

        private void SetTenantAndUnitFromContractId(int contractId)
        {
            string query = @"
                SELECT U.UnitNumber, C.tenantId
                FROM Contract C
                INNER JOIN Unit U ON C.UnitID = U.UnitID
                INNER JOIN PersonalInformation T ON C.tenantID = T.tenantID
                WHERE C.contractID = @ContractID";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ContractID", contractId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tbUnitNumber.Text = reader["UnitNumber"].ToString();
                        tbUnitNumber.Tag = contractId;
                        if (cbTenantName.DataSource is DataTable dtTenants)
                        {
                            DataRow[] rows = dtTenants.Select($"tenantId = {reader["tenantId"]}");
                            if (rows.Length > 0)
                            {
                                cbTenantName.SelectedValue = reader["tenantId"];
                            }
                        }
                    }
                }
            }
        }

        private void AddPayment_Load(object sender, EventArgs e)
        {
            dtpPaymentDate.MinDate = DateTime.Today;
            dtpPaymentDate.Value = DateTime.Today;
            LoadTenants();
            LoadPaymentMethods();
            LoadPaymentTypes();

            if (contractId > 0)
            {
                SetTenantAndUnitFromContractId(contractId);
            }
        }

        private void LoadTenants()
        {
            string query = @"
        SELECT DISTINCT T.tenantId, 
               T.firstName + ' ' + T.lastName AS TenantName,
               T.lastName
        FROM PersonalInformation T
        INNER JOIN Contract C ON T.tenantId = C.tenantID
        LEFT JOIN Payment P ON C.contractID = P.contractId
        WHERE LOWER(C.contractStatus) = 'active'
        AND (P.paymentStatus IN ('Pending', 'Overdue') OR P.paymentId IS NULL)
        ORDER BY T.lastName";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cbTenantName.DataSource = dt;
                    cbTenantName.DisplayMember = "TenantName";
                    cbTenantName.ValueMember = "tenantId";
                    cbTenantName.SelectedIndex = -1;
                    cbTenantName.Text = "Select Tenant";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void LoadPaymentMethods()
        {
            string query = "SELECT paymentMethodId, methodName FROM PaymentMethod ORDER BY methodName";
            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    cbPaymentMethod.DataSource = dt;
                    cbPaymentMethod.DisplayMember = "methodName";
                    cbPaymentMethod.ValueMember = "paymentMethodId";
                    cbPaymentMethod.SelectedIndex = -1;
                    cbPaymentMethod.Text = "Select Method";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void LoadPaymentTypes()
        {
            string query = "SELECT paymentTypeID, typeName FROM PaymentType";
            using (SqlConnection connection = new SqlConnection(DataConnection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                cbPaymentType.DataSource = dt;
                cbPaymentType.DisplayMember = "typeName";
                cbPaymentType.ValueMember = "paymentTypeID";
            }
        }

        private void cbTenantName_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (contractId > 0) return;

            if (cbTenantName.SelectedValue == null || !(cbTenantName.SelectedValue is int tenantId))
            {
                tbUnitNumber.Text = string.Empty;
                tbUnitNumber.Tag = null;
                return;
            }

            string query = @"
                SELECT U.UnitNumber, C.contractID
                FROM Contract C
                INNER JOIN Unit U ON C.UnitID = U.UnitID
                WHERE C.tenantID = @TenantID AND LOWER(C.contractStatus) = 'active'";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TenantID", tenantId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tbUnitNumber.Text = reader["UnitNumber"].ToString();
                            tbUnitNumber.Tag = reader["contractID"];
                        }
                        else
                        {
                            tbUnitNumber.Text = "No active unit found";
                            tbUnitNumber.Tag = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            decimal amountPaid = Convert.ToDecimal(tbAmountPaid.Text);
            int currentContractId = (int)tbUnitNumber.Tag;
            int unitId = 0;
            decimal monthlyRent = 0;
            string finalStatus = "Pending";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                {
                    connection.Open();

                    string getContractInfo = @"
                SELECT U.MonthlyRent, C.UnitID 
                FROM dbo.Contract C 
                INNER JOIN dbo.Unit U ON C.UnitID = U.UnitID 
                WHERE C.contractID = @ContractId";

                    using (SqlCommand cmd = new SqlCommand(getContractInfo, connection))
                    {
                        cmd.Parameters.AddWithValue("@ContractId", currentContractId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                monthlyRent = Convert.ToDecimal(reader["MonthlyRent"]);
                                unitId = Convert.ToInt32(reader["UnitID"]);
                            }
                        }
                    }

                    finalStatus = (amountPaid < monthlyRent) ? "Pending" : "Paid";

                    string sqlQuery = @"INSERT INTO dbo.Payment (tenantId, contractId, UnitID, paymentMethodId, paymentTypeId, 
                                 paymentAmount, paymentDate, referenceID, paymentStatus)
                                 VALUES (@TenantId, @ContractId, @UnitId, @PaymentMethodId, @PaymentTypeId, 
                                 @PaymentAmount, @PaymentDate, @ReferenceId, @Status)";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TenantId", (int)cbTenantName.SelectedValue);
                        command.Parameters.AddWithValue("@ContractId", currentContractId);
                        command.Parameters.AddWithValue("@UnitId", unitId);
                        command.Parameters.AddWithValue("@PaymentMethodId", (int)cbPaymentMethod.SelectedValue);
                        command.Parameters.AddWithValue("@PaymentTypeId", (int)cbPaymentType.SelectedValue);
                        command.Parameters.AddWithValue("@PaymentAmount", amountPaid);
                        command.Parameters.AddWithValue("@PaymentDate", dtpPaymentDate.Value.Date);
                        command.Parameters.AddWithValue("@Status", finalStatus);

                        string refId = tbReferenceID.Text.Trim();
                        command.Parameters.AddWithValue("@ReferenceId", string.IsNullOrEmpty(refId) ? (object)DBNull.Value : refId);

                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Saved Successfully!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private bool ValidateInput()
        {
            // Tenant check
            if (cbTenantName.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a tenant.");
                return false;
            }

            // Amount check
            if (!decimal.TryParse(tbAmountPaid.Text, out decimal amt) || amt <= 0)
            {
                MessageBox.Show("Invalid amount. Please enter a valid payment amount.");
                return false;
            }

            // Get payment method as text (simple)
            string method = cbPaymentMethod.Text.Trim().ToUpper();
            string refId = tbReferenceID.Text.Trim();

            // Non-Cash payments: reference required & must be unique
            if (method != "CASH")
            {
                if (string.IsNullOrEmpty(refId))
                {
                    MessageBox.Show("Reference ID is required for this payment method.");
                    return false;
                }

                if (IsReferenceDuplicate(refId, paymentIdToUpdate))
                {
                    MessageBox.Show("Duplicate Reference ID! This reference number is already used.");
                    return false;
                }
            }

            // Cash payments: reference optional, no checks needed
            return true;
        }

        // Check duplicate reference (ignore current record if editing)
        private bool IsReferenceDuplicate(string refId, int currentPaymentId = -1)
        {
            string query = "SELECT COUNT(*) FROM Payment WHERE referenceID = @refId";

            if (currentPaymentId > 0)
                query += " AND paymentId <> @currentId";

            using (SqlConnection conn = new SqlConnection(DataConnection))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@refId", refId);
                if (currentPaymentId > 0)
                    cmd.Parameters.AddWithValue("@currentId", currentPaymentId);

                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
    }
}