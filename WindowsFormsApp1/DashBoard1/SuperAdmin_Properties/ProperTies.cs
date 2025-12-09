using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Super_Admin_Account;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_Properties
{
    public partial class ProperTies : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private string UserName;
        private string UserRole;

        private DataTable propertyData;

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

                string query = @" SELECT P.propertyID, P.propertyName, P.unitNumber, P.AddressID, P.propertyOwnerID, A.city, A.street, A.barangay, A.province, A.postalCode, PO.firstName, PO.middleName, PO.lastName, PO.contactNumber, PO.Email, R.RentAmount
                               FROM Property P INNER JOIN Address A ON P.AddressID = A.AddressID INNER JOIN PropertyOwner PO ON P.propertyOwnerID = PO.propertyOwnerID LEFT JOIN Rent R ON P.propertyID = R.propertyID;";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }   
            return dt;
        }


        private Panel CreatePropertyCard(int propertyID, string propertyName, string unitNumber, string rentAmount, string firstName, string lastName, string middleName, string contactNumber, string email, string street, string barangay, string city, string province, string postalCode)
        {
            Panel PropertyCards = new Panel();
            PropertyCards.Size = new Size(363, 500);
            PropertyCards.BackColor = Color.White;
            PropertyCards.BorderStyle = BorderStyle.None;
            PropertyCards.Margin = new Padding(15);

            Label lbProperty = new Label();
            lbProperty.AutoSize = false;
            lbProperty.Size = new Size(225, 30);
            lbProperty.Text = propertyName;
            lbProperty.Font = new Font("Calibri", 14, FontStyle.Bold);
            lbProperty.Location = new Point(30, 185);
            PropertyCards.Controls.Add(lbProperty);

            //Label lbStatus = new Label();
            //lbStatus.AutoSize = false;
            //lbStatus.Size = new Size(83, 20);
            //lbStatus.Text = propertyStatus;
            //lbStatus.BackColor = Color.LightGreen;
            //lbStatus.ForeColor = Color.Green;
            //lbStatus.TextAlign = ContentAlignment.MiddleCenter;
            //lbStatus.Font = new Font("Calibri", 10, FontStyle.Regular);
            //lbStatus.Location = new Point(255, 190);

            //switch (propertyStatus)
            //{
            //    case "Occupied":
            //        lbStatus.BackColor = Color.LightGreen;
            //        lbStatus.ForeColor = Color.Green;
            //        break;
            //    case "Vacant":
            //        lbStatus.BackColor = Color.LightYellow;
            //        lbStatus.ForeColor = Color.Orange;
            //        break;
            //    case "Maintenance":
            //        lbStatus.BackColor = Color.LightPink;
            //        lbStatus.ForeColor = Color.Red;
            //        break;
            //}
            //PropertyCards.Controls.Add(lbStatus);

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
            UnderLine1.Location = new Point(lbRentTitle.Left, lbRentTitle.Bottom + 20);
            PropertyCards.Controls.Add(UnderLine1);

            Label UnderLine2 = new Label();
            UnderLine2.BackColor = Color.DarkGray;
            UnderLine2.Size = new Size(lbRent.Width, 2);
            UnderLine2.Location = new Point(lbRent.Left, lbRent.Bottom + 20);
            PropertyCards.Controls.Add(UnderLine2);

            // --------------- Button View Style --------------- //
            Button btnView = new Button();
            btnView.Text = "View";
            btnView.Size = new Size(95, 50);
            btnView.Location = new Point(35, 420);
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
            btnEdit.Size = new Size(95, 50);
            btnEdit.Location = new Point(135, 420);
            btnEdit.Font = new Font("Calibri", 14, FontStyle.Regular);
            btnEdit.Click += (s, e) => MessageBox.Show($"Editing {propertyName}");

            btnEdit.BackColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.ForeColor = Color.FromArgb(102, 103, 171);
            btnEdit.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnEdit.FlatAppearance.MouseOverBackColor = Color.FromArgb(204, 204, 255);

            btnEdit.Click += (s, e) =>
            {
                AddProperties editForm = new AddProperties( propertyID, firstName, lastName, middleName, contactNumber, email, street, barangay, city, province, postalCode, propertyName, unitNumber, decimal.TryParse(rentAmount, out decimal rent) ? rent : 0 );

                editForm.PropertyAdded += (sender, args) =>
                {
                    propertyData = GetProperties();
                    DisplayPropertyCards(propertyData);
                };

                editForm.ShowDialog();
            };

            PropertyCards.Controls.Add(btnEdit);

            // --------------- Button Delete Style --------------- //
            Button btnDelete = new Button();
            btnDelete.Size = new Size(95, 50);
            btnDelete.Location = new Point(235, 420);
            btnDelete.Font = new Font("Calibri", 14, FontStyle.Regular);
            btnDelete.Click += (s, e) => MessageBox.Show($"Delete {propertyName}");

            //try
            //{
            //    Image original = Properties.Resources.delete;
            //    Image zoomed = new Bitmap(original, new Size(20, 20));
            //    btnDelete.Image = zoomed;
            //}
            //catch
            //{
            //    btnDelete.Text = "Delete";
            //}

            btnDelete.BackColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnDelete.FlatAppearance.MouseOverBackColor = Color.FromArgb(251, 187, 185);
            PropertyCards.Controls.Add(btnDelete);

            btnDelete.Click += (s, e) =>
            {
                DialogResult result = MessageBox.Show($"Are you sure you want to delete \"{propertyName}\"?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes) return;

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();
                    using (SqlTransaction tx = con.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmdRent = new SqlCommand("DELETE FROM Rent WHERE propertyID = @pid", con, tx))
                            {
                                cmdRent.Parameters.AddWithValue("@pid", propertyID);
                                cmdRent.ExecuteNonQuery();
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

                            tx.Commit();

                            if (flowLayoutPanelRightSideBar.InvokeRequired)
                            {
                                flowLayoutPanelRightSideBar.Invoke((Action)(() =>
                                {
                                    flowLayoutPanelRightSideBar.Controls.Remove(PropertyCards);
                                    PropertyCards.Dispose();
                                }));
                            }
                            else
                            {
                                flowLayoutPanelRightSideBar.Controls.Remove(PropertyCards);
                                PropertyCards.Dispose();
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
                Panel card = CreatePropertyCard(
                    propertyID,
                    row["PropertyName"].ToString(),
                    row["UnitNumber"].ToString(),
                    row["RentAmount"].ToString(),
                    row["FirstName"].ToString(),
                    row["LastName"].ToString(),
                    row["MiddleName"].ToString(),
                    row["contactNumber"].ToString(),
                    row["Email"].ToString(),
                    row["Street"].ToString(),
                    row["Barangay"].ToString(),
                    row["City"].ToString(),
                    row["Province"].ToString(),
                    row["PostalCode"].ToString()
                );
                flowLayoutPanelRightSideBar.Controls.Add(card);
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
            dashboard.Show();
            this.Hide();
        }

        // -------------------- Admin Accounts Buttons Click Event -------------------- //
        private void btnAdminAcc_Click(object sender, EventArgs e)
        {
            SuperAdmin_AdminAccounts AdminAccount = new SuperAdmin_AdminAccounts(UserName, UserRole);
            AdminAccount.Show();
            this.Hide();
        }

        // -------------------- Tenants Buttons Click Event -------------------- //
        private void btnTenant_Click(object sender, EventArgs e)
        {
            Tenants tenant = new Tenants(UserName, UserRole);
            tenant.Show();
            this.Hide();
        }

        // -------------------- Payment Accounts Buttons Click Event -------------------- //
        private void btnPaymentRec_Click(object sender, EventArgs e)
        {
            Payment_Records payments = new Payment_Records(UserName, UserRole);
            payments.Show();
            this.Hide();
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
                Panel card = CreatePropertyCard(
                    propertyID,
                    row["PropertyName"].ToString(),
                    row["UnitNumber"].ToString(),
                    row["RentAmount"].ToString(),
                    row["FirstName"].ToString(),
                    row["LastName"].ToString(),
                    row["MiddleName"].ToString(),
                    row["ContactNumber"].ToString(),
                    row["Email"].ToString(),
                    row["Street"].ToString(),
                    row["Barangay"].ToString(),
                    row["City"].ToString(),
                    row["Province"].ToString(),
                    row["PostalCode"].ToString()
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

        private void AddProperties_PropertyAdded(object sender, EventArgs e)
        {
            propertyData = GetProperties();
            DisplayPropertyCards(propertyData);
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

    }
}
