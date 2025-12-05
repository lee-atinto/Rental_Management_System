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
        private readonly string DataConnection =
            @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

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

        private void AddTenants_Load(object sender, EventArgs e) { }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --------------------------------------------
        // LOAD TENANT DATA FOR EDIT MODE
        // --------------------------------------------
        private void LoadTenantData(int tenantID)
        {
            string sql = @"
                SELECT 
                    P.firstName, P.middleName, P.lastName,
                    P.contactNumber, P.email,
                    T.tenantStatus
                FROM PersonalInformation P
                INNER JOIN Tenant T ON P.tenantID = T.tenantID
                WHERE P.tenantID = @id";

            using (SqlConnection con = new SqlConnection(DataConnection))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", tenantID);

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
        }

        // --------------------------------------------
        // INSERT NEW TENANT
        // --------------------------------------------
        private void InsertNewTenant(string fn, string mn, string ln, string cn, string em)
        {
            using (var scope = new TransactionScope())
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();

                // 1. Insert into PersonalInformation
                string sqlPI = @"
                    INSERT INTO PersonalInformation
                        (firstName, middleName, lastName, contactNumber, email)
                    OUTPUT INSERTED.personalInfoID
                    VALUES (@fn, @mn, @ln, @cn, @em)";

                int newID;

                using (SqlCommand cmd = new SqlCommand(sqlPI, con))
                {
                    cmd.Parameters.AddWithValue("@fn", fn);
                    cmd.Parameters.AddWithValue("@ln", ln);
                    cmd.Parameters.AddWithValue("@cn", cn);
                    cmd.Parameters.AddWithValue("@em", em);

                    if (string.IsNullOrWhiteSpace(mn))
                        cmd.Parameters.AddWithValue("@mn", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@mn", mn);

                    newID = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 2. Insert into Tenant
                string sqlTenant = @"
                    INSERT INTO Tenant (tenantID, tenantStatus, dateRegistered)
                    VALUES (@id, 'Inactive', @date)";

                using (SqlCommand cmd = new SqlCommand(sqlTenant, con))
                {
                    cmd.Parameters.AddWithValue("@id", newID);
                    cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
                    cmd.ExecuteNonQuery();
                }

                // 3. Update PersonalInformation.tenantID
                string sqlUpdate = @"UPDATE PersonalInformation SET tenantID = @id WHERE personalInfoID = @id";

                using (SqlCommand cmd = new SqlCommand(sqlUpdate, con))
                {
                    cmd.Parameters.AddWithValue("@id", newID);
                    cmd.ExecuteNonQuery();
                }

                scope.Complete();
            }
        }

        // --------------------------------------------
        // UPDATE TENANT (EDIT MODE)
        // --------------------------------------------
        private void UpdateTenant(int id, string fn, string mn, string ln, string cn, string em)
        {
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();

                string sql = @"
                    UPDATE PersonalInformation SET
                        firstName = @fn,
                        middleName = @mn,
                        lastName = @ln,
                        contactNumber = @cn,
                        email = @em
                    WHERE tenantID = @id";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@fn", fn);
                    cmd.Parameters.AddWithValue("@ln", ln);
                    cmd.Parameters.AddWithValue("@cn", cn);
                    cmd.Parameters.AddWithValue("@em", em);

                    if (string.IsNullOrWhiteSpace(mn))
                        cmd.Parameters.AddWithValue("@mn", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@mn", mn);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // --------------------------------------------
        // SUBMIT BUTTON — ADD OR EDIT
        // --------------------------------------------
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string fn = tbFirstName.Text.Trim();
            string mn = tbMiddleName.Text.Trim();
            string ln = tbLastName.Text.Trim();
            string cn = tbContactNum.Text.Trim();
            string em = tbEmail.Text.Trim();

            if (string.IsNullOrEmpty(fn) || string.IsNullOrEmpty(ln) || string.IsNullOrEmpty(cn))
            {
                MessageBox.Show("First Name, Last Name, and Contact Number are required.",
                    "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (tenantIDToEdit == -1)
                {
                    InsertNewTenant(fn, mn, ln, cn, em);
                    MessageBox.Show("Tenant added successfully!");
                }
                else
                {
                    UpdateTenant(tenantIDToEdit, fn, mn, ln, cn, em);
                    MessageBox.Show("Tenant updated successfully!");
                }

                parentForm.LoadTenantsData(null, null);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
