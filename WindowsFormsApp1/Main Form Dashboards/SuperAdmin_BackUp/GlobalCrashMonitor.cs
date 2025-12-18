using System;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Helpers
{
    public class GlobalCrashMonitor
    {
        private static readonly Lazy<GlobalCrashMonitor> lazy =
            new Lazy<GlobalCrashMonitor>(() => new GlobalCrashMonitor());

        public static GlobalCrashMonitor Instance => lazy.Value;

        private string connectionString;
        private string[] criticalTables;
        private string backupFolder = @"C:\RentalSystem_Backups";
        private bool recoveryInProgress = false;

        public event Action<string> OnCriticalDataMissing;

        private GlobalCrashMonitor() { }

        public void Initialize(string connString, string[] tables)
        {
            connectionString = connString;
            criticalTables = tables;

            Task.Run(async () => await CheckAndRecoverAsync());
        }

        public async Task CheckAndRecoverAsync()
        {
            if (recoveryInProgress) return;
            recoveryInProgress = true;

            try
            {
                var missingTables = await GetMissingTablesAsync();

                if (missingTables.Any())
                {
                    // Attempt automatic recovery
                    string backupZip = GetLatestBackupZip();
                    if (backupZip != null)
                    {
                        bool restored = await RestoreDatabaseFromZipAsync(backupZip);
                        if (restored)
                        {
                            MessageBox.Show(
                                "System detected missing tables:\n" + string.Join("\n", missingTables) +
                                "\n\nAutomatic recovery successful. Please restart the application.",
                                "Recovery Success",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                        else
                        {
                            MessageBox.Show(
                                "System detected missing tables:\n" + string.Join("\n", missingTables) +
                                "\n\nAutomatic recovery failed. Please restore manually.",
                                "Recovery Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "System detected missing tables:\n" + string.Join("\n", missingTables) +
                            "\n\nNo backup found. Please restore manually.",
                            "Recovery Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }

                    OnCriticalDataMissing?.Invoke(string.Join(", ", missingTables));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Crash monitor error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                recoveryInProgress = false;
            }
        }

        private async Task<string[]> GetMissingTablesAsync()
        {
            var missing = new System.Collections.Generic.List<string>();
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
                        if (exists == 0) missing.Add(table);
                    }
                }
            }
            return missing.ToArray();
        }

        private string GetLatestBackupZip()
        {
            if (!Directory.Exists(backupFolder)) return null;
            var zips = Directory.GetFiles(backupFolder, "*.zip");
            if (zips.Length == 0) return null;
            Array.Sort(zips);
            return zips[zips.Length - 1];
        }

        private async Task<bool> RestoreDatabaseFromZipAsync(string zipFile)
        {
            try
            {
                string tempFolder = Path.Combine(Path.GetTempPath(), "RecoveryTemp");
                if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, true);
                Directory.CreateDirectory(tempFolder);

                ZipFile.ExtractToDirectory(zipFile, tempFolder);
                string bak = Directory.GetFiles(tempFolder, "*.bak", SearchOption.AllDirectories).FirstOrDefault();
                if (bak == null) return false;

                SqlConnectionStringBuilder scb = new SqlConnectionStringBuilder(connectionString);
                string dbName = scb.InitialCatalog;
                scb.InitialCatalog = "master";
                using (SqlConnection con = new SqlConnection(scb.ConnectionString))
                {
                    await con.OpenAsync();
                    string sql = $@"
                        ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        RESTORE DATABASE [{dbName}] FROM DISK=@path WITH REPLACE;
                        ALTER DATABASE [{dbName}] SET MULTI_USER;";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@path", bak);
                        cmd.CommandTimeout = 0;
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                Directory.Delete(tempFolder, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
