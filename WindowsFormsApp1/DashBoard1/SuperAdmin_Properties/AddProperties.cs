using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_Properties
{
    public partial class AddProperties : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private int propertyID = 0;
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
        }

        // -------------------- ADD MODE -------------------- //
        public AddProperties()
        {
            InitializeComponent();
            panel1.AutoScroll = true;
        }

        private void AddProperties_Load(object sender, EventArgs e)
        {
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

        // -------------------- EDIT MODE -------------------- //
        public AddProperties(int propertyID, string firstName, string lastName, string middleName, string contactNumber,
                     string email, string street, string barangay, string city, string province, string postalCode,
                     string propertyName, string unitNumber, decimal rentAmount)
        {
            InitializeComponent();
            this.propertyID = propertyID;

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
                tbRentAmount.Text = rentAmount.ToString();

                panel1.AutoScroll = true;
            }
            else
            {

            }
        }

        // -------------------- SAVE / UPDATE -------------------- //
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbFirstName.Text) || string.IsNullOrWhiteSpace(tbPropertyName.Text))
            {
                MessageBox.Show("First Name and Property Name are required fields.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Property property = new Property
            {
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                MiddleName = tbMiddleName.Text,
                ContactNumber = tbContactNumber.Text,
                Email = tbEmail.Text,

                Street = tbStreet.Text,
                Barangay = tbBarangay.Text,
                City = tbCity.Text,
                Province = tbProvince.Text,
                PostalCode = tbPostalCode.Text,

                PropertyName = tbPropertyName.Text,
                UnitNumber = tbUnitNumber.Text,
                RentAmount = decimal.TryParse(tbRentAmount.Text, out decimal rent) ? rent : 0.00m
            };

            if (IsEditMode)
            {
                UpdatePropertyInDatabase(propertyID, property);
            }
            else
            {
                SavePropertyToDatabase(property);
            }

            PropertyAdded?.Invoke(this, EventArgs.Empty);
        }

        // -------------------- INSERT NEW PROPERTY -------------------- //
        private void SavePropertyToDatabase(Property property)
        {
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    string insertOwner = @"INSERT INTO PropertyOwner (firstName, middleName, lastName) OUTPUT INSERTED.propertyOwnerID VALUES (@fn, @mn, @ln)";
                    SqlCommand cmdOwner = new SqlCommand(insertOwner, con, transaction);
                    cmdOwner.Parameters.AddWithValue("@fn", property.FirstName);
                    cmdOwner.Parameters.AddWithValue("@mn", property.MiddleName);
                    cmdOwner.Parameters.AddWithValue("@ln", property.LastName);
                    int ownerID = (int)cmdOwner.ExecuteScalar();

                    string insertAddress = @"INSERT INTO Address (street, barangay, city, province, postalCode) OUTPUT INSERTED.AddressID VALUES (@st, @brgy, @city, @prov, @pc)";
                    SqlCommand cmdAddress = new SqlCommand(insertAddress, con, transaction);
                    cmdAddress.Parameters.AddWithValue("@st", property.Street);
                    cmdAddress.Parameters.AddWithValue("@brgy", property.Barangay);
                    cmdAddress.Parameters.AddWithValue("@city", property.City);
                    cmdAddress.Parameters.AddWithValue("@prov", property.Province);
                    cmdAddress.Parameters.AddWithValue("@pc", property.PostalCode);
                    int addressID = (int)cmdAddress.ExecuteScalar();

                    string insertProperty = @"INSERT INTO Property (PropertyName, UnitNumber, AddressID, propertyOwnerID) OUTPUT INSERTED.propertyID VALUES (@pname, @unit, @aid, @oid)";
                    SqlCommand cmdProperty = new SqlCommand(insertProperty, con, transaction);
                    cmdProperty.Parameters.AddWithValue("@pname", property.PropertyName);
                    cmdProperty.Parameters.AddWithValue("@unit", property.UnitNumber);
                    cmdProperty.Parameters.AddWithValue("@aid", addressID);
                    cmdProperty.Parameters.AddWithValue("@oid", ownerID);
                    int newPropertyID = (int)cmdProperty.ExecuteScalar();

                    string insertRent = @"INSERT INTO Rent (propertyID, RentAmount) VALUES (@pid, @rent)";
                    SqlCommand cmdRent = new SqlCommand(insertRent, con, transaction);
                    cmdRent.Parameters.AddWithValue("@pid", newPropertyID);
                    cmdRent.Parameters.AddWithValue("@rent", property.RentAmount);
                    cmdRent.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Property successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // -------------------- UPDATE EXISTING PROPERTY -------------------- //
        private void UpdatePropertyInDatabase(int propertyID, Property property)
        {
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    string updateOwner = @"UPDATE PO SET firstName=@fn, middleName=@mn, lastName=@ln FROM PropertyOwner PO INNER JOIN Property P ON P.propertyOwnerID = PO.propertyOwnerID WHERE P.propertyID=@pid";
                    SqlCommand cmdOwner = new SqlCommand(updateOwner, con, transaction);
                    cmdOwner.Parameters.AddWithValue("@fn", property.FirstName);
                    cmdOwner.Parameters.AddWithValue("@mn", property.MiddleName);
                    cmdOwner.Parameters.AddWithValue("@ln", property.LastName);
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

                    string updateProperty = @"UPDATE Property SET PropertyName=@pname, UnitNumber=@unit WHERE propertyID=@pid";
                    SqlCommand cmdProperty = new SqlCommand(updateProperty, con, transaction);
                    cmdProperty.Parameters.AddWithValue("@pname", property.PropertyName);
                    cmdProperty.Parameters.AddWithValue("@unit", property.UnitNumber);
                    cmdProperty.Parameters.AddWithValue("@pid", propertyID);
                    cmdProperty.ExecuteNonQuery();

                    string updateRent = @"UPDATE Rent SET RentAmount=@rent WHERE propertyID=@pid";
                    SqlCommand cmdRent = new SqlCommand(updateRent, con, transaction);
                    cmdRent.Parameters.AddWithValue("@rent", property.RentAmount);
                    cmdRent.Parameters.AddWithValue("@pid", propertyID);
                    cmdRent.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Property successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
