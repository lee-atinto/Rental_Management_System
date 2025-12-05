using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rental_Management_System
{
    internal class SessionManager
    {
        public static string LoggedInUsername { get; private set; }
        public static string LoggedInUserRole { get; private set; }

        public static void StartSession(string username, string role)
        {
            LoggedInUsername = username;
            LoggedInUserRole = role;
        }

        public static void EndSession()
        {
            LoggedInUsername = null;
            LoggedInUserRole = null;
        }
    }
}