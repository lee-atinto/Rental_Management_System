using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DashBoard1.SuperAdmin_AdminAccount;
using WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp;
using WindowsFormsApp1.DashBoard1.SuperAdmin_PaymentRecords;
using WindowsFormsApp1.DashBoard1.SuperAdmin_Properties;
using WindowsFormsApp1.Login_ResetPassword;
using WindowsFormsApp1.Main_Form_Dashboards;
using WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Contract;
using WindowsFormsApp1.Super_Admin_Account;


namespace WindowsFormsApp1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new LoginPage());

            //Application.Run(new ProperTies("TestUser", " Super Admin"));

            // --------------- Superadmin Login Tester --------------- //
            Application.Run(new BackUp("TestUser", "Super A dmin"));
            //Application.Run(new Tenants("TestUser", " Super A dmin"));
            //Application.Run(new DashBoard("TestUser", "Super Admin"));
            //Application.Run(new ProperTies("TestUser", "Super Admin"));
            //Application.Run(new Contracts("TestUser", " Super A dmin"));
            //Application.Run(new Maintenance("TestUser", "Super Admin"));
            //Application.Run(new View_Reports("TestUser", "Super A dmin"));
            //Application.Run(new Payment_Records("TestUser", "Super Admin"));
            //Application.Run(new SuperAdmin_AdminAccounts("TestUser", "Super Admin"));
        }
    }
}
