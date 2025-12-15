using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;

namespace WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Maintenance
{
    public partial class AddRecquest : Form
    {
        private readonly string DataConnection = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        private int maintenanceReqID = -1;

        public AddRecquest(int requestID = -1)
        {
            InitializeComponent();
            this.maintenanceReqID = requestID;
        }

        private void AddRecquest_Load(object sender, EventArgs e)
        {
            SetupForm();
            LoadTenantsAndUnits();
            LoadRequestTypes();

            if (maintenanceReqID != -1)
            {
                this.Text = "Edit Maintenance Request";
                LoadRequestData(maintenanceReqID);
            }
            else
            {
                this.Text = "Add New Maintenance Request";
            }
        }

        private void SetupForm()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.AddRange(new object[] { "Pending", "In Progress", "Completed" });
            cbStatus.SelectedIndex = 0;

            cbTenant.SelectedIndexChanged += cbTenant_SelectedIndexChanged;

            dtpRequestDate.Value = DateTime.Now;
            dtpRequestDate.Enabled = (maintenanceReqID == -1);

            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;

            this.BackColor = Color.LightGray;
        }

        private void LoadTenantsAndUnits()
        {
            string query = @"
                SELECT 
                    T.TenantID, 
                    PI.firstName, 
                    PI.lastName, 
                    U.UnitNumber, 
                    C.UnitID,
                    P.propertyID  -- IDINAGDAG: Kinuha ang PropertyID
                FROM Tenant AS T
                INNER JOIN PersonalInformation AS PI ON T.TenantID = PI.TenantID
                INNER JOIN Contract AS C ON T.TenantID = C.TenantID
                INNER JOIN Unit AS U ON C.UnitID = U.UnitID
                INNER JOIN Property AS P ON U.PropertyID = P.PropertyID -- IDINAGDAG: I-join ang Property table
                WHERE C.ContractStatus = 'Active' 
                ORDER BY PI.lastName;";

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                cbTenant.DataSource = dt;
                cbTenant.DisplayMember = "firstName";
                cbTenant.ValueMember = "TenantID";
                cbTenant.SelectedIndex = -1;
                tbUnitNumber.Text = "Unit: N/A";

                if (dt.Rows.Count > 0 && maintenanceReqID == -1)
                {
                    cbTenant.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading tenants: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRequestTypes()
        {
            string query = "SELECT requestTypeID, typeName FROM RequestType ORDER BY typeName;";
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }

                cbRequestType.DataSource = dt;
                cbRequestType.DisplayMember = "typeName";
                cbRequestType.ValueMember = "requestTypeID";
                cbRequestType.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading request types: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTenant.SelectedValue != null && cbTenant.DataSource is DataTable dt)
            {
                DataRowView selectedRow = cbTenant.SelectedItem as DataRowView;
                if (selectedRow != null)
                {
                    string unitNum = selectedRow["UnitNumber"].ToString();
                    tbUnitNumber.Text = $"UNIT - {unitNum}";
                }
            }
            else
            {
                tbUnitNumber.Text = "Unit: N/A";
            }
        }

        private void LoadRequestData(int requestID)
        {
            string query = @"
                SELECT 
                    MR.description, 
                    MR.requestDate, 
                    MR.Status, 
                    MR.TenantID,
                    MR.requestTypeID,   -- IDINAGDAG
                    MR.propertyID       -- IDINAGDAG
                FROM MaintenanceRequest AS MR
                WHERE MR.maintenanceReqID = @RequestID;";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RequestID", requestID);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tbDescription.Text = reader["description"].ToString();
                            dtpRequestDate.Value = Convert.ToDateTime(reader["requestDate"]);
                            cbStatus.SelectedItem = reader["Status"].ToString();
                            int tenantId = Convert.ToInt32(reader["TenantID"]);

                            if (reader["requestTypeID"] != DBNull.Value)
                            {
                                cbRequestType.SelectedValue = Convert.ToInt32(reader["requestTypeID"]);
                            }

                            cbTenant.SelectedValue = tenantId;
                            cbTenant.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading request data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbTenant.SelectedValue == null)
            {
                MessageBox.Show("Please select a tenant.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(tbDescription.Text))
            {
                MessageBox.Show("Please enter the maintenance description/comments.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbRequestType.SelectedValue == null)
            {
                MessageBox.Show("Please select a request type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int tenantID = Convert.ToInt32(cbTenant.SelectedValue);
            string description = tbDescription.Text.Trim();
            DateTime requestDate = dtpRequestDate.Value;
            string status = cbStatus.SelectedItem.ToString();

            int requestTypeID = Convert.ToInt32(cbRequestType.SelectedValue);
            int propertyID = -1;
            DataRowView selectedRow = cbTenant.SelectedItem as DataRowView;
            if (selectedRow != null && selectedRow["propertyID"] != DBNull.Value)
            {
                propertyID = Convert.ToInt32(selectedRow["propertyID"]);
            }
            else
            {
                MessageBox.Show("Could not determine property ID for the selected tenant. Please check contract/unit data.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (maintenanceReqID == -1)
            {
                InsertRequest(tenantID, description, requestDate, status, propertyID, requestTypeID);
            }
            else
            {
                UpdateRequest(maintenanceReqID, description, status, requestTypeID);
            }
        }

        private void InsertRequest(int tenantID, string description, DateTime requestDate, string status, int propertyID, int requestTypeID)
        {
            string query = @"
                INSERT INTO MaintenanceRequest (TenantID, description, requestDate, Status, propertyID, requestTypeID)
                VALUES (@TenantID, @Description, @RequestDate, @Status, @PropertyID, @RequestTypeID);";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TenantID", tenantID);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@RequestDate", requestDate);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@PropertyID", propertyID);
                    cmd.Parameters.AddWithValue("@RequestTypeID", requestTypeID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Maintenance Request added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting request: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRequest(int requestID, string description, string status, int requestTypeID)
        {
            string query = @"
                UPDATE MaintenanceRequest 
                SET description = @Description, 
                    Status = @Status,
                    requestTypeID = @RequestTypeID -- IDINAGDAG: Update Request Type
                WHERE maintenanceReqID = @RequestID;";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RequestID", requestID);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@RequestTypeID", requestTypeID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Maintenance Request updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating request: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}