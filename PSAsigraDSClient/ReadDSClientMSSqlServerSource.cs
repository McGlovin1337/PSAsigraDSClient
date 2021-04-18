using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommunications.Read, "DSClientMSSqlServerSource")]
    [OutputType(typeof(SourceMSSqlItemInfo))]

    public class ReadDSClientMSSqlServerSource: BaseDSClientBackupSource
    {
        [Parameter(HelpMessage = "Set Database Credentials")]
        [ValidateNotNullOrEmpty]
        public PSCredential DbCredential { get; set; }

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
            Win32FS_Generic_BackupSetCredentials computerCredentials = Win32FS_Generic_BackupSetCredentials.from(dataSourceBrowser.neededCredentials(computer));

            if (Credential != null)
            {
                string user = Credential.UserName;
                string pass = Credential.GetNetworkCredential().Password;
                computerCredentials.setCredentials(user, pass);
            }
            else
            {
                WriteVerbose("Notice: Credentials not specified, using DS-Client Credentials");
                computerCredentials.setUsingClientCredentials(true);
            }
            dataSourceBrowser.setCurrentCredentials(computerCredentials);

            // Extend the Data Source Browser
            SQLDataBrowserWithSetCreation sqlDataSourceBrowser = SQLDataBrowserWithSetCreation.from(dataSourceBrowser);

            // Set the Database Credentials if specified, otherwise use the Computer credentials
            Win32FS_Generic_BackupSetCredentials dbCredentials = new Win32FS_Generic_BackupSetCredentials();
            if (DbCredential != null)
            {
                string dbUser = DbCredential.UserName;
                string dbPass = DbCredential.GetNetworkCredential().Password;

                dbCredentials.setCredentials(dbUser, dbPass);
                sqlDataSourceBrowser.setDBCredentials(dbCredentials);
            }
            else
                sqlDataSourceBrowser.setDBCredentials(computerCredentials);

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

            dbCredentials.Dispose();
            computerCredentials.Dispose();
            sqlDataSourceBrowser.Dispose();
        }
    }
}