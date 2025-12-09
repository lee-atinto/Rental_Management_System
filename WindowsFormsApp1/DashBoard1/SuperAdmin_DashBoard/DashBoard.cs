using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Login_ResetPassword;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.Super_Admin_Account
{
    public partial class DashBoard : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private string UserName;
        private string UserRole;

        // -------------------- Button Style -------------------- //
        private readonly Color activeColor = Color.FromArgb(46, 51, 73);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        // -------------------- Initialize Button Style -------------------- //
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

        // -------------------- Set Active Button Style -------------------- //
        private void SetButtonActiveStyle(Button button, Color backColor)
        {
            if (button != null)
            {
                button.BackColor = backColor;
                button.FlatAppearance.MouseDownBackColor = backColor;
                button.FlatAppearance.MouseOverBackColor = backColor;
            }
        }

        private void AddContentToForceScroll(Control container)
        {
            Panel contentPanel = new Panel();
            contentPanel.Width = container.ClientSize.Width;
            contentPanel.Height = 1500;
            contentPanel.Location = new Point(0, 0);
            contentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            container.Controls.Add(contentPanel);

            Label testLabel = new Label();
            testLabel.Text = "Scroll Down to See This Text at the Bottom";
            testLabel.Location = new Point(10, contentPanel.Height - 30);
            contentPanel.Controls.Add(testLabel);
        }

        public DashBoard(string username, string userRole)
        {
            InitializeComponent();

            // -------------------- Set User Information -------------------- //
            this.UserName = username;
            this.UserRole = userRole;
            this.lbName.Text = $"{username} \n ({userRole})";

            // -------------------- Initialize Button Style -------------------- //
            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnProperties);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);
            InitializeButtonStyle(btnBackUp);

            panelHeader.BackColor = Color.White;
            PanelBackGroundProfile.BackColor = Color.FromArgb(46, 51, 73);

            // -------------------- Set Icon Button -------------------- //

            // -------------------- Set Padding Button -------------------- //
            btnDashBoard.Padding = new Padding(30, 0, 0, 0);
            btnAdminAcc.Padding = new Padding(30, 0, 0, 0);
            btnTenant.Padding = new Padding(30, 0, 0, 0);
            btnProperties.Padding = new Padding(30, 0, 0, 0);
            btnViewReport.Padding = new Padding(30, 0, 0, 0);
            btnBackUp.Padding = new Padding(30, 0, 0, 0);
            btnPaymentRec.Padding = new Padding(30, 0, 0, 0);

            // -------------------- Set Color Unactive Button -------------------- //
            btnAdminAcc.ForeColor = Color.Black;
            btnTenant.ForeColor = Color.Black;
            btnProperties.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;


            // -------------------- Set Borderline -------------------- //
            plRecentPayments.BorderStyle = BorderStyle.FixedSingle;
            plUpcomingRenewals.BorderStyle = BorderStyle.FixedSingle;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel4.BorderStyle = BorderStyle.FixedSingle;

            // -------------------- Set Borderline color -------------------- //
            plRecentPayments.BackColor = Color.FromArgb(240, 240, 240);
            plUpcomingRenewals.BackColor = Color.FromArgb(240, 240, 240);
            panel1.BackColor = Color.White;
            panel2.BackColor = Color.White;
            panel3.BackColor = Color.White;
            panel4.BackColor = Color.White;

            // -------------------- Set Scroll Form -------------------- //
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnDashBoard, activeColor);
        }

        // -------------------- Remove Focus Rectangle from Buttons -------------------- //
        protected override bool ShowFocusCues
        {
            get { return false; }
        }

        // -------------------- Dashboard Buttons Click Event -------------------- //
        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts AdminAccount = new SuperAdmin_AdminAccounts(UserName, UserRole);
            AdminAccount.Show();
            this.Hide();
        }

        // -------------------- Properties Buttons Click Event -------------------- //
        private void btnProperties_Click(object sender, EventArgs e)
        {
            ProperTies properties = new ProperTies(UserName, UserRole);
            properties.Show();
            this.Hide();
        }

        // -------------------- Tenants Buttons Click Event -------------------- //
        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants Tenants = new Tenants(UserName, UserRole);
            Tenants.Show();
            this.Hide();
        }

        // -------------------- Payment Records Buttons Click Event -------------------- //
        private void btnPaymentRec_Click_1(object sender, EventArgs e)
        {
            Payment_Records payment = new Payment_Records(UserName, UserRole);
            payment.Show();
            this.Hide();
        }

        // -------------------- View Reports Buttons Click Event -------------------- //
        private void btnViewReport_Click(object sender, EventArgs e)
        {
            
        }

        // -------------------- Backup Buttons Click Event -------------------- //
        private void btnBackUp_Click(object sender, EventArgs e)
        {

        }

        // -------------------- Logout Buttons Click Event -------------------- //
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