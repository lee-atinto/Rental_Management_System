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
using WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Tenants;
using WindowsFormsApp1.Helpers;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.Super_Admin_Account
{
    public partial class Tenants : Form
    {
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private readonly string UserName;
        private readonly string UserRole;

        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        public string CurrentStatusFilter
        {
            get { return comboBox1.SelectedItem?.ToString(); }
        }
        public string CurrentSearchText
        {
            get { return tbSearch.Text; }
        }

        public Tenants(string username, string userRole)
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            this.UserName = username;
            this.UserRole = userRole;
            this.lbName.Text = $"{UserName} \n ({UserRole})";

            this.TenantsData.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DataTenants_CellFormatting);

            if (!comboBox1.Items.Contains("All Statuses"))
            {
                comboBox1.Items.Insert(0, "All Statuses");
            }

            comboBox1.SelectedIndex = 0;

            LoadTenantsData(comboBox1.SelectedItem?.ToString());

            // Initialize Menu Buttons
            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);
            InitializeButtonStyle(btnProperties);
            InitializeButtonStyle(btnContracts);
            InitializeButtonStyle(btnMaintenance);

            SubscribeToCrashMonitor();
            ApplyRoleRestrictions();

            // UI Styling
            panelHeader.BackColor = Color.White;
            lbName.BackColor = Color.FromArgb(46, 51, 73);
            PicUserProfile.Image = Properties.Resources.profile;
            PicUserProfile.BackColor = Color.FromArgb(46, 51, 73);
            SideBarBakground.BackColor = Color.FromArgb(46, 51, 73);

            // Set Button Padding for cleaner look
            btnDashBoard.Padding = new Padding(30, 0, 0, 0);
            btnAdminAcc.Padding = new Padding(30, 0, 0, 0);
            btnTenant.Padding = new Padding(30, 0, 0, 0);
            btnPaymentRec.Padding = new Padding(30, 0, 0, 0);
            btnViewReport.Padding = new Padding(30, 0, 0, 0);
            btnBackUp.Padding = new Padding(30, 0, 0, 0);
            btnProperties.Padding = new Padding(30, 0, 0, 0);
            btnContracts.Padding = new Padding(30, 0, 0, 0);
            btnMaintenance.Padding = new Padding(30, 0, 0, 0);

            // Set Default Button Text Colors
            btnDashBoard.ForeColor = Color.Black;
            btnAdminAcc.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;
            btnContracts.ForeColor = Color.Black;
            btnMaintenance.ForeColor = Color.Black;

            // Grid Styling
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

            if (!TenantsData.Columns.Contains("Edit"))
            {
                DataGridViewButtonColumn viewBtnCol = new DataGridViewButtonColumn();
                viewBtnCol.Name = "Edit";
                viewBtnCol.Text = "Edit";
                viewBtnCol.UseColumnTextForButtonValue = true;
                TenantsData.Columns.Add(viewBtnCol);
            }

            if (!TenantsData.Columns.Contains("Delete"))
            {
                DataGridViewButtonColumn editBtnCol = new DataGridViewButtonColumn();
                editBtnCol.Name = "Delete";
                editBtnCol.Text = "Delete";
                editBtnCol.UseColumnTextForButtonValue = true;
                TenantsData.Columns.Add(editBtnCol);
            }
        }

        private void SubscribeToCrashMonitor()
        {
            GlobalCrashMonitor.Instance.OnCriticalDataMissing += ShowCriticalAlert;
        }

        private void ShowCriticalAlert(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ShowCriticalAlert(message)));
                return;
            }

            MessageBox.Show(
                $"System Alert: {message}",
                "Critical Data Missing / Crash Detected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void Tenants_Load(object sender, EventArgs e)
        {

            SetButtonActiveStyle(btnTenant, activeColor);

            if (!comboBox1.Items.Contains("Active")) comboBox1.Items.Add("Active");
            if (!comboBox1.Items.Contains("Inactive")) comboBox1.Items.Add("Inactive");
            if (!comboBox1.Items.Contains("Move-Out")) comboBox1.Items.Add("Move-Out");
            if (!comboBox1.Items.Contains("Evicted")) comboBox1.Items.Add("Evicted");

            if (comboBox1.SelectedIndex < 0)
                comboBox1.SelectedIndex = 0;
        }

        public void LoadTenantsData(string statusFilter = null, string searchText = null)
        {
            string syncStatusQuery = @"
    UPDATE Tenant 
    SET tenantStatus = 'Inactive', 
        UnitID = NULL
    WHERE tenantID NOT IN (
        SELECT tenantID FROM Contract WHERE contractStatus = 'Active'
    );";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                {
                    connection.Open();

                    using (SqlCommand syncCmd = new SqlCommand(syncStatusQuery, connection))
                    {
                        syncCmd.ExecuteNonQuery();
                    }

                    List<string> whereConditions = new List<string>();

                    string query = $@" 
            SELECT 
                T.tenantID AS TenantID, 
                T.tenantID_New AS [Tenant ID New], 
                ISNULL(P.firstName + ' ' + P.lastName, 'No Name Provided') AS TenantName, 
                ISNULL(P.contactNumber, 'N/A') AS Contact, 
                T.tenantStatus AS Status, 
                T.dateRegistered AS DateRegistered 
            FROM Tenant T 
            LEFT JOIN PersonalInformation P ON T.tenantID = P.tenantID";

                    if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All Statuses")
                    {
                        whereConditions.Add("T.tenantStatus = @StatusFilter");
                    }

                    if (!string.IsNullOrEmpty(searchText))
                    {
                        whereConditions.Add("(P.firstName + ' ' + P.lastName LIKE @SearchText OR P.contactNumber LIKE @SearchText OR T.tenantID_New LIKE @SearchText)");
                    }

                    if (whereConditions.Any())
                    {
                        query += " WHERE " + string.Join(" AND ", whereConditions);
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All Statuses")
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading tenant data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteTenant(string tenantId, string tenantName)
        {
            string deletePayments = "DELETE FROM Payment WHERE tenantID = @TenantID";
            string deleteContracts = "DELETE FROM Contract WHERE tenantID = @TenantID";
            string deleteMaintenance = "DELETE FROM MaintenanceRequest WHERE tenantID = @TenantID";
            string deleteRent = "DELETE FROM Rent WHERE tenantID = @TenantID";
            string deletePersonalInfo = "DELETE FROM PersonalInformation WHERE tenantID = @TenantID";
            string deleteTenant = "DELETE FROM Tenant WHERE tenantID = @TenantID";

            using (SqlConnection connection = new SqlConnection(DataConnection))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.Parameters.AddWithValue("@TenantID", tenantId);

                        cmd.CommandText = deletePayments; cmd.ExecuteNonQuery();
                        cmd.CommandText = deleteContracts; cmd.ExecuteNonQuery();
                        cmd.CommandText = deleteMaintenance; cmd.ExecuteNonQuery();
                        cmd.CommandText = deleteRent; cmd.ExecuteNonQuery();
                        cmd.CommandText = deletePersonalInfo; cmd.ExecuteNonQuery();
                        cmd.CommandText = deleteTenant;

                        int rowsAffected = cmd.ExecuteNonQuery();

                        transaction.Commit();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Tenant '{tenantName}' and all associated records have been successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadTenantsData(comboBox1.SelectedItem?.ToString(), tbSearch.Text);
                        }
                        else
                        {
                            MessageBox.Show("The specified tenant record could not be found.", "Record Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    try { transaction.Rollback(); } catch { }
                    MessageBox.Show($"A critical error occurred during the deletion process: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                button.ForeColor = Color.White;
            }
        }

        private void ConfigureTenantDataGrid()
        {
            if (TenantsData.DataSource == null || TenantsData.Columns.Count == 0)
                return;

            TenantsData.ReadOnly = true;
            TenantsData.AllowUserToAddRows = false;
            TenantsData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (TenantsData.Columns.Contains("TenantID")) TenantsData.Columns["TenantID"].Visible = false;
            if (TenantsData.Columns.Contains("Tenant ID New")) TenantsData.Columns["Tenant ID New"].Visible = false;

            if (TenantsData.Columns.Contains("TenantName"))
            {
                TenantsData.Columns["TenantName"].HeaderText = "Full Name";
                TenantsData.Columns["TenantName"].DisplayIndex = 0;
            }

            if (TenantsData.Columns.Contains("Contact"))
            {
                TenantsData.Columns["Contact"].HeaderText = "Contact Number";
                TenantsData.Columns["Contact"].DisplayIndex = 1;
            }

            if (TenantsData.Columns.Contains("Status"))
            {
                TenantsData.Columns["Status"].HeaderText = "Current Status";
                TenantsData.Columns["Status"].DisplayIndex = 2;
            }

            if (TenantsData.Columns.Contains("DateRegistered"))
            {
                TenantsData.Columns["DateRegistered"].HeaderText = "Date Registered";
                TenantsData.Columns["DateRegistered"].DisplayIndex = 3;
            }

            if (TenantsData.Columns.Contains("Edit")) TenantsData.Columns["Edit"].DisplayIndex = 4;
            if (TenantsData.Columns.Contains("Delete")) TenantsData.Columns["Delete"].DisplayIndex = 5;

            foreach (DataGridViewColumn column in TenantsData.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void DataTenants_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (TenantsData.Columns[e.ColumnIndex] is DataGridViewButtonColumn) return;

            if (TenantsData.Columns.Contains("Status"))
            {
                DataGridViewRow row = TenantsData.Rows[e.RowIndex];
                object statusValue = row.Cells["Status"].Value;

                if (statusValue != null && statusValue is string status)
                {
                    Color rowColor;
                    if (status.Equals("Active", StringComparison.OrdinalIgnoreCase)) rowColor = Color.LightGreen;
                    else if (status.Equals("Inactive", StringComparison.OrdinalIgnoreCase)) rowColor = Color.LightCoral;
                    else if (status.Equals("Move-Out", StringComparison.OrdinalIgnoreCase)) rowColor = Color.LightSkyBlue;
                    else if (status.Equals("Evicted", StringComparison.OrdinalIgnoreCase)) rowColor = Color.Orange;
                    else rowColor = Color.White;

                    row.DefaultCellStyle.BackColor = rowColor;
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
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (!(TenantsData.Columns[e.ColumnIndex] is DataGridViewButtonColumn)) return;

            // Kunin ang pangalan ng column na pinindot
            string clickedColumnName = TenantsData.Columns[e.ColumnIndex].Name;

            // --- DAGDAG: SECURITY CHECK PARA SA DELETE ---
            if (clickedColumnName == "Delete" && UserRole == "Admin")
            {
                MessageBox.Show("Access Denied: Only SuperAdmins are allowed to delete tenant records.",
                                "Restricted Action", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return; // Hihinto na dito ang code, hindi na tutuloy sa delete logic
            }

            // Pagpapatuloy ng normal na logic para sa ID at Name
            var idCell = TenantsData.Rows[e.RowIndex].Cells["TenantID"];
            var nameCell = TenantsData.Rows[e.RowIndex].Cells["TenantName"];

            if (idCell == null)
            {
                MessageBox.Show("Error: Tenant ID column missing.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string tenantIdString = idCell.Value?.ToString();
            string tenantName = nameCell?.Value?.ToString() ?? "Unknown Tenant";

            if (string.IsNullOrEmpty(tenantIdString) || !int.TryParse(tenantIdString, out int tenantID))
            {
                MessageBox.Show("The selected row contains an invalid Tenant ID.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Action Logic
            if (clickedColumnName == "Edit")
            {
                AddTenants editForm = new AddTenants(this, tenantID);
                editForm.ShowDialog();
            }
            else if (clickedColumnName == "Delete")
            {
                // Dahil na-filter na natin si Admin sa taas, sigurado tayong SuperAdmin ang nandito
                if (MessageBox.Show($"Are you sure you want to permanently delete {tenantName}? \n\nThis action will remove all associated payment and contract history.",
                    "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteTenant(tenantID.ToString(), tenantName);
                }
            }
        }

        private void ApplyRoleRestrictions()
        {
            if (UserRole == "Admin")
            {
                btnBackUp.Visible = false;
                btnViewReport.Visible = false;
                panelHeader.BackColor = Color.LightBlue;

                if (TenantsData.Columns.Contains("Delete"))
                {
                    TenantsData.Columns["Delete"].Visible = false;
                }
            }
            else if (UserRole == "SuperAdmin")
            {
                btnAdminAcc.Visible = true;
                btnBackUp.Visible = true;
                btnViewReport.Visible = true;
                panelHeader.BackColor = Color.White;
            }
        }

        private void tbSearch_TextChanged_1(object sender, EventArgs e)
        {
            LoadTenantsData(comboBox1.SelectedItem?.ToString(), tbSearch.Text);
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadTenantsData(comboBox1.SelectedItem?.ToString(), tbSearch.Text);
        }

        private void btnDashBoard_Click(object sender, EventArgs e) { NavigateTo(new DashBoard(UserName, UserRole)); }
        private void btnProperties_Click_1(object sender, EventArgs e) { NavigateTo(new ProperTies(UserName, UserRole)); }
        private void btnPaymentRec_Click_1(object sender, EventArgs e) { NavigateTo(new Payment_Records(UserName, UserRole)); }
        private void btnContracts_Click(object sender, EventArgs e) { NavigateTo(new Contracts(UserName, UserRole)); }
        private void btnMaintenance_Click(object sender, EventArgs e) { NavigateTo(new Maintenance(UserName, UserRole)); }
        private void btnAdminAcc_Click(object sender, EventArgs e) { NavigateTo(new SuperAdmin_AdminAccounts(UserName, UserRole)); }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            if (UserRole == "SuperAdmin") NavigateTo(new View_Reports(UserName, UserRole));
            else MessageBox.Show("Access Denied: You do not have permission to view administrative reports.", "Permission Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void btnBackUp_Click(object sender, EventArgs e)
        {
            if (UserRole == "SuperAdmin") NavigateTo(new BackUp(UserName, UserRole));
            else MessageBox.Show("Access Denied: Database backup tools are restricted to SuperAdmins.", "Permission Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void NavigateTo(Form targetForm)
        {
            targetForm.Show();
            this.Hide();
        }

        private void btnlogout_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection))
                {
                    string query = "UPDATE Account SET active = 0 WHERE username=@u";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@u", this.UserName);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                this.Hide();
                new LoginPage().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Logout failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}