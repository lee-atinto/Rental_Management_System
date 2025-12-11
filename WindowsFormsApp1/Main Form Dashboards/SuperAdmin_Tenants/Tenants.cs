using Rental_Management_System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Tenants;
using WindowsFormsApp1.Login_ResetPassword;

namespace WindowsFormsApp1.Super_Admin_Account
{
    public partial class Tenants : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private readonly string Username;
        private readonly string UserRole;

        private readonly Color activeColor = Color.FromArgb(46, 51, 73);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        public Tenants(string username, string userRole)
        {
            InitializeComponent();
            this.Username = username;
            this.UserRole = userRole;
            this.lbName.Text = $"{Username} \n ({UserRole})";


            this.TenantsData.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataTenants_CellFormatting);

            if (!comboBox1.Items.Contains("All"))
            {
                comboBox1.Items.Insert(0, "All");
            }

            comboBox1.SelectedIndex = 0;

            LoadTenantsData(comboBox1.SelectedItem?.ToString());

            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);
            InitializeButtonStyle(btnProperties);

            panelHeader.BackColor = Color.White;
            PanelBackGroundProfile.BackColor = Color.FromArgb(46, 51, 73);

            btnDashBoard.Padding = new Padding(30, 0, 0, 0);
            btnAdminAcc.Padding = new Padding(30, 0, 0, 0);
            btnTenant.Padding = new Padding(30, 0, 0, 0);
            btnPaymentRec.Padding = new Padding(30, 0, 0, 0);
            btnViewReport.Padding = new Padding(30, 0, 0, 0);
            btnBackUp.Padding = new Padding(30, 0, 0, 0);
            btnProperties.Padding = new Padding(30, 0, 0, 0);

            btnDashBoard.ForeColor = Color.Black;
            btnAdminAcc.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;

            TenantsData.BorderStyle = BorderStyle.None;
            TenantsData.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            TenantsData.RowHeadersVisible = false;
            TenantsData.EnableHeadersVisualStyles = false;
            TenantsData.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            Color defaultBG = Color.FromArgb(240, 240, 240);
            TenantsData.BackgroundColor = defaultBG;
            TenantsData.DefaultCellStyle.BackColor = Color.White;
            TenantsData.AlternatingRowsDefaultCellStyle.BackColor = defaultBG;

            TenantsData.RowTemplate.Height = 60;

            DataGridViewButtonColumn viewBtnCol = new DataGridViewButtonColumn();
            viewBtnCol.Name = "Edit";
            viewBtnCol.Text = "Edit";
            viewBtnCol.UseColumnTextForButtonValue = true;
            TenantsData.Columns.Add(viewBtnCol);

            DataGridViewButtonColumn editBtnCol = new DataGridViewButtonColumn();
            editBtnCol.Name = "Delete";
            editBtnCol.Text = "Delete";
            editBtnCol.UseColumnTextForButtonValue = true;
            TenantsData.Columns.Add(editBtnCol);
        }

        private void Tenants_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnTenant, activeColor);

            comboBox1.Items.Add("Active");
            comboBox1.Items.Add("Inactive");
            comboBox1.SelectedIndex = 0;
        }

        public void LoadTenantsData(string statusFilter = null, string searchText = null)
        {
            List<string> whereConditions = new List<string>();

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
            {
                whereConditions.Add("T.tenantStatus = @StatusFilter");
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                whereConditions.Add("(P.firstName + ' ' + P.lastName LIKE @SearchText OR P.contactNumber LIKE @SearchText)");
            }

            string whereClause = whereConditions.Any() ? " WHERE " + string.Join(" AND ", whereConditions) : "";
            string query = $@" SELECT P.personalInfoID AS TenantID, P.firstName + ' ' + P.lastName AS TenantName, P.contactNumber AS Contact, T.tenantStatus AS Status, T.dateRegistered AS DateRegistered FROM PersonalInformation P INNER JOIN Tenant T ON P.tenantID = T.tenantID {whereClause}";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
                    {
                        command.Parameters.AddWithValue("@StatusFilter", statusFilter);
                    }
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        command.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                    }

                    DataTable tenantTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(tenantTable);

                    TenantsData.DataSource = tenantTable;
                    ConfigureTenantDataGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteTenant(string tenantId, string tenantName)
        {
            string deletePersonalInfoQuery = "DELETE FROM PersonalInformation WHERE tenantID = @TenantID";
            string deleteTenantQuery = "DELETE FROM Tenant WHERE tenantID = @TenantID";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    using (SqlCommand command1 = new SqlCommand(deletePersonalInfoQuery, connection, transaction))
                    {
                        command1.Parameters.AddWithValue("@TenantID", tenantId);
                        command1.ExecuteNonQuery();
                    }

                    int rowsAffected;
                    using (SqlCommand command2 = new SqlCommand(deleteTenantQuery, connection, transaction))
                    {
                        command2.Parameters.AddWithValue("@TenantID", tenantId);
                        rowsAffected = command2.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"{tenantName} has been successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadTenantsData(comboBox1.SelectedItem?.ToString(), tbSearch.Text);
                    }
                    else
                    {
                        MessageBox.Show("Could not find the tenant to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Deletion failed due to: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InitializeButtonStyle(Button button)
        {
            if (button != null)
            {
                button.TabStop = false;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = Color.FromArgb(46, 51, 73);
                button.Padding = new Padding(50, 5, 5, 5);

                button.BackColor = defaultBackColor;
                button.FlatAppearance.MouseDownBackColor = defaultBackColor;
                button.FlatAppearance.MouseOverBackColor = defaultBackColor;

                button.TextAlign = ContentAlignment.MiddleLeft;
                button.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void SetButtonActiveStyle(Button button, Color backColor)
        {
            if (button != null)
            {
                button.BackColor = backColor;
                button.FlatAppearance.MouseDownBackColor = backColor;
                button.FlatAppearance.MouseOverBackColor = backColor;
            }
        }

        private void ConfigureTenantDataGrid()
        {
            if (TenantsData.DataSource == null || TenantsData.Columns.Count == 0)
                return;

            TenantsData.ReadOnly = true;
            TenantsData.AllowUserToAddRows = false;
            TenantsData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (TenantsData.Columns.Contains("TenantID"))
                TenantsData.Columns["TenantID"].Visible = false;

            if (TenantsData.Columns.Contains("TenantName"))
            {
                TenantsData.Columns["TenantName"].HeaderText = "Name";
                TenantsData.Columns["TenantName"].DisplayIndex = 0;
            }

            if (TenantsData.Columns.Contains("Contact"))
            {
                TenantsData.Columns["Contact"].HeaderText = "Contact";
                TenantsData.Columns["Contact"].DisplayIndex = 1;
            }

            if (TenantsData.Columns.Contains("Status"))
            {
                TenantsData.Columns["Status"].HeaderText = "Status";
                TenantsData.Columns["Status"].DisplayIndex = 2;
            }

            if (TenantsData.Columns.Contains("DateRegistered"))
            {
                TenantsData.Columns["DateRegistered"].HeaderText = "Date Registered";
                TenantsData.Columns["DateRegistered"].DisplayIndex = 3;
            }

            if (TenantsData.Columns.Contains("Edit"))
                TenantsData.Columns["Edit"].DisplayIndex = 4;

            if (TenantsData.Columns.Contains("Delete"))
                TenantsData.Columns["Delete"].DisplayIndex = 5;
        }

        private void DataTenants_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && TenantsData.Columns.Contains("Status"))
            {
                DataGridViewRow row = TenantsData.Rows[e.RowIndex];
                object statusValue = row.Cells["Status"].Value;

                if (statusValue != null && statusValue is string status)
                {
                    Color rowColor;
                    if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                    {
                        rowColor = Color.LightGreen;
                    }
                    else
                    {
                        rowColor = Color.LightCoral;
                    }

                    row.DefaultCellStyle.BackColor = rowColor;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = TenantsData.DefaultCellStyle.BackColor;
                }
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            AddTenants addTenants = new AddTenants(this);
            addTenants.ShowDialog();
        }

        private void TenantsData_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string tenantIdString = TenantsData.Rows[e.RowIndex].Cells["TenantID"].Value?.ToString();
            string tenantName = TenantsData.Rows[e.RowIndex].Cells["TenantName"].Value?.ToString();
            string clickedColumnName = TenantsData.Columns[e.ColumnIndex].Name;

            if (string.IsNullOrEmpty(tenantIdString) || !int.TryParse(tenantIdString, out int tenantID))
            {
                MessageBox.Show("Invalid Tenant ID found in the row.", "Data Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clickedColumnName == "Edit")
            {
                AddTenants editForm = new AddTenants(this, tenantID);
                editForm.ShowDialog();
            }
            else if (clickedColumnName == "Delete")
            {
                if (MessageBox.Show($"Are you sure you want to delete {tenantName}?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteTenant(tenantID.ToString(), tenantName);
                }
            }

            if (e.RowIndex < 0 || !(TenantsData.Columns[e.ColumnIndex] is DataGridViewButtonColumn))
            {
                return;
            }
        }

        private void tbSearch_TextChanged_1(object sender, EventArgs e)
        {
            string searchValue = tbSearch.Text;
            string selectedStatus = comboBox1.SelectedItem?.ToString();
            LoadTenantsData(selectedStatus, searchValue);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedStatus = comboBox1.SelectedItem?.ToString();
            LoadTenantsData(selectedStatus, tbSearch.Text);
        }

        // -------------------- Dashboard Buttons Click Event -------------------- //
        private void btnDashBoard_Click_1(object sender, EventArgs e)
        {
            DashBoard dashboard = new DashBoard(Username, UserRole);
            dashboard.Show();
            this.Hide();
        }

        // -------------------- Admin Accounts Buttons Click Event -------------------- //
        private void btnAdminAcc_Click_1(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts AdminAccounts = new SuperAdmin_AdminAccounts(Username, UserRole);
            AdminAccounts.Show();
            this.Hide();
        }

        // -------------------- Properties Buttons Click Event -------------------- //
        private void btnProperties_Click(object sender, EventArgs e)
        {
            ProperTies properties = new ProperTies(Username, UserRole);
            properties.Show();
            this.Hide();
        }

        // -------------------- Payment Records Buttons Click Event -------------------- //
        private void btnPaymentRec_Click(object sender, EventArgs e)
        {
            Payment_Records payment = new Payment_Records(Username, UserRole);
            payment.Show();
            this.Hide();
        }

        // -------------------- Logout Buttons Click Event -------------------- //
        private void btnlogout_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = "UPDATE Account SET active = 0 WHERE username=@u";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", this.Username);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            this.Hide();
            new LoginPage().Show();
        }
    }
}