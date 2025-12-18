using System;
using System.Configuration;
using System.Windows.Forms;
using WindowsFormsApp1.Helpers;
using WindowsFormsApp1.Login_ResetPassword;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string[] criticalTables = {
                "Account", "Address", "Contract", "LoginLogs",
                "MaintenanceRequest", "PasswordHistory", "Payment",
                "PaymentMethod", "PaymentType", "PersonalInformation",
                "Property", "PropertyOwner", "Rent", "RequestType",
                "Requirements", "Tenant", "Unit"
            };

            GlobalCrashMonitor.Instance.Initialize(
                ConfigurationManager.ConnectionStrings["DB"].ConnectionString,
                criticalTables,
                checkIntervalMs: 30000
            );

            Application.Run(new LoginPage());
        }
    }
}
