using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;
using WindowsFormsApp1.Super_Admin_Account;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_Properties
{
    public partial class ProperTies : Form
    {
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private string UserName;
        private string UserRole;

        private DataTable propertyData;

        // -------------------- Button Style -------------------- //
        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

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

        public ProperTies(string userName, string userRole)
        {
            InitializeComponent();

            // -------------------- Set User Information -------------------- //
            this.UserName = userName;
            this.UserRole = userRole;
            this.lbName.Text = $"{userName} \n ({userRole})";

            cbStatus.Items.Clear();
            cbStatus.Items.AddRange(new object[] { "All Status", "Vacant", "Occupied", "Under Maintenance" ,"Reserved"});
            cbStatus.SelectedIndex = 0;
            cbStatus.SelectedIndexChanged += cbStatusFilter_SelectedIndexChanged;

            LoadPropertyCards();
            ApplyRoleRestrictions();

            propertyData = GetProperties();
            DisplayPropertyCards(propertyData);

            // -------------------- Initialize Button Style -------------------- //
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

            // -------------------- Set Padding Button -------------------- //
            btnDashBoard.Padding = new Padding(30, 0, 0, 0);
            btnAdminAcc.Padding = new Padding(30, 0, 0, 0);
            btnTenant.Padding = new Padding(30, 0, 0, 0);
            btnProperties.Padding = new Padding(30, 0, 0, 0);
            btnViewReport.Padding = new Padding(30, 0, 0, 0);
            btnBackUp.Padding = new Padding(30, 0, 0, 0);
            btnPaymentRec.Padding = new Padding(30, 0, 0, 0);
            btnContracts.Padding = new Padding(30, 0, 0, 0);
            btnMaintenance.Padding = new Padding(30, 0, 0, 0);

            // -------------------- Set Color Unactive Button -------------------- //
            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnDashBoard.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
            btnMaintenance.ForeColor = Color.Black;
            btnContracts.ForeColor = Color.Black;
        }

        private DataTable GetProperties()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();
                string query = @" SELECT 
                                     P.propertyID, P.propertyName, P.AddressID, P.propertyOwnerID, 
                                     A.city, A.street, A.barangay, A.province, A.postalCode, 
                                     PO.firstName, PO.middleName, PO.lastName, PO.contactNumber, PO.Email, 
                                     U.UnitID, U.UnitNumber, U.UnitType, U.MonthlyRent AS RentAmount, U.Status
                                 FROM Property P 
                                 INNER JOIN Address A ON P.AddressID = A.AddressID 
                                 INNER JOIN PropertyOwner PO ON P.propertyOwnerID = PO.propertyOwnerID 
                                 LEFT JOIN Unit U ON P.propertyID = U.PropertyID;";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }
            return dt;
        }

        private Panel CreatePropertyCard(int propertyID, int unitID, string propertyName, string unitNumber, string rentAmount, string firstName, string lastName, string middleName, string contactNumber, string email, string street, string barangay, string city, string province, string postalCode, string unitType, string status)
        {
            Panel PropertyCards = new Panel();
            PropertyCards.Size = new Size(363, 500);
            PropertyCards.BackColor = Color.White;
            PropertyCards.BorderStyle = BorderStyle.None;
            PropertyCards.Margin = new Padding(15);
            PropertyCards.Tag = status;

            Color statusBackColor;
            Color statusForeColor;
            switch (status.ToLower())
            {
                case "vacant":
                    statusBackColor = Color.FromArgb(189, 230, 191);
                    statusForeColor = Color.FromArgb(0, 128, 0);
                    break;
                case "occupied":
                    statusBackColor = Color.FromArgb(255, 192, 203);
                    statusForeColor = Color.FromArgb(255, 0, 0);
                    break;
                case "under maintenance":
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
            lbStatus.Size = new Size(100, 25);

            lbStatus.Location = new Point(245, 185);
            lbStatus.BackColor = statusBackColor;
            lbStatus.ForeColor = statusForeColor;
            lbStatus.BorderStyle = BorderStyle.FixedSingle;
            lbStatus.Margin = new Padding(0);
            PropertyCards.Controls.Add(lbStatus);

            Label lbProperty = new Label();
            lbProperty.AutoSize = false;
            lbProperty.Size = new Size(225, 30);
            lbProperty.Text = propertyName;
            lbProperty.Font = new Font("Calibri", 14, FontStyle.Bold);
            lbProperty.Location = new Point(30, 185);
            PropertyCards.Controls.Add(lbProperty);

            Label UnitNumber = new Label();
            UnitNumber.Text = unitNumber;
            UnitNumber.AutoSize = false;
            UnitNumber.Font = new Font("Calibri", 14, FontStyle.Regular);
            UnitNumber.Location = new Point(30, 220);
            PropertyCards.Controls.Add(UnitNumber);

            Label lbAddress = new Label();
            lbAddress.Text = $"{city}{barangay}{street}";
            lbAddress.AutoSize = false;
            lbAddress.Size = new Size(300, 30);
            lbAddress.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbAddress.Location = new Point(30, 255);
            PropertyCards.Controls.Add(lbAddress);

            Label lbOwnerTitle = new Label();
            lbOwnerTitle.Text = "Owner: ";
            lbOwnerTitle.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbOwnerTitle.Location = new Point(30, 300);
            PropertyCards.Controls.Add(lbOwnerTitle);

            Label lbOwner = new Label();
            lbOwner.Text = $"{firstName}{middleName}{lastName}";
            lbOwner.Size = new Size(400, 30);
            lbOwner.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbOwner.Location = new Point(170, 300);
            PropertyCards.Controls.Add(lbOwner);

            Label lbRentTitle = new Label();
            lbRentTitle.Text = "Rent:";
            lbRentTitle.Size = new Size(190, 50);
            lbRentTitle.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbRentTitle.Location = new Point(30, 330);
            PropertyCards.Controls.Add(lbRentTitle);

            Label lbRent = new Label();
            lbRent.Text = $"{rentAmount}/mo";
            lbRent.Size = new Size(110, 50);
            lbRent.ForeColor = Color.FromArgb(102, 103, 171);
            lbRent.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbRent.Location = new Point(220, 330);
            PropertyCards.Controls.Add(lbRent);

            Label UnderLine1 = new Label();
            UnderLine1.BackColor = Color.DarkGray;
            UnderLine1.Size = new Size(lbRentTitle.Width, 2);
            UnderLine1.Location = new Point(lbRentTitle.Left, lbRentTitle.Bottom + 50);
            PropertyCards.Controls.Add(UnderLine1);

            Label UnderLine2 = new Label();
            UnderLine2.BackColor = Color.DarkGray;
            UnderLine2.Size = new Size(lbRent.Width, 2);
            UnderLine2.Location = new Point(lbRent.Left, lbRent.Bottom + 50);
            PropertyCards.Controls.Add(UnderLine2);

            Button btnView = new Button();
            btnView.Text = "View";
            btnView.Size = new Size(95, 50);
            btnView.Location = new Point(35, 440);
            btnView.Font = new Font("Calibri", 14, FontStyle.Regular);
            PropertyCards.Controls.Add(btnView);

            btnView.BackColor = Color.White;
            btnView.FlatStyle = FlatStyle.Flat;
            btnView.FlatAppearance.BorderSize = 0;
            btnView.ForeColor = Color.FromArgb(88, 101, 242);
            btnView.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnView.FlatAppearance.MouseOverBackColor = Color.FromArgb(204, 204, 255);
            PropertyCards.Controls.Add(btnView);

            btnView.Click += (s, e) =>
            {
                string fullAddress = $"{street}, {barangay}, {city}, {province} {postalCode}";

                WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Properties.View_Properties viewForm =
                    new WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Properties.View_Properties(
                        firstName,
                        middleName,
                        lastName,
                        contactNumber,
                        email,
                        fullAddress,
                        propertyName,
                        unitNumber,
                        unitType,
                        status,
                        rentAmount
                    );

                viewForm.ShowDialog();
            };

            Button btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Size = new Size(95, 50);
            btnEdit.Location = new Point(135, 440);
            btnEdit.Font = new Font("Calibri", 14, FontStyle.Regular);

            btnEdit.BackColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.ForeColor = Color.FromArgb(102, 103, 171);
            btnEdit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(204, 204, 255);

            btnEdit.Click += (s, e) =>
            {
                decimal rent = decimal.TryParse(rentAmount, out decimal parsedRent) ? parsedRent : 0;
                AddProperties editForm = new AddProperties(
                    propertyID,
                    firstName,
                    lastName,
                    middleName,
                    contactNumber,
                    email,
                    street,
                    barangay,
                    city,
                    province,
                    postalCode,
                    propertyName,
                    unitNumber,
                    rent,
                    unitID,
                    unitType,
                    status
                );

                editForm.PropertyAdded += (sender, args) =>
                {
                    propertyData = GetProperties();
                    DisplayPropertyCards(propertyData);
                };

                editForm.ShowDialog();
            };

            PropertyCards.Controls.Add(btnEdit);

            Button btnDelete = new Button();
            btnDelete.Size = new Size(95, 50);
            btnDelete.Location = new Point(235, 440);
            btnDelete.Font = new Font("Calibri", 14, FontStyle.Regular);

            try
            {
                Image original = Properties.Resources.delete;
                Image zoomed = new Bitmap(original, new Size(20, 20));
                btnDelete.Image = zoomed;
            }

            catch
            {
                btnDelete.Text = "Delete";
            }

            btnDelete.BackColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(251, 187, 185);
            PropertyCards.Controls.Add(btnDelete);

            btnDelete.Click += (s, e) =>
            {
                DialogResult result = MessageBox.Show($"Are you sure you want to delete \"{propertyName}\" and ALL its associated units and records?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes) return;

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();
                    using (SqlTransaction tx = con.BeginTransaction())
                    {
                        try
                        {
                            int ownerID = 0;
                            int addressID = 0;
                            using (SqlCommand cmdGetIDs = new SqlCommand("SELECT propertyOwnerID, AddressID FROM Property WHERE propertyID = @pid", con, tx))
                            {
                                cmdGetIDs.Parameters.AddWithValue("@pid", propertyID);
                                using (SqlDataReader reader = cmdGetIDs.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        ownerID = reader.GetInt32(0);
                                        addressID = reader.GetInt32(1);
                                    }
                                }
                            }

                            using (SqlCommand cmdRentDelete = new SqlCommand("DELETE FROM Rent WHERE PropertyID = @pid", con, tx))
                            {
                                cmdRentDelete.Parameters.AddWithValue("@pid", propertyID);
                                cmdRentDelete.ExecuteNonQuery();
                            }

                            using (SqlCommand cmdContractDelete = new SqlCommand("DELETE FROM Contract WHERE PropertyID = @pid", con, tx))
                            {
                                cmdContractDelete.Parameters.AddWithValue("@pid", propertyID);
                                cmdContractDelete.ExecuteNonQuery();
                            }

                            using (SqlCommand cmdMaintDelete = new SqlCommand("DELETE FROM MaintenanceRequest WHERE PropertyID = @pid", con, tx))
                            {
                                cmdMaintDelete.Parameters.AddWithValue("@pid", propertyID);
                                cmdMaintDelete.ExecuteNonQuery();
                            }

                            using (SqlCommand cmdUnitDelete = new SqlCommand("DELETE FROM Unit WHERE PropertyID = @pid", con, tx))
                            {
                                cmdUnitDelete.Parameters.AddWithValue("@pid", propertyID);
                                cmdUnitDelete.ExecuteNonQuery();
                            }


                            using (SqlCommand cmdProp = new SqlCommand("DELETE FROM Property WHERE propertyID = @pid", con, tx))
                            {
                                cmdProp.Parameters.AddWithValue("@pid", propertyID);
                                int affected = cmdProp.ExecuteNonQuery();
                                if (affected == 0)
                                {
                                    tx.Rollback();
                                    MessageBox.Show("Delete failed: property not found.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            using (SqlCommand cmdOwner = new SqlCommand(@" DELETE FROM PropertyOwner WHERE propertyOwnerID = @oid AND NOT EXISTS (SELECT 1 FROM Property WHERE propertyOwnerID = @oid)", con, tx))
                            {
                                cmdOwner.Parameters.AddWithValue("@oid", ownerID);
                                cmdOwner.ExecuteNonQuery();
                            }

                            using (SqlCommand cmdAddress = new SqlCommand(@" DELETE FROM Address WHERE AddressID = @aid AND NOT EXISTS (SELECT 1 FROM Property WHERE AddressID = @aid)", con, tx))
                            {
                                cmdAddress.Parameters.AddWithValue("@aid", addressID);
                                cmdAddress.ExecuteNonQuery();
                            }

                            tx.Commit();
                            propertyData = GetProperties();

                            if (flowLayoutPanelRightSideBar.InvokeRequired)
                            {
                                flowLayoutPanelRightSideBar.Invoke((Action)(() =>
                                {
                                    DisplayPropertyCards(propertyData);
                                }));
                            }
                            else
                            {
                                DisplayPropertyCards(propertyData);
                            }

                            MessageBox.Show("Property deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            try { tx.Rollback(); } catch { }
                            MessageBox.Show("Error deleting property: " + ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            };

            return PropertyCards;
        }

        private void LoadPropertyCards()
        {
            LoadPropertyCards(flowLayoutPanelRightSideBar);
        }

        private void LoadPropertyCards(FlowLayoutPanel flowLayoutPanelRightSideBar)
        {
            DataTable dt = GetProperties();
            flowLayoutPanelRightSideBar.Controls.Clear();
            flowLayoutPanelRightSideBar.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanelRightSideBar.WrapContents = true;
            flowLayoutPanelRightSideBar.AutoScroll = true;

            foreach (DataRow row in dt.Rows)
            {
                int propertyID = Convert.ToInt32(row["propertyID"]);
                int unitID = row["UnitID"] != DBNull.Value ? Convert.ToInt32(row["UnitID"]) : 0;
                string unitNumber = row["UnitNumber"] != DBNull.Value ? row["UnitNumber"].ToString() : "N/A (No Units)";
                string rentAmount = row["RentAmount"] != DBNull.Value ? row["RentAmount"].ToString() : "N/A";
                string unitType = row["UnitType"] != DBNull.Value ? row["UnitType"].ToString() : string.Empty;
                string status = row["Status"] != DBNull.Value ? row["Status"].ToString() : string.Empty;

                Panel card = CreatePropertyCard(
                    propertyID,
                    unitID,
                    row["PropertyName"].ToString(),
                    unitNumber,
                    rentAmount,
                    row["FirstName"].ToString(),
                    row["LastName"].ToString(),
                    row["MiddleName"].ToString(),
                    row["contactNumber"].ToString(),
                    row["Email"].ToString(),
                    row["Street"].ToString(),
                    row["Barangay"].ToString(),
                    row["City"].ToString(),
                    row["Province"].ToString(),
                    row["PostalCode"].ToString(),
                    unitType,
                    status
                );
                flowLayoutPanelRightSideBar.Controls.Add(card);
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

        private void ProperTies_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnProperties, activeColor);
        }

        private void DisplayPropertyCards(DataTable dt)
        {
            flowLayoutPanelRightSideBar.Controls.Clear();
            flowLayoutPanelRightSideBar.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanelRightSideBar.WrapContents = true;
            flowLayoutPanelRightSideBar.AutoScroll = true;

            foreach (DataRow row in dt.Rows)
            {
                int propertyID = Convert.ToInt32(row["propertyID"]);
                int unitID = row["UnitID"] != DBNull.Value ? Convert.ToInt32(row["UnitID"]) : 0;
                string unitNumber = row["UnitNumber"] != DBNull.Value ? row["UnitNumber"].ToString() : "N/A (No Units)";
                string rentAmount = row["RentAmount"] != DBNull.Value ? row["RentAmount"].ToString() : "N/A";
                string unitType = row["UnitType"] != DBNull.Value ? row["UnitType"].ToString() : string.Empty;
                string status = row["Status"] != DBNull.Value ? row["Status"].ToString() : string.Empty;

                Panel card = CreatePropertyCard(
                    propertyID,
                    unitID,
                    row["PropertyName"].ToString(),
                    unitNumber,
                    rentAmount,
                    row["FirstName"].ToString(),
                    row["LastName"].ToString(),
                    row["MiddleName"].ToString(),
                    row["ContactNumber"].ToString(),
                    row["Email"].ToString(),
                    row["Street"].ToString(),
                    row["Barangay"].ToString(),
                    row["City"].ToString(),
                    row["Province"].ToString(),
                    row["PostalCode"].ToString(),
                    unitType,
                    status
                );
                flowLayoutPanelRightSideBar.Controls.Add(card);

            }
        }

        private void tbSearch_TextChanged_1(object sender, EventArgs e)
        {
            string keyword = tbSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                DisplayPropertyCards(propertyData);
                return;
            }

            DataRow[] filteredRows = propertyData.Select(
                $"propertyName LIKE '%{keyword}%'"
            );

            if (filteredRows.Length > 0)
            {
                DataTable filteredDT = filteredRows.CopyToDataTable();
                DisplayPropertyCards(filteredDT);
            }
            else
            {
                flowLayoutPanelRightSideBar.Controls.Clear();
            }
        }

        private void cbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb == null) return;

            string selectedStatus = cb.SelectedItem.ToString();

            string filterExpression = "";
            if (selectedStatus != "All Status")
            {
                filterExpression = $"Status = '{selectedStatus}'";
            }

            DataRow[] filteredRows;
            if (string.IsNullOrEmpty(filterExpression))
            {
                filteredRows = propertyData.Select();
            }
            else
            {
                try
                {
                    filteredRows = propertyData.Select(filterExpression);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error applying status filter: " + ex.Message, "Filter Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (filteredRows.Length > 0)
            {
                DataTable filteredDT = filteredRows.CopyToDataTable();
                DisplayPropertyCards(filteredDT);
            }
            else
            {
                flowLayoutPanelRightSideBar.Controls.Clear();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddProperties add = new AddProperties();

            add.FormClosed += (s, args) =>
            {
                propertyData = GetProperties();
                DisplayPropertyCards(propertyData);
                this.Show();
            };

            add.ShowDialog();
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

        // -------------------- Button Side Bar -------------------- //

        // --------------- Dashboard Button --------------- //
        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard dashboard = new DashBoard(UserName, UserRole);
            dashboard.Show();
            this.Hide();
        }

        // --------------- Tenant Button --------------- //
        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants tenants = new Tenants(UserName, UserRole);
            tenants.Show();
            this.Hide();
        }

        // --------------- Payment Record Button --------------- //
        private void btnPaymentRec_Click(object sender, EventArgs e)
        {
            Payment_Records paymentRec = new Payment_Records(UserName, UserRole);
            paymentRec.Show();
            this.Hide();
        }

        // --------------- Contract Button --------------- //
        private void btnContracts_Click(object sender, EventArgs e)
        {
            Contracts contract = new Contracts(UserName, UserRole);
            contract.Show();
            this.Hide();
        }

        // --------------- Maintenances Button --------------- //
        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            Maintenance maintenance = new Maintenance(UserName, UserRole);
            maintenance.Show();
            this.Hide();
        }

        // --------------- Admin Accoutn Button --------------- //
        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts adminAcc = new SuperAdmin_AdminAccounts(UserName, UserRole);
            adminAcc.Show();
            this.Hide();
        }

        // --------------- Vuew Reports Button --------------- //
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

        // --------------- Baskup Button --------------- //
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

        // --------------- Logout Button --------------- //
        private void btnlogout_Click(object sender, EventArgs e)
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
    }
}