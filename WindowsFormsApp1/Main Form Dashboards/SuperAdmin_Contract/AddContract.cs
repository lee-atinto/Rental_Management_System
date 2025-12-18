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
        private Form parentContractsForm;
        private int editContractID = 0;

        private const string STATUS_ACTIVE = "Active";
        private const string STATUS_PENDING = "Pending";
        private const string STATUS_EXPIRED = "Expired";

        private const string UNIT_VACANT = "Vacant";
        private const string UNIT_OCCUPIED = "Occupied";

        public AddContract(Form parent)
        {
            InitializeComponent();
            this.parentContractsForm = parent;
            lbTitle.Text = "Add Contract";
            this.Text = "Add New Contract";
        }

        public AddContract(int contractID)
        {
            InitializeComponent();
            this.editContractID = contractID;
            lbTitle.Text = "Edit Contract";
            this.Text = "Edit Contract Details";
        }

        public AddContract() : this(null) { }

        private void AddContract_Load(object sender, EventArgs e)
        {
            InitializeInputControls();
            LoadProperties();

            if (editContractID > 0)
            {
                LoadAllTenants();
                LoadContractDataToEdit();
            }
            else
            {
                LoadTenants();
                LoadVacantUnits();
            }
        }

        private void InitializeInputControls()
        {
            if (cbStatus != null)
            {
                cbStatus.Items.Clear();
                cbStatus.Items.Add(STATUS_ACTIVE);
                cbStatus.Items.Add(STATUS_PENDING);
                cbStatus.Items.Add(STATUS_EXPIRED);
                cbStatus.SelectedIndex = 0;
                cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            if (dtpStartDate != null) dtpStartDate.Value = DateTime.Today;
            if (dtpEndDate != null) dtpEndDate.Value = DateTime.Today.AddYears(1);

            if (cbUnit != null) cbUnit.SelectedIndexChanged += cbUnit_SelectedIndexChanged;
            if (cbProperty != null) cbProperty.SelectedIndexChanged += cbProperty_SelectedIndexChanged;

            if (tbMonthlyRent != null) tbMonthlyRent.Text = "0.00";
        }

        private void LoadContractDataToEdit()
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = @"
                    SELECT C.*, U.PropertyID 
                    FROM Contract C 
                    INNER JOIN Unit U ON C.UnitID = U.UnitID 
                    WHERE C.contractID = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", editContractID);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int propID = Convert.ToInt32(reader["PropertyID"]);
                    int unitID = Convert.ToInt32(reader["UnitID"]);

                    cbProperty.SelectedValue = propID;
                    LoadUnitsInternal(propID, unitID);

                    cbUnit.SelectedValue = unitID;
                    cbTenant.SelectedValue = reader["tenantID"];
                    dtpStartDate.Value = Convert.ToDateTime(reader["startDate"]);
                    dtpEndDate.Value = Convert.ToDateTime(reader["endDate"]);
                    cbStatus.Text = reader["contractStatus"].ToString();

                    if (cbStatus.Text == STATUS_ACTIVE)
                        dtpStartDate.Enabled = false;
                }
            }
        }

        private void LoadTenants()
        {
            string query = @"
                SELECT T.TenantID, PI.firstName + ' ' + PI.lastName AS FullName 
                FROM Tenant T 
                INNER JOIN PersonalInformation PI ON T.TenantID = PI.TenantID 
                WHERE T.tenantStatus = 'Inactive'
                ORDER BY PI.lastName";

            FillComboBox(cbTenant, query, "FullName", "TenantID", "Tenant");
        }

        private void LoadAllTenants()
        {
            string query = @"
                SELECT T.TenantID, PI.firstName + ' ' + PI.lastName AS FullName 
                FROM Tenant T 
                INNER JOIN PersonalInformation PI ON T.TenantID = PI.TenantID 
                ORDER BY PI.lastName";

            FillComboBox(cbTenant, query, "FullName", "TenantID", "Tenant");
        }

        private void LoadProperties()
        {
            string sql = "SELECT propertyID, propertyName FROM Property";
            SqlDataAdapter da = new SqlDataAdapter(sql, DataConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cbProperty.DataSource = dt;
            cbProperty.DisplayMember = "propertyName";
            cbProperty.ValueMember = "propertyID";
        }

        private void LoadVacantUnits(int propertyID = -1)
        {
            LoadUnitsInternal(propertyID, -1);
        }

        private void LoadUnitsInternal(int propertyID, int currentUnitID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("UnitID", typeof(int));
            dt.Columns.Add("UnitDetail", typeof(string));
            dt.Columns.Add("RentAmount", typeof(decimal));

            if (propertyID != -1)
            {
                string query = @"
            SELECT 
                U.UnitID,
                'Unit ' + U.UnitNumber AS UnitDetail,
                ISNULL(U.MonthlyRent, 0) AS RentAmount
            FROM Unit U
            WHERE (U.Status = @Vacant OR U.UnitID = @CurrentUnitID)
              AND U.PropertyID = @PropertyID";

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@Vacant", UNIT_VACANT);
                    da.SelectCommand.Parameters.AddWithValue("@CurrentUnitID", currentUnitID);
                    da.SelectCommand.Parameters.AddWithValue("@PropertyID", propertyID);
                    da.Fill(dt);
                }
            }

            cbUnit.DataSource = dt;
            cbUnit.DisplayMember = "UnitDetail";
            cbUnit.ValueMember = "UnitID";
        }



        private void FillComboBox(ComboBox cb, string query, string display, string value, string label)
        {
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow r = dt.NewRow();
                r[value] = DBNull.Value;
                r[display] = $"-- Select {label} --";
                dt.Rows.InsertAt(r, 0);

                cb.DataSource = dt;
                cb.DisplayMember = display;
                cb.ValueMember = value;
            }
        }

        private void DisplayUnitRent(DataRow row)
        {
            if (row == null) return;

            if (row.Table.Columns.Contains("RentAmount"))
            {
                tbMonthlyRent.Text = row["RentAmount"] != DBNull.Value
                    ? Convert.ToDecimal(row["RentAmount"]).ToString("N2")
                    : "0.00";
            }
        }

        private void cbProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandlePropertyChange();
        }

        private void cbProperty_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            HandlePropertyChange();
        }

        private void HandlePropertyChange()
        {
            if (cbProperty.SelectedValue != null &&
                int.TryParse(cbProperty.SelectedValue.ToString(), out int pid))
            {
                LoadVacantUnits(pid);
            }
            else
            {
                LoadVacantUnits();
            }
        }

        private void cbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbUnit.SelectedItem is DataRowView drv)
                DisplayUnitRent(drv.Row);
        }

        private void cbUnit_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cbUnit.SelectedItem is DataRowView drv)
                DisplayUnitRent(drv.Row);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            ClearErrorHighlights();
            if (!ValidateInput()) return;

            if (editContractID > 0)
                UpdateContractInDatabase();
            else
                SaveContractToDatabase();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void UpdateContractInDatabase()
        {
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                string query = @"UPDATE Contract 
                                 SET StartDate=@SD, EndDate=@ED, contractStatus=@ST 
                                 WHERE contractID=@ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@SD", dtpStartDate.Value.Date);
                cmd.Parameters.AddWithValue("@ED", dtpEndDate.Value.Date);
                cmd.Parameters.AddWithValue("@ST", cbStatus.Text);
                cmd.Parameters.AddWithValue("@ID", editContractID);

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Contract Updated!");
            }
        }

        private void SaveContractToDatabase()
        {
            int tenantID = Convert.ToInt32(cbTenant.SelectedValue);
            int unitID = Convert.ToInt32(cbUnit.SelectedValue);
            int propertyID = Convert.ToInt32(cbProperty.SelectedValue);

            if (cbStatus.Text == STATUS_ACTIVE && dtpStartDate.Value.Date > DateTime.Today)
            {
                MessageBox.Show("Active contracts cannot start in the future.");
                return;
            }

            string insertQuery = @"
                BEGIN TRY
                    BEGIN TRANSACTION;

                    INSERT INTO Contract (TenantID, UnitID, PropertyID, StartDate, EndDate, contractStatus)
                    VALUES (@TenantID, @UnitID, @PropertyID, @StartDate, @EndDate, @Status);

                    UPDATE Unit SET Status = @Occupied WHERE UnitID = @UnitID;

                    UPDATE Tenant 
                    SET tenantStatus = @Active, UnitID = @UnitID 
                    WHERE tenantID = @TenantID;

                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
                    THROW;
                END CATCH";

            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@TenantID", tenantID);
                cmd.Parameters.AddWithValue("@UnitID", unitID);
                cmd.Parameters.AddWithValue("@PropertyID", propertyID);
                cmd.Parameters.AddWithValue("@StartDate", dtpStartDate.Value.Date);
                cmd.Parameters.AddWithValue("@EndDate", dtpEndDate.Value.Date);
                cmd.Parameters.AddWithValue("@Status", cbStatus.Text);
                cmd.Parameters.AddWithValue("@Occupied", UNIT_OCCUPIED);
                cmd.Parameters.AddWithValue("@Active", STATUS_ACTIVE);

                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("New contract saved successfully!");
            }
        }

        private bool ValidateInput()
        {
            if (cbTenant.SelectedValue == DBNull.Value || cbUnit.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("Please fill all required fields.");
                return false;
            }

            if (dtpEndDate.Value <= dtpStartDate.Value)
            {
                MessageBox.Show("End Date must be later than Start Date.");
                return false;
            }

            return true;
        }

        private void ClearErrorHighlights() { }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            Close();
        }
    }
}
