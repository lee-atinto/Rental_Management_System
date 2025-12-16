using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Linq;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_Properties
{
    public partial class AddProperties : Form
    {
        private readonly string DataConnection = System.Configuration.ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private int propertyID = 0;
        private int unitID = 0;
        public bool IsEditMode => propertyID > 0;

        public event EventHandler PropertyAdded;

        public class Property
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }
            public string ContactNumber { get; set; }
            public string Email { get; set; }
            public string Street { get; set; }
            public string Barangay { get; set; }
            public string City { get; set; }
            public string Province { get; set; }
            public string PostalCode { get; set; }
            public string PropertyName { get; set; }
            public string UnitNumber { get; set; }
            public decimal RentAmount { get; set; }
            public string UnitType { get; set; }
            public string Status { get; set; }
        }

        public AddProperties()
        {
            InitializeComponent();
            panel1.AutoScroll = true;
            tbMonthlyRent.TextChanged += tbMonthlyRent_TextChanged;
        }

        private void AddProperties_Load(object sender, EventArgs e)
        {
            PopulateComboBoxes();

            if (propertyID > 0)
            {
                btnSave.Text = "Update Property";
                this.Text = "Edit Property";
            }
            else
            {
                btnSave.Text = "Add Property";
                this.Text = "Add New Property";
            }
        }

        private void PopulateComboBoxes()
        {
            cbUnitType.Items.Clear();
            cbUnitStatus.Items.Clear();

            string[] unitTypes = { "Studio", "1-Bedroom", "2-Bedroom", "Suite", "Twin Room", "Double Room", "Loft", "Penthouse", "Family Room", "Executive" };
            cbUnitType.Items.AddRange(unitTypes);

            string[] unitStatuses = { "Vacant", "Occupied", "Under Maintenance", "Reserved" };
            cbUnitStatus.Items.AddRange(unitStatuses);

            if (!IsEditMode)
            {
                if (cbUnitType.Items.Count > 0) cbUnitType.SelectedIndex = 0;
                if (cbUnitStatus.Items.Count > 0) cbUnitStatus.SelectedIndex = 0;
            }
        }

        public AddProperties(int propertyID, string firstName, string lastName, string middleName, string contactNumber,
              string email, string street, string barangay, string city, string province, string postalCode,
              string propertyName, string unitNumber, decimal rentAmount, int unitID, string unitType, string status)
        {
            InitializeComponent();
            this.propertyID = propertyID;
            this.unitID = unitID;

            if (propertyID > 0)
            {
                tbFirstName.Text = firstName;
                tbLastName.Text = lastName;
                tbMiddleName.Text = middleName;
                tbContactNumber.Text = contactNumber;
                tbEmail.Text = email;
                tbStreet.Text = street;
                tbBarangay.Text = barangay;
                tbCity.Text = city;
                tbProvince.Text = province;
                tbPostalCode.Text = postalCode;
                tbPropertyName.Text = propertyName;
                tbUnitNumber.Text = unitNumber;
                tbMonthlyRent.Text = rentAmount.ToString();
                cbUnitType.Text = unitType;
                cbUnitStatus.Text = status;
                panel1.AutoScroll = true;
                tbMonthlyRent.TextChanged += tbMonthlyRent_TextChanged;
            }
        }

        private void tbMonthlyRent_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbMonthlyRent.Text, "[^0-9.]"))
            {
                tbMonthlyRent.Text = System.Text.RegularExpressions.Regex.Replace(tbMonthlyRent.Text, "[^0-9.]", "");
                tbMonthlyRent.SelectionStart = tbMonthlyRent.Text.Length;
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFirstName.Text) ||
                string.IsNullOrWhiteSpace(tbLastName.Text) ||
                string.IsNullOrWhiteSpace(tbContactNumber.Text) ||
                string.IsNullOrWhiteSpace(tbStreet.Text) ||
                string.IsNullOrWhiteSpace(tbBarangay.Text) ||
                string.IsNullOrWhiteSpace(tbCity.Text) ||
                string.IsNullOrWhiteSpace(tbProvince.Text) ||
                string.IsNullOrWhiteSpace(tbPostalCode.Text) ||
                string.IsNullOrWhiteSpace(tbPropertyName.Text) ||
                string.IsNullOrWhiteSpace(tbUnitNumber.Text) ||
                string.IsNullOrWhiteSpace(tbMonthlyRent.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(tbMonthlyRent.Text, out decimal rentAmount))
            {
                MessageBox.Show("Invalid Monthly Rent. Numbers only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Property property = new Property
            {
                FirstName = tbFirstName.Text.Trim(),
                LastName = tbLastName.Text.Trim(),
                MiddleName = tbMiddleName.Text.Trim(),
                ContactNumber = tbContactNumber.Text.Trim(),
                Email = tbEmail.Text.Trim(),
                Street = tbStreet.Text.Trim(),
                Barangay = tbBarangay.Text.Trim(),
                City = tbCity.Text.Trim(),
                Province = tbProvince.Text.Trim(),
                PostalCode = tbPostalCode.Text.Trim(),
                PropertyName = tbPropertyName.Text.Trim(),
                UnitNumber = tbUnitNumber.Text.Trim(),
                RentAmount = rentAmount,
                UnitType = cbUnitType.Text,
                Status = cbUnitStatus.Text
            };

            if (IsEditMode)
            {
                UpdatePropertyInDatabase(propertyID, unitID, property);
            }
            else
            {
                SavePropertyToDatabase(property);
            }

            PropertyAdded?.Invoke(this, EventArgs.Empty);
        }

        private void SavePropertyToDatabase(Property property)
        {
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    string insertOwner = @"INSERT INTO PropertyOwner (firstName, middleName, lastName, contactNumber, Email) OUTPUT INSERTED.propertyOwnerID VALUES (@fn, @mn, @ln, @cn, @em)";
                    SqlCommand cmdOwner = new SqlCommand(insertOwner, con, transaction);
                    cmdOwner.Parameters.AddWithValue("@fn", property.FirstName);
                    cmdOwner.Parameters.AddWithValue("@mn", string.IsNullOrWhiteSpace(property.MiddleName) ? (object)DBNull.Value : property.MiddleName);
                    cmdOwner.Parameters.AddWithValue("@ln", property.LastName);
                    cmdOwner.Parameters.AddWithValue("@cn", property.ContactNumber);
                    cmdOwner.Parameters.AddWithValue("@em", string.IsNullOrWhiteSpace(property.Email) ? (object)DBNull.Value : property.Email);
                    int ownerID = (int)cmdOwner.ExecuteScalar();

                    string insertAddress = @"INSERT INTO Address (street, barangay, city, province, postalCode) OUTPUT INSERTED.AddressID VALUES (@st, @brgy, @city, @prov, @pc)";
                    SqlCommand cmdAddress = new SqlCommand(insertAddress, con, transaction);
                    cmdAddress.Parameters.AddWithValue("@st", property.Street);
                    cmdAddress.Parameters.AddWithValue("@brgy", property.Barangay);
                    cmdAddress.Parameters.AddWithValue("@city", property.City);
                    cmdAddress.Parameters.AddWithValue("@prov", property.Province);
                    cmdAddress.Parameters.AddWithValue("@pc", property.PostalCode);
                    int addressID = (int)cmdAddress.ExecuteScalar();

                    string insertProperty = @"INSERT INTO Property (PropertyName, AddressID, propertyOwnerID) OUTPUT INSERTED.propertyID VALUES (@pname, @aid, @oid)";
                    SqlCommand cmdProperty = new SqlCommand(insertProperty, con, transaction);
                    cmdProperty.Parameters.AddWithValue("@pname", property.PropertyName);
                    cmdProperty.Parameters.AddWithValue("@aid", addressID);
                    cmdProperty.Parameters.AddWithValue("@oid", ownerID);
                    int newPropertyID = (int)cmdProperty.ExecuteScalar();

                    string insertUnit = @"INSERT INTO Unit (PropertyID, UnitNumber, MonthlyRent, Status, UnitType) VALUES (@pid, @unitNum, @rent, @status, @type)";
                    SqlCommand cmdUnit = new SqlCommand(insertUnit, con, transaction);
                    cmdUnit.Parameters.AddWithValue("@pid", newPropertyID);
                    cmdUnit.Parameters.AddWithValue("@unitNum", property.UnitNumber);
                    cmdUnit.Parameters.AddWithValue("@rent", property.RentAmount);
                    cmdUnit.Parameters.AddWithValue("@status", property.Status);
                    cmdUnit.Parameters.AddWithValue("@type", property.UnitType);
                    cmdUnit.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Property successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdatePropertyInDatabase(int propertyID, int unitID, Property property)
        {
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    string updateOwner = @"UPDATE PO SET firstName=@fn, middleName=@mn, lastName=@ln, contactNumber=@cn, Email=@em FROM PropertyOwner PO INNER JOIN Property P ON P.propertyOwnerID = PO.propertyOwnerID WHERE P.propertyID=@pid";
                    SqlCommand cmdOwner = new SqlCommand(updateOwner, con, transaction);
                    cmdOwner.Parameters.AddWithValue("@fn", property.FirstName);
                    cmdOwner.Parameters.AddWithValue("@mn", string.IsNullOrWhiteSpace(property.MiddleName) ? (object)DBNull.Value : property.MiddleName);
                    cmdOwner.Parameters.AddWithValue("@ln", property.LastName);
                    cmdOwner.Parameters.AddWithValue("@cn", property.ContactNumber);
                    cmdOwner.Parameters.AddWithValue("@em", string.IsNullOrWhiteSpace(property.Email) ? (object)DBNull.Value : property.Email);
                    cmdOwner.Parameters.AddWithValue("@pid", propertyID);
                    cmdOwner.ExecuteNonQuery();

                    string updateAddress = @"UPDATE A SET street=@st, barangay=@brgy, city=@city, province=@prov, postalCode=@pc FROM Address A INNER JOIN Property P ON P.AddressID = A.AddressID WHERE P.propertyID=@pid";
                    SqlCommand cmdAddress = new SqlCommand(updateAddress, con, transaction);
                    cmdAddress.Parameters.AddWithValue("@st", property.Street);
                    cmdAddress.Parameters.AddWithValue("@brgy", property.Barangay);
                    cmdAddress.Parameters.AddWithValue("@city", property.City);
                    cmdAddress.Parameters.AddWithValue("@prov", property.Province);
                    cmdAddress.Parameters.AddWithValue("@pc", property.PostalCode);
                    cmdAddress.Parameters.AddWithValue("@pid", propertyID);
                    cmdAddress.ExecuteNonQuery();

                    SqlCommand cmdProperty = new SqlCommand("UPDATE Property SET PropertyName=@pname WHERE propertyID=@pid", con, transaction);
                    cmdProperty.Parameters.AddWithValue("@pname", property.PropertyName);
                    cmdProperty.Parameters.AddWithValue("@pid", propertyID);
                    cmdProperty.ExecuteNonQuery();

                    SqlCommand cmdUnit = new SqlCommand("UPDATE Unit SET UnitNumber=@unitNum, MonthlyRent=@rent, UnitType=@type, Status=@status WHERE UnitID=@uid", con, transaction);
                    cmdUnit.Parameters.AddWithValue("@unitNum", property.UnitNumber);
                    cmdUnit.Parameters.AddWithValue("@rent", property.RentAmount);
                    cmdUnit.Parameters.AddWithValue("@type", property.UnitType);
                    cmdUnit.Parameters.AddWithValue("@status", property.Status);
                    cmdUnit.Parameters.AddWithValue("@uid", unitID);
                    cmdUnit.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Property successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error updating database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e) => this.Close();

        private void tbContactNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back) e.Handled = true;
            if (char.IsDigit(e.KeyChar) && tbContactNumber.Text.Length >= 11) e.Handled = true;
        }

        private void tbMonthlyRent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '.') e.Handled = true;
            if (e.KeyChar == '.' && tbMonthlyRent.Text.Contains(".")) e.Handled = true;
        }
    }
}