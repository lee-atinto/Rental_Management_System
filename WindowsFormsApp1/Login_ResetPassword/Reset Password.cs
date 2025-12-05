using Rental_Management_System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Super_Admin_Account;

namespace WindowsFormsApp1.Login_ResetPassword
{
    public partial class Reset_Password : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        public Reset_Password()
        {
            InitializeComponent();

            this.panelStrengthBar.Visible = false;
            this.lblStrengthMessage.Visible = false;
            this.lbMatchPassword.Visible = false;
        }

        // -------------------- Password Strength Scoring Method -------------------- //
        private int GetPasswordStrengthScore(string password)
        {
            int score = 0;

            if (password.Length >= 8) score += 1;
            if (password.Length >= 10) score += 1;

            if (Regex.IsMatch(password, @"[A-Z]")) score += 1;
            if (Regex.IsMatch(password, @"[a-z]")) score += 1;
            if (Regex.IsMatch(password, @"\d")) score += 1;
            if (Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]")) score += 1;

            return score;
        }

        // -------------------- Password Strength Check Method -------------------- //
        public static string CheckPasswordStrength(string password)
        {
            if (password.Length < 8)
            {
                return "Password must be at least 8 characters long.";
            }

            var failures = new List<string>();

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                failures.Add("one uppercase letter");
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                failures.Add("one lowercase letter");
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                failures.Add("one number");
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
            {
                failures.Add("one special character (e.g., !@#$%)");
            }

            if (failures.Any())
            {
                string requirementList = string.Join(", ", failures);
                return $"Password must contain: {requirementList}.";
            }

            return "";
        }

        // -------------------- Password Hashing Method -------------------- //
        public string HashPassword(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        }

        private void Reset_Password_Load(object sender, EventArgs e)
        {
            this.panelStrengthBar.Visible = false;
            this.lblStrengthMessage.Visible = false;
            this.lbMatchPassword.Visible = false;
        }

        // -------------------- New Password Text Changed Event -------------------- //
        private void TbNewPassword_TextChanged(object sender, EventArgs e)
        {
            string password = TbNewPassword.Text;

            if (string.IsNullOrEmpty(password))
            {
                panelStrengthBar.Visible = false;
                lblStrengthMessage.Visible = false;
                return;
            }

            panelStrengthBar.Visible = true;
            lblStrengthMessage.Visible = true;

            int score = GetPasswordStrengthScore(password);
            string strengthText;
            Color indicatorColor;

            if (score <= 2)
            {
                indicatorColor = Color.Red;
                strengthText = "Password is weak";
            }
            else if (score <= 4)
            {
                indicatorColor = Color.Orange;
                strengthText = "Password is medium";
            }
            else
            {
                indicatorColor = Color.Green;
                strengthText = "Password is strong";
            }
            panelStrengthBar.BackColor = indicatorColor;
            lblStrengthMessage.Text = strengthText;
            lblStrengthMessage.ForeColor = indicatorColor;
        }

        // -------------------- Confirm Password Text Changed Event -------------------- //
        private void TbConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            string newPass = TbNewPassword.Text;
            string confirmPass = TbConfirmPassword.Text;

            if (!string.IsNullOrEmpty(confirmPass) && newPass != confirmPass)
            {
                lbMatchPassword.Text = "Passwords do not match";
                lbMatchPassword.Visible = true;

                this.TbConfirmPassword.BackColor = Color.LightPink;
            }
            else
            {
                lbMatchPassword.Visible = false;
                this.TbConfirmPassword.BackColor = SystemColors.Window;
            }
        }

        // -------------------- Reset Password Button -------------------- //
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string email = TbEmail.Text.Trim();
            string newPass = TbNewPassword.Text;
            string confirmPass = TbConfirmPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Please fill all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("New passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string validationResult = CheckPasswordStrength(newPass);

            if (!string.IsNullOrEmpty(validationResult))
            {
                MessageBox.Show(
                    $"Password Policy Violation: {validationResult}",
                    "Password Security Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // --- START: Database Logic for Password Reset ---

            // 1. Hash the new password using the method you defined
            string hashedPassword = HashPassword(newPass);

            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string updateQuery = "UPDATE Account SET password_hash = @hash, last_login = NULL WHERE email = @email";

                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@hash", hashedPassword);
                cmd.Parameters.AddWithValue("@email", email);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show(
                            "Password reset successfully. You can now log in with your new password.",
                            "Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        LinkSignIn_LinkClicked(null, null);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Email not found. Please check your email address.",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Database Error: Failed to reset password. {ex.Message}",
                                    "Database Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        private void LinkSignIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            this.Hide();
        }
    }
}