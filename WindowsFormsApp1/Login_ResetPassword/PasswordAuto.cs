using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Login_ResetPassword
{
    internal class PasswordAuto
    {
        public static string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public static bool VerifyPassword(string plainPassword, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hash);
        }

        public static string ValidatePassword(string password)
        {
            if (password.Length < 8)
                return "Password must be at least 8 characters long.";

            var failures = new List<string>();

            if (!Regex.IsMatch(password, @"[A-Z]")) failures.Add("one uppercase letter");
            if (!Regex.IsMatch(password, @"[a-z]")) failures.Add("one lowercase letter");
            if (!Regex.IsMatch(password, @"\d")) failures.Add("one number");
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]")) failures.Add("one special character");

            if (failures.Any())
                return "Password must contain: " + string.Join(", ", failures) + ".";

            return string.Empty; // valid
        }
    }
}