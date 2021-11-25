using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommunications.Read, "DSClientMSSqlServerSource")]
    [OutputType(typeof(SourceMSSqlItemInfo))]

    sealed public class ReadDSClientMSSqlServerSource: BaseDSClientBackupSource
    {
        [Parameter(HelpMessage = "Set Database Credentials")]
        [ValidateNotNullOrEmpty]
        public DSClientCredential DbCredential { get; set; }

        [Parameter(HelpMessage = "Database Instance to Query")]
        [ValidateNotNullOrEmpty]
        public string Instance { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Create Data Source Browser to Resolve Computer (expandToFullPath method doesn't work when data browser is DataType is SQLServer)
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Resolve the supplied Computer Name
            string computer = dataSourceBrowser.expandToFullPath(Computer);
            computer = dataSourceBrowser.expandToFullPath(computer);
            string sqlComputer = computer.Split('\\').Last();
            WriteVerbose($"Notice: Specified Computer resolved to: {computer}");
            WriteVerbose($"Notice: SQL Server Computer resolved to: {sqlComputer}");

            // Change Data Source Browser to correct type
            dataSourceBrowser.Dispose();
            dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__SQLServer);

            // Set the Computer Credentials
            if (Credential != null)
            {
                dataSourceBrowser.setCurrentCredentials(Credential.GetCredentials());
            }
            else
            {
                Win32FS_Generic_BackupSetCredentials backupSetCredentials = Win32FS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(computer));
                backupSetCredentials.setUsingClientCredentials(true);
                dataSourceBrowser.setCurrentCredentials(backupSetCredentials);
            }

            // Extend the Data Source Browser
            SQLDataBrowserWithSetCreation sqlDataSourceBrowser = SQLDataBrowserWithSetCreation.from(dataSourceBrowser);

            // Set the Database Credentials if specified, otherwise use the Computer credentials
            if (DbCredential != null)
            {
                sqlDataSourceBrowser.setDBCredentials(Win32FS_Generic_BackupSetCredentials.from(DbCredential.GetCredentials()));
            }
            else
            {
                sqlDataSourceBrowser.setDBCredentials(Win32FS_Generic_BackupSetCredentials.from(Credential.GetCredentials()));
            }

            // Get the Databases Info
            List<mssql_db_path> dbPaths = new List<mssql_db_path>();
            if (Instance != null)
                dbPaths = sqlDataSourceBrowser.getDatabasesPath(sqlComputer, Instance).ToList();
            else
            {
                // Get the Instances Info
                WriteVerbose("Performing Action: Retrieve SQL Instance Info from Source Computer");
                mssql_instance_info[] dbInstances = sqlDataSourceBrowser.getInstancesInfo(sqlComputer);

                foreach (mssql_instance_info instance in dbInstances)
                    dbPaths.AddRange(sqlDataSourceBrowser.getDatabasesPath(sqlComputer, instance.name).ToList());
            }

            List<SourceMSSqlItemInfo> sqlItems = new List<SourceMSSqlItemInfo>();
            foreach (mssql_db_path database in dbPaths)
                sqlItems.Add(new SourceMSSqlItemInfo(database));

            sqlItems.ForEach(WriteObject);

            sqlDataSourceBrowser.Dispose();
        }
    }
}