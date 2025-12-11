using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Main_Form_Dashboards
{
    public partial class Maintenance : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private string UserName;
        private string UserRole;

        private DataTable MaintenanceData;
        public Maintenance(String username, string userRole)
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
            string thisMonthQuery = @" SELECT COUNT(*) FROM MaintenanceRequest WHERE DATENAME(month, requestDate) = DATENAME(month, GETDATE()) AND DATENAME(year, requestDate) = DATENAME(year, GETDATE())";

            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                try
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand(totalRequestsQuery, con))
                    {
                        values["TotalRequests"] = (int)cmd.ExecuteScalar();
                    }

                    using (SqlCommand cmd = new SqlCommand(pendingQuery, con))
                    {
                        values["Pending"] = (int)cmd.ExecuteScalar();
                    }

                    using (SqlCommand cmd = new SqlCommand(completedQuery, con))
                    {
                        values["Completed"] = (int)cmd.ExecuteScalar();
                    }

                    using (SqlCommand cmd = new SqlCommand(thisMonthQuery, con))
                    {
                        values["ThisMonth"] = (int)cmd.ExecuteScalar();
                    }
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
            Dictionary<string, int> data = GetDashboardValues();

            lbTotalRecquest.Text = data["TotalRequests"].ToString();
            lbPending.Text = data["Pending"].ToString();
            lbCompleted.Text = data["Completed"].ToString();
            lbThisMonth.Text = data["ThisMonth"].ToString();
        }

        private DataTable GetMaintenance()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();
                string query = @" SELECT MR.maintenanceReqID, MR.description, MR.requestDate, MR.Status AS MaintenanceStatus,P.unitNumber, PI.firstName, PI.middleName, PI.lastName
                                  FROM MaintenanceRequest AS MR INNER JOIN Property AS P ON MR.PropertyID = P.PropertyID INNER JOIN PersonalInformation AS PI ON MR.maintenanceReqID = PI.personalInfoID;";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
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
            MaintenanceCards.Size = new Size(1165, 150);
            MaintenanceCards.BackColor = Color.White;
            MaintenanceCards.BorderStyle = BorderStyle.None;
            MaintenanceCards.Margin = new Padding(15);

            Label lbStatus = new Label();
            lbStatus.AutoSize = false;
            lbStatus.Size = new Size(83, 20);
            lbStatus.Text = maintenanceStatus;
            lbStatus.TextAlign = ContentAlignment.MiddleCenter;
            lbStatus.Font = new Font("Calibri", 10, FontStyle.Bold);
            lbStatus.Location = new Point(255, 20);

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

            PictureBox pbToolIcon = new PictureBox();
            pbToolIcon.Image = Properties.Resources.tool;
            pbToolIcon.Size = new Size(35, 35);
            pbToolIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            pbToolIcon.Location = new Point(lbDescriptionTitle.Left - 60, lbDescriptionTitle.Top + 5);
            MaintenanceCards.Controls.Add(pbToolIcon);

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

            Label lbDateTitle = new Label();
            lbDateTitle.Text = "Date:";
            lbDateTitle.Font = new Font("Calibri", 12, FontStyle.Bold);
            lbDateTitle.Location = new Point(200, 140);
            MaintenanceCards.Controls.Add(lbDateTitle);

            Label lbDate = new Label();
            lbDate.Text = requestDate;
            lbDate.Font = new Font("Calibri", 12, FontStyle.Regular);
            lbDate.Location = new Point(250, 140);
            MaintenanceCards.Controls.Add(lbDate);

            Label lbRequesterTitle = new Label();
            lbRequesterTitle.Text = "Requester:";
            lbRequesterTitle.Font = new Font("Calibri", 12, FontStyle.Bold);
            lbRequesterTitle.Location = new Point(30, 170);
            MaintenanceCards.Controls.Add(lbRequesterTitle);

            Button btnView = new Button();
            btnView.Text = "View";
            btnView.Size = new Size(95, 40);
            btnView.Location = new Point(35, 230);

            btnView.Click += (s, e) => MessageBox.Show($"Viewing Request ID: {requestID} - {description}");
            MaintenanceCards.Controls.Add(btnView);

            Button btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Size = new Size(95, 40);
            btnEdit.Location = new Point(135, 230); 

            btnEdit.Click += (s, e) => MessageBox.Show($"Editing Request ID: {requestID} - {description}");
            MaintenanceCards.Controls.Add(btnEdit);

            Button btnDelete = new Button();
            btnDelete.Size = new Size(95, 40);
            btnDelete.Location = new Point(235, 230);

            MaintenanceCards.Controls.Add(btnDelete);

            return MaintenanceCards;
        }

        private void Maintenance_Load(object sender, EventArgs e)
        {

        }
    }
}
