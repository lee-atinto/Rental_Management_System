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
    public partial class Admin_DashBoardcs : Form
    {
        private readonly string Username;
        private readonly string UserRole;

        //  -------------------- Button Style -------------------- //
        private readonly Color activeColor = Color.FromArgb(46, 51, 73);
        private readonly Color defaultBackColor = Color.FromArgb(240, 240, 240);

        // -------------------- Initialize Button Style -------------------- //
        private void InitializeButtonStyle(Button button)
        {
            if (button != null)
            {
                button.TabStop = false;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = Color.FromArgb(46, 51, 73);
                button.Padding = new Padding(50, 5, 5, 5);

                button.BackColor = defaultBackColor;
                button.FlatAppearance.MouseDownBackColor = defaultBackColor;
                button.FlatAppearance.MouseOverBackColor = defaultBackColor;

                button.TextAlign = ContentAlignment.MiddleLeft;
                button.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        // -------------------- Set Active Button Style -------------------- //
        private void SetButtonActiveStyle(Button button, Color backColor)
        {
            if (button != null)
            {
                button.BackColor = backColor;
                button.FlatAppearance.MouseDownBackColor = backColor;
                button.FlatAppearance.MouseOverBackColor = backColor;
            }
        }

        public Admin_DashBoardcs(string username, string role)
        {
            InitializeComponent();

            // -------------------- Set User Information -------------------- //
            Username = username;
            UserRole = role;
            this.lbName.Text = $"{Username} \n ({UserRole})";

            // -------------------- Initialize Button Style -------------------- //
            InitializeButtonStyle(btnDashBoard);
            InitializeButtonStyle(btnAdminAcc);
            InitializeButtonStyle(btnTenant);
            InitializeButtonStyle(btnPaymentRec);
            InitializeButtonStyle(btnViewReport);

            panelHeader.BackColor = Color.FromArgb(46, 51, 73);
            PanelBackGroundProfile.BackColor = Color.FromArgb(46, 51, 73);

            // -------------------- Set Padding Button -------------------- //
            btnDashBoard.Padding = new Padding(30, 0, 0, 0);
            btnAdminAcc.Padding = new Padding(30, 0, 0, 0);
            btnTenant.Padding = new Padding(30, 0, 0, 0);
            btnPaymentRec.Padding = new Padding(30, 0, 0, 0);
            btnViewReport.Padding = new Padding(30, 0, 0, 0);

            // -------------------- Set Color Unactive Button -------------------- //
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel5.BorderStyle = BorderStyle.FixedSingle;
            panel6.BorderStyle = BorderStyle.FixedSingle;
            panel7.BorderStyle = BorderStyle.FixedSingle;
            panel8.BorderStyle = BorderStyle.FixedSingle;

            // -------------------- Set Borderline color -------------------- //
            panel1.BackColor = Color.FromArgb(240, 240, 240);
            panel2.BackColor = Color.FromArgb(240, 240, 240);
            panel3.BackColor = Color.FromArgb(240, 240, 240);
            panel4.BackColor = Color.FromArgb(240, 240, 240);
            panel5.BackColor = Color.FromArgb(240, 240, 240);
            panel6.BackColor = Color.FromArgb(240, 240, 240);
            panel7.BackColor = Color.FromArgb(240, 240, 240);
            panel8.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void Admin_DashBoardcs_Load(object sender, EventArgs e)
        {
            SetButtonActiveStyle(btnDashBoard, activeColor);
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }
    }
}
