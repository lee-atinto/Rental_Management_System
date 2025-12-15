using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_PaymentRecords
{
    public partial class AddPayment : Form
    {
        private readonly string DataConnection = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private int contractId = -1;
        private int paymentIdToUpdate = -1;

        public AddPayment()
        {
            InitializeComponent();
        }

        public AddPayment(int contractId)
        {
            InitializeComponent();

            this.contractId = contractId;

            // Assuming cbTenantName is the ComboBox for tenant names
            cbTenantName.Enabled = false;

            if (contractId > 0)
            {
                SetupFormForEdit(contractId);
            }
        }

        private void SetupFormForEdit(int contractId)
        {
            this.Text = "I-update ang Payment Record (Contract ID: " + contractId + ")";
            LoadExistingPaymentData(contractId);
        }

        private void LoadExistingPaymentData(int contractId)
        {
            string query = @"
                SELECT TOP 1 
                    P.paymentId, P.paymentAmount, P.paymentDate, P.paymentMethodId, P.paymentTypeId, T.tenantId,
                    P.referenceID
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
                        }
                        else
                        {
                            MessageBox.Show("Walang mahanap na payment record para sa contract na ito. Maaari ka lang mag-add ng BAGO.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            paymentIdToUpdate = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading payment data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetTenantAndUnitFromContractId(int contractId)
        {
            // INAYOS ANG JOIN DITO: Mula sa C.propertyID = U.propertyID ay ginawang C.UnitID = U.UnitID
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
                MessageBox.Show($"Error loading tenants: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show($"Error loading payment methods: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPaymentTypes()
        {
            string query = "SELECT paymentTypeID, typeName FROM PaymentType";
            using (SqlConnection connection = new SqlConnection(DataConnection))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
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

            // INAYOS ANG JOIN DITO: Mula sa C.propertyID = U.propertyID ay ginawang C.UnitID = U.UnitID
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
                MessageBox.Show($"Error retrieving unit details: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            // In ADD mode, get selected Tenant ID and Contract ID from the Tag
            int currentTenantId;
            int currentContractId;

            if (contractId > 0)
            {
                // EDIT mode: contractId is known, tenantId is already loaded/selected (and disabled)
                currentContractId = contractId;
                currentTenantId = (int)cbTenantName.SelectedValue;
            }
            else
            {
                // ADD mode: get IDs from selections
                if (cbTenantName.SelectedValue == null || tbUnitNumber.Tag == null)
                {
                    MessageBox.Show("Missing tenant or contract information.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                currentTenantId = (int)cbTenantName.SelectedValue;
                currentContractId = (int)tbUnitNumber.Tag;
            }

            if (currentContractId <= 0 || cbPaymentMethod.SelectedValue == null || cbPaymentType.SelectedValue == null)
            {
                MessageBox.Show("Missing contract or payment details.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int paymentMethodId = (int)cbPaymentMethod.SelectedValue;
            int paymentTypeId = (int)cbPaymentType.SelectedValue;
            decimal paymentAmount = Convert.ToDecimal(tbAmountPaid.Text);
            DateTime paymentDate = dtpPaymentDate.Value.Date;

            string referenceId = tbReferenceID.Text.Trim();
            string paymentStatus = "Paid"; // Defaulting to 'Paid' since the control was removed.

            string sqlQuery;

            if (paymentIdToUpdate > 0)
            {
                // UPDATE query (Removed paymentStatus update)
                sqlQuery = @"
                    UPDATE Payment 
                    SET paymentAmount = @PaymentAmount,
                        paymentDate = @PaymentDate,
                        paymentMethodId = @PaymentMethodId,
                        paymentTypeId = @PaymentTypeId,
                        referenceID = @ReferenceId
                    WHERE paymentId = @PaymentIdToUpdate";
            }
            else
            {
                // INSERT query (Hardcoded paymentStatus to 'Paid')
                sqlQuery = @"
                    INSERT INTO Payment (tenantId, contractId, paymentMethodId, paymentTypeId, paymentAmount, paymentDate, referenceID, paymentStatus)
                    VALUES (@TenantId, @ContractId, @PaymentMethodId, @PaymentTypeId, @PaymentAmount, @PaymentDate, @ReferenceId, @PaymentStatus)";
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

                    // Only pass PaymentStatus parameter if INSERTING or if you want to explicitly update it to 'Paid'
                    if (paymentIdToUpdate <= 0)
                    {
                        command.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
                    }


                    if (paymentIdToUpdate > 0)
                    {
                        command.Parameters.AddWithValue("@PaymentIdToUpdate", paymentIdToUpdate);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@TenantId", currentTenantId);
                        command.Parameters.AddWithValue("@ContractId", currentContractId);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();

                    string message = (paymentIdToUpdate > 0) ? "Payment updated successfully!" : "Payment recorded successfully!";
                    MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (SqlException sqlex)
            {
                MessageBox.Show($"Database Error saving payment: {sqlex.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving payment: {ex.Message}", "General Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Could not find an active contract/unit for the selected tenant.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (dtpPaymentDate.Value.Date > DateTime.Today.Date)
            {
                MessageBox.Show("Date of Payment cannot be a future date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(tbAmountPaid.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Please enter a valid positive numeric value for Amount Paid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(tbReferenceID.Text))
            {
                MessageBox.Show("Please enter a Reference ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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