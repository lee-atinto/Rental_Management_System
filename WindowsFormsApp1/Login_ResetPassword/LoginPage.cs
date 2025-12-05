using BCrypt.Net;
using Rental_Management_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Super_Admin_Account;

namespace WindowsFormsApp1.Login_ResetPassword
{
    public partial class LoginPage : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        // --------------- Active Background Color --------------- //
        private readonly Color superAdminBackColor = Color.Thistle;
        private readonly Color adminBackColor = Color.LightBlue;
        private readonly Color inactiveBackColor = Color.White;

        // --------------- Active Text Color --------------- //
        private readonly Color superAdminTextColor = Color.DarkViolet;
        private readonly Color AdminTextColor = Color.DarkBlue;
        private readonly Color inactiveForeColor = Color.Black;

        private List<Button> navButtons;
        private string selectedRole = "";

        private void ButtonStyle(Button button)
        {
            if (button != null)
            {
                button.TabStop = false;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = inactiveForeColor;
                button.Padding = new Padding(50, 5, 5, 5);

                button.ForeColor = inactiveForeColor;
                button.BackColor = inactiveBackColor;
                button.FlatAppearance.MouseDownBackColor = inactiveBackColor;
                button.FlatAppearance.MouseOverBackColor = inactiveBackColor;

                button.TextAlign = ContentAlignment.MiddleLeft;
                button.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        public LoginPage()
        {
            InitializeComponent();

            navButtons = new List<Button> { btnSuperAdmin, btnAdmin };

            foreach (var button in navButtons)
            {
                ButtonStyle(button);
            }
        }

        // --------------- Active Button Color --------------- //
        private void ActivateButton(Button clickedButton)
        {
            foreach (var button in navButtons)
            {
                button.BackColor = inactiveBackColor;
                button.ForeColor = inactiveForeColor;
                button.FlatAppearance.BorderColor = inactiveForeColor;
                button.FlatAppearance.MouseDownBackColor = inactiveBackColor;
                button.FlatAppearance.MouseOverBackColor = inactiveBackColor;
            }

            if (clickedButton == btnSuperAdmin)
            {
                clickedButton.BackColor = superAdminBackColor;
                clickedButton.ForeColor = superAdminTextColor;
                clickedButton.FlatAppearance.BorderColor = superAdminTextColor;

                BtnLogin.Text = "Sign In as Superadmin";
                BtnLogin.BackColor = superAdminBackColor;
                BtnLogin.ForeColor = inactiveForeColor;
            }

            else if (clickedButton == btnAdmin)
            {
                clickedButton.BackColor = adminBackColor;
                clickedButton.ForeColor = AdminTextColor;
                clickedButton.FlatAppearance.BorderColor = AdminTextColor;

                BtnLogin.Text = "Sign In as Admin";
                BtnLogin.BackColor = adminBackColor;
                BtnLogin.ForeColor = inactiveForeColor;
            }
        }

        public static Image ResizeImage(Image originalImage, int newWidth, int newHeight)
        {
            if (originalImage == null)
            {
                return null;
            }

            Bitmap resizedImage = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return resizedImage;
        }

        private void UpdateLastLoginTime(string username)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string updateQuery = "UPDATE Account SET last_login = SYSDATETIME() WHERE username = @u";

                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@u", username);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Failed to update last login time for user {username}: {ex.Message}");
                }
            }
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {
            TbPassword.UseSystemPasswordChar = true;

            Image Admin = Properties.Resources.setting;
            Image SuperAdmin = Properties.Resources.shield;

            const int ICON_SIZE = 32;
            Image admin = ResizeImage(Admin, ICON_SIZE, ICON_SIZE);
            Image superadmin = ResizeImage(SuperAdmin, ICON_SIZE, ICON_SIZE);

            btnSuperAdmin.Image = superadmin;
            btnSuperAdmin.Padding = new Padding(40, 10, 40, 0);
            btnSuperAdmin.ImageAlign = ContentAlignment.TopCenter;
            btnSuperAdmin.TextAlign = ContentAlignment.BottomCenter;

            btnAdmin.Image = admin;
            btnAdmin.Padding = new Padding(40, 10, 40, 0);
            btnAdmin.ImageAlign = ContentAlignment.TopCenter;
            btnAdmin.TextAlign = ContentAlignment.BottomCenter;
        }

        private void btnSuperAdmin_Click(object sender, EventArgs e)
        {
            selectedRole = "SuperAdmin";
            ActivateButton((Button)sender);
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            selectedRole = "Admin";
            ActivateButton((Button)sender);
        }

        // -------------------- Show Password Checkbox -------------------- //
        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            TbPassword.UseSystemPasswordChar = !ShowPassword.Checked;
        }

        // -------------------- Login Button -------------------- //
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedRole))
            {
                MessageBox.Show("Please select SuperAdmin or Admin first.");
                return;
            }

            string inputUsername = TbUsername.Text.Trim();
            string inputPassword = TbPassword.Text.Trim();

            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = "SELECT username, password_hash FROM Account WHERE username=@u AND role=@role";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", inputUsername);
                cmd.Parameters.AddWithValue("@role", selectedRole);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.Read())
                    {
                        reader.Close();
                        MessageBox.Show("Invalid username or role.");
                        return;
                    }

                    string loggedInUsername = reader["username"].ToString();
                    string storedHash = reader["password_hash"].ToString();
                    reader.Close();

                    if (BCrypt.Net.BCrypt.Verify(inputPassword, storedHash))
                    {
                        MarkAdminOnline(loggedInUsername);

                        if (selectedRole == "SuperAdmin")
                        {
                            DashBoard superAdminDashboard = new DashBoard(loggedInUsername, selectedRole);
                            superAdminDashboard.Show();
                        }
                        else if (selectedRole == "Admin")
                        {
                            Admin_DashBoardcs adminDashboard = new Admin_DashBoardcs(loggedInUsername, selectedRole);
                            adminDashboard.Show();
                        }

                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Wrong password.");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}");
                }
            }
        }

        private void MarkAdminOnline(string username)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = "UPDATE Account SET active = 1, last_login = SYSDATETIME() WHERE username=@u";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Failed to mark {username} online: {ex.Message}");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string userName = "lee anthony";
            string userRole = "SuperAdmin";
            DashBoard Superadmin = new DashBoard(userName, userRole);
            Superadmin.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string userName = "jewel beduya";
            string userRole = "Admin";
            Admin_DashBoardcs Admin = new Admin_DashBoardcs(userName, userRole);
            Admin.Show();
            this.Hide();
        }
        
        // -------------------- Reset Password Link -------------------- //
        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Reset_Password login = new Reset_Password();
            login.Show();
            this.Hide();
        }
    }
}