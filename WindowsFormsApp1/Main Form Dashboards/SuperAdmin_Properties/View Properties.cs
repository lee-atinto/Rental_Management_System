using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Main_Form_Dashboards.SuperAdmin_Properties
{
    public partial class View_Properties : Form
    {
        public View_Properties(string firstName, string middleName, string lastName, string mobile, string email,
                               string address, string propertyName, string unitNum, string unitType,
                               string status, string amount)
        {
            InitializeComponent();

            string fullName = string.IsNullOrWhiteSpace(middleName)
                ? $"{firstName} {lastName}"
                : $"{firstName} {middleName} {lastName}";

            label1.Text = fullName;
            label2.Text = mobile;
            label3.Text = string.IsNullOrWhiteSpace(email) ? "None" : email;
            label4.Text = address;

            label5.Text = propertyName;
            label6.Text = unitNum;
            label7.Text = unitType;
            label8.Text = status;
            label9.Text = amount;
        }

        private void View_Properties_Load(object sender, EventArgs e)
        {
        }
    }
}