using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_Properties
{
    public partial class AddProperties : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private int propertyID = 0;
        private int unitID = 0; // FIX: Added unitID field to manage specific unit editing/updating
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
            public decimal RentAmount { get; set; } // Renamed UI control to tbMonthlyRent, but internal class property remains RentAmount (or monthly rent)
            public string UnitType { get; set; } // FIX: Added UnitType
            public string Status { get; set; } // FIX: Added Status
        }

        // -------------------- ADD MODE -------------------- //
        public AddProperties()
        {
            InitializeComponent();
            panel1.AutoScroll = true;
        }

        private void AddProperties_Load(object sender, EventArgs e)
        {
            // NEW: Populate the ComboBoxes first
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

        // NEW: Method to populate the Unit Type and Unit Status ComboBoxes
        private void PopulateComboBoxes()
        {
            // Clear existing items (safe guard against multiple calls)
            cbUnitType.Items.Clear();
            cbUnitStatus.Items.Clear();

            // 1. Unit Type options
            string[] unitTypes = { "Apartment", "Condominium", "House", "Dormitory", "Commercial Space", "Studio" };
            cbUnitType.Items.AddRange(unitTypes);

            // 2. Unit Status options
            string[] unitStatuses = { "Vacant", "Occupied", "Under Maintenance", "Reserved" };
            cbUnitStatus.Items.AddRange(unitStatuses);

            // Set default selection for ADD mode only (In Edit Mode, values are set by the constructor)
            if (!IsEditMode)
            {
                if (cbUnitType.Items.Count > 0)
                {
                    cbUnitType.SelectedIndex = 0; // Default to the first type (Apartment)
                }
                if (cbUnitStatus.Items.Count > 0)
                {
                    cbUnitStatus.SelectedIndex = 0; // Default to the first status (Vacant)
                }
            }
        }

        // -------------------- EDIT MODE -------------------- //
        // FIX: Added unitID, unitType, and status parameters to the edit constructor.
        // NOTE: The calling code in ProperTies.cs's Edit button handler MUST pass this new parameter.
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
            }
            else
            {

            }

        }

        // -------------------- SAVE / UPDATE -------------------- //
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // Required fields validation
            if (string.IsNullOrWhiteSpace(tbFirstName.Text) || string.IsNullOrWhiteSpace(tbPropertyName.Text))
            {
                MessageBox.Show("First Name and Property Name are required.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Contact Number validation: must be 11 digits and numeric only
            if (string.IsNullOrWhiteSpace(tbContactNumber.Text) || tbContactNumber.Text.Length != 11)
            {
                MessageBox.Show("Contact Number must be exactly 11 digits.", "Invalid Contact", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!long.TryParse(tbContactNumber.Text, out _))
            {
                MessageBox.Show("Contact Number must contain only numbers.", "Invalid Contact", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Rent Amount validation: must be numeric
            if (!decimal.TryParse(tbMonthlyRent.Text, out decimal rentAmount)) // FIX: Changed to tbMonthlyRent
            {
                MessageBox.Show("Monthly Rent must be a valid number.", "Invalid Rent", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prepare property object
            Property property = new Property
            {
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                MiddleName = tbMiddleName.Text,
                ContactNumber = tbContactNumber.Text,
                Email = string.IsNullOrWhiteSpace(tbEmail.Text) ? null : tbEmail.Text, // email optional

                Street = tbStreet.Text,
                Barangay = tbBarangay.Text,
                City = tbCity.Text,
                Province = tbProvince.Text,
                PostalCode = tbPostalCode.Text,

                PropertyName = tbPropertyName.Text,
                UnitNumber = tbUnitNumber.Text,
                RentAmount = rentAmount,
                UnitType = cbUnitType.Text,  // FIX: Get UnitType from new ComboBox
                Status = cbUnitStatus.Text   // FIX: Get Status from new ComboBox
            };

            // Save or update
            if (IsEditMode)
            {
                // FIX: Ensure unitID is available for update mode
                if (unitID > 0)
                {
                    UpdatePropertyInDatabase(propertyID, unitID, property);
                }
                else
                {
                    MessageBox.Show("Cannot update unit: UnitID not found.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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
                    string insertOwner = @"INSERT INTO PropertyOwner (firstName, middleName, lastName, contactNumber, Email) OUTPUT INSERTED.propertyOwnerID VALUES (@fn, @mn, @ln, @cn, @em)";
                    SqlCommand cmdOwner = new SqlCommand(insertOwner, con, transaction);
                    cmdOwner.Parameters.AddWithValue("@fn", property.FirstName);
                    cmdOwner.Parameters.AddWithValue("@mn", property.MiddleName);
                    cmdOwner.Parameters.AddWithValue("@ln", property.LastName);
                    cmdOwner.Parameters.AddWithValue("@cn", property.ContactNumber);
                    cmdOwner.Parameters.AddWithValue("@em", (object)property.Email ?? DBNull.Value);
                    int ownerID = (int)cmdOwner.ExecuteScalar();

                    string insertAddress = @"INSERT INTO Address (street, barangay, city, province, postalCode) OUTPUT INSERTED.AddressID VALUES (@st, @brgy, @city, @prov, @pc)";
                    SqlCommand cmdAddress = new SqlCommand(insertAddress, con, transaction);
                    cmdAddress.Parameters.AddWithValue("@st", property.Street);
                    cmdAddress.Parameters.AddWithValue("@brgy", property.Barangay);
                    cmdAddress.Parameters.AddWithValue("@city", property.City);
                    cmdAddress.Parameters.AddWithValue("@prov", property.Province);
                    cmdAddress.Parameters.AddWithValue("@pc", property.PostalCode);
                    int addressID = (int)cmdAddress.ExecuteScalar();

                    // FIX: Removed UnitNumber from the Property table insert
                    string insertProperty = @"INSERT INTO Property (PropertyName, AddressID, propertyOwnerID) OUTPUT INSERTED.propertyID VALUES (@pname, @aid, @oid)";
                    SqlCommand cmdProperty = new SqlCommand(insertProperty, con, transaction);
                    cmdProperty.Parameters.AddWithValue("@pname", property.PropertyName);
                    // cmdProperty.Parameters.AddWithValue("@unit", property.UnitNumber); // REMOVED
                    cmdProperty.Parameters.AddWithValue("@aid", addressID);
                    cmdProperty.Parameters.AddWithValue("@oid", ownerID);
                    int newPropertyID = (int)cmdProperty.ExecuteScalar();

                    // FIX: Insert into the new Unit table instead of the obsolete Rent table, including UnitType and Status
                    string insertUnit = @"INSERT INTO Unit (PropertyID, UnitNumber, MonthlyRent, Status, UnitType) VALUES (@pid, @unitNum, @rent, @status, @type)";
                    SqlCommand cmdUnit = new SqlCommand(insertUnit, con, transaction);
                    cmdUnit.Parameters.AddWithValue("@pid", newPropertyID);
                    cmdUnit.Parameters.AddWithValue("@unitNum", property.UnitNumber);
                    cmdUnit.Parameters.AddWithValue("@rent", property.RentAmount);
                    cmdUnit.Parameters.AddWithValue("@status", property.Status); // Use value from cbUnitStatus
                    cmdUnit.Parameters.AddWithValue("@type", property.UnitType); // Use value from cbUnitType
                    cmdUnit.ExecuteNonQuery();

                    // REMOVED OBSOLETE RENT INSERT
                    // string insertRent = @"INSERT INTO Rent (propertyID, RentAmount) VALUES (@pid, @rent)";
                    // SqlCommand cmdRent = new SqlCommand(insertRent, con, transaction);
                    // cmdRent.Parameters.AddWithValue("@pid", newPropertyID);
                    // cmdRent.Parameters.AddWithValue("@rent", property.RentAmount);
                    // cmdRent.ExecuteNonQuery();

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

        // -------------------- UPDATE EXISTING PROPERTY -------------------- //
        // FIX: Added unitID to the method signature for specific unit update
        private void UpdatePropertyInDatabase(int propertyID, int unitID, Property property)
        {
            using (SqlConnection con = new SqlConnection(DataConnection))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // Update Owner
                    string updateOwner = @"UPDATE PO SET firstName=@fn, middleName=@mn, lastName=@ln, contactNumber=@cn, Email=@em FROM PropertyOwner PO INNER JOIN Property P ON P.propertyOwnerID = PO.propertyOwnerID WHERE P.propertyID=@pid";
                    SqlCommand cmdOwner = new SqlCommand(updateOwner, con, transaction);
                    cmdOwner.Parameters.AddWithValue("@fn", property.FirstName);
                    cmdOwner.Parameters.AddWithValue("@mn", property.MiddleName);
                    cmdOwner.Parameters.AddWithValue("@ln", property.LastName);
                    cmdOwner.Parameters.AddWithValue("@cn", property.ContactNumber);
                    cmdOwner.Parameters.AddWithValue("@em", (object)property.Email ?? DBNull.Value);
                    cmdOwner.Parameters.AddWithValue("@pid", propertyID);
                    cmdOwner.ExecuteNonQuery();

                    // Update Address
                    string updateAddress = @"UPDATE A SET street=@st, barangay=@brgy, city=@city, province=@prov, postalCode=@pc FROM Address A INNER JOIN Property P ON P.AddressID = A.AddressID WHERE P.propertyID=@pid";
                    SqlCommand cmdAddress = new SqlCommand(updateAddress, con, transaction);
                    cmdAddress.Parameters.AddWithValue("@st", property.Street);
                    cmdAddress.Parameters.AddWithValue("@brgy", property.Barangay);
                    cmdAddress.Parameters.AddWithValue("@city", property.City);
                    cmdAddress.Parameters.AddWithValue("@prov", property.Province);
                    cmdAddress.Parameters.AddWithValue("@pc", property.PostalCode);
                    cmdAddress.Parameters.AddWithValue("@pid", propertyID);
                    cmdAddress.ExecuteNonQuery();

                    // Update Property (Name only)
                    string updateProperty = @"UPDATE Property SET PropertyName=@pname WHERE propertyID=@pid";
                    SqlCommand cmdProperty = new SqlCommand(updateProperty, con, transaction);
                    cmdProperty.Parameters.AddWithValue("@pname", property.PropertyName);
                    // cmdProperty.Parameters.AddWithValue("@unit", property.UnitNumber); // REMOVED
                    cmdProperty.Parameters.AddWithValue("@pid", propertyID);
                    cmdProperty.ExecuteNonQuery();

                    // FIX: Replaced obsolete Rent update with Unit table update using UnitID, and added UnitType and Status
                    string updateUnit = @"UPDATE Unit SET UnitNumber=@unitNum, MonthlyRent=@rent, UnitType=@type, Status=@status WHERE UnitID=@uid";
                    SqlCommand cmdUnit = new SqlCommand(updateUnit, con, transaction);
                    cmdUnit.Parameters.AddWithValue("@unitNum", property.UnitNumber);
                    cmdUnit.Parameters.AddWithValue("@rent", property.RentAmount);
                    cmdUnit.Parameters.AddWithValue("@type", property.UnitType);  // FIX: Update UnitType
                    cmdUnit.Parameters.AddWithValue("@status", property.Status); // FIX: Update Status
                    cmdUnit.Parameters.AddWithValue("@uid", unitID); // Use the unitID of the specific unit
                    cmdUnit.ExecuteNonQuery();

                    // REMOVED OBSOLETE RENT UPDATE
                    // string updateRent = @"UPDATE Rent SET RentAmount=@rent WHERE propertyID=@pid";
                    // SqlCommand cmdRent = new SqlCommand(updateRent, con, transaction);
                    // cmdRent.Parameters.AddWithValue("@rent", property.RentAmount);
                    // cmdRent.Parameters.AddWithValue("@pid", propertyID);
                    // cmdRent.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Property successfully updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
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

        private void tbContactNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }

            if (char.IsDigit(e.KeyChar) && tbContactNumber.Text.Length >= 11)
            {
                e.Handled = true;
            }
        }

        // FIX: Renamed method from tbRentAmount_KeyPress to tbMonthlyRent_KeyPress
        private void tbMonthlyRent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && tbMonthlyRent.Text.Contains(".")) // FIX: Changed to tbMonthlyRent.Text
            {
                e.Handled = true;
            }
        }
    }
}