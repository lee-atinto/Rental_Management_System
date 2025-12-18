using System;
using System.Windows.Forms;
using WindowsFormsApp1.Helpers;

namespace WindowsFormsApp1
{
    public class BaseForm : Form
    {
        public BaseForm()
        {
            this.Load += BaseForm_Load;
        }

        private void BaseForm_Load(object sender, EventArgs e)
        {
            SubscribeToCrashMonitor();
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
    }
}
