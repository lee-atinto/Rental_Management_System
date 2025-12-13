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

        public AddContract(string username, string userRole)
        {
            InitializeComponent();
        }

        public AddContract() : this(string.Empty, string.Empty)
        {
        }

        private void AddContract_Load(object sender, EventArgs e)
        {
            InitializeInputControls();
            LoadTenants();
            LoadVacantUnits();
        }

        private void InitializeInputControls()
        {
            // Initialize Status Dropdown
            if (cbStatus != null)
            {
                cbStatus.Items.Clear();
                cbStatus.Items.Add("Active");
                cbStatus.Items.Add("Expired");
                cbStatus.Items.Add("Terminated");
                cbStatus.SelectedIndex = 0;
            }

            // Set Date Pickers
            if (dtpStartDate != null) dtpStartDate.Value = DateTime.Today;
            if (dtpEndDate != null) dtpEndDate.Value = DateTime.Today.AddYears(1);

            // Wire up the Unit ComboBox event handler
            if (cbUnit != null)
            {
                cbUnit.SelectedIndexChanged += cbUnit_SelectedIndexChanged;
            }

            // Clear Deposit Amount display upon load
            if (tbDepositAmount != null) tbDepositAmount.Text = "0.00";
        }

        // --- Data Loading Methods ---

        private void LoadTenants()
        {
            if (cbTenant == null) return;
            string query = "SELECT T.TenantID, PI.firstName + ' ' + PI.lastName AS FullName FROM Tenant T INNER JOIN PersonalInformation PI ON T.TenantID = PI.TenantID;";
            FillComboBox(cbTenant, query, "FullName", "TenantID", "Tenant List");
        }

        // Load ALL VACANT Units (Unit and Property)
        private void LoadVacantUnits()
        {
            if (cbUnit == null) return;

            // FIXES Applied: Using Unit.UnitType (column, not table), Unit.MonthlyRent, and Unit.Status
            string query = @"
                SELECT 
                    U.UnitID, 
                    P.propertyName + ' - Unit ' + U.UnitNumber + ' (' + U.UnitType + ')' AS UnitDetail,
                    U.MonthlyRent    
                FROM Unit U 
                INNER JOIN Property P ON U.PropertyID = P.propertyID 
                WHERE U.Status = 'Vacant'  
                ORDER BY P.propertyName, U.UnitNumber;";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Add a placeholder item
                    DataRow firstRow = dt.NewRow();
                    firstRow["UnitID"] = DBNull.Value;
                    firstRow["UnitDetail"] = $"-- Select a Unit (Vacant Only) --";
                    firstRow["MonthlyRent"] = 0.00M;
                    dt.Rows.InsertAt(firstRow, 0);

                    cbUnit.DataSource = dt;
                    cbUnit.DisplayMember = "UnitDetail";
                    cbUnit.ValueMember = "UnitID";
                    cbUnit.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Units: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                    // Add a placeholder item
                    DataRow firstRow = dt.NewRow();
                    firstRow[valueMember] = DBNull.Value;
                    firstRow[displayMember] = $"-- Select a {listName} --";
                    dt.Rows.InsertAt(firstRow, 0);

                    cb.DataSource = dt;
                    cb.DisplayMember = displayMember;
                    cb.ValueMember = valueMember;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading {listName}: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event Handler to Display Monthly Rent when Unit is selected
        private void cbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbDepositAmount == null) return;

            if (cbUnit.SelectedItem is DataRowView rowView)
            {
                DataRow row = rowView.Row;
                // Using correct column name 'MonthlyRent' from the Unit table data source
                if (row != null && row["MonthlyRent"] != DBNull.Value)
                {
                    decimal monthlyRent = Convert.ToDecimal(row["MonthlyRent"]);
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
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add contract: {ex.Message}\nFinal check required on Contract table columns.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }
        }

        private bool ValidateInput()
        {
            bool isValid = true;
            decimal depositAmount;

            if (cbTenant.SelectedValue == DBNull.Value || cbTenant.SelectedValue == null)
            {
                MessageBox.Show("Tenant must be selected.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                HighlightControl(cbTenant);
                isValid = false;
            }
            if (cbUnit.SelectedValue == DBNull.Value || cbUnit.SelectedValue == null)
            {
                MessageBox.Show("Unit must be selected.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                HighlightControl(cbUnit);
                isValid = false;
            }

            if (dtpStartDate.Value.Date >= dtpEndDate.Value.Date)
            {
                MessageBox.Show("End Date must be *after* Start Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                HighlightControl(dtpStartDate);
                HighlightControl(dtpEndDate);
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(tbDepositAmount.Text) ||
                !decimal.TryParse(tbDepositAmount.Text.Replace("₱", "").Replace(",", "").Trim(), out depositAmount) ||
                depositAmount <= 0)
            {
                MessageBox.Show("Deposit Amount is required and must be a positive numeric value.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                HighlightControl(tbDepositAmount);
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(cbStatus.Text))
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

            // Getting Deposit Amount
            string depositText = tbDepositAmount.Text.Replace("₱", "").Replace(",", "").Trim();
            decimal depositAmount = decimal.Parse(depositText);

            string status = cbStatus.Text;

            // Getting Monthly Rent value
            decimal monthlyRent = 0.00M;
            if (cbUnit.SelectedItem is DataRowView rowView)
            {
                DataRow row = rowView.Row;
                if (row != null && row["MonthlyRent"] != DBNull.Value)
                {
                    monthlyRent = Convert.ToDecimal(row["MonthlyRent"]);
                }
            }

            string insertQuery = @" BEGIN TRANSACTION; INSERT INTO Contract 
                                    ( TenantID, UnitID, StartDate, EndDate, contractStatus )
                                    VALUES 
                                    ( @TenantID, @UnitID, @StartDate, @EndDate, @Status );
                                    UPDATE Unit SET Status = 'Occupied' WHERE UnitID = @UnitID; COMMIT TRANSACTION ";


            // ... (The rest of the method remains the same) ...

            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@TenantID", tenantID); // Siguraduhin na ang parameter name ay TAMA
                    cmd.Parameters.AddWithValue("@UnitID", unitID);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    cmd.Parameters.AddWithValue("@DepositAmount", depositAmount);
                    cmd.Parameters.AddWithValue("@MonthlyRent", monthlyRent);
                    cmd.Parameters.AddWithValue("@Status", status);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // --- Error Highlighting Helpers ---

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
                ((DateTimePicker)control).CalendarTitleBackColor = Color.LightCoral;
                ((DateTimePicker)control).CalendarMonthBackground = Color.LightCoral;
            }
        }

        private void ClearErrorHighlights()
        {
            if (tbDepositAmount != null) tbDepositAmount.BackColor = SystemColors.Window;
            if (cbTenant != null) cbTenant.BackColor = SystemColors.Window;
            if (cbUnit != null) cbUnit.BackColor = SystemColors.Window;
            if (cbStatus != null) cbStatus.BackColor = SystemColors.Window;
            if (tbDepositAmount != null) tbDepositAmount.BackColor = SystemColors.Window;

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