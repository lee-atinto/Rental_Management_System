using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount
{
    public partial class AddAmin_Acccounts : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS;Initial Catalog=RentalManagementSystem;Integrated Security=True";
        private SuperAdmin_AdminAccounts parentForm;
        private int? EditingUserID = null;

        public AddAmin_Acccounts(SuperAdmin_AdminAccounts parent)
        {
            InitializeComponent();
            parentForm = parent;
            this.Text = "Add New Admin Account";
        }

        public AddAmin_Acccounts(SuperAdmin_AdminAccounts parent, int userID, string username, string email, string role, string active, string lastLogin)
        {
            InitializeComponent();
            parentForm = parent;
            EditingUserID = userID;

            this.Text = "Edit Admin Account";
            tbUsername.Text = username;
            tbEmail.Text = email;

            if (role == "Super Admin")
                rbtnSuperAdmin.Checked = true;
            else
                rbtnAdmin.Checked = true;

            tbPassword.Clear();
            tbConfirmPassword.Clear();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        private void AddAmin_Acccounts_Load(object sender, EventArgs e) 
        { 

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbUsername.Text) || string.IsNullOrWhiteSpace(tbEmail.Text))
                {
                    MessageBox.Show("Username and Email are required.");
                    return;
                }

                if (!rbtnAdmin.Checked && !rbtnSuperAdmin.Checked)
                {
                    MessageBox.Show("Please select a Role.");
                    return;
                }

                if (EditingUserID == null && string.IsNullOrWhiteSpace(tbPassword.Text))
                {
                    MessageBox.Show("Password is required for new accounts.");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(tbPassword.Text) && tbPassword.Text != tbConfirmPassword.Text)
                {
                    MessageBox.Show("Passwords do not match!");
                    return;
                }

                string role = rbtnAdmin.Checked ? "Admin" : "Super Admin";

                using (SqlConnection con = new SqlConnection(DataConnection))
                {
                    con.Open();

                    if (EditingUserID.HasValue)
                    {
                        string query = "UPDATE Account SET Username=@Username, Email=@Email, Role=@Role" + (string.IsNullOrWhiteSpace(tbPassword.Text) ? "" : ", password_Hash=@Password") + " WHERE user_id=@UserID";

                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Username", tbUsername.Text);
                        cmd.Parameters.AddWithValue("@Email", tbEmail.Text);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@UserID", EditingUserID.Value);

                        if (!string.IsNullOrWhiteSpace(tbPassword.Text))
                            cmd.Parameters.AddWithValue("@Password", HashPassword(tbPassword.Text));

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Account updated successfully.");
                    }
                    else
                    {
                        string hashedPassword = HashPassword(tbPassword.Text);
                        string query = "INSERT INTO Account (Username, Email, Role, password_Hash, active) " + "VALUES (@Username, @Email, @Role, @Password, 1)";

                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Username", tbUsername.Text);
                        cmd.Parameters.AddWithValue("@Email", tbEmail.Text);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Account added successfully.");
                    }
                }

                parentForm.LoadAdmins();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message);
            }
        }
    }
}
