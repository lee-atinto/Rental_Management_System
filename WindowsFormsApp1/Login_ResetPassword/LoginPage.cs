using BCrypt.Net;
using Rental_Management_System;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp1.Helpers;
using WindowsFormsApp1.Super_Admin_Account;

namespace WindowsFormsApp1.Login_ResetPassword
{
    public partial class LoginPage : BaseForm
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

            this.Load += LoginPage_Load;
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
            SubscribeToCrashMonitor();

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

            // Load last user
            if (File.Exists(lastUserFilePath))
            {
                string lastUser = File.ReadAllText(lastUserFilePath).Trim();
                if (!string.IsNullOrEmpty(lastUser))
                    TbUsername.Text = lastUser;
            }
        }

        private void SubscribeToCrashMonitor()
        {
            GlobalCrashMonitor.Instance.OnCriticalDataMissing += ShowCriticalAlert;
        }

        private void ShowCriticalAlert(string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ShowCriticalAlert(message)));
                return;
            }

            MessageBox.Show(
                $"System Alert: {message}",
                "Critical Data Missing / Crash Detected",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
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

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            BtnLogin.Enabled = false;
            var oldText = BtnLogin.Text;
            Cursor.Current = Cursors.WaitCursor;
            BtnLogin.Text = "Signing in...";

            // Run login in background to avoid freezing UI
            System.Threading.ThreadPool.QueueUserWorkItem(_ =>
            {
                bool success = PerformLogin();
                this.Invoke((MethodInvoker)(() =>
                {
                    BtnLogin.Enabled = true;
                    BtnLogin.Text = oldText;
                    Cursor.Current = Cursors.Default;

                    if (!success)
                        MessageBox.Show("Invalid username, password, or role.");
                }));
            });
        }

        private bool PerformLogin()
        {
            try
            {
                string username = TbUsername.Text.Trim();
                string password = TbPassword.Text.Trim();

                if (string.IsNullOrEmpty(selectedRole) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                    return false;

                using (SqlConnection conn = new SqlConnection(DataConnection))
                {
                    conn.Open();

                    string query = @"SELECT username, password_hash, role 
                                     FROM Account 
                                     WHERE username=@u AND role=@role";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        cmd.Parameters.AddWithValue("@role", selectedRole);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return false;

                            string dbUser = reader["username"].ToString();
                            string dbHash = reader["password_hash"].ToString();
                            string role = reader["role"].ToString();
                            reader.Close();

                            if (!BCrypt.Net.BCrypt.Verify(password, dbHash))
                                return false;

                            // Successful login
                            ResetFailedAttempts(dbUser);
                            SaveLastUser(dbUser);
                            if (chkRememberMe.Checked) SaveRememberMe(dbUser);
                            MarkUserOnline(dbUser);

                            this.Invoke((MethodInvoker)(() =>
                            {
                                new DashBoard(dbUser, role).Show();
                                this.Hide();
                            }));

                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void ResetFailedAttempts(string username)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                conn.Open();
                string query = "UPDATE Account SET failed_attempts=0, lock_until=NULL WHERE username=@u";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.ExecuteNonQuery();
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

        private void SaveRememberMe(string username)
        {
            try
            {
                string token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
                DateTime expiry = DateTime.UtcNow.AddDays(30);

                using (SqlConnection conn = new SqlConnection(DataConnection))
                {
                    conn.Open();
                    string query = "UPDATE Account SET login_token=@t, token_expiration=@e WHERE username=@u";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@t", token);
                    cmd.Parameters.AddWithValue("@e", expiry);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.ExecuteNonQuery();
                }

                Directory.CreateDirectory(appFolder);
                File.WriteAllText(tokenFilePath, $"{token}|{expiry:o}|{username}");
            }
            catch { }
        }

        private void MarkUserOnline(string username)
        {
            using (SqlConnection conn = new SqlConnection(DataConnection))
            {
                conn.Open();
                string query = "UPDATE Account SET active=1, last_login=SYSDATETIME() WHERE username=@u";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.ExecuteNonQuery();
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Reset_Password reset = new Reset_Password();
            reset.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string demoUsername = "DEMO_SUPER";
            string demoRole = "SuperAdmin";
            DashBoard dashBoard = new DashBoard(demoUsername, demoRole);
            dashBoard.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string demoUsername = "DEMO_ADMIN";
            string demoRole = "Admin";
            DashBoard dashBoard = new DashBoard(demoUsername, demoRole);
            dashBoard.Show();
            this.Hide();
        }
    }
}
