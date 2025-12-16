using System;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Windows.Forms;
using WindowsFormsApp1.Super_Admin_Account;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_Tenants
{
    public partial class AddTenants : Form
    {
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private Tenants parentForm;
        private int tenantIDToEdit = -1;

        public AddTenants(Tenants parent, int tenantID = -1)
        {
            InitializeComponent();
            parentForm = parent;
            tenantIDToEdit = tenantID;

            if (tenantIDToEdit != -1)
            {
                this.Text = "Edit Tenant";
                LoadTenantData(tenantIDToEdit);
            }
            else
            {
                this.Text = "Add New Tenant";
            }
        }

        private void AddTenants_Load(object sender, EventArgs e)
        {
            tbContactNum.MaxLength = 11;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadTenantData(int tenantID)
        {
            string sql = @" SELECT P.firstName, P.middleName, P.lastName, P.contactNumber, P.email, T.tenantStatus FROM PersonalInformation P INNER JOIN Tenant T ON P.tenantID = T.tenantID WHERE P.tenantID = @id";

            using (SqlConnection con = new SqlConnection(DataConnection))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", tenantID);

                try
                {
                    con.Open();
                    SqlDataReader r = cmd.ExecuteReader();

                    if (r.Read())
                    {
                        tbFirstName.Text = r["firstName"].ToString();
                        tbMiddleName.Text = r["middleName"] == DBNull.Value ? "" : r["middleName"].ToString();
                        tbLastName.Text = r["lastName"].ToString();
                        tbContactNum.Text = r["contactNumber"].ToString();
                        tbEmail.Text = r["email"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading tenant data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ActivateTenantAndAssignUnit(int tenantID)
        {
            int assignedUnitIDValue = tenantID;
            string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

            string sql = @"
                UPDATE Tenant
                SET tenantStatus = 'Active', 
                    UnitID = @UnitIDValue 
                WHERE tenantID = @TenantID 
                AND tenantStatus = 'Inactive';
            ";

            using (SqlConnection con = new SqlConnection(DataConnection))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@UnitIDValue", assignedUnitIDValue);
                cmd.Parameters.AddWithValue("@TenantID", tenantID);

                try
                {
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Tenant ID {tenantID} successfully activated and assigned Unit ID {assignedUnitIDValue}.",
                                        "Activation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Activation failed for Tenant ID {tenantID}. Tenant may already be active or record not found.",
                                        "Activation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error during tenant activation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool IsDuplicateTenant(string fn, string ln, int currentTenantId)
        {
            string sql = @"
                SELECT COUNT(*) FROM PersonalInformation 
                WHERE firstName = @fn AND lastName = @ln AND tenantID != @id";

            using (SqlConnection con = new SqlConnection(DataConnection))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@fn", fn);
                cmd.Parameters.AddWithValue("@ln", ln);
                cmd.Parameters.AddWithValue("@id", currentTenantId);

                try
                {
                    con.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception($"Database check for duplicate failed: {ex.Message}");
                }
            }
        }


        private void InsertNewTenant(string fn, string mn, string ln, string cn, string em)
        {
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    string sqlInsertTenant = @" 
                        INSERT INTO Tenant (tenantStatus, dateRegistered, UnitID, tenantID_New) 
                        OUTPUT INSERTED.tenantID 
                        VALUES ('Inactive', @date, NULL, 0); 
                    ";

                    int newTenantID;

                    using (SqlCommand cmd = new SqlCommand(sqlInsertTenant, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);

                        object result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                            throw new Exception("Failed to get new tenantID. Check if tenantID column is an IDENTITY column.");

                        newTenantID = Convert.ToInt32(result);
                    }

                    string sqlUpdateTenantNewID = @" 
                        UPDATE Tenant 
                        SET tenantID_New = @NewID 
                        WHERE tenantID = @NewID;
                    ";

                    using (SqlCommand cmd = new SqlCommand(sqlUpdateTenantNewID, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@NewID", newTenantID);
                        cmd.ExecuteNonQuery();
                    }

                    string sqlInsertPersonalInfo = @" 
                        INSERT INTO PersonalInformation (tenantID, firstName, middleName, lastName, contactNumber, email) 
                        VALUES (@id, @fn, @mn, @ln, @cn, @em);
                    ";

                    using (SqlCommand cmd = new SqlCommand(sqlInsertPersonalInfo, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@id", newTenantID);
                        cmd.Parameters.AddWithValue("@fn", fn);
                        cmd.Parameters.AddWithValue("@ln", ln);
                        cmd.Parameters.AddWithValue("@cn", cn);
                        cmd.Parameters.AddWithValue("@em", em);
                        cmd.Parameters.AddWithValue("@mn", string.IsNullOrWhiteSpace(mn) ? (object)DBNull.Value : mn);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        private void UpdateTenant(int id, string fn, string mn, string ln, string cn, string em)
        {
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();

                string sql = @" UPDATE PersonalInformation SET firstName = @fn, middleName = @mn, lastName = @ln, contactNumber = @cn, email = @em WHERE tenantID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@fn", fn);
                    cmd.Parameters.AddWithValue("@ln", ln);
                    cmd.Parameters.AddWithValue("@cn", cn);
                    cmd.Parameters.AddWithValue("@em", em);
                    cmd.Parameters.AddWithValue("@mn", string.IsNullOrWhiteSpace(mn) ? (object)DBNull.Value : mn);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string fn = tbFirstName.Text.Trim();
            string mn = tbMiddleName.Text.Trim();
            string ln = tbLastName.Text.Trim();
            string cn = tbContactNum.Text.Trim();
            string em = tbEmail.Text.Trim();

            if (string.IsNullOrEmpty(fn) || string.IsNullOrEmpty(ln) || string.IsNullOrEmpty(cn))
            {
                MessageBox.Show("First Name, Last Name, and Contact Number are required fields.",
                    "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!long.TryParse(cn, out _))
            {
                MessageBox.Show("Contact number must contain numbers only.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cn.Length != 11)
            {
                MessageBox.Show("Contact number must be exactly 11 digits.", "Invalid Input",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (IsDuplicateTenant(fn, ln, tenantIDToEdit))
                {
                    MessageBox.Show($"A tenant with the name '{fn} {ln}' already exists. Please check the name or if the tenant is already in the system.",
                        "Duplicate Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (tenantIDToEdit == -1)
                {
                    InsertNewTenant(fn, mn, ln, cn, em);
                    MessageBox.Show("Tenant added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    parentForm.LoadTenantsData("All", parentForm.CurrentSearchText);
                }
                else
                {
                    UpdateTenant(tenantIDToEdit, fn, mn, ln, cn, em);
                    MessageBox.Show("Tenant updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    parentForm.LoadTenantsData(parentForm.CurrentStatusFilter, parentForm.CurrentSearchText);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                string action = tenantIDToEdit == -1 ? "adding" : "updating";
                MessageBox.Show($"An error occurred while {action} the tenant: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbContactNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }
}