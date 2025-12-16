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

        // Idinagdag para makuha ang amount mula sa Payment_Records
        public decimal NewAmount { get; private set; }
        // Idinagdag para sa concurrency check ng dalawang admin
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

        // Overloaded constructor para sa concurrency safety
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

                            // Kunin ang RowVersion mula sa database kung wala pang naipasa
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
                    // DELAY para sa safety ng transaction ng dalawang admin
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
                SELECT T.tenantId, 
                        T.firstName + ' ' + COALESCE(T.middleName + ' ', '') + T.lastName AS TenantName
                FROM PersonalInformation T
                INNER JOIN Contract C ON T.tenantId = C.tenantID
                WHERE LOWER(C.contractStatus) = 'active'
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

            // I-set ang NewAmount para makuha ng Payment_Records form
            this.NewAmount = Convert.ToDecimal(tbAmountPaid.Text);

            int currentTenantId;
            int currentContractId;

            if (contractId > 0)
            {
                currentContractId = contractId;
                currentTenantId = (int)cbTenantName.SelectedValue;
            }
            else
            {
                currentTenantId = (int)cbTenantName.SelectedValue;
                currentContractId = (int)tbUnitNumber.Tag;
            }

            decimal paymentAmount = Convert.ToDecimal(tbAmountPaid.Text);
            DateTime paymentDate = dtpPaymentDate.Value.Date;
            int paymentMethodId = (int)cbPaymentMethod.SelectedValue;
            int paymentTypeId = (int)cbPaymentType.SelectedValue;
            string referenceId = tbReferenceID.Text.Trim();

            string sqlQuery;
            if (paymentIdToUpdate > 0)
            {
                sqlQuery = @"UPDATE Payment SET paymentAmount = @PaymentAmount, paymentDate = @PaymentDate, 
                     paymentMethodId = @PaymentMethodId, paymentTypeId = @PaymentTypeId, 
                     referenceID = @ReferenceId WHERE paymentId = @PaymentIdToUpdate";
            }
            else
            {
                sqlQuery = @"INSERT INTO Payment (tenantId, contractId, paymentMethodId, paymentTypeId, 
                     paymentAmount, paymentDate, referenceID, paymentStatus)
                     VALUES (@TenantId, @ContractId, @PaymentMethodId, @PaymentTypeId, 
                     @PaymentAmount, @PaymentDate, @ReferenceId, 'Paid')";
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                    command.Parameters.AddWithValue("@PaymentDate", paymentDate);
                    command.Parameters.AddWithValue("@PaymentMethodId", paymentMethodId);
                    command.Parameters.AddWithValue("@PaymentTypeId", paymentTypeId);
                    command.Parameters.AddWithValue("@ReferenceId", referenceId);

                    if (paymentIdToUpdate > 0)
                        command.Parameters.AddWithValue("@PaymentIdToUpdate", paymentIdToUpdate);
                    else
                    {
                        command.Parameters.AddWithValue("@TenantId", currentTenantId);
                        command.Parameters.AddWithValue("@ContractId", currentContractId);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show("Payment record saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (cbTenantName.SelectedIndex == -1 || cbTenantName.SelectedValue == null)
            {
                MessageBox.Show("Please select a Tenant.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (tbUnitNumber.Tag == null)
            {
                MessageBox.Show("No active contract found for this tenant.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbAmountPaid.Text) || !decimal.TryParse(tbAmountPaid.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid Amount Paid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbReferenceID.Text))
            {
                MessageBox.Show("Reference ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbPaymentType.SelectedIndex == -1 || cbPaymentType.SelectedValue == null)
            {
                MessageBox.Show("Please select a Payment Type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbPaymentMethod.SelectedIndex == -1 || cbPaymentMethod.SelectedValue == null)
            {
                MessageBox.Show("Please select a Payment Method.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}