using BCrypt.Net;
using Rental_Management_System;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Super_Admin_Account;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
                ApplyDefaultButtonStyle(button);

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

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Reset_Password reset = new Reset_Password();
            reset.Show();
            this.Hide();
        }

        // -------------------- Async Helpers -------------------- //

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
                                await MarkUserOnlineAsync(username);

                                this.Invoke((MethodInvoker)delegate
                                {
                                    new DashBoard(username, role).Show();
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
                            MessageBox.Show("Incorrect password.");
                            return;
                        }

                        await ResetFailedAttemptsAsync(dbUser);
                        SaveLastUser(dbUser);
                        if (chkRememberMe.Checked) await SaveRememberMeAsync(dbUser);
                        await MarkUserOnlineAsync(dbUser);

                        this.Invoke((MethodInvoker)delegate
                        {
                            new DashBoard(dbUser, role).Show();
                            this.Hide();
                        });
                    }
                }
            }
        }


        private async void button1_Click(object sender, EventArgs e)
        {
            string demoUsername = "DEMO_SUPER";
            string demoRole = "SuperAdmin";
            DashBoard dashBoard = new DashBoard(demoUsername, demoRole);
            dashBoard.Show();
            this.Hide();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string demoUsername = "DEMO_ADMIN";
            string demoRole = "Admin";
            DashBoard dashBoard = new DashBoard(demoUsername, demoRole);
            dashBoard.Show();
            this.Hide();
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

        private async Task MarkUserOnlineAsync(string username)
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
