using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Common;


using System.Windows.Forms;

namespace DB_Backup
{
    class Program
    {
        private string dbName = string.Empty;
        private static SqlConnection sqlConn;
        private static Server sqlServer;
        private static List<Database> dbList;

        static void Main(string[] args)
        {
            try
            {
                sqlConn = new SqlConnection("Data Source=POTEAU-PC\\SQLSERVER_2016;Initial Catalog=Master;User ID=lpqc;Password=jesusestsauveur");
                sqlServer = new Server(new ServerConnection(sqlConn));

                //dbList = new List<Database>();
                //foreach (Database db in sqlServer.Databases)
                //{
                //    dbList.Add(db);
                //}
                string filename = DateTime.Now.ToString("mmMMMyy");
                BackupDb("UEspoirDB", "c:\\extras\\tech\\SqlServer_Auto_Backup\\UEspoir"+ filename+".bk");
            }
            catch (Exception exc)
            {
                MessageBox.Show(string.Format("Exception occured.\nMessage: {0}", exc.Message));
            }

        }

        private static void BackupDb(string dbName, string fileName)
        {
            Backup dbBackup = new Backup();

            try
            {
                dbBackup.Action = BackupActionType.Database;
                dbBackup.Database = dbName;
                dbBackup.BackupSetName = string.Format("{0} backup set.", dbName);
                dbBackup.BackupSetDescription = string.Format("Database: {0}. Date: {1}.",
                    dbName, DateTime.Now.ToString("dd.MM.yyyy hh:m"));
                dbBackup.MediaDescription = "Disk";

                BackupDeviceItem device = new BackupDeviceItem(fileName, DeviceType.File);
                dbBackup.Devices.Add(device);

                dbBackup.SqlBackup(sqlServer);
            }
            catch (Exception exc)
            {
                dbBackup.Abort();
                MessageBox.Show(string.Format("Exception occurred.\nMessage: {0}", exc.Message));
            }
            finally
            {
                sqlConn.Close();
            }
        }
    }
}
