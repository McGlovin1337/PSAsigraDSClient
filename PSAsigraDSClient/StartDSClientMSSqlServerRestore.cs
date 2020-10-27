using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientMSSqlServerRestore")]

    public class StartDSClientMSSqlServerRestore: BaseDSClientBackupSetRestore
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the items to validate")]
        [ValidateNotNullOrEmpty]
        [Alias("Path")]
        public new string[] Items { get; set; }

        [Parameter(HelpMessage = "Database Credentials")]
        public PSCredential DbCredential { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "InstanceRestore", HelpMessage = "Specify the Database Instance")]
        public string Instance { get; set; }

        [Parameter(HelpMessage = "Map Source and Destination Databases in format 'sourceDb,destinationDb,dataPath,logPath'")]
        public string[] MapDatabase { get; set; }

        [Parameter(HelpMessage = "Specify the Database Dump Method")]
        [ValidateSet("DumpLocal", "DumpBuffer")]
        public string DumpMethod { get; set; } = "DumpBuffer";

        [Parameter(HelpMessage = "Specify the Dump Path")]
        [ValidateNotNullOrEmpty]
        public string DumpPath { get; set; }

        [Parameter(HelpMessage = "Leave Database(s) in Restoring Mode")]
        public SwitchParameter LeaveDatabaseRestoring { get; set; }

        [Parameter(HelpMessage = "Preserve Database(s) File Location")]
        public SwitchParameter PreserveFileLocation { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "DumpRestore", HelpMessage = "Only Restore the Database Dump File")]
        public SwitchParameter RestoreDumpOnly { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Parameter Validation
            if (RestoreDumpOnly && DumpPath == null)
                throw new ParameterBindingException("DumpPath must be specified to Restore Dump only");

            // Check for a Backup Set Restore View stored in SessionState
            WriteVerbose("Checking for DS-Client Restore View Session...");
            BackupSetRestoreView restoreSession = SessionState.PSVariable.GetValue("RestoreView", null) as BackupSetRestoreView;

            if (restoreSession == null)
                throw new Exception("There is no Backup Set Restore View Session, use Initialize-DSClientBackupSetRestore Cmdlet to create a Restore Session");

            // Check for Data Type for the Restore stored in SessionState
            EBackupDataType dataType = (EBackupDataType)SessionState.PSVariable.GetValue("RestoreType", EBackupDataType.EBackupDataType__UNDEFINED);

            if (dataType != EBackupDataType.EBackupDataType__SQLServer)
                throw new Exception("Invalid Data Type for MS SQL Server Restore");

            // Get the selected items
            List<long> selectedItems = new List<long>();

            if (Items != null)
            {
                foreach (string item in Items)
                {
                    try
                    {
                        selectedItems.Add(restoreSession.getItem(item).id);
                    }
                    catch
                    {
                        WriteWarning("Unable to select item: " + item);
                        continue;
                    }
                }
            }

            // Prepare restore
            MSSQL_RestoreActivityInitiator sqlRestoreActivityInitiator = MSSQL_RestoreActivityInitiator.from(restoreSession.prepareRestore(selectedItems.ToArray()));

            // Create Data Source Browser to Resolve Computer (expandToFullPath method doesn't work when data browser is DataType is SQLServer)
            DataSourceBrowser dataSourceBrowser = DSClientSession.createBrowser(EBackupDataType.EBackupDataType__FileSystem);

            // Resolve the supplied Computer Name
            string computer = dataSourceBrowser.expandToFullPath(Computer);
            computer = dataSourceBrowser.expandToFullPath(computer);
            string sqlComputer = computer.Split('\\').Last();
            WriteVerbose("Specified Computer resolved to: " + computer);
            WriteVerbose("SQL Server Computer resolved to: " + sqlComputer);

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
                WriteVerbose("Credentials not specified, using DS-Client Credentials...");
                computerCredentials.setUsingClientCredentials(true);
            }
            dataSourceBrowser.setCurrentCredentials(computerCredentials);

            // Set Restore Configuration
            sqlRestoreActivityInitiator.setRestoreReason(StringToERestoreReason(RestoreReason));
            sqlRestoreActivityInitiator.setRestoreClassification(StringToERestoreClassification(RestoreClassification));
            sqlRestoreActivityInitiator.setMaxPendingAsyncIO(MaxPendingAsyncIO);
            sqlRestoreActivityInitiator.setDSSystemReadThreads(ReadThreads);
            sqlRestoreActivityInitiator.setDetailLogReporting(UseDetailedLog);
            sqlRestoreActivityInitiator.setLocalStorageHandling(StringToERestoreLocalStorageHandling(LocalStorageMethod));

            // Set the share mapping
            selected_shares[] selectedShares = sqlRestoreActivityInitiator.selected_shares();

            List<share_mapping> shareMapping = new List<share_mapping>();
            foreach (selected_shares share in selectedShares)
            {
                share_mapping shareMap = new share_mapping
                {
                    destination_path = (RestoreDumpOnly) ? DumpPath : sqlComputer,
                    source_share = share.share_name,
                    truncate_level = TruncateSource
                };
                shareMapping.Add(shareMap);
            }

            // Set the Dump parameters
            if (DumpMethod != null)
            {
                mssql_dump_parameters dumpParameters = new mssql_dump_parameters
                {
                    dump_method = BaseDSClientMSSqlServerBackupSet.StringToESQLDumpMethod(DumpMethod),
                    path = DumpPath ?? ""
                };
                sqlRestoreActivityInitiator.setDumpParameters(dumpParameters);
            }

            if (RestoreDumpOnly)
            {
                sqlRestoreActivityInitiator.setRestoreDumpOnly(true);

                sqlRestoreActivityInitiator.startRestore(dataSourceBrowser, computer, shareMapping.ToArray());
            }
            else
            {
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

                // Get the Destination Instance Info
                WriteVerbose("Retrieving SQL Instance Info from Destination Computer...");
                mssql_instance_info[] dbInstances = sqlDataSourceBrowser.getInstancesInfo(sqlComputer);

                // Get the Database Paths from the Computer we're restoring to
                WriteVerbose("Retrieving Database Info from Destination SQL Instance...");
                mssql_db_path[] dbPaths = sqlDataSourceBrowser.getDatabasesPath(sqlComputer, Instance);

                // Create a List of Selected Databases that need to be mapped
                List<string> databasesToMap = new List<string>();
                foreach (string item in Items)
                    databasesToMap.Add(item.Split('\\').Last());

                // Set the Database Restore Mappings
                List<mssql_restore_path> restorePaths = new List<mssql_restore_path>();
                if (MapDatabase != null)
                {
                    foreach (string mapping in MapDatabase)
                    {
                        string[] splitMap = mapping.Split(',');
                        int splitMapLength = splitMap.Length;
                        string sourceDb = splitMap[0];
                        string destDb = splitMap[1];
                        string dataPath;
                        string logPath;

                        // Select the destination database if it exists, will throw exception if not
                        try
                        {
                            mssql_db_path existDestDb = dbPaths.Single(dbPath => dbPath.destination_db == destDb);

                            dataPath = (splitMapLength > 2) ? splitMap[2] : existDestDb.files_path.Single(path => path.is_data).path;
                            logPath = (splitMapLength > 3) ? splitMap[3] : existDestDb.files_path.Single(path => !path.is_data).path;
                        }
                        catch
                        {
                            throw new Exception("Destination database: " + destDb + " does not exist!");
                        }

                        List<mssql_path_item> filePaths = new List<mssql_path_item>();

                        mssql_path_item restoreDataPath = new mssql_path_item
                        {
                            is_data = true,
                            path = dataPath
                        };
                        filePaths.Add(restoreDataPath);

                        mssql_path_item restoreLogPath = new mssql_path_item
                        {
                            is_data = false,
                            path = logPath
                        };
                        filePaths.Add(restoreLogPath);

                        restorePaths.Add(new mssql_restore_path
                        {
                            source_db = sourceDb,
                            destination_db = destDb,
                            files_path = filePaths.ToArray()
                        });

                        // Remove the database from the list of databases needing to be mapped
                        databasesToMap.Remove(sourceDb);
                    }

                    // Attempt to create default mappings for any remaining selected databases still to be mapped
                    foreach (string database in databasesToMap)
                        restorePaths.Add(DefaultSqlDatabaseMapping(database, dbPaths));

                    sqlRestoreActivityInitiator.setRestorePath(restorePaths.ToArray());
                }
                else
                {
                    // Attempt to create default database mappings for selected databases
                    foreach (string database in databasesToMap)
                        restorePaths.Add(DefaultSqlDatabaseMapping(database, dbPaths));

                    sqlRestoreActivityInitiator.setRestorePath(restorePaths.ToArray());
                }

                sqlRestoreActivityInitiator.setPreserveOriginalLocation(PreserveFileLocation);
                sqlRestoreActivityInitiator.setLeaveRestoringMode(LeaveDatabaseRestoring);

                sqlRestoreActivityInitiator.startRestore(sqlDataSourceBrowser, computer, shareMapping.ToArray());

                sqlDataSourceBrowser.Dispose();
            }

            dataSourceBrowser.Dispose();
            sqlRestoreActivityInitiator.Dispose();
            restoreSession.Dispose();

            SessionState.PSVariable.Remove("RestoreType");
            SessionState.PSVariable.Remove("RestoreView");
        }

        private static mssql_restore_path DefaultSqlDatabaseMapping(string sourceDatabase, IEnumerable<mssql_db_path> destinationPaths)
        {
            mssql_restore_path mappedDatabase = new mssql_restore_path();

            try
            {
                mssql_db_path destinationPath = destinationPaths.Single(dest => dest.destination_db == sourceDatabase);

                mappedDatabase.source_db = sourceDatabase;
                mappedDatabase.destination_db = destinationPath.destination_db;
                mappedDatabase.files_path = destinationPath.files_path;
            }
            catch
            {
                throw new Exception("Unable to create a default database mapping for selected database: " + sourceDatabase);
            }

            return mappedDatabase;
        }
    }
}