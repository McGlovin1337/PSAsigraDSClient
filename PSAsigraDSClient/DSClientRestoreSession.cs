using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.DSClientRestoreOptions;

namespace PSAsigraDSClient
{
    public class DSClientRestoreSession
    {
        private BackupSetRestoreView _backupSetRestoreView;
        private RestoreActivityInitiator _restoreActivityInitiator;
        private List<long> _selectedItemIds;
        private readonly DataSourceBrowser _dataSourceBrowser;             
        private readonly List<DSClientBackupSetItemInfo> _browsedItems;
        private readonly EBackupDataType _dataType;
        private readonly Type _credType;
        private readonly Type _setType;
        private readonly Type _restoreOptionsType;

        public int RestoreId { get; private set; }
        public int BackupSetId { get; private set; }        
        public DSClientBackupSetItemInfo[] SelectedItems { get; private set; }
        public ReadyStatus Ready { get; private set; }
        public string DataType { get; private set; }
        public string Computer { get; private set; }
        public DSClientCredentialSet[] CredentialSet { get; set; }
        public RestoreDestination[] DestinationPaths { get; private set; }
        public string RestoreReason { get; private set; }
        public string RestoreClassification { get; private set; }
        public DSClientRestoreOption[] RestoreOptions { get; private set; }

        internal DSClientRestoreSession(int restoreId, BackupSet backupSet, Type setType)
        {
            _backupSetRestoreView = backupSet.prepare_restore(0, DateTimeToUnixEpoch(DateTime.Now), 0);
            _browsedItems = new List<DSClientBackupSetItemInfo>();
            _dataSourceBrowser = backupSet.dataBrowser();
            _dataType = backupSet.getDataType();
            _restoreActivityInitiator = null;
            _selectedItemIds = new List<long>();
            _setType = setType;

            RestoreId = restoreId;
            BackupSetId = backupSet.getID();
            DataType = EnumToString(_dataType);
            Computer = backupSet.getComputerName();
            RestoreClassification = "Production";

            switch (_dataType)
            {
                case EBackupDataType.EBackupDataType__FileSystem:
                    _restoreOptionsType = (_setType == typeof(Win32FS_BackupSet)) ? typeof(RestoreOptions_Win32FileSystem) : typeof(RestoreOptions_FileSystem);
                    RestoreOptions = (_setType == typeof(Win32FS_BackupSet))
                        ? new RestoreOptions_Win32FileSystem().GetRestoreOptions().ToArray()
                        : new RestoreOptions_FileSystem().GetRestoreOptions().ToArray();
                    break;
                case EBackupDataType.EBackupDataType__SQLServer:
                    _restoreOptionsType = typeof(RestoreOptions_MSSQLServer);
                    RestoreOptions = new RestoreOptions_MSSQLServer().GetRestoreOptions().ToArray();
                    break;
                default:
                    _restoreOptionsType = typeof(RestoreOptions_Base);
                    RestoreOptions = new RestoreOptions_Base().GetRestoreOptions().ToArray();
                    break;
            }

            List<DSClientCredentialSet> credSet = new List<DSClientCredentialSet>();
            BackupSetCredentials backupSetCredentials = _dataSourceBrowser.getCurrentCredentials();

            // The Type of Computer Credentials is determined by the Type of BackupSet              
            if (_setType == typeof(UnixFS_Generic_BackupSet))
            {
                if (backupSet.getComputerName().Split('\\')[0] == "UNIX-SSH")
                    _credType = typeof(UnixFS_SSH_BackupSetCredentials);
                else
                    _credType = typeof(UnixFS_Generic_BackupSetCredentials);
            }
            else if (_setType == typeof(DB2_BackupSet))
                _credType = typeof(DB2_BackupSetCredentials);
            else
                _credType = typeof(Win32FS_Generic_BackupSetCredentials);

            // Add the Computer Credentials
            if (_credType == typeof(UnixFS_Generic_BackupSetCredentials))
            {
                UnixFS_Generic_BackupSetCredentials unixCredentials = UnixFS_Generic_BackupSetCredentials.from(backupSetCredentials);
                DSClientCredential credential = new DSClientCredential(unixCredentials, unixCredentials.getUserName());
                credSet.Add(new DSClientCredentialSet(credential, DSClientConnectionOption.ComputerCredential));
            }
            else if (_credType == typeof(UnixFS_SSH_BackupSetCredentials))
            {
                UnixFS_SSH_BackupSetCredentials sshBackupSetCreds = UnixFS_SSH_BackupSetCredentials.from(backupSetCredentials);
                DSClientSSHCredential sshCredential = new DSClientSSHCredential(sshBackupSetCreds, sshBackupSetCreds.getUserName());
                credSet.Add(new DSClientCredentialSet(sshCredential, DSClientConnectionOption.SSHCredential));
            }
            else if (_credType == typeof(Win32FS_Generic_BackupSetCredentials))
            {
                Win32FS_Generic_BackupSetCredentials win32BackupSetCreds = Win32FS_Generic_BackupSetCredentials.from(backupSetCredentials);
                DSClientCredential credential = new DSClientCredential(win32BackupSetCreds, win32BackupSetCreds.getUserName());
                credSet.Add(new DSClientCredentialSet(credential, DSClientConnectionOption.ComputerCredential));
            }

            // If the Backup Set is MSSQL, add the Database Credentials
            if (_setType == typeof(MSSQL_BackupSet))
            {
                Win32FS_Generic_BackupSetCredentials databaseCredential = MSSQL_BackupSet.from(backupSet).getDBCredentials();
                DSClientCredential dbCredential = new DSClientCredential(databaseCredential, databaseCredential.getUserName());
                credSet.Add(new DSClientCredentialSet(dbCredential, DSClientConnectionOption.DatabaseCredential));
            }

            CredentialSet = credSet.ToArray();

            UpdateReadyStatus();

            backupSet.Dispose();
        }

        internal void AddBrowsedItem(DSClientBackupSetItemInfo item)
        {
            if (!_browsedItems.Exists(i => i.ItemId == item.ItemId))
                _browsedItems.Add(item);
        }

        internal void AddBrowsedItems(IEnumerable<DSClientBackupSetItemInfo> items)
        {
            // This method helps keep track of all the items discovered when calling Get-DSClientStoredItem Cmdlet
            // Since the BackedUpDataView class doesn't allow selecting items by id, this allows to select items by id, and view the details of each selected item when added to SelectedItems Property
            _browsedItems.AddRange(items.Except(_browsedItems));
        }

        internal void AddSelectedItem(long itemId)
        {
            if (!_selectedItemIds.Contains(itemId))
                _selectedItemIds.Add(itemId);

            SetSelectedItems();
            UpdateReadyStatus();
        }

        internal void AddSelectedItems(long[] itemIds)
        {
            _selectedItemIds.AddRange(itemIds.Except(_selectedItemIds));

            SetSelectedItems();
            UpdateReadyStatus();
        }

        private void ApplyRestoreOptions()
        {
            bool log = (bool?)RestoreOptions.SingleOrDefault(v => v.Option == "UseDetailedLog").Value ?? false;
            _restoreActivityInitiator.setDetailLogReporting(log);

            try
            {
                int readThreads = (int?)RestoreOptions.SingleOrDefault(v => v.Option == "DSSystemReadThreads").Value ?? 0;
                _restoreActivityInitiator.setDSSystemReadThreads(readThreads);
            }
            catch
            {
                // Do nothing, it's likely this option isn't supported
            }

            try
            {
                int asyncIO = (int?)RestoreOptions.SingleOrDefault(v => v.Option == "MaxPendingAsyncIO").Value ?? 0;
                _restoreActivityInitiator.setMaxPendingAsyncIO(asyncIO);
            }
            catch
            {
                // Do nothing it's likely this option isn't supported
            }

            _restoreActivityInitiator.setRestoreClassification(StringToEnum<ERestoreClassification>(RestoreClassification));

            if (RestoreReason != null)
                _restoreActivityInitiator.setRestoreReason(StringToEnum<ERestoreReason>(RestoreReason));

            string localStorageMethod = (string)RestoreOptions.SingleOrDefault(v => v.Option == "LocalStorageMethod").Value;
            if (localStorageMethod != null)
                _restoreActivityInitiator.setLocalStorageHandling(StringToEnum<ERestoreLocalStorageHandling>(localStorageMethod));

            if (_restoreOptionsType == typeof(RestoreOptions_FileSystem) || _restoreOptionsType == typeof(RestoreOptions_Win32FileSystem))
            {
                FS_RestoreActivityInitiator fs = FS_RestoreActivityInitiator.from(_restoreActivityInitiator);
                try
                {
                    string overwrite = (string)RestoreOptions.Single(v => v.Option == "FileOverwriteOption").Value;
                    fs.setFileOverwriteOption(StringToEnum<EFileOverwriteOption>(overwrite));
                }
                catch (InvalidOperationException)
                {
                    throw new Exception("Missing FileOverwriteOption Option");
                }

                try
                {
                    string restoreMethod = (string)RestoreOptions.Single(v => v.Option == "RestoreMethod").Value;
                    fs.setRestoreMethod(StringToEnum<ERestoreMethod>(restoreMethod));
                }
                catch (InvalidOperationException)
                {
                    throw new Exception("Missing RestoreMethod Option");
                }

                try
                {
                    string restorePermissions = (string)RestoreOptions.Single(v => v.Option == "RestorePermissions").Value;
                    fs.setRestorePermission(StringToEnum<ERestorePermission>(restorePermissions));
                }
                catch (InvalidOperationException)
                {
                    throw new Exception("Missing RestorePermissions Option");
                }

                if (_restoreOptionsType == typeof(RestoreOptions_Win32FileSystem))
                {
                    Win32FS_RestoreActivityInitiator winfs = Win32FS_RestoreActivityInitiator.from(fs);
                    if (winfs.hasActiveDirectory())
                    {
                        try
                        {
                            bool authoritative = (bool)RestoreOptions.Single(v => v.Option == "AuthoritativeRestore").Value;
                            winfs.setAuthoritativeRestore(authoritative);
                        }
                        catch (InvalidOperationException)
                        {
                            throw new Exception("Missing AuthoritativeRestore Option");
                        }
                    }

                    try
                    {
                        bool junctOverwrite = (bool)RestoreOptions.Single(v => v.Option == "OverwriteJunctionPoint").Value;
                        winfs.setOverwriteJunctionPoint(junctOverwrite);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new Exception("Missing OverwriteJunctionPoint Option");
                    }

                    try
                    {
                        bool skipoffline = (bool)RestoreOptions.Single(v => v.Option == "SkipOfflineFiles").Value;
                        winfs.setSkipOfflineFiles(skipoffline);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new Exception("Missing SkipOfflineFiles Option");
                    }
                }
            }
            else if (_restoreOptionsType == typeof(RestoreOptions_MSSQLServer))
            {
                MSSQL_RestoreActivityInitiator sqlRestoreActivityInitiator = MSSQL_RestoreActivityInitiator.from(_restoreActivityInitiator);

                mssql_dump_parameters dumpParams = new mssql_dump_parameters
                {
                    dump_method = StringToEnum<ESQLDumpMethod>(RestoreOptions.Single(v => v.Option == "DumpMethod").Value as string),
                    path = RestoreOptions.SingleOrDefault(option => option.Option == "DumpPath").Value as string ?? ""
                };
                sqlRestoreActivityInitiator.setDumpParameters(dumpParams);

                sqlRestoreActivityInitiator.setLeaveRestoringMode((bool)RestoreOptions.Single(v => v.Option == "LeaveRestoringMode").Value);

                sqlRestoreActivityInitiator.setRestoreDumpOnly((bool)RestoreOptions.Single(v => v.Option == "RestoreDumpOnly").Value);

                sqlRestoreActivityInitiator.setPreserveOriginalLocation((bool)RestoreOptions.Single(v => v.Option == "PreserveOriginalLocation").Value);

                // Get all of the Database mappings into a single List and pass to setRestorePath method
                List<mssql_restore_path> dbrestorePaths = new List<mssql_restore_path>();
                foreach (RestoreDestination destination in DestinationPaths)
                    foreach (MSSQLDatabaseMap dbMap in destination.DatabaseMapping)
                    {
                        mssql_restore_path restorePath = dbMap.GetRestorePath();
                        if (restorePath != null && restorePath.destination_db != null)
                            dbrestorePaths.Add(restorePath);
                    }
                sqlRestoreActivityInitiator.setRestorePath(dbrestorePaths.ToArray());
            }
        }

        internal void Dispose()
        {
            if (_backupSetRestoreView != null)
                _backupSetRestoreView.Dispose();

            if (_dataSourceBrowser != null)
                _dataSourceBrowser.Dispose();

            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.Dispose();

            foreach (DSClientCredentialSet credential in CredentialSet)
                credential.GetCredential()
                    .GetCredentials()
                    .Dispose();
        }

        protected RestoreActivityInitiator GetRestoreActivityInitiator()
        {
            return _restoreActivityInitiator;
        }

        internal Type GetRestoreOptionsType()
        {
            return _restoreOptionsType;
        }

        internal BackupSetRestoreView GetRestoreView()
        {
            return _backupSetRestoreView;
        }

        internal void RemoveSelectedItem(long itemId)
        {
            _selectedItemIds.Remove(itemId);

            SetSelectedItems();
            UpdateReadyStatus();
        }

        internal void RemoveSelectedItems(long[] itemIds)
        {
            foreach (long item in itemIds)
                _selectedItemIds.Remove(item);

            SetSelectedItems();
            UpdateReadyStatus();
        }

        internal void SetComputer(string computer)
        {
            if (_dataSourceBrowser != null)
            {
                string comp = _dataSourceBrowser.expandToFullPath(computer);
                Computer = _dataSourceBrowser.expandToFullPath(comp);
            }

            UpdateReadyStatus();
        }

        internal void SetCredentials(DSClientCredential credential)
        {
            _dataSourceBrowser.setCurrentCredentials(credential.GetCredentials());

            for (int i = 0; i < CredentialSet.Length; i++)
                if (CredentialSet[i].Type == DSClientConnectionOption.ComputerCredential)
                    CredentialSet[i] = new DSClientCredentialSet(credential, DSClientConnectionOption.ComputerCredential);

            UpdateReadyStatus();
        }

        internal void SetDatabaseCredentials(DSClientCredential credential)
        {
            if (_setType == typeof(MSSQL_BackupSet))
                SQLDataBrowserWithSetCreation.from(_dataSourceBrowser).setDBCredentials(Win32FS_Generic_BackupSetCredentials.from(credential.GetCredentials()));
            else
                throw new Exception("Database Credentials not supported in this Restore Session");

            for (int i = 0; i < CredentialSet.Length; i++)
                if (CredentialSet[i].Type == DSClientConnectionOption.DatabaseCredential)
                    CredentialSet[i] = new DSClientCredentialSet(credential, DSClientConnectionOption.DatabaseCredential);

            UpdateReadyStatus();
        }

        internal void SetRestoreClassification(string classification)
        {
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.setRestoreClassification(StringToEnum<ERestoreClassification>(classification));

            RestoreClassification = classification;
            UpdateReadyStatus();
        }

        internal void SetRestoreOptions(RestoreOptions_Base restoreOptions)
        {
            Type type = restoreOptions.GetType();

            if (type == typeof(RestoreOptions_FileSystem))
            {
                RestoreOptions_FileSystem fsOptions = restoreOptions as RestoreOptions_FileSystem;
                RestoreOptions = fsOptions.GetRestoreOptions();
            }
            else if (type == typeof(RestoreOptions_Win32FileSystem))
            {
                RestoreOptions_Win32FileSystem winfsOptions = restoreOptions as RestoreOptions_Win32FileSystem;
                RestoreOptions = winfsOptions.GetRestoreOptions();
            }
            else if (type == typeof(RestoreOptions_MSSQLServer))
            {
                RestoreOptions_MSSQLServer sqlOptions = restoreOptions as RestoreOptions_MSSQLServer;
                RestoreOptions = sqlOptions.GetRestoreOptions();
            }
            else
            {
                throw new Exception("Unsupported Option Type");
            }

            ApplyRestoreOptions();
        }

        internal void SetRestoreReason(string reason)
        {
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.setRestoreReason(StringToEnum<ERestoreReason>(reason));

            RestoreReason = reason;
            UpdateReadyStatus();
        }

        private void SetSelectedItems()
        {
            // Every time the items are changed a new RestoreActivityInitiator is required
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.Dispose();

            long[] items = _selectedItemIds.ToArray();

            if (items.Length > 0)
            {
                _restoreActivityInitiator = _backupSetRestoreView.prepareRestore(items);                

                SelectedItems = new DSClientBackupSetItemInfo[items.Length];
                for (int i = 0; i < items.Length; i++)
                    SelectedItems[i] = _browsedItems.Single(item => item.ItemId == items[i]);

                selected_shares[] selectedShares = _restoreActivityInitiator.selected_shares();
                DestinationPaths = new RestoreDestination[selectedShares.Length];
                for (int i = 0; i < selectedShares.Length; i++)
                        DestinationPaths[i] = new RestoreDestination(i + 1, selectedShares[i]);

                if (_setType == typeof(MSSQL_BackupSet))
                {
                    foreach (DSClientBackupSetItemInfo item in SelectedItems)
                    {
                        string instance = Regex.Match(item.Path, @"\[.*?\]").Value;
                        DestinationPaths.Single(dbInst => dbInst.Destination == instance)
                            .AddDatabaseMapping(item, Computer, SQLDataBrowserWithSetCreation.from(_dataSourceBrowser));
                    }
                }

                ApplyRestoreOptions();
            }
            else
            {
                SelectedItems = null;
                DestinationPaths = null;
                _restoreActivityInitiator = null;
            }
        }

        internal GenericActivity StartRestore()
        {
            if (Ready.Ready)
            {
                share_mapping[] shares = new share_mapping[DestinationPaths.Length];
                for (int i = 0; i < DestinationPaths.Length; i++)
                    shares[i] = DestinationPaths[i].GetShareMapping();

                GenericActivity restoreActivity = _restoreActivityInitiator.startRestore(_dataSourceBrowser, Computer, shares);
                Dispose();
                return restoreActivity;
            }

            throw new Exception("Restore Session is not Ready to Start a Restore");
        }

        internal void UpdateMSSQLDatabaseRestoreMap(int destinationId, MSSQLDatabaseMap databaseMap)
        {
            RestoreDestination instance;
            try
            {
                instance = DestinationPaths.Single(dest => dest.DestinationId == destinationId);
            }
            catch
            {
                throw new Exception("DestinationPath Not Found");
            }

            // Check the specified source database is actually selected for restore
            if (!instance.DatabaseMapping.Any(db => db.SourceDatabase == databaseMap.SourceDatabase))
                throw new Exception("Specified Source Database not Selected for Restore");

            // Check the restore destination has a matching Destination Database Name
            SQLDataBrowserWithSetCreation sqlDataBrowser = SQLDataBrowserWithSetCreation.from(_dataSourceBrowser);
            mssql_db_path[] destinationDatabases = sqlDataBrowser.getDatabasesPath(Computer.Split('\\').Last(), instance.Destination.TrimStart('[').TrimEnd(']'));
            if (!destinationDatabases.Any(db => db.destination_db == databaseMap.DestinationDatabase))
                throw new Exception("Specified Destination Database does not exist");

            // If the specified new mapping doesn't have a configured mssql_restore_path object, we need to complete the setup
            if (databaseMap.GetRestorePath() == null)
            {
                MSSQLDatabaseMap newDatabaseMap = new MSSQLDatabaseMap(SelectedItems.Single(item => item.Name == databaseMap.SourceDatabase), destinationDatabases.Single(dest => dest.destination_db == databaseMap.DestinationDatabase));
                databaseMap = newDatabaseMap;
            }   

            // If we get this far, update the mapping
            for (int i = 0; i < instance.DatabaseMapping.Length; i++)
            {
                if (instance.DatabaseMapping[i].SourceDatabase == databaseMap.SourceDatabase)
                {
                    instance.DatabaseMapping[i] = databaseMap;
                    break;
                }
            }

            ApplyRestoreOptions();
            UpdateReadyStatus();
        }

        protected virtual void UpdateReadyStatus()
        {
            // Checks all minimum required configuration is in place to allow a restore to be started
            // Needs to include a credential check against the target computer
            List<string> errors = new List<string>();

            if (_restoreActivityInitiator == null)
                errors.Add("No Items Selected");

            if (RestoreReason == null)
                errors.Add("No Restore Reason Specified");

            // Test Destination Credentials are Valid
            try
            {
                CredentialSet.Single(c => c.Type == DSClientConnectionOption.ComputerCredential)
                    .GetCredential()
                    .GetCredentials()
                    .check(Computer);
            }
            catch
            {
                errors.Add("Invalid Credentials");
            }

            // Check a Destination has been specified
            if (DestinationPaths == null)
                errors.Add("DestinationPaths Not Specified");

            // If this is a SQL Database restore, check each selected database has a destination database mapping (except when RestoreDumpOnly option is set)
            if (_setType == typeof(MSSQL_BackupSet))
                if (!(bool)RestoreOptions.Single(o => o.Option == "RestoreDumpOnly").Value && DestinationPaths != null)
                    foreach (RestoreDestination destination in DestinationPaths)
                        if (destination.DatabaseMapping.Any(map => string.IsNullOrEmpty(map.DestinationDatabase)))
                            errors.Add("Missing Database Mapping");

            foreach (DSClientRestoreOption option in RestoreOptions)
            {
                if (option.GetClassType() != _restoreOptionsType)
                {
                    errors.Add("Invalid Restore Options");
                    break;
                }
            }

            if (errors.Count > 0)
                Ready = new ReadyStatus(false, "Unable to Start Restore", errors.ToArray());
            else
                Ready = new ReadyStatus(true, "Ready to Start Restore", null);
        }

        public class ReadyStatus
        {
            public bool Ready { get; private set; }
            public string Description { get; private set; }
            public string[] Errors { get; private set; }

            internal ReadyStatus(bool ready, string description, string[] errs)
            {
                Ready = ready;
                Description = description;
                Errors = errs;
            }

            public override string ToString()
            {
                return Ready.ToString();
            }
        }

        public class RestoreDestination
        {
            private readonly share_mapping _shareMapping;

            public int DestinationId { get; private set; }
            public string Source { get; private set; }
            public string Destination { get; private set; }
            public string TruncatablePath { get; private set; }
            public int TruncateAmount { get; private set; }
            public MSSQLDatabaseMap[] DatabaseMapping { get; private set; }

            internal RestoreDestination(int id, selected_shares selectedShares)
            {
                DestinationId = id;
                Source = selectedShares.share_name;
                Destination = selectedShares.share_name;
                TruncatablePath = selectedShares.truncatable_path;
                TruncateAmount = 0;

                _shareMapping = new share_mapping
                {
                    destination_path = selectedShares.share_name,
                    source_share = selectedShares.share_name,
                    truncate_level = 0
                };
            }

            internal RestoreDestination(int id, selected_shares selectedShares, mssql_restore_path[] dbRestorePaths) : this(id, selectedShares)
            {
                DatabaseMapping = new MSSQLDatabaseMap[dbRestorePaths.Length];

                for (int i = 0; i < dbRestorePaths.Length; i++)
                    DatabaseMapping[i] = new MSSQLDatabaseMap(dbRestorePaths[i]);
            }

            internal RestoreDestination(int id, selected_shares selectedShares, string destination)
            {
                DestinationId = id;
                Source = selectedShares.share_name;
                Destination = destination;
                TruncatablePath = selectedShares.truncatable_path;
                TruncateAmount = 0;

                _shareMapping = new share_mapping
                {
                    destination_path = destination,
                    source_share = selectedShares.share_name,
                    truncate_level = 0
                };
            }

            internal void AddDatabaseMapping(DSClientBackupSetItemInfo selectedItem, string computer, SQLDataBrowserWithSetCreation dataSourceBrowser)
            {
                mssql_db_path[] databases = dataSourceBrowser.getDatabasesPath(computer.Split('\\').Last(), Destination.TrimStart('[').TrimEnd(']'));

                MSSQLDatabaseMap dbMap;
                mssql_db_path matchingDatabase = databases.SingleOrDefault(db => db.destination_db == selectedItem.Name);
                if (matchingDatabase != null)
                    dbMap = new MSSQLDatabaseMap(selectedItem, matchingDatabase);
                else
                    dbMap = new MSSQLDatabaseMap(selectedItem);

                List<MSSQLDatabaseMap> existingMaps = (DatabaseMapping == null) ? new List<MSSQLDatabaseMap>() : DatabaseMapping.ToList();
                existingMaps.Add(dbMap);
                DatabaseMapping = existingMaps.ToArray();
            }

            internal share_mapping GetShareMapping()
            {
                return _shareMapping;
            }

            internal void SetDestination(string destination)
            {
                _shareMapping.destination_path = destination;
                Destination = _shareMapping.destination_path;
            }

            internal void SetTruncateLevel(int truncate)
            {
                int maxTruncate = TruncatablePath.Split('\\').Length;
                _shareMapping.truncate_level = (truncate > maxTruncate) ? maxTruncate : truncate;
                TruncateAmount = _shareMapping.truncate_level;
            }

            public override string ToString()
            {
                return $"{Source} => {Destination}";
            }
        }
    }

    public class MSSQLDatabaseMap
    {
        private readonly mssql_restore_path _mssqlRestorePath;
        public string SourceDatabase { get; private set; }
        public string DestinationDatabase { get; private set; }
        public MSSQLFilePath[] FilePaths { get; private set; }

        internal MSSQLDatabaseMap(mssql_restore_path restorePath)
        {
            _mssqlRestorePath = restorePath;
            SourceDatabase = restorePath.source_db;
            DestinationDatabase = restorePath.destination_db;
            FilePaths = new MSSQLFilePath[restorePath.files_path.Length];

            for (int i = 0; i < restorePath.files_path.Length; i++)
                FilePaths[i] = new MSSQLFilePath(restorePath.files_path[i]);
        }

        internal MSSQLDatabaseMap(DSClientBackupSetItemInfo selectedItem)
        {
            SourceDatabase = selectedItem.Name;

            _mssqlRestorePath = new mssql_restore_path
            {
                source_db = SourceDatabase
            };
        }

        internal MSSQLDatabaseMap(DSClientBackupSetItemInfo selectedItem, mssql_db_path destinationDb)
        {
            SourceDatabase = selectedItem.Name;
            DestinationDatabase = destinationDb.destination_db;
            FilePaths = new MSSQLFilePath[destinationDb.files_path.Length];

            for (int i = 0; i < destinationDb.files_path.Length; i++)
                FilePaths[i] = new MSSQLFilePath(destinationDb.files_path[i]);

            _mssqlRestorePath = new mssql_restore_path
            {
                source_db = SourceDatabase,
                destination_db = DestinationDatabase,
                files_path = destinationDb.files_path
            };
        }

        internal MSSQLDatabaseMap(string sourceDb, string destinationDb)
        {
            SourceDatabase = sourceDb;
            DestinationDatabase = destinationDb;
        }

        internal mssql_restore_path GetRestorePath()
        {
            return _mssqlRestorePath;
        }

        public override string ToString()
        {
            return $"{SourceDatabase} => {DestinationDatabase}";
        }

        public class MSSQLFilePath
        {
            private readonly mssql_path_item _filePath;
            public MSSQLFileType FileType { get; private set; }
            public string Path { get; private set; }

            internal MSSQLFilePath(mssql_path_item pathItem)
            {
                _filePath = pathItem;
                FileType = (pathItem.is_data) ? MSSQLFileType.Data : MSSQLFileType.Log;
                Path = pathItem.path;
            }

            internal mssql_path_item  GetPathItem()
            {
                return _filePath;
            }

            public override string ToString()
            {
                return $"{FileType}: {Path}";
            }

            public enum MSSQLFileType
            {
                Data,
                Log
            }
        }
    }

    public class DSClientRestoreOption
    {
        private readonly Type _classType;
        public string Option { get; private set; }
        public object Value { get; private set; }

        internal DSClientRestoreOption(Type classType, string option, object value)
        {
            _classType = classType;
            Option = option;
            Value = value;
        }

        internal Type GetClassType()
        {
            return _classType;
        }

        public override string ToString()
        {
            return Option;
        }
    }
}