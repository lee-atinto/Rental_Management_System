using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract
{
    public partial class AddContract : Form
    {
        private readonly string DataConnection = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        // Parent form (Contracts) is typically passed to enable refreshing the list after addition.
        private Form parentContractsForm;

        // Removed unused constructor parameters (username, userRole) as they aren't used in this form.
        public AddContract(Form parent)
        {
            InitializeComponent();
            this.parentContractsForm = parent;
        }

        public AddContract() : this(null)
        {
        }

        private void AddContract_Load(object sender, EventArgs e)
        {
            InitializeInputControls();
            LoadTenants();
            LoadProperties();
            LoadVacantUnits(); // Loads with default prompt
        }

        private void InitializeInputControls()
        {
            if (cbStatus != null)
            {
                cbStatus.Items.Clear();
                // Set initial status to Active for a new contract
                cbStatus.Items.Add("Active");
                cbStatus.Items.Add("Expired");
                cbStatus.Items.Add("Terminated");
                cbStatus.SelectedIndex = 0;
            }

            // Initialize dates
            if (dtpStartDate != null) dtpStartDate.Value = DateTime.Today;
            if (dtpEndDate != null) dtpEndDate.Value = DateTime.Today.AddYears(1);

            // Attach event handlers
            if (cbUnit != null)
            {
                cbUnit.SelectedIndexChanged += cbUnit_SelectedIndexChanged;
            }

            if (cbProperty != null)
            {
                cbProperty.SelectedIndexChanged += cbProperty_SelectedIndexChanged;
            }

            if (tbDepositAmount != null) tbDepositAmount.Text = "0.00";
        }

        private void LoadTenants()
        {
            if (cbTenant == null) return;
            // Only load 'Inactive' tenants, as they are available for a new contract.
            string query = @"
                SELECT 
                    T.TenantID, 
                    PI.firstName + ' ' + PI.lastName AS FullName 
                FROM Tenant T 
                INNER JOIN PersonalInformation PI ON T.TenantID = PI.TenantID 
                WHERE T.tenantStatus = 'Inactive' 
                ORDER BY PI.lastName, PI.firstName;";

            FillComboBox(cbTenant, query, "FullName", "TenantID", "Tenant List (Inactive Only)");
        }

        private void LoadProperties()
        {
            if (cbProperty == null) return;
            string query = "SELECT propertyID, propertyName FROM Property ORDER BY propertyName;";
            FillComboBox(cbProperty, query, "propertyName", "propertyID", "Property List");
        }

        private void LoadVacantUnits(int propertyID = -1)
        {
            if (cbUnit == null) return;

            // Use DataTable to hold Unit data, including MonthlyRent
            DataTable dt = new DataTable();
            dt.Columns.Add("UnitID", typeof(int));
            dt.Columns.Add("UnitDetail", typeof(string));
            dt.Columns.Add("MonthlyRent", typeof(decimal));

            // Populate default row
            DataRow firstRow = dt.NewRow();
            firstRow["UnitID"] = DBNull.Value;
            firstRow["UnitDetail"] = $"-- Select a Unit (Vacant Only) --";
            firstRow["MonthlyRent"] = 0.00M;
            dt.Rows.InsertAt(firstRow, 0);

            if (propertyID != -1)
            {
                // Optimized query to fetch vacant units for the selected property
                string query = @"
                    SELECT 
                        UnitID, 
                        'Unit ' + UnitNumber + ' (' + UnitType + ')' AS UnitDetail,
                        MonthlyRent
                    FROM Unit
                    WHERE Status = 'Vacant' AND PropertyID = @PropertyID
                    ORDER BY UnitNumber;";

                try
                {
                    using (SqlConnection con = new SqlConnection(DataConnection))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(query, con);
                        da.SelectCommand.Parameters.AddWithValue("@PropertyID", propertyID);

                        // Temporarily fill a new DataTable from the database
                        DataTable dbDt = new DataTable();
                        da.Fill(dbDt);

                        // Merge DB results into the main DT, starting after the first row (the prompt)
                        foreach (DataRow row in dbDt.Rows)
                        {
                            dt.ImportRow(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading Units: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Bind the ComboBox to the prepared DataTable
            cbUnit.DataSource = dt;
            cbUnit.DisplayMember = "UnitDetail";
            cbUnit.ValueMember = "UnitID";
            cbUnit.SelectedIndex = 0;
        }

        private void FillComboBox(ComboBox cb, string query, string displayMember, string valueMember, string listName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Insert the prompt row
                    DataRow firstRow = dt.NewRow();
                    firstRow[valueMember] = DBNull.Value;
                    firstRow[displayMember] = $"-- Select a {listName} --";

                    // Note: If the list requires a third column (like MonthlyRent), 
                    // this generic method might break unless the third column is nullable or defaulted.
                    // The LoadVacantUnits method is specialized to handle this.
                    if (dt.Columns.Contains("MonthlyRent") && firstRow.Table.Columns.Contains("MonthlyRent"))
                    {
                        firstRow["MonthlyRent"] = 0.00M;
                    }

                    dt.Rows.InsertAt(firstRow, 0);

                    cb.DataSource = dt;
                    cb.DisplayMember = displayMember;
                    cb.ValueMember = valueMember;
                    cb.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading {listName}: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProperty.SelectedValue != null && cbProperty.SelectedValue != DBNull.Value && int.TryParse(cbProperty.SelectedValue.ToString(), out int propertyID))
            {
                LoadVacantUnits(propertyID);
            }
            else
            {
                LoadVacantUnits();
            }

            // Reset deposit amount when property changes
            if (tbDepositAmount != null) tbDepositAmount.Text = "0.00";
        }


        private void cbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbDepositAmount == null) return;

            // Safely extract MonthlyRent from the selected DataRowView
            if (cbUnit.SelectedItem is DataRowView rowView)
            {
                DataRow row = rowView.Row;
                if (row != null && row["MonthlyRent"] != DBNull.Value)
                {
                    decimal monthlyRent = Convert.ToDecimal(row["MonthlyRent"]);
                    // Set deposit amount equal to one month's rent (common practice)
                    tbDepositAmount.Text = monthlyRent.ToString("N2");
                }
                else
                {
                    tbDepositAmount.Text = "0.00";
                }
            }
            else
            {
                tbDepositAmount.Text = "0.00";
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            ClearErrorHighlights();

            if (!ValidateInput())
            {
                return;
            }

            try
            {
                SaveContractToDatabase();

                // If the parent form is available, typically reload its data
                if (parentContractsForm != null)
                {
                    // This uses reflection/dynamic invocation, which can be brittle.
                    // A better approach is to define a public method in Contracts (e.g., RefreshData)
                    // and cast parentContractsForm to Contracts to call it.
                    try
                    {
                        // Example if parentContractsForm is an instance of Contracts class:
                        // (parentContractsForm as Contracts)?.LoadContractsData(); 
                    }
                    catch (Exception)
                    {
                        // Ignore refresh error
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
                MessageBox.Show("Contract successfully added, unit is Occupied, and Tenant is Active!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add contract: {ex.Message}\nPlease check required Contract table columns (DepositAmount, MonthlyRent).", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }

        private bool ValidateInput()
        {
            bool isValid = true;
            decimal depositAmount;

            // Validation 1: Tenant Selected
            if (cbTenant.SelectedValue == DBNull.Value || cbTenant.SelectedValue == null)
            {
                MessageBox.Show("Tenant must be selected.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                HighlightControl(cbTenant);
                isValid = false;
            }
            // Validation 2: Property Selected
            else if (cbProperty.SelectedValue == DBNull.Value || cbProperty.SelectedValue == null)
            {
                MessageBox.Show("Property must be selected.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                HighlightControl(cbProperty);
                isValid = false;
            }
            // Validation 3: Unit Selected
            else if (cbUnit.SelectedValue == DBNull.Value || cbUnit.SelectedValue == null)
            {
                MessageBox.Show("Unit must be selected.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                HighlightControl(cbUnit);
                isValid = false;
            }
            // Validation 4: Dates are logical
            else if (dtpStartDate.Value.Date >= dtpEndDate.Value.Date)
            {
                MessageBox.Show("End Date must be *after* Start Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                HighlightControl(dtpStartDate);
                HighlightControl(dtpEndDate);
                isValid = false;
            }
            // Validation 5: Deposit amount is valid
            else if (string.IsNullOrWhiteSpace(tbDepositAmount.Text) ||
                !decimal.TryParse(tbDepositAmount.Text.Replace("₱", "").Replace(",", "").Trim(), out depositAmount) ||
                depositAmount < 0) // Allowing 0 deposit, but typically must be > 0. Adjusted from <= 0 to < 0 to allow exactly 0.
            {
                MessageBox.Show("Deposit Amount is required and must be a non-negative numeric value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                HighlightControl(tbDepositAmount);
                isValid = false;
            }
            // Validation 6: Status Selected (Although it defaults to Active)
            else if (string.IsNullOrWhiteSpace(cbStatus.Text))
            {
                MessageBox.Show("Contract Status is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                HighlightControl(cbStatus);
                isValid = false;
            }

            return isValid;
        }

        private void SaveContractToDatabase()
        {
            int tenantID = Convert.ToInt32(cbTenant.SelectedValue);
            int unitID = Convert.ToInt32(cbUnit.SelectedValue);

            DateTime startDate = dtpStartDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date;

            string depositText = tbDepositAmount.Text.Replace("₱", "").Replace(",", "").Trim();
            decimal depositAmount = decimal.Parse(depositText);

            string status = cbStatus.Text;

            // Retrieve MonthlyRent from the Unit ComboBox DataRow
            decimal monthlyRent = 0.00M;
            if (cbUnit.SelectedItem is DataRowView rowView)
            {
                DataRow row = rowView.Row;
                if (row != null && row["MonthlyRent"] != DBNull.Value)
                {
                    monthlyRent = Convert.ToDecimal(row["MonthlyRent"]);
                }
            }

            // Multi-step Transaction Query for Atomicity
            string insertQuery = @" 
                BEGIN TRANSACTION; 
                
                -- 1. Insert New Contract Record
                INSERT INTO Contract ( 
                    TenantID, 
                    UnitID, 
                    StartDate, 
                    EndDate, 
                    contractStatus,
                    DepositAmount,
                    MonthlyRent -- Assuming your Contract table includes MonthlyRent
                )
                VALUES ( 
                    @TenantID, 
                    @UnitID, 
                    @StartDate, 
                    @EndDate, 
                    @Status,
                    @DepositAmount,
                    @MonthlyRent
                );
                
                -- 2. Update Unit Status to Occupied
                UPDATE Unit SET Status = 'Occupied' WHERE UnitID = @UnitID; 

                -- 3. Update Tenant Status to Active
                UPDATE Tenant SET tenantStatus = 'Active' WHERE tenantID = @TenantID;
                
                COMMIT TRANSACTION 
            ";

            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    // Add all necessary parameters
                    cmd.Parameters.AddWithValue("@TenantID", tenantID);
                    cmd.Parameters.AddWithValue("@UnitID", unitID);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@DepositAmount", depositAmount);
                    cmd.Parameters.AddWithValue("@MonthlyRent", monthlyRent);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Helper methods for UI feedback
        private void HighlightControl(Control control)
        {
            if (control is TextBox)
            {
                ((TextBox)control).BackColor = Color.LightCoral;
            }
            else if (control is ComboBox)
            {
                ((ComboBox)control).BackColor = Color.LightCoral;
            }
            else if (control is DateTimePicker)
            {
                // DateTimePicker highlighting is tricky. For simplicity, just set the border/background if possible.
                // Note: CalendarMonthBackground usually only shows when the dropdown calendar is open.
                ((DateTimePicker)control).CalendarTitleBackColor = SystemColors.Control;
                ((DateTimePicker)control).CalendarMonthBackground = Color.LightCoral;
            }
        }

        private void ClearErrorHighlights()
        {
            if (tbDepositAmount != null) tbDepositAmount.BackColor = SystemColors.Window;
            if (cbTenant != null) cbTenant.BackColor = SystemColors.Window;
            if (cbUnit != null) cbUnit.BackColor = SystemColors.Window;
            if (cbProperty != null) cbProperty.BackColor = SystemColors.Window;
            if (cbStatus != null) cbStatus.BackColor = SystemColors.Window;

            if (dtpStartDate != null)
            {
                dtpStartDate.CalendarTitleBackColor = SystemColors.Control;
                dtpStartDate.CalendarMonthBackground = SystemColors.Window;
            }
            if (dtpEndDate != null)
            {
                dtpEndDate.CalendarTitleBackColor = SystemColors.Control;
                dtpEndDate.CalendarMonthBackground = SystemColors.Window;
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}