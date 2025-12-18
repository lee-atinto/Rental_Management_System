using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Tenants;
using WindowsFormsApp1.Helpers;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Super_Admin_Account;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract
{
    public partial class Contracts : Form
    {
        private readonly string DataConnection = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private DataTable contractData;
        private string UserName;
        private string UserRole;

        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);


        public Contracts(string username, string userRole)
        {
            InitializeComponent();

            this.UserName = username;
            this.UserRole = userRole;

            ApplyRoleRestrictions();

            this.WindowState = FormWindowState.Maximized;

            lbName.Text = $"{username} \n{userRole}";

            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnProperties);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);
            InitializeButtonStyle(btnContracts);
            InitializeButtonStyle(btnMaintenance);

            panelHeader.BackColor = Color.White;
            lbName.BackColor = Color.FromArgb(46, 51, 73);
            PicUserProfile.Image = Properties.Resources.profile;
            PicUserProfile.BackColor = Color.FromArgb(46, 51, 73);
            SideBarBakground.BackColor = Color.FromArgb(46, 51, 73);

            int padding = 30;
            btnDashBoard.Padding = new Padding(padding, 0, 0, 0);
            btnAdminAcc.Padding = new Padding(padding, 0, 0, 0);
            btnTenant.Padding = new Padding(padding, 0, 0, 0);
            btnProperties.Padding = new Padding(padding, 0, 0, 0);
            btnViewReport.Padding = new Padding(padding, 0, 0, 0);
            btnBackUp.Padding = new Padding(padding, 0, 0, 0);
            btnPaymentRec.Padding = new Padding(padding, 0, 0, 0);
            btnContracts.Padding = new Padding(padding, 0, 0, 0);
            btnMaintenance.Padding = new Padding(padding, 0, 0, 0);

            btnDashBoard.ForeColor = Color.Black;
            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
            btnMaintenance.ForeColor = Color.Black;

            SubscribeToCrashMonitor();
            InitializeStatusFilter();

            if (tbSearch != null)
            {
                tbSearch.TextChanged += new EventHandler(tbSearch_TextChanged);
            }
        }

        private void InitializeStatusFilter()
        {
            if (cbStatus != null)
            {
                cbStatus.Items.Clear();
                cbStatus.Items.Add("All Contracts");
                cbStatus.Items.Add("Active");
                cbStatus.Items.Add("Pending");
                cbStatus.Items.Add("Expired");

                cbStatus.SelectedIndex = 0;
                cbStatus.SelectedIndexChanged += new EventHandler(cbStatus_SelectedIndexChanged);
            }
        }

        private void ApplyRoleRestrictions()
        {
            if (UserRole == "Admin")
            {
                btnBackUp.Visible = false;
                btnViewReport.Visible = false;

                panelHeader.BackColor = Color.LightBlue;
            }
            else if (UserRole == "SuperAdmin")
            {
                btnAdminAcc.Visible = true;
                btnBackUp.Visible = true;
                btnViewReport.Visible = true;

                panelHeader.BackColor = Color.White;
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

        private void LoadContractsData()
        {
            if (cbStatus?.SelectedItem == null)
            {
                return;
            }

            string selectedStatus = cbStatus.SelectedItem.ToString();
            string searchTerm = tbSearch.Text.Trim();

            contractData = GetContracts(selectedStatus, searchTerm);
            DisplayContractCards(contractData);
        }

        private void Contracts_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnContracts, activeColor);

            if (cbStatus.SelectedItem != null)
            {
                LoadContractsData();
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


        private DataTable GetContracts(string filterStatus, string searchTerm)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                try
                {
                    con.Open();
                    string query = @"
                SELECT C.contractID, C.startDate, C.endDate, U.MonthlyRent, 
                       C.contractStatus AS Status, P.propertyName, U.UnitNumber, 
                       PI.firstName AS TenantFirstName, PI.lastName AS TenantLastName, 
                       PI.contactNumber AS TenantContact
                FROM Contract C
                INNER JOIN Unit U ON C.UnitID = U.UnitID
                INNER JOIN Property P ON U.PropertyID = P.propertyID
                INNER JOIN Tenant T ON C.tenantID = T.tenantID
                INNER JOIN PersonalInformation PI ON T.tenantID = PI.tenantID 
                WHERE C.contractStatus != 'Expired'";

                    if (filterStatus != "All Contracts")
                    {
                        query = query.Replace("C.contractStatus != 'Expired'", "C.contractStatus = @Status");
                    }

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query += " AND (P.propertyName LIKE @SearchTerm OR PI.firstName LIKE @SearchTerm OR PI.lastName LIKE @SearchTerm) ";
                    }

                    query += " ORDER BY C.endDate DESC;";

                    SqlCommand cmd = new SqlCommand(query, con);
                    if (filterStatus != "All Contracts") cmd.Parameters.AddWithValue("@Status", filterStatus);
                    if (!string.IsNullOrEmpty(searchTerm)) cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return dt;
        }

        private void DisplayContractCards(DataTable dt)
        {
            flowLayoutPanelRightSideBar.Controls.Clear();

            if (dt.Rows.Count == 0)
            {
                Label noResults = new Label();
                noResults.Text = "No contracts found based on your filter and search criteria.";
                noResults.AutoSize = true;
                noResults.Font = new Font("Calibri", 14, FontStyle.Italic);
                noResults.Padding = new Padding(20);
                flowLayoutPanelRightSideBar.Controls.Add(noResults);
                return;
            }

            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    int contractID = Convert.ToInt32(row["contractID"]);
                    string propertyName = row["PropertyName"].ToString();
                    string unitNumber = row["UnitNumber"].ToString();
                    string tenantName = $"{row["TenantFirstName"]} {row["TenantLastName"]}";
                    string tenantContact = row["TenantContact"].ToString();

                    DateTime startDate = Convert.ToDateTime(row["StartDate"]);
                    DateTime endDate = Convert.ToDateTime(row["EndDate"]);
                    decimal monthlyRate = row["MonthlyRent"] != DBNull.Value ? Convert.ToDecimal(row["MonthlyRent"]) : 0.00M;
                    string status = row["Status"].ToString();


                    Panel card = CreateContractCard(
                        contractID,
                        propertyName,
                        unitNumber,
                        tenantName,
                        tenantContact,
                        startDate,
                        endDate,
                        monthlyRate,
                        status
                    );
                    flowLayoutPanelRightSideBar.Controls.Add(card);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating card for a contract: {ex.Message}", "Card Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private Panel CreateContractCard(int contractID, string propertyName, string unitNumber, string tenantName, string tenantContact, DateTime startDate, DateTime endDate, decimal monthlyRate, string status)
        {
            Panel ContractCards = new Panel();
            ContractCards.Size = new Size(363, 350);
            ContractCards.BackColor = Color.White;
            ContractCards.BorderStyle = BorderStyle.FixedSingle;
            ContractCards.Margin = new Padding(15);
            ContractCards.Tag = status;
            ContractCards.Name = $"Contract_{contractID}";

            Color statusBackColor;
            Color statusForeColor;
            switch (status.ToLower())
            {
                case "active":
                    statusBackColor = Color.FromArgb(189, 230, 191);
                    statusForeColor = Color.FromArgb(0, 128, 0);
                    break;
                case "expired":
                    statusBackColor = Color.FromArgb(255, 204, 204);
                    statusForeColor = Color.FromArgb(170, 0, 0);
                    break;
                case "pending":
                    statusBackColor = Color.FromArgb(255, 255, 153);
                    statusForeColor = Color.FromArgb(255, 140, 0);
                    break;
                default:
                    statusBackColor = Color.LightGray;
                    statusForeColor = Color.Black;
                    break;
            }

            Label lbStatus = new Label();
            lbStatus.Text = status;
            lbStatus.AutoSize = false;
            lbStatus.TextAlign = ContentAlignment.MiddleCenter;
            lbStatus.Font = new Font("Calibri", 10, FontStyle.Bold);
            lbStatus.Size = new Size(110, 25);
            lbStatus.Location = new Point(ContractCards.Width - lbStatus.Width - 15, 15);
            lbStatus.BackColor = statusBackColor;
            lbStatus.ForeColor = statusForeColor;
            lbStatus.BorderStyle = BorderStyle.None;
            ContractCards.Controls.Add(lbStatus);

            Label lbProperty = new Label();
            lbProperty.AutoSize = false;
            lbProperty.Size = new Size(225, 30);
            lbProperty.Text = propertyName;
            lbProperty.Font = new Font("Calibri", 14, FontStyle.Bold);
            lbProperty.Location = new Point(30, 15);
            ContractCards.Controls.Add(lbProperty);

            Label lbUnit = new Label();
            lbUnit.Text = $"Unit: {unitNumber}";
            lbUnit.AutoSize = true;
            lbUnit.Font = new Font("Calibri", 11, FontStyle.Regular);
            lbUnit.Location = new Point(30, 45);
            lbUnit.ForeColor = Color.DarkSlateGray;
            ContractCards.Controls.Add(lbUnit);

            Label lbTenantTitle = new Label();
            lbTenantTitle.Text = "Tenant:";
            lbTenantTitle.AutoSize = true;
            lbTenantTitle.Font = new Font("Calibri", 11, FontStyle.Bold);
            lbTenantTitle.Location = new Point(30, 85);
            ContractCards.Controls.Add(lbTenantTitle);

            Label lbTenant = new Label();
            lbTenant.Text = tenantName;
            lbTenant.AutoSize = true;
            lbTenant.Font = new Font("Calibri", 11, FontStyle.Regular);
            lbTenant.Location = new Point(90, 85);
            ContractCards.Controls.Add(lbTenant);

            Label lbContact = new Label();
            lbContact.Text = $"Contact: {tenantContact}";
            lbContact.AutoSize = true;
            lbContact.Font = new Font("Calibri", 10, FontStyle.Regular);
            lbContact.Location = new Point(30, 105);
            lbContact.ForeColor = Color.Gray;
            ContractCards.Controls.Add(lbContact);

            Label lbRateTitle = new Label();
            lbRateTitle.Text = "Monthly Rate:";
            lbRateTitle.AutoSize = true;
            lbRateTitle.Font = new Font("Calibri", 12, FontStyle.Bold);
            lbRateTitle.Location = new Point(30, 145);
            ContractCards.Controls.Add(lbRateTitle);

            Label lbRate = new Label();
            lbRate.Text = $"₱{monthlyRate:N2}";
            lbRate.AutoSize = true;
            lbRate.ForeColor = Color.FromArgb(88, 101, 242);
            lbRate.Font = new Font("Calibri", 14, FontStyle.Bold);
            lbRate.Location = new Point(30, 165);
            ContractCards.Controls.Add(lbRate);

            Label lbDuration = new Label();
            lbDuration.Text = $"Duration: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
            lbDuration.AutoSize = true;
            lbDuration.Font = new Font("Calibri", 10, FontStyle.Italic);
            lbDuration.Location = new Point(30, 200);
            lbDuration.ForeColor = Color.DarkSlateGray;
            ContractCards.Controls.Add(lbDuration);

            Panel separator = new Panel();
            separator.BackColor = Color.LightGray;
            separator.Size = new Size(ContractCards.Width - 60, 1);
            separator.Location = new Point(30, 250);
            ContractCards.Controls.Add(separator);

            Action<Button> SetActionButtonStyle = (btn) =>
            {
                btn.Size = new Size(100, 30);
                btn.Font = new Font("Calibri", 11, FontStyle.Bold);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.White;
                btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
                btn.Tag = contractID;
            };

            Button btnEdit = new Button();
            btnEdit.Text = "EDIT";
            btnEdit.Location = new Point(130, 270);
            btnEdit.ForeColor = Color.FromArgb(102, 103, 171);
            btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 255);
            SetActionButtonStyle(btnEdit);
            btnEdit.Click += new EventHandler(BtnAction_Click);
            ContractCards.Controls.Add(btnEdit);

            Button btndelete = new Button();
            btndelete.Text = "TERMINATE";
            btndelete.Location = new Point(240, 270);
            btndelete.ForeColor = Color.FromArgb(255, 0, 0);
            btndelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(251, 200, 200);
            SetActionButtonStyle(btndelete);
            btndelete.Click += new EventHandler(BtnAction_Click);
            ContractCards.Controls.Add(btndelete);

            return ContractCards;
        }

        private void DeleteContractInDatabase(int contractID)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                try
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        string getIDsQuery = "SELECT UnitID, tenantID FROM Contract WHERE contractID = @id";
                        SqlCommand cmdGetIDs = new SqlCommand(getIDsQuery, conn, transaction);
                        cmdGetIDs.Parameters.AddWithValue("@id", contractID);

                        int unitID = 0;
                        int tenantID = 0;

                        using (SqlDataReader reader = cmdGetIDs.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                unitID = Convert.ToInt32(reader["UnitID"]);
                                tenantID = Convert.ToInt32(reader["tenantID"]);
                            }
                        }

                        if (unitID != 0 && tenantID != 0)
                        {
                            // PINALITAN: UnitStatus -> Status
                            string updateUnitQuery = "UPDATE Unit SET Status = 'Vacant' WHERE UnitID = @unitID";
                            SqlCommand cmdUnit = new SqlCommand(updateUnitQuery, conn, transaction);
                            cmdUnit.Parameters.AddWithValue("@unitID", unitID);
                            cmdUnit.ExecuteNonQuery();

                            string updateTenantQuery = "UPDATE Tenant SET tenantStatus = 'Inactive' WHERE tenantID = @tenantID";
                            SqlCommand cmdTenant = new SqlCommand(updateTenantQuery, conn, transaction);
                            cmdTenant.Parameters.AddWithValue("@tenantID", tenantID);
                            cmdTenant.ExecuteNonQuery();

                            string deleteContractQuery = "DELETE FROM Contract WHERE contractID = @id";
                            SqlCommand cmdDelete = new SqlCommand(deleteContractQuery, conn, transaction);
                            cmdDelete.Parameters.AddWithValue("@id", contractID);
                            cmdDelete.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show("Success: Contract deleted, Unit is Vacant, and Tenant is now Inactive.", "System Updated");
                        }
                        else
                        {
                            transaction.Rollback();
                            MessageBox.Show("Error: Could not find linked Unit or Tenant data.");
                        }

                        LoadContractsData();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Transaction failed: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database Connection Error: " + ex.Message);
                }
            }
        }

        private void BtnAction_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            if (clickedButton.Tag == null) return;

            int contractID = (int)clickedButton.Tag;
            string action = clickedButton.Text.ToUpper();

            Panel card = (Panel)clickedButton.Parent;
            string currentStatus = card.Tag.ToString();

            switch (action)
            {
                case "VIEW":
                    MessageBox.Show($"View Contract ID: {contractID}");
                    break;
                case "EDIT":
                    using (var editForm = new AddContract(contractID))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadContractsData();
                        }
                    }
                    break;
                case "TERMINATE":
                    if (currentStatus.Equals("Active", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Hindi maaaring i-delete ang ACTIVE na kontrata dahil kasalukuyang naninirahan ang tenant. " +
                                        "Mangyaring tapusin muna ang contract o i-set ito sa Expired bago i-delete.",
                                        "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }

                    string message = $"Sigurado ka bang nais mong PERMANENTLY DELETE ang Contract #{contractID}?\n\n" +
                                     $"Status: {currentStatus}\n" +
                                     "Ang action na ito ay itatakda ang Unit bilang VACANT at ang Tenant bilang INACTIVE.";

                    if (MessageBox.Show(message, "Confirm Permanent Deletion",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        DeleteContractInDatabase(contractID);
                    }
                    break;
            }
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadContractsData();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            LoadContractsData();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (var addContractForm = new AddContract())
            {
                DialogResult result = addContractForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    LoadContractsData();
                    MessageBox.Show("New contract added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // -------------------- Button Side Bar Navigation -------------------- //

        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard dashboard = new DashBoard(UserName, UserRole);
            dashboard.Show();
            this.Hide();
        }

        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants tenant = new Tenants(UserName, UserRole);
            tenant.Show();
            this.Hide();
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            ProperTies properties = new ProperTies(UserName, UserRole);
            properties.Show();
            this.Hide();
        }

        private void btnPaymentRec_Click(object sender, EventArgs e)
        {
            Payment_Records paymentRec = new Payment_Records(UserName, UserRole);
            paymentRec.Show();
            this.Hide();
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            Maintenance maintenance = new Maintenance(UserName, UserRole);
            maintenance.Show();
            this.Hide();
        }

        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts adminAcc = new SuperAdmin_AdminAccounts(UserName, UserRole);
            adminAcc.Show();
            this.Hide();
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            if (UserRole == "SuperAdmin")
            {
                View_Reports view = new View_Reports(UserName, UserRole);
                view.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Access Denied: Admin cannot view full reports.");
            }
        }

        private void btnBackUp_Click(object sender, EventArgs e)
        {
            if (UserRole == "SuperAdmin")
            {
                BackUp backup = new BackUp(UserName, UserRole);
                backup.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Access Denied: Admin cannot access backups.");
            }
        }

        private void btnlogout_Click(object sender, EventArgs e)
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
                MessageBox.Show("Logout failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}