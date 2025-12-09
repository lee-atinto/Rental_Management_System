using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.DashBoard1.SuperAdmin_BackUp
{
    public partial class BackUp : Form
    {
        private readonly string DataConnection = @"Data Source=LEEANTHONYDATIN\SQLEXPRESS; Initial Catalog=RentalManagementSystem;Integrated Security=True";
        
        public BackUp()
        {
            InitializeComponent();
        }

        private void BackUp_Load(object sender, EventArgs e)
        {

        }

        private void btnOverView_Click(object sender, EventArgs e)
        {
            plSystemOverView.Visible = true;

            plDataBase.Visible = false;
            plSystemUsers.Visible = false;
            plBackUpRec.Visible = false;
        }

        private void btnDataBase_Click(object sender, EventArgs e)
        {
            LoadData();
            plDataBase.Visible = true;

            plSystemOverView.Visible = false;
            plSystemUsers.Visible = false;
            plBackUpRec.Visible = false;
        }

        private void btnSystemUsers_Click(object sender, EventArgs e)
        {
            plSystemUsers.Visible = true;

            plSystemOverView.Visible = false;
            plDataBase.Visible = false;
            plBackUpRec.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            plBackUpRec.Visible = true;

            plSystemUsers.Visible = false;
            plSystemOverView.Visible = false;
            plDataBase.Visible = false;
        }

        private void LoadData()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT t.Name AS TableName,
                     p.rows AS RowCounts, 
                     MAX(ISNULL(ius.last_user_update, ius.last_user_seek)) AS LastActivityTime, 
                     CASE 
                         WHEN SUM(a.total_pages) * 8.0 >= 1048576 
                             THEN CAST(ROUND(((SUM(a.total_pages) * 8.0) / 1048576.00), 2) AS VARCHAR) + ' GB'
                         WHEN SUM(a.total_pages) * 8.0 >= 1024 
                             THEN CAST(ROUND(((SUM(a.total_pages) * 8.0) / 1024.00), 2) AS VARCHAR) + ' MB'
                         ELSE CAST(ROUND((SUM(a.total_pages) * 8.0), 2) AS VARCHAR) + ' KB'
                     END AS TotalReservedSpace
                 FROM sys.tables t 
                 INNER JOIN sys.indexes i ON t.OBJECT_ID = i.object_id
                 INNER JOIN sys.partitions p ON i.object_id = p.OBJECT_ID AND i.index_id = p.index_id
                 INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
                 LEFT JOIN sys.dm_db_index_usage_stats ius ON t.OBJECT_ID = ius.object_id AND i.index_id = ius.index_id
                 WHERE t.is_ms_shipped = 0 
                   AND i.OBJECT_ID > 255 
                   AND i.index_id IN (0, 1)
                 GROUP BY t.Name, p.Rows
                 ORDER BY (SUM(a.total_pages) * 8.0) DESC;";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dt);
                    }
                }

                DataTables.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading database data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataTables_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void OptimizeDatabase()
        {
            string optimizeQuery = @" EXEC sp_MSforeachtable @command1='ALTER INDEX ALL ON ? REORGANIZE;'; EXEC sp_updatestats;";

            try
            {
                using (SqlConnection connection = new SqlConnection(DataConnection))
                {
                    using (SqlCommand command = new SqlCommand(optimizeQuery, connection))
                    {
                        connection.Open();
                        command.CommandTimeout = 300;
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Database optimization complete! Reorganized indexes and updated statistics.", "Optimization Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during database optimization: " + ex.Message, "Optimization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOptimize_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show( "Optimization can take a few minutes and temporarily increase CPU usage. Do you want to continue?", "Confirm Optimization", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                OptimizeDatabase();
            }
        }
    }
}
