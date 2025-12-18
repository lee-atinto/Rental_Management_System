using System;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Helpers
{
    public class CrashRecoveryManager
    {
        private readonly string connectionString;
        private readonly string backupFolder;
        private readonly string[] criticalTables;

        /// <summary>
        /// Event triggered when a critical table is missing.
        /// </summary>
        public event Action<string> OnCriticalTableMissing;

        /// <summary>
        /// Event triggered when recovery succeeds.
        /// </summary>
        public event Action<string> OnRecoverySuccess;

        /// <summary>
        /// Event triggered when recovery fails.
        /// </summary>
        public event Action<string> OnRecoveryFailed;

        public CrashRecoveryManager(string connectionString, string backupFolder, string[] criticalTables)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            this.backupFolder = backupFolder ?? throw new ArgumentNullException(nameof(backupFolder));
            this.criticalTables = criticalTables ?? throw new ArgumentNullException(nameof(criticalTables));
        }

        /// <summary>
        /// Checks all critical tables and triggers recovery if needed.
        /// </summary>
        public async Task CheckAndRecoverCriticalTablesAsync()
        {
            bool anyTableMissing = false;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();

                    foreach (string table in criticalTables)
                    {
                        string query = "IF OBJECT_ID(@table, 'U') IS NULL SELECT 0 ELSE SELECT 1";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@table", table);
                            int exists = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                            if (exists == 0)
                            {
                                anyTableMissing = true;
                                OnCriticalTableMissing?.Invoke(table);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                anyTableMissing = true;
                OnRecoveryFailed?.Invoke($"Database error detected: {ex.Message}");
            }
            catch (Exception ex)
            {
                anyTableMissing = true;
                OnRecoveryFailed?.Invoke($"Unexpected error detected: {ex.Message}");
            }

            if (anyTableMissing)
            {
                await RecoverSystemAsync();
            }
        }

        /// <summary>
        /// Performs automatic recovery using the latest backup ZIP.
        /// </summary>
        public async Task RecoverSystemAsync()
        {
            string marker = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recovery_in_progress.flag");
            if (File.Exists(marker))
            {
                OnRecoveryFailed?.Invoke("Automatic recovery already attempted. Check backups manually.");
                return;
            }

            try
            {
                File.WriteAllText(marker, "1");

                if (!Directory.Exists(backupFolder))
                {
                    OnRecoveryFailed?.Invoke("Backup folder not found. Recovery failed.");
                    return;
                }

                string latestZip = GetLatestBackup();
                if (latestZip == null)
                {
                    OnRecoveryFailed?.Invoke("No backup ZIP found. Recovery failed.");
                    return;
                }

                string tempPath = Path.Combine(Path.GetTempPath(), "RecoveryTemp");
                if (Directory.Exists(tempPath)) Directory.Delete(tempPath, true);
                Directory.CreateDirectory(tempPath);

                // Extract backup
                ZipFile.ExtractToDirectory(latestZip, tempPath);

                // Find .bak file
                string bakFile = Directory.GetFiles(tempPath, "*.bak", SearchOption.AllDirectories).FirstOrDefault();
                if (bakFile == null)
                {
                    OnRecoveryFailed?.Invoke("No .bak file found inside backup ZIP.");
                    return;
                }

                await RestoreDatabaseAsync(bakFile);

                OnRecoverySuccess?.Invoke("System restored successfully!");
            }
            catch (Exception ex)
            {
                OnRecoveryFailed?.Invoke($"Recovery failed: {ex.Message}");
            }
            finally
            {
                if (File.Exists(marker)) File.Delete(marker);
                Application.Restart();
            }
        }

        /// <summary>
        /// Returns the latest ZIP backup file.
        /// </summary>
        private string GetLatestBackup()
        {
            var files = Directory.GetFiles(backupFolder, "*.zip");
            return files.Length == 0 ? null : files.OrderBy(f => f).Last();
        }

        /// <summary>
        /// Restores the database from a .bak file safely.
        /// </summary>
        private async Task RestoreDatabaseAsync(string bakFile)
        {
            SqlConnectionStringBuilder scb = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = "master"
            };

            using (SqlConnection con = new SqlConnection(scb.ConnectionString))
            {
                await con.OpenAsync();

                string dbName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
                string sql = $@"
                    ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE [{dbName}] FROM DISK = @bak WITH REPLACE;
                    ALTER DATABASE [{dbName}] SET MULTI_USER;";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@bak", bakFile);
                    cmd.CommandTimeout = 0;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
