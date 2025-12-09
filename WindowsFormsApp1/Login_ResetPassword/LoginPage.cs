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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Super_Admin_Account;

namespace WindowsFormsApp1.Login_ResetPassword
{
    public partial class LoginPage : Form
    {
        private readonly string DataConnection = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        private readonly Color superAdminBackColor = Color.Thistle;
        private readonly Color adminBackColor = Color.LightBlue;
        private readonly Color inactiveBackColor = Color.White;

        private readonly Color superAdminTextColor = Color.DarkViolet;
        private readonly Color adminTextColor = Color.DarkBlue;
        private readonly Color inactiveForeColor = Color.Black;

        private List<Button> navButtons;
        private string selectedRole = "";

        private const int MAX_FAILED_ATTEMPTS = 5;
        private readonly TimeSpan LOCKOUT_DURATION = TimeSpan.FromMinutes(10);
        private readonly TimeSpan REMEMBER_ME_DURATION = TimeSpan.FromDays(30);

        private readonly string appFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RentalManagementSystem");
        private readonly string tokenFilePath;
        private readonly string lastUserFilePath;

        private CheckBox chkRememberMe;

        public LoginPage()
        {
            InitializeComponent();

            navButtons = new List<Button> { btnSuperAdmin, btnAdmin };
            foreach (var button in navButtons)
            {
                ApplyDefaultButtonStyle(button);
            }

            tokenFilePath = Path.Combine(appFolder, "login.token");
            lastUserFilePath = Path.Combine(appFolder, "lastuser.txt");

            chkRememberMe = new CheckBox
            {
                Text = "Remember Me",
                AutoSize = true,
                Location = new Point(TbPassword.Left, TbPassword.Bottom + 5)
            };

            this.Controls.Add(chkRememberMe);
            this.Load += async (s, e) => { await LoginPage_LoadAsync(); };
        }

        private void ApplyDefaultButtonStyle(Button button)
        {
            if (button == null) return;

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

        private void ActivateButton(Button clickedButton)
        {
            foreach (var button in navButtons)
            {
                button.BackColor = inactiveBackColor;
                button.ForeColor = inactiveForeColor;
                button.FlatAppearance.BorderColor = inactiveForeColor;
            }

            if (clickedButton == btnSuperAdmin)
            {
                SetActiveButtonStyle(clickedButton, superAdminBackColor, superAdminTextColor);
                BtnLogin.Text = "Sign In as Superadmin";
                BtnLogin.BackColor = superAdminBackColor;
            }
            else if (clickedButton == btnAdmin)
            {
                SetActiveButtonStyle(clickedButton, adminBackColor, adminTextColor);
                BtnLogin.Text = "Sign In as Admin";
                BtnLogin.BackColor = adminBackColor;
            }

            BtnLogin.ForeColor = inactiveForeColor;
        }

        private void SetActiveButtonStyle(Button btn, Color back, Color fore)
        {
            btn.BackColor = back;
            btn.ForeColor = fore;
            btn.FlatAppearance.BorderColor = fore;
            btn.FlatAppearance.MouseDownBackColor = back;
            btn.FlatAppearance.MouseOverBackColor = back;
        }

        public static Image ResizeImage(Image originalImage, int newWidth, int newHeight)
        {
            if (originalImage == null) return null;

            Bitmap resized = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
            }

            return resized;
        }

        private void LoginPage_Load(object sender, EventArgs e)
        {
            TbPassword.UseSystemPasswordChar = true;

            const int ICON_SIZE = 32;
            btnSuperAdmin.Image = ResizeImage(Properties.Resources.shield, ICON_SIZE, ICON_SIZE);
            btnAdmin.Image = ResizeImage(Properties.Resources.setting, ICON_SIZE, ICON_SIZE);

            btnSuperAdmin.Padding = new Padding(40, 10, 40, 0);
            btnAdmin.Padding = new Padding(40, 10, 40, 0);

            btnSuperAdmin.ImageAlign = ContentAlignment.TopCenter;
            btnSuperAdmin.TextAlign = ContentAlignment.BottomCenter;

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

        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            TbPassword.UseSystemPasswordChar = !ShowPassword.Checked;
        }

        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            BtnLogin.Enabled = false;
            var oldText = BtnLogin.Text;
            Cursor.Current = Cursors.WaitCursor;
            BtnLogin.Text = "Signing in...";

            try
            {
                await PerformLoginAsync();
            }
            finally
            {
                BtnLogin.Enabled = true;
                BtnLogin.Text = oldText;
                Cursor.Current = Cursors.Default;
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
                catch { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new DashBoard("lee anthony", "SuperAdmin").Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Admin_DashBoardcs("jewel beduya", "Admin").Show();
            this.Hide();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Reset_Password reset = new Reset_Password();
            reset.Show();
            this.Hide();
        }

        // -------------------- NEW FEATURE METHODS -------------------- //

        private async Task LoginPage_LoadAsync()
        {
            try
            {
                if (File.Exists(lastUserFilePath))
                {
                    string lastUser = File.ReadAllText(lastUserFilePath).Trim();
                    if (!string.IsNullOrEmpty(lastUser))
                        TbUsername.Text = lastUser;
                }
            }
            catch { }

            try
            {
                if (File.Exists(tokenFilePath))
                {
                    var parts = File.ReadAllText(tokenFilePath).Split('|');
                    if (parts.Length == 3 && DateTime.TryParse(parts[1], out DateTime expiry) && expiry > DateTime.UtcNow)
                    {
                        string token = parts[0];
                        string user = parts[2];
                        bool success = await ValidateTokenAsync(user, token);
                        if (success) return;
                        else File.Delete(tokenFilePath);
                    }
                }
            }
            catch { }
        }

        private async Task<bool> ValidateTokenAsync(string username, string token)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = "SELECT login_token, token_expiration, role FROM Account WHERE username=@u";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);

                try
                {
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string dbToken = reader["login_token"].ToString();
                            DateTime dbExp = Convert.ToDateTime(reader["token_expiration"]);
                            string role = reader["role"].ToString();

                            if (dbToken == token && dbExp > DateTime.UtcNow)
                            {
                                await MarkAdminOnlineAsync(username);

                                this.Invoke((MethodInvoker)delegate
                                {
                                    if (role == "SuperAdmin")
                                        new DashBoard(username, role).Show();
                                    else
                                        new Admin_DashBoardcs(username, role).Show();

                                    this.Hide();
                                });

                                return true;
                            }
                        }
                    }
                }
                catch { }
            }

            return false;
        }

        private async Task PerformLoginAsync()
        {
            string username = TbUsername.Text.Trim();
            string password = TbPassword.Text.Trim();

            if (string.IsNullOrEmpty(selectedRole) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return;

            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                await conn.OpenAsync();

                string query = @"SELECT username, password_hash, role, failed_attempts, lock_until 
                         FROM Account 
                         WHERE username=@u AND role=@role";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@role", selectedRole);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (!await reader.ReadAsync())
                        {
                            MessageBox.Show("Invalid username or role.");
                            return;
                        }

                        string dbUser = reader["username"].ToString();
                        string dbHash = reader["password_hash"].ToString();
                        string role = reader["role"].ToString();
                        DateTime? lockUntil = reader["lock_until"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["lock_until"]);

                        reader.Close();

                        if (!BCrypt.Net.BCrypt.Verify(password, dbHash))
                        {
                            string updateQuery = @" UPDATE Account SET failed_attempts = CASE  WHEN ISNULL(failed_attempts, 0) < 3 THEN ISNULL(failed_attempts, 0) + 1 ELSE 3
                                                    END, lock_until = CASE WHEN ISNULL(failed_attempts, 0) + 1 >= 3 THEN DATEADD(MINUTE, 1, SYSDATETIME()) 
                                                    ELSE NULL END WHERE username = @u; SELECT failed_attempts, lock_until FROM Account WHERE username = @u;";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@u", dbUser);

                                using (var updateReader = await updateCmd.ExecuteReaderAsync())
                                {
                                    if (await updateReader.ReadAsync())
                                    {
                                        int currentAttempts = Convert.ToInt32(updateReader["failed_attempts"]);
                                        DateTime? updatedLock = updateReader["lock_until"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(updateReader["lock_until"]);

                                        string msg = $"Incorrect password. Attempt {currentAttempts} of 3.";
                                        if (currentAttempts >= 3 && updatedLock.HasValue)
                                            msg += " Account locked for 1 minute.";

                                        MessageBox.Show(msg);
                                    }
                                }
                            }
                            return;
                        }
                        
                        string resetQuery = @"UPDATE Account 
                                      SET failed_attempts = 0, lock_until = NULL 
                                      WHERE username=@u";
                        using (SqlCommand resetCmd = new SqlCommand(resetQuery, conn))
                        {
                            resetCmd.Parameters.AddWithValue("@u", dbUser);
                            await resetCmd.ExecuteNonQueryAsync();
                        }

                        SaveLastUser(dbUser);
                        if (chkRememberMe.Checked) await SaveRememberMeAsync(dbUser);
                        await MarkAdminOnlineAsync(dbUser);

                        this.Invoke((MethodInvoker)delegate
                        {
                            if (role == "SuperAdmin")
                                new DashBoard(dbUser, role).Show();
                            else
                                new Admin_DashBoardcs(dbUser, role).Show();
                            this.Hide();
                        });
                    }
                }
            }
        }


        private async Task IncreaseFailedAttemptsAsync(string username)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = @"
                UPDATE Account SET failed_attempts = failed_attempts + 1
                WHERE username=@u;
                IF (SELECT failed_attempts FROM Account WHERE username=@u) >= @max
                BEGIN
                    UPDATE Account SET lock_until = DATEADD(MINUTE, @mins, SYSDATETIME()) WHERE username=@u;
                END";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@max", MAX_FAILED_ATTEMPTS);
                cmd.Parameters.AddWithValue("@mins", LOCKOUT_DURATION.TotalMinutes);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private async Task ResetFailedAttemptsAsync(string username)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = "UPDATE Account SET failed_attempts=0, lock_until=NULL WHERE username=@u";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        private void SaveLastUser(string username)
        {
            try
            {
                Directory.CreateDirectory(appFolder);
                File.WriteAllText(lastUserFilePath, username);
            }
            catch { }
        }

        private async Task SaveRememberMeAsync(string username)
        {
            string token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
            DateTime expiry = DateTime.UtcNow.Add(REMEMBER_ME_DURATION);

            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = "UPDATE Account SET login_token=@t, token_expiration=@e WHERE username=@u";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@t", token);
                cmd.Parameters.AddWithValue("@e", expiry);
                cmd.Parameters.AddWithValue("@u", username);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            try
            {
                Directory.CreateDirectory(appFolder);
                File.WriteAllText(tokenFilePath, $"{token}|{expiry:o}|{username}");
            }
            catch { }
        }

        private async Task MarkAdminOnlineAsync(string username)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                string query = "UPDATE Account SET active=1, last_login=SYSDATETIME() WHERE username=@u";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}