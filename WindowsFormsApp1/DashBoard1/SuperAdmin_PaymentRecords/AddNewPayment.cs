using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Super_Admin_Account
{
    public partial class AddNewPayment : Form
    {
        private int tenantID;

        public AddNewPayment(int TenantID)
        {
            InitializeComponent();

            TenantID = tenantID;
        }

        private void AddNewPayment_Load(object sender, EventArgs e)
        {
            tbReference.Enabled = false;
        }

        private void cbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMethod.SelectedItem != null)
            {
                string selectedMethod = cbMethod.SelectedItem.ToString();

                if (selectedMethod.Equals("Cash", StringComparison.OrdinalIgnoreCase))
                {
                    tbReference.Enabled = false;
                    tbReference.Text = string.Empty;
                }
                else
                {
                    tbReference.Enabled = true;
                }
            }
            else
            {
                tbReference.Enabled = false;
            }
        }
    }
}
