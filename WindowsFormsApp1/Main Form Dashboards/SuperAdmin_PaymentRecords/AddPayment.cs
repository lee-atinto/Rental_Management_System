using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration; // Idagdag ito para sa ConfigurationManager

namespace WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_PaymentRecords
{
    public partial class AddPayment : Form
    {
        private readonly string DataConnection = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        public AddPayment()
        {
            InitializeComponent();
        }

        private void AddPayment_Load(object sender, EventArgs e)
        {
            dtpPaymentDate.Value = DateTime.Today;

            LoadTenants();
            LoadPaymentMethods();
            LoadPaymentTypes();
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
            if (cbTenantName.SelectedValue == null || !(cbTenantName.SelectedValue is int tenantId))
            {
                tbUnitNumber.Text = string.Empty;
                tbUnitNumber.Tag = null;
                return;
            }

            string query = @"
                SELECT U.UnitNumber, C.contractID
                FROM Contract C
                INNER JOIN Unit U ON C.propertyID = U.propertyID
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

            if (cbTenantName.SelectedValue == null ||
                tbUnitNumber.Tag == null ||
                cbPaymentMethod.SelectedValue == null ||
                cbPaymentType.SelectedValue == null)
            {
                MessageBox.Show("Please complete all required fields (Tenant, Unit, Method, Type).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int tenantId = (int)cbTenantName.SelectedValue;
            int contractId = (int)tbUnitNumber.Tag;
            int paymentMethodId = (int)cbPaymentMethod.SelectedValue;
            int paymentTypeId = (int)cbPaymentType.SelectedValue;

            decimal paymentAmount = Convert.ToDecimal(tbAmountPaid.Text);
            DateTime paymentDate = dtpPaymentDate.Value.Date;

            string insertQuery = @"
        INSERT INTO Payment (tenantId, contractId, paymentMethodId, paymentTypeId, paymentAmount, paymentDate)
        VALUES (@TenantId, @ContractId, @PaymentMethodId, @PaymentTypeId, @PaymentAmount, @PaymentDate)";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@TenantId", tenantId);
                    command.Parameters.AddWithValue("@ContractId", contractId);
                    command.Parameters.AddWithValue("@PaymentMethodId", paymentMethodId);
                    command.Parameters.AddWithValue("@PaymentTypeId", paymentTypeId);
                    command.Parameters.AddWithValue("@PaymentAmount", paymentAmount);
                    command.Parameters.AddWithValue("@PaymentDate", paymentDate);

                    connection.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show("Payment recorded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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