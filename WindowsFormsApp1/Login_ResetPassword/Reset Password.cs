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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Super_Admin_Account;
using WindowsFormsApp1.Login_ResetPassword;

namespace WindowsFormsApp1.Login_ResetPassword
{
    public partial class Reset_Password : Form
    {
        private readonly string DataConnection = ConfigurationManager.ConnectionStrings["DB"]?.ConnectionString ?? @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";

        private const int MaxHistoryCount = 5;
        private const int MaxResetAttempts = 5;
        private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(10);

        private CheckBox chkShowPasswordProgrammatic;

        public Reset_Password()
        {
            InitializeComponent();

            this.panelStrengthBar.Visible = false;
            this.lblStrengthMessage.Visible = false;
            this.lbMatchPassword.Visible = false;

            AddShowHideCheckbox();
        }

        private void AddShowHideCheckbox()
        {
            try
            {
                chkShowPasswordProgrammatic = new CheckBox
                {
                    Text = "Show Password",
                    AutoSize = true,
                    Location = new Point(Math.Min(TbConfirmPassword.Right + 10, this.ClientSize.Width - 120), TbConfirmPassword.Top + 3),
                    TabIndex = TbConfirmPassword.TabIndex + 1
                };
                chkShowPasswordProgrammatic.CheckedChanged += (s, e) =>
                {
                    TbNewPassword.UseSystemPasswordChar = !chkShowPasswordProgrammatic.Checked;
                    TbConfirmPassword.UseSystemPasswordChar = !chkShowPasswordProgrammatic.Checked;
                };
                this.Controls.Add(chkShowPasswordProgrammatic);
            }
            catch
            {

            }
        }

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

        private async void BtnLogin_Click(object sender, EventArgs e)
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

            string helperValidation = PasswordAuto.ValidatePassword(newPass);
            if (!string.IsNullOrEmpty(helperValidation))
            {
                MessageBox.Show($"Password Policy Violation: {helperValidation}", "Password Security Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DataConnection))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmdFind = new SqlCommand("SELECT user_id, reset_failed_attempts, reset_lock_until FROM Account WHERE email = @email", conn))
                    {
                        cmdFind.Parameters.Add("@email", SqlDbType.NVarChar, 200).Value = email;

                        using (SqlDataReader reader = await cmdFind.ExecuteReaderAsync(CommandBehavior.SingleRow))
                        {
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("Email not found. Please check your email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            await reader.ReadAsync();
                            int accountId = reader.GetInt32(0);
                            int resetAttempts = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            DateTime? lockUntil = null;

                            if (!reader.IsDBNull(2))
                                lockUntil = reader.GetDateTime(2);
                            reader.Close();

                            if (lockUntil.HasValue && lockUntil.Value > DateTime.Now)
                            {
                                MessageBox.Show($"Too many attempts. Please try again at {lockUntil.Value}.", "Locked Out", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            using (SqlCommand cmdHistory = new SqlCommand("SELECT TOP (@count) old_password_hash FROM PasswordHistory WHERE account_id = @aid ORDER BY changed_date DESC", conn))
                            {
                                cmdHistory.Parameters.Add("@count", SqlDbType.Int).Value = MaxHistoryCount;
                                cmdHistory.Parameters.Add("@aid", SqlDbType.Int).Value = accountId;

                                using (SqlDataReader histReader = await cmdHistory.ExecuteReaderAsync())
                                {
                                    while (await histReader.ReadAsync())
                                    {
                                        if (!histReader.IsDBNull(0))
                                        {
                                            string oldHash = histReader.GetString(0);
                                            if (PasswordAuto.VerifyPassword(newPass, oldHash))
                                            {
                                                await IncrementResetAttempt(conn, accountId, resetAttempts + 1);
                                                MessageBox.Show($"You cannot reuse any of your last {MaxHistoryCount} passwords.", "Password Reuse Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }

                            string hashedPassword = HashPassword(newPass);

                            using (SqlCommand cmdUpdate = new SqlCommand("UPDATE Account SET password_hash = @hash, reset_failed_attempts = 0, reset_lock_until = NULL, last_login = NULL WHERE user_id = @id", conn))
                            {
                                cmdUpdate.Parameters.Add("@hash", SqlDbType.NVarChar, 255).Value = hashedPassword;
                                cmdUpdate.Parameters.Add("@id", SqlDbType.Int).Value = accountId;

                                int rowsAffected = await cmdUpdate.ExecuteNonQueryAsync();
                                if (rowsAffected > 0)
                                {
                                    using (SqlCommand cmdInsertHistory = new SqlCommand("INSERT INTO PasswordHistory (account_id, old_password_hash, changed_date) VALUES (@aid, @hash, GETDATE())", conn))
                                    {
                                        cmdInsertHistory.Parameters.Add("@aid", SqlDbType.Int).Value = accountId;
                                        cmdInsertHistory.Parameters.Add("@hash", SqlDbType.NVarChar, 255).Value = hashedPassword;
                                        await cmdInsertHistory.ExecuteNonQueryAsync();
                                    }

                                    using (SqlCommand cmdTrim = new SqlCommand(@"
                                WITH ToDelete AS (
                                    SELECT history_id, ROW_NUMBER() OVER (ORDER BY changed_date DESC) AS rn
                                    FROM PasswordHistory
                                    WHERE account_id = @aidTrim
                                )
                                DELETE FROM PasswordHistory WHERE history_id IN (SELECT history_id FROM ToDelete WHERE rn > @keep)
                            ", conn))
                                    {
                                        cmdTrim.Parameters.Add("@aidTrim", SqlDbType.Int).Value = accountId;
                                        cmdTrim.Parameters.Add("@keep", SqlDbType.Int).Value = MaxHistoryCount;

                                        await cmdTrim.ExecuteNonQueryAsync();
                                    }

                                    MessageBox.Show("Password reset successfully. You can now log in with your new password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LinkSignIn_LinkClicked(null, null);
                                    return;
                                }
                                else
                                {
                                    await IncrementResetAttempt(conn, accountId, resetAttempts + 1);
                                    MessageBox.Show("Failed to reset password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: Failed to reset password. {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private async Task IncrementResetAttempt(SqlConnection conn, int accountId, int newAttemptCount)
        {
            try
            {
                DateTime? lockUntil = null;
                if (newAttemptCount >= MaxResetAttempts)
                {
                    lockUntil = DateTime.Now.Add(LockoutDuration);
                }

                using (SqlCommand cmd = new SqlCommand("UPDATE Account SET reset_failed_attempts = @att, reset_lock_untils = @lock WHERE user_id = @id", conn))
                {
                    cmd.Parameters.Add("@att", SqlDbType.Int).Value = newAttemptCount;
                    if (lockUntil.HasValue)
                        cmd.Parameters.Add("@lock", SqlDbType.DateTime).Value = lockUntil.Value;
                    else
                        cmd.Parameters.Add("@lock", SqlDbType.DateTime).Value = DBNull.Value;

                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = accountId;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch
            {

            }
        }

        private void LinkSignIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            this.Hide();
        }

        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            TbNewPassword.UseSystemPasswordChar = !ShowPassword.Checked;
            TbConfirmPassword.UseSystemPasswordChar = !ShowPassword.Checked;
        }

    }
}
