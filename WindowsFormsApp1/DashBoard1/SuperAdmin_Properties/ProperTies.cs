using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Super_Admin_Account;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_Properties
{
    public partial class ProperTies : Form
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

        public ProperTies(string userName, string userRole)
        {
            InitializeComponent();

            // -------------------- Set User Information -------------------- //
            this.UserName = userName;
            this.UserRole = userRole;
            this.lbName.Text = $"{userName} \n ({userRole})";

            LoadPropertyCards();

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
            btnDashBoard.ForeColor = Color.Black;
            btnViewReport.ForeColor = Color.Black;
            btnBackUp.ForeColor = Color.Black;
            btnPaymentRec.ForeColor = Color.Black;
        }

        private DataTable GetProperties()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();
                string query = @" SELECT P.propertyName, P.unitNumber, A.city + ' ' + A.street + ' ' + A.barangay AS Address, PO.firstName + ' ' + PO.middleName + ' ' + PO.lastName AS FullName, R.RentAmount FROM Property P  JOIN Address A ON P.AddressID = A.AddressID JOIN PropertyOwner PO ON P.propertyOwnerID = PO.propertyOwnerID JOIN Rent R ON P.propertyID = R.propertyID";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }

            return dt;
        }

        private Panel CreatePropertyCard(string propertyName, string unitNumber, string address, string fullName, string rentAmount)
        {
            Panel PropertyCards = new Panel();
            PropertyCards.Size = new Size(368, 400);
            PropertyCards.BackColor = Color.White;
            PropertyCards.BorderStyle = BorderStyle.None;
            PropertyCards.Margin = new Padding(15);

            Label lbProperty = new Label();
            lbProperty.Text = propertyName;
            lbProperty.Font = new Font("Calibri", 18, FontStyle.Bold);
            lbProperty.Location = new Point(30, 160);
            PropertyCards.Controls.Add(lbProperty);

            Label lbUnit = new Label();
            lbUnit.Text = unitNumber;
            lbUnit.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbUnit.Location = new Point(30, 185);
            PropertyCards.Controls.Add(lbUnit);

            Label lbAddress = new Label();
            lbAddress.Text = address;
            lbAddress.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbAddress.Location = new Point(30, 215);
            PropertyCards.Controls.Add(lbAddress);

            Label lbOwnerTitle = new Label();
            lbOwnerTitle.Text = "Owner: ";
            lbOwnerTitle.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbOwnerTitle.Location = new Point(30, 245);
            PropertyCards.Controls.Add(lbOwnerTitle);

            Label lbOwner = new Label();
            lbOwner.Text = fullName;
            lbOwner.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbOwner.Location = new Point(250, 245);
            PropertyCards.Controls.Add(lbOwner);

            Label lbRentTitle = new Label();
            lbRentTitle.Text = "Rent: ";
            lbRentTitle.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbRentTitle.Location = new Point(30, 270);
            PropertyCards.Controls.Add(lbRentTitle);

            Label lbRent = new Label();
            lbRent.Text = $"{rentAmount}/m";
            lbRent.ForeColor = Color.FromArgb(102, 103, 171);
            lbRent.Font = new Font("Calibri", 14, FontStyle.Regular);
            lbRent.Location = new Point(205, 270);
            PropertyCards.Controls.Add(lbRent);

            // --------------- Button View Style --------------- //
            Button btnView = new Button();
            btnView.Text = "View";
            btnView.Size = new Size(90, 50);
            btnView.Location = new Point(30, 330);
            btnView.Font = new Font("Calibri", 14, FontStyle.Regular);
            btnView.Click += (s, e) => MessageBox.Show($"Viewing {propertyName}");
            PropertyCards.Controls.Add(btnView);

            btnView.BackColor = Color.White;
            btnView.FlatStyle = FlatStyle.Flat;
            btnView.FlatAppearance.BorderSize = 0;
            btnView.ForeColor = Color.FromArgb(88, 101, 242);
            btnView.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnView.FlatAppearance.MouseOverBackColor = Color.FromArgb(204, 204, 255);
            PropertyCards.Controls.Add(btnView);

            // --------------- Button Edit Style --------------- //
            Button btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Size = new Size(90, 50);
            btnEdit.Location = new Point(130, 330);
            btnEdit.Font = new Font("Calibri", 14, FontStyle.Regular);
            btnEdit.Click += (s, e) => MessageBox.Show($"Editing {propertyName}");

            btnEdit.BackColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.ForeColor = Color.FromArgb(102, 103, 171);
            btnEdit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(204, 204, 255);

            PropertyCards.Controls.Add(btnEdit);

            // --------------- Button Delete Style --------------- //
            Button btnDelete = new Button();
            btnDelete.Size = new Size(75, 50);
            btnDelete.Location = new Point(230, 330);
            btnDelete.Font = new Font("Calibri", 14, FontStyle.Regular);
            btnDelete.Click += (s, e) => MessageBox.Show($"Delete {propertyName}");

            Image original = Properties.Resources.delete;
            Image zoomed = new Bitmap(original, new Size(20, 20));
            btnDelete.Image = zoomed;

            btnDelete.BackColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(251, 187, 185
);

            PropertyCards.Controls.Add(btnDelete);

            return PropertyCards;
        }

        private void LoadPropertyCards()
        {
            DataTable dt = GetProperties();
            flowLayoutPanelRightSideBar.Controls.Clear();
            flowLayoutPanelRightSideBar.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanelRightSideBar.WrapContents = true;
            flowLayoutPanelRightSideBar.AutoScroll = true;

            foreach (DataRow row in dt.Rows)
            {
                Panel PropertyCards = CreatePropertyCard(
                    row["PropertyName"].ToString(),
                    row["UnitNumber"].ToString(),
                    row["Address"].ToString(),
                    row["FullName"].ToString(),
                    row["RentAmount"].ToString()
                );
                flowLayoutPanelRightSideBar.Controls.Add(PropertyCards);
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

        private void ProperTies_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnProperties, activeColor);
        }

        // -------------------- Dashboard Buttons Click Event -------------------- //
        private void btnDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard dashboard = new DashBoard(UserName, UserRole);
            dashboard.ShowDialog();
            this.Hide();
        }

        // -------------------- Admin Accounts Buttons Click Event -------------------- //
        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts AdminAccount = new SuperAdmin_AdminAccounts(UserName, UserRole);
            AdminAccount.ShowDialog();
            this.Hide();
        }

        // -------------------- Tenants Buttons Click Event -------------------- //
        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants tenant = new Tenants(UserName, UserRole);
            tenant.ShowDialog();
            this.Hide();
        }

        // -------------------- Payment Accounts Buttons Click Event -------------------- //
        private void btnPaymentRec_Click(object sender, EventArgs e)
        {
            Payment_Records payments = new Payment_Records(UserName, UserRole);
            payments.ShowDialog();
            this.Hide();
        }
    }
}
