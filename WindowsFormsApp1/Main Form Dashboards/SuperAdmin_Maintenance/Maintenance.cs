using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Helpers;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Maintenance;
using WindowsFormsApp1.Super_Admin_Account;

namespace WindowsFormsApp1.Main_Form_Dashboards
{
    public partial class Maintenance : Form
    {
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private string UserName;
        private string UserRole;

        private readonly Color activeColor = Color.FromArgb(56, 55, 83);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        public Maintenance(string username, string userRole)
        {
            InitializeComponent();

            this.UserName = username;
            this.UserRole = userRole;

            lbName.Text = $"{username} \n{userRole}";

            LoadPropertyCards();
            DisplayDashboardValues();

            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnProperties);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);
            InitializeButtonStyle(btnContracts);
            InitializeButtonStyle(btnMaintenance);

            SubscribeToCrashMonitor();
            ApplyRoleRestrictions();

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
            btnContracts.ForeColor = Color.Black;

            if (btnAddRequest != null)
            {
                btnAddRequest.Click += btnAddRequest_Click;
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

        private Dictionary<string, int> GetDashboardValues()
        {
            var values = new Dictionary<string, int>();

            string totalRequestsQuery = "SELECT COUNT(*) FROM MaintenanceRequest";
            string pendingQuery = "SELECT COUNT(*) FROM MaintenanceRequest WHERE Status = 'Pending'";
            string completedQuery = "SELECT COUNT(*) FROM MaintenanceRequest WHERE Status = 'Completed'";
            string thisMonthQuery = @"SELECT COUNT(*) FROM MaintenanceRequest WHERE DATENAME(month, requestDate) = DATENAME(month, GETDATE()) AND DATENAME(year, requestDate) = DATENAME(year, GETDATE())";

            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                try
                {
                    con.Open();
                    values["TotalRequests"] = (int)new SqlCommand(totalRequestsQuery, con).ExecuteScalar();
                    values["Pending"] = (int)new SqlCommand(pendingQuery, con).ExecuteScalar();
                    values["Completed"] = (int)new SqlCommand(completedQuery, con).ExecuteScalar();
                    values["ThisMonth"] = (int)new SqlCommand(thisMonthQuery, con).ExecuteScalar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching dashboard data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return values;
        }

        private void DisplayDashboardValues()
        {
            var data = GetDashboardValues();
            if (lbTotalRecquest != null) lbTotalRecquest.Text = data["TotalRequests"].ToString();
            if (lbPending != null) lbPending.Text = data["Pending"].ToString();
            if (lbCompleted != null) lbCompleted.Text = data["Completed"].ToString();
            if (lbThisMonth != null) lbThisMonth.Text = data["ThisMonth"].ToString();
        }

        private DataTable GetMaintenance()
        {
            DataTable dt = new DataTable();
            string query = @" 
        SELECT 
            MR.maintenanceReqID, 
            MR.description, 
            MR.requestDate, 
            MR.Status AS MaintenanceStatus,
            U.UnitNumber,
            PI.firstName, 
            PI.middleName, 
            PI.lastName,
            MR.propertyID, 
            RT.typeName
        FROM MaintenanceRequest AS MR 
        LEFT JOIN Tenant AS T ON MR.TenantID = T.TenantID
        LEFT JOIN PersonalInformation AS PI ON T.TenantID = PI.TenantID 
        LEFT JOIN Contract AS C ON T.TenantID = C.TenantID 
        LEFT JOIN Unit AS U ON C.UnitID = U.UnitID
        LEFT JOIN RequestType AS RT ON MR.requestTypeID = RT.requestTypeID
        
        ORDER BY MR.requestDate DESC;
    ";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();
                    new SqlDataAdapter(query, con).Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        private void LoadMaintenanceCards(FlowLayoutPanel flowLayoutPanelRightSideBar)
        {
            DataTable dt = GetMaintenance();
            flowLayoutPanelRightSideBar.Controls.Clear();
            flowLayoutPanelRightSideBar.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelRightSideBar.WrapContents = false;
            flowLayoutPanelRightSideBar.AutoScroll = true;

            foreach (DataRow row in dt.Rows)
            {
                int requestID = Convert.ToInt32(row["maintenanceReqID"]);
                Panel card = CreateMaintenanceCard(
                    requestID,
                    row["description"].ToString(),
                    row["requestDate"].ToString(),
                    row["MaintenanceStatus"].ToString(),
                    row["unitNumber"].ToString(),
                    row["firstName"].ToString(),
                    row["lastName"].ToString(),
                    row["middleName"].ToString(),
                    row["typeName"].ToString()
                );
                flowLayoutPanelRightSideBar.Controls.Add(card);
            }
        }

        private void LoadPropertyCards()
        {
            LoadMaintenanceCards(flowLayoutPanelRightSideBar);
        }

        private Panel CreateMaintenanceCard(int requestID, string description, string requestDate, string maintenanceStatus, string unitNumber, string firstName, string lastName, string middleName, string typeName)
        {
            Panel MaintenanceCards = new Panel();
            MaintenanceCards.Size = new Size(1130, 180);
            MaintenanceCards.BackColor = Color.White;
            MaintenanceCards.BorderStyle = BorderStyle.None;
            MaintenanceCards.Margin = new Padding(15);


            Label lbStatus = new Label();
            lbStatus.AutoSize = false;
            lbStatus.Size = new Size(100, 20);
            lbStatus.Text = maintenanceStatus;
            lbStatus.TextAlign = ContentAlignment.MiddleCenter;
            lbStatus.Font = new Font("Calibri", 10, FontStyle.Bold);
            lbStatus.Location = new Point(1000, 40);

            switch (maintenanceStatus.ToLower())
            {
                case "completed":
                    lbStatus.BackColor = Color.LightGreen;
                    lbStatus.ForeColor = Color.Green;
                    break;
                case "pending":
                    lbStatus.BackColor = Color.LightYellow;
                    lbStatus.ForeColor = Color.Orange;
                    break;
                case "in progress":
                    lbStatus.BackColor = Color.LightSkyBlue;
                    lbStatus.ForeColor = Color.Blue;
                    break;
                default:
                    lbStatus.BackColor = Color.LightPink;
                    lbStatus.ForeColor = Color.Red;
                    break;
            }
            MaintenanceCards.Controls.Add(lbStatus);

            Label lbRequestTypeContent = new Label();
            lbRequestTypeContent.Text = typeName;
            lbRequestTypeContent.AutoSize = false;
            lbRequestTypeContent.Size = new Size(150, 30);
            lbRequestTypeContent.Font = new Font("Calibri", 14, FontStyle.Bold);
            lbRequestTypeContent.Location = new Point(95, 35);
            MaintenanceCards.Controls.Add(lbRequestTypeContent);

            Label lbCommentsDetail = new Label();
            lbCommentsDetail.Text = description;
            lbCommentsDetail.AutoSize = false;
            lbCommentsDetail.Size = new Size(500, 30);
            lbCommentsDetail.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbCommentsDetail.Location = new Point(95, 115);
            MaintenanceCards.Controls.Add(lbCommentsDetail);

            Label lbRequester = new Label();
            lbRequester.Text = $"{firstName} {middleName} {lastName}";
            lbRequester.Size = new Size(250, 30);
            lbRequester.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbRequester.Location = new Point(95, 75);
            MaintenanceCards.Controls.Add(lbRequester);

            Label lbUnit = new Label();
            lbUnit.Text = $"Unit: {unitNumber}";
            lbUnit.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbUnit.Location = new Point(200, 35);
            MaintenanceCards.Controls.Add(lbUnit);

            Label lbMergedDate = new Label();
            lbMergedDate.AutoSize = true;
            lbMergedDate.Text = $"Date: {requestDate}";
            lbMergedDate.Size = new Size(100, 30);
            lbMergedDate.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbMergedDate.Location = new Point(95, 145);
            MaintenanceCards.Controls.Add(lbMergedDate);

            if (maintenanceStatus.ToLower() != "completed")
            {
                Button btnComplete = new Button();
                btnComplete.Size = new Size(150, 45);
                btnComplete.Text = "Mark Complete";
                btnComplete.Tag = requestID;
                btnComplete.Location = new Point(780, 90);
                btnComplete.BackColor = Color.LightGreen;
                btnComplete.ForeColor = Color.DarkGreen;
                btnComplete.FlatStyle = FlatStyle.Flat;
                btnComplete.Click += (s, ev) => UpdateRequestStatus(requestID, "Completed");
                MaintenanceCards.Controls.Add(btnComplete);
            }

            Button btnEdit = new Button();
            btnEdit.Size = new Size(70, 45);
            btnEdit.Text = "Edit";
            btnEdit.Tag = requestID;
            btnEdit.Location = new Point(940, 90);
            btnEdit.BackColor = Color.LightSkyBlue;
            btnEdit.ForeColor = Color.DarkBlue;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Click += btnEdit_Click;
            MaintenanceCards.Controls.Add(btnEdit);

            Button btnDelete = new Button();
            btnDelete.Size = new Size(70, 45);
            btnDelete.Text = "Delete";
            btnDelete.Tag = requestID;
            btnDelete.Location = new Point(1020, 90);
            btnDelete.BackColor = Color.LightPink;
            btnDelete.ForeColor = Color.DarkRed;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Click += btnDelete_Click;
            MaintenanceCards.Controls.Add(btnDelete);

            return MaintenanceCards;
        }

        private void SetupStatusComboBox()
        {
            if (cbStatus == null) return;

            cbStatus.Items.Clear();
            cbStatus.Items.Add("All Status");
            cbStatus.Items.Add("Pending");
            cbStatus.Items.Add("In Progress");
            cbStatus.Items.Add("Completed");
            cbStatus.SelectedIndex = 0;

            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;
        }

        private void LoadMaintenanceCardsByStatusAndSearch(string status, string searchText)
        {
            DataTable dt = new DataTable();
            string query;
            string baseQuery = @" 
        SELECT 
            MR.maintenanceReqID, 
            MR.description, 
            MR.requestDate, 
            MR.Status AS MaintenanceStatus,
            U.UnitNumber,
            PI.firstName, 
            PI.middleName, 
            PI.lastName,
            MR.propertyID, 
            RT.typeName
        FROM MaintenanceRequest AS MR 
        LEFT JOIN Tenant AS T ON MR.TenantID = T.TenantID
        LEFT JOIN PersonalInformation AS PI ON T.TenantID = PI.TenantID 
        LEFT JOIN Contract AS C ON T.TenantID = C.TenantID 
        LEFT JOIN Unit AS U ON C.UnitID = U.UnitID
        LEFT JOIN RequestType AS RT ON MR.requestTypeID = RT.requestTypeID
        WHERE 1=1 ";

            string whereClause = "";


            if (status != "All Status")
            {
                whereClause += " AND MR.Status = @Status";
            }

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                whereClause += @" AND (MR.description LIKE @Search OR U.UnitNumber LIKE @Search OR 
                             PI.firstName + ' ' + PI.middleName + ' ' + PI.lastName LIKE @Search) ";
            }

            query = baseQuery + whereClause + " ORDER BY MR.requestDate DESC;";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (status != "All Status")
                        cmd.Parameters.AddWithValue("@Status", status);

                    if (!string.IsNullOrWhiteSpace(searchText))
                        cmd.Parameters.AddWithValue("@Search", "%" + searchText + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error during search/filter: {ex.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            flowLayoutPanelRightSideBar.Controls.Clear();
            foreach (DataRow row in dt.Rows)
            {
                int requestID = Convert.ToInt32(row["maintenanceReqID"]);
                Panel card = CreateMaintenanceCard(
                    requestID,
                    row["description"].ToString(),
                    row["requestDate"].ToString(),
                    row["MaintenanceStatus"].ToString(),
                    row["unitNumber"].ToString(),
                    row["firstName"].ToString(),
                    row["lastName"].ToString(),
                    row["middleName"].ToString(),
                    row["typeName"].ToString()
                );
                flowLayoutPanelRightSideBar.Controls.Add(card);
            }
        }

        private void UpdateRequestStatus(int maintenanceReqID, string newStatus)
        {
            string query = "UPDATE MaintenanceRequest SET Status = @Status WHERE maintenanceReqID = @RequestID";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@RequestID", maintenanceReqID);
                    con.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Request ID {maintenanceReqID} status updated to {newStatus}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadPropertyCards();
                    DisplayDashboardValues();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error updating status: {ex.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteMaintenanceRequest(int maintenanceReqID)
        {
            string query = "DELETE FROM MaintenanceRequest WHERE maintenanceReqID = @RequestID";

            try
            {
                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RequestID", maintenanceReqID);
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Request ID {maintenanceReqID} deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadPropertyCards();
                        DisplayDashboardValues();
                    }
                    else
                    {
                        MessageBox.Show("Delete failed. Request ID not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database Error deleting request: {ex.Message}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddRequest_Click(object sender, EventArgs e)
        {
            try
            {
                using (AddRecquest addForm = new AddRecquest())
                {
                    if (addForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadPropertyCards();
                        DisplayDashboardValues();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Add Request form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn?.Tag is int requestID)
            {
                try
                {
                    using (AddRecquest editForm = new AddRecquest(requestID))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadPropertyCards();
                            DisplayDashboardValues();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening Edit Request form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn?.Tag is int requestID)
            {
                if (MessageBox.Show($"Are you sure you want to delete Request ID {requestID}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    DeleteMaintenanceRequest(requestID);
                }
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

        private void Maintenance_Load(object sender, EventArgs e)
        {
            SetupStatusComboBox();
            SetButtonActiveStyle(btnMaintenance, activeColor);
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = tbSearch.Text.Trim();
            string selectedStatus = cbStatus.SelectedItem?.ToString() ?? "All Status";
            LoadMaintenanceCardsByStatusAndSearch(selectedStatus, searchText);
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string searchText = tbSearch.Text.Trim();
            string selected = cbStatus.SelectedItem?.ToString() ?? "All Status";
            LoadMaintenanceCardsByStatusAndSearch(selected, searchText);
        }

        // -------------------- Button Side Bar -------------------- //

        // --------------- Dashboard Button --------------- //
        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard dashboard = new DashBoard(UserName, UserRole);
            dashboard.Show();
            this.Close();
        }

        // --------------- Tenant Button --------------- //
        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants tenants = new Tenants(UserName, UserRole);
            tenants.Show();
            this.Hide();
        }

        // --------------- Properties Button --------------- //
        private void btnProperties_Click(object sender, EventArgs e)
        {
            ProperTies properties = new ProperTies(UserName, UserRole);
            properties.Show();
            this.Hide();
        }

        // --------------- Payment Records Button --------------- //
        private void btnPaymentRec_Click(object sender, EventArgs e)
        {
            Payment_Records paymentRec = new Payment_Records(UserName, UserRole);
            paymentRec.Show();
            this.Hide();
        }

        // --------------- Contracts Button --------------- //
        private void btnContracts_Click(object sender, EventArgs e)
        {
            Contracts contract = new Contracts(UserName, UserRole);
            contract.Show();
            this.Hide();
        }

        // --------------- Admin Account Button --------------- //
        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts adminAcc = new SuperAdmin_AdminAccounts(UserName, UserRole);
            adminAcc.Show();
            this.Hide();
        }

        // --------------- View Reports Button --------------- //
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

        // --------------- Backup Button --------------- //
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
        private void button1_Click(object sender, EventArgs e)
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