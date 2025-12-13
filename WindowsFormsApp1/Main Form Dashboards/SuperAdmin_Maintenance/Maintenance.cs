using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1.Main_Form_Dashboards
{
    public partial class Maintenance : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private string UserName;
        private string UserRole;

        public Maintenance(string username, string userRole)
        {
            this.UserName = username;
            this.UserRole = userRole;

            InitializeComponent();
            LoadPropertyCards();
            DisplayDashboardValues();
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
            lbTotalRecquest.Text = data["TotalRequests"].ToString();
            lbPending.Text = data["Pending"].ToString();
            lbCompleted.Text = data["Completed"].ToString();
            lbThisMonth.Text = data["ThisMonth"].ToString();
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
                    PI.lastName
                FROM MaintenanceRequest AS MR 
                INNER JOIN Tenant AS T ON MR.TenantID = T.TenantID
                INNER JOIN PersonalInformation AS PI ON T.TenantID = PI.TenantID 
                INNER JOIN Contract AS C ON T.TenantID = C.TenantID 
                INNER JOIN Unit AS U ON C.UnitID = U.UnitID
                WHERE C.ContractStatus = 'Active' OR C.ContractStatus IS NULL 
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
            flowLayoutPanelRightSideBar.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanelRightSideBar.WrapContents = true;
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
                    row["middleName"].ToString()
                );
                flowLayoutPanelRightSideBar.Controls.Add(card);
            }
        }

        private void LoadPropertyCards()
        {
            LoadMaintenanceCards(flowLayoutPanelRightSideBar);
        }

        private Panel CreateMaintenanceCard(int requestID, string description, string requestDate, string maintenanceStatus, string unitNumber, string firstName, string lastName, string middleName)
        {
            Panel MaintenanceCards = new Panel();
            MaintenanceCards.Size = new Size(1130, 180);
            MaintenanceCards.BackColor = Color.White;
            MaintenanceCards.BorderStyle = BorderStyle.None;
            MaintenanceCards.Margin = new Padding(15);

            Label lbStatus = new Label();
            lbStatus.AutoSize = false;
            lbStatus.Size = new Size(83, 20);
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

            Label lbDescriptionTitle = new Label();
            lbDescriptionTitle.Text = "Description:";
            lbDescriptionTitle.Size = new Size(130, 30);
            lbDescriptionTitle.Font = new Font("Calibri", 16, FontStyle.Bold);
            lbDescriptionTitle.Location = new Point(95, 35);
            MaintenanceCards.Controls.Add(lbDescriptionTitle);

            Label lbDescription = new Label();
            lbDescription.Text = description;
            lbDescription.AutoSize = false;
            lbDescription.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbDescription.Location = new Point(95, 105);
            MaintenanceCards.Controls.Add(lbDescription);

            Label lbUnit = new Label();
            lbUnit.Text = unitNumber;
            lbUnit.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbUnit.Location = new Point(235, 38);
            MaintenanceCards.Controls.Add(lbUnit);

            Label lbRequester = new Label();
            lbRequester.Text = $"{firstName} {middleName} {lastName}";
            lbRequester.Size = new Size(300, 30);
            lbRequester.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbRequester.Location = new Point(95, 75);
            MaintenanceCards.Controls.Add(lbRequester);

            Label lbMergedDate = new Label();
            lbMergedDate.AutoSize = true;
            lbMergedDate.Text = $"Date: {requestDate}";
            lbMergedDate.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbMergedDate.Location = new Point(95, 140);
            MaintenanceCards.Controls.Add(lbMergedDate);

            return MaintenanceCards;
        }

        private void SetupStatusComboBox()
        {
            cbStatus.Items.Clear(); 
            cbStatus.Items.Add("All Status");
            cbStatus.Items.Add("Pending");
            cbStatus.Items.Add("In Progress");
            cbStatus.Items.Add("Completed");
            cbStatus.SelectedIndex = 0;

            cbStatus.SelectedIndexChanged += (s, e) =>
            {
                string selected = cbStatus.SelectedItem.ToString();
                LoadMaintenanceCardsByStatus(selected);
            };
        }

        private void LoadMaintenanceCardsByStatus(string status)
        {
            DataTable dt = new DataTable();
            string query;

            if (status == "All Status")
            {
                // Show all maintenance requests
                query = @" 
            SELECT 
                MR.maintenanceReqID, 
                MR.description, 
                MR.requestDate, 
                MR.Status AS MaintenanceStatus,
                U.UnitNumber,
                PI.firstName, 
                PI.middleName, 
                PI.lastName
            FROM MaintenanceRequest AS MR 
            INNER JOIN Tenant AS T ON MR.TenantID = T.TenantID
            INNER JOIN PersonalInformation AS PI ON T.TenantID = PI.TenantID 
            INNER JOIN Contract AS C ON T.TenantID = C.TenantID 
            INNER JOIN Unit AS U ON C.UnitID = U.UnitID
            WHERE C.ContractStatus = 'Active' OR C.ContractStatus IS NULL
            ORDER BY MR.requestDate DESC;";

                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            else
            {
                // Filter by the specific status
                query = @" 
            SELECT 
                MR.maintenanceReqID, 
                MR.description, 
                MR.requestDate, 
                MR.Status AS MaintenanceStatus,
                U.UnitNumber,
                PI.firstName, 
                PI.middleName, 
                PI.lastName
            FROM MaintenanceRequest AS MR 
            INNER JOIN Tenant AS T ON MR.TenantID = T.TenantID
            INNER JOIN PersonalInformation AS PI ON T.TenantID = PI.TenantID 
            INNER JOIN Contract AS C ON T.TenantID = C.TenantID 
            INNER JOIN Unit AS U ON C.UnitID = U.UnitID
            WHERE (C.ContractStatus = 'Active' OR C.ContractStatus IS NULL)
              AND MR.Status = @Status
            ORDER BY MR.requestDate DESC;";

                using (SqlConnection con = new SqlConnection(DataConnection))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
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
                    row["middleName"].ToString()
                );
                flowLayoutPanelRightSideBar.Controls.Add(card);
            }
        }

        private void LoadMaintenanceCardsByStatusAndSearch(string status, string searchText)
        {
            DataTable dt = new DataTable();
            string query;

            if (status == "All Status")
            {
                query = @" 
            SELECT 
                MR.maintenanceReqID, 
                MR.description, 
                MR.requestDate, 
                MR.Status AS MaintenanceStatus,
                U.UnitNumber,
                PI.firstName, 
                PI.middleName, 
                PI.lastName
            FROM MaintenanceRequest AS MR 
            INNER JOIN Tenant AS T ON MR.TenantID = T.TenantID
            INNER JOIN PersonalInformation AS PI ON T.TenantID = PI.TenantID 
            INNER JOIN Contract AS C ON T.TenantID = C.TenantID 
            INNER JOIN Unit AS U ON C.UnitID = U.UnitID
            WHERE (C.ContractStatus = 'Active' OR C.ContractStatus IS NULL)
              AND (MR.description LIKE @Search OR U.UnitNumber LIKE @Search OR 
                   PI.firstName + ' ' + PI.middleName + ' ' + PI.lastName LIKE @Search)
            ORDER BY MR.requestDate DESC;";
            }
            else
            {
                query = @" 
            SELECT 
                MR.maintenanceReqID, 
                MR.description, 
                MR.requestDate, 
                MR.Status AS MaintenanceStatus,
                U.UnitNumber,
                PI.firstName, 
                PI.middleName, 
                PI.lastName
            FROM MaintenanceRequest AS MR 
            INNER JOIN Tenant AS T ON MR.TenantID = T.TenantID
            INNER JOIN PersonalInformation AS PI ON T.TenantID = PI.TenantID 
            INNER JOIN Contract AS C ON T.TenantID = C.TenantID 
            INNER JOIN Unit AS U ON C.UnitID = U.UnitID
            WHERE (C.ContractStatus = 'Active' OR C.ContractStatus IS NULL)
              AND MR.Status = @Status
              AND (MR.description LIKE @Search OR U.UnitNumber LIKE @Search OR 
                   PI.firstName + ' ' + PI.middleName + ' ' + PI.lastName LIKE @Search)
            ORDER BY MR.requestDate DESC;";
            }

            using (SqlConnection con = new SqlConnection(DataConnection))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Search", "%" + searchText + "%");
                if (status != "All Status")
                    cmd.Parameters.AddWithValue("@Status", status);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
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
                    row["middleName"].ToString()
                );
                flowLayoutPanelRightSideBar.Controls.Add(card);
            }
        }

        private void Maintenance_Load(object sender, EventArgs e)
        {
            SetupStatusComboBox();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = tbSearch.Text.Trim();
            string selectedStatus = cbStatus.SelectedItem.ToString();
            LoadMaintenanceCardsByStatusAndSearch(selectedStatus, searchText);
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string searchText = tbSearch.Text.Trim();
            string selected = cbStatus.SelectedItem.ToString();
            LoadMaintenanceCardsByStatusAndSearch(selected, searchText);
        }
    }
}
