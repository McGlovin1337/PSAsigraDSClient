using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public class DSClientRestoreSession
    {
        private BackupSetRestoreView _backupSetRestoreView;
        private RestoreActivityInitiator _restoreActivityInitiator;
        private readonly BackupSetCredentials _credentials;
        private List<int> _destinationIds;
        private List<long> _selectedItemIds;
        private readonly EBackupDataType _dataType;
        private readonly DataSourceBrowser _dataSourceBrowser;             
        private readonly List<DSClientBackupSetItemInfo> _browsedItems;
        private readonly Type _setType;

        public int RestoreId { get; private set; }
        public int BackupSetId { get; private set; }        
        public DSClientBackupSetItemInfo[] SelectedItems { get; private set; }
        public ReadyStatus Ready { get; private set; }
        public string DataType { get; private set; }
        public string Computer { get; private set; }
        public string Credential { get; private set; }
        public RestoreDestination[] DestinationPaths { get; private set; }
        public string RestoreReason { get; private set; }
        public string RestoreClassification { get; private set; }
        public bool UseDetailedLog { get; private set; }
        public string LocalStorageMethod { get; private set; }
        public int DSSystemReadThreads { get; private set; }
        public int MaxPendingAsyncIO { get; private set; }
        public FileSystemRestoreOptions FileSystemOptions { get; private set; }

        public DSClientRestoreSession(int restoreId,
            int backupSetId,
            string computer,
            Type setType,
            EBackupDataType dataType,
            DataSourceBrowser dataSourceBrowser,
            BackupSetRestoreView restoreView)
        {
            _setType = setType;
            _dataType = dataType;
            _backupSetRestoreView = restoreView;
            _restoreActivityInitiator = null;
            _dataSourceBrowser = dataSourceBrowser;
            _credentials = dataSourceBrowser.getCurrentCredentials();
            _browsedItems = new List<DSClientBackupSetItemInfo>();
            _destinationIds = new List<int>();
            _selectedItemIds = new List<long>();

            RestoreId = restoreId;
            BackupSetId = backupSetId;
            DataType = EnumToString(dataType);
            Computer = computer;
            RestoreClassification = "Production";
            UseDetailedLog = false;
            LocalStorageMethod = "ContinueIfDisconnect";
            DSSystemReadThreads = 0;
            MaxPendingAsyncIO = 0;
            
            switch (dataType)
            {
                case EBackupDataType.EBackupDataType__FileSystem:
                    FileSystemOptions = (_setType == typeof(Win32FS_BackupSet)) ? new FileSystemRestoreOptions(true, true) : new FileSystemRestoreOptions(true, false);
                    break;
                default:
                    FileSystemOptions = new FileSystemRestoreOptions(false);
                    break;
            }

            GetCredentials();
            UpdateReadyStatus();
        }

        public void AddBrowsedItems(IEnumerable<DSClientBackupSetItemInfo> items)
        {
            // This method helps keep track of all the items discovered when calling Get-DSClientStoredItem Cmdlet
            // Since the BackedUpDataView class doesn't allow selecting items by id, this allows to select items by id, and view the details of each selected item when added to SelectedItems Property
            _browsedItems.AddRange(items.Except(_browsedItems));
        }

        public void AddSelectedItem(long itemId)
        {
            if (!_selectedItemIds.Contains(itemId))
                _selectedItemIds.Add(itemId);

            SetSelectedItems();
        }

        public void AddSelectedItems(long[] itemIds)
        {
            _selectedItemIds.AddRange(itemIds.Except(_selectedItemIds));

            SetSelectedItems();
        }

        public void Dispose()
        {
            if (_backupSetRestoreView != null)
                _backupSetRestoreView.Dispose();

            if (_dataSourceBrowser != null)
                _dataSourceBrowser.Dispose();

            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.Dispose();

            if (_credentials != null)
                _credentials.Dispose();
        }

        private void GetCredentials()
        {
            if (_credentials.isUsingClientCredentials())
                Credential = "DS-Client Service Impersonation";
            else if (_setType == typeof(UnixFS_Generic_BackupSet))
                Credential = UnixFS_Generic_BackupSetCredentials.from(_credentials).getUserName();
            else if (_setType == typeof(DB2_BackupSet))
                Credential = DB2_BackupSetCredentials.from(_credentials).getUserName();
            else
                Credential = Win32FS_Generic_BackupSetCredentials.from(_credentials).getUserName();
        }

        protected RestoreActivityInitiator GetRestoreActivityInitiator()
        {
            return _restoreActivityInitiator;
        }

        public ref BackupSetRestoreView GetRestoreView()
        {
            return ref _backupSetRestoreView;
        }

        public void RemoveSelectedItem(long itemId)
        {
            _selectedItemIds.Remove(itemId);

            SetSelectedItems();
        }

        public void RemoveSelectedItems(long[] itemIds)
        {
            foreach (long item in itemIds)
                _selectedItemIds.Remove(item);

            SetSelectedItems();
        }

        public virtual void SetSelectedItems()
        {
            // Every time the items are changed a new RestoreActivityInitiator is required
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.Dispose();

            long[] items = _selectedItemIds.ToArray();

            if (items.Length > 0)
            {
                _restoreActivityInitiator = _backupSetRestoreView.prepareRestore(items);

                _restoreActivityInitiator.setDetailLogReporting(UseDetailedLog);
                _restoreActivityInitiator.setDSSystemReadThreads(DSSystemReadThreads);
                _restoreActivityInitiator.setMaxPendingAsyncIO(MaxPendingAsyncIO);
                _restoreActivityInitiator.setRestoreClassification(StringToEnum<ERestoreClassification>(RestoreClassification));

                if (RestoreReason != null)
                    _restoreActivityInitiator.setRestoreReason(StringToEnum<ERestoreReason>(RestoreReason));

                if (LocalStorageMethod != null)
                    _restoreActivityInitiator.setLocalStorageHandling(StringToEnum<ERestoreLocalStorageHandling>(LocalStorageMethod));

                SelectedItems = new DSClientBackupSetItemInfo[items.Length];
                for (int i = 0; i < items.Length; i++)
                    SelectedItems[i] = _browsedItems.Single(item => item.ItemId == items[i]);

                selected_shares[] selectedShares = _restoreActivityInitiator.selected_shares();
                DestinationPaths = new RestoreDestination[selectedShares.Length];
                for (int i = 0; i < selectedShares.Length; i++)
                    DestinationPaths[i] = new RestoreDestination(i+1, selectedShares[i]);
            }
            else
                _restoreActivityInitiator = null;
        }

        public void SetComputer(string computer)
        {
            if (_dataSourceBrowser != null)
            {
                string comp = _dataSourceBrowser.expandToFullPath(computer);
                Computer = _dataSourceBrowser.expandToFullPath(comp);
            }

            UpdateReadyStatus();
        }

        public void SetCredentials(PSCredential credential)
        {
            if (_setType == typeof(UnixFS_Generic_BackupSet))
            {
                UnixFS_Generic_BackupSetCredentials creds = UnixFS_Generic_BackupSetCredentials.from(_credentials);
                creds.setCredentials(credential.UserName, credential.GetNetworkCredential().Password);
                creds.setUsingClientCredentials(false);
            }
            else if (_setType == typeof(Win32FS_BackupSet))
            {
                Win32FS_Generic_BackupSetCredentials creds = Win32FS_Generic_BackupSetCredentials.from(_credentials);
                creds.setCredentials(credential.UserName, credential.GetNetworkCredential().Password);
                creds.setUsingClientCredentials(false);
            }

            GetCredentials();
            UpdateReadyStatus();
        }

        public void SetSudoCredentials(PSCredential credential)
        {
            if (_setType == typeof(UnixFS_Generic_BackupSet))
            {
                UnixFS_SSH_BackupSetCredentials creds = UnixFS_SSH_BackupSetCredentials.from(_credentials);
                creds.setSudoAs(credential.UserName, credential.GetNetworkCredential().Password);
            }
            else
                throw new Exception("This Restore Session does not support Sudo Credentials");
        }

        public void SetDSSystemReadThreads(int threads)
        {
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.setDSSystemReadThreads(threads);

            DSSystemReadThreads = threads;
        }

        public void SetLocalStorageMethod(string method)
        {
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.setLocalStorageHandling(StringToEnum<ERestoreLocalStorageHandling>(method));

            LocalStorageMethod = method;
            UpdateReadyStatus();
        }

        public void SetMaxPendingAsyncIO(int io)
        {
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.setMaxPendingAsyncIO(io);

            MaxPendingAsyncIO = io;
        }

        public void SetRestoreClassification(string classification)
        {
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.setRestoreClassification(StringToEnum<ERestoreClassification>(classification));

            RestoreClassification = classification;
            UpdateReadyStatus();
        }

        public void SetRestoreReason(string reason)
        {
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.setRestoreReason(StringToEnum<ERestoreReason>(reason));

            RestoreReason = reason;
            UpdateReadyStatus();
        }

        public void SetUseDetailedLog(bool v)
        {
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.setDetailLogReporting(v);

            UseDetailedLog = v;
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
                _credentials.check(Computer);
            }
            catch
            {
                errors.Add("Invalid Credentials");
            }

            if (DestinationPaths == null)
                errors.Add("DestinationPaths Not Specified");

            if (errors.Count > 0)
                Ready = new ReadyStatus(false, "Unable to Start Restore", errors.ToArray());
            else
                Ready = new ReadyStatus(true, "Ready to Start Restore", null);
        }

        public GenericActivity StartRestore()
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

        public class ReadyStatus
        {
            public bool Ready { get; private set; }
            public string Description { get; private set; }
            public string[] Errors { get; private set; }

            public ReadyStatus(bool ready, string description, string[] errs)
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

        public class FileSystemRestoreOptions
        {
            public bool Applicable { get; private set; }
            public string FileOverwriteOption { get; private set; }
            public string RestoreMethod { get; private set; }
            public string RestorePermissions { get; private set; }
            public WinFSRestoreOptions WinFSOptions { get; private set; }

            public FileSystemRestoreOptions()
            {
                FileOverwriteOption = "RestoreAll";
                RestoreMethod = "Fast";
                RestorePermissions = "Yes";
            }

            public FileSystemRestoreOptions(bool applicable) : this()
            {
                Applicable = applicable;
            }

            public FileSystemRestoreOptions(bool applicable, bool winFSapplicable) : this(applicable)
            {
                WinFSOptions = new WinFSRestoreOptions(winFSapplicable);
            }

            public void SetOverwriteOption(string overwriteOption)
            {
                FileOverwriteOption = overwriteOption;
            }

            public void SetRestoreMethod(string restoreMethod)
            {
                RestoreMethod = restoreMethod;
            }

            public void SetRestorePermissions(string restorePermissions)
            {
                RestorePermissions = restorePermissions;
            }

            public override string ToString()
            {
                return Applicable.ToString();
            }
        }

        public class WinFSRestoreOptions
        {
            public bool Applicable { get; private set; }
            public bool AuthoritativeRestore { get; private set; }
            public bool OverwriteJunctionPoint { get; private set; }
            public bool SkipOfflineFiles { get; private set; }

            public WinFSRestoreOptions()
            {
                AuthoritativeRestore = false;
                OverwriteJunctionPoint = false;
                SkipOfflineFiles = true;
            }

            public WinFSRestoreOptions(bool applicable) : this()
            {
                Applicable = applicable;
            }

            public void SetAuthoritative(bool v)
            {
                AuthoritativeRestore = v;
            }

            public void SetOverwriteJunctionPoint(bool v)
            {
                OverwriteJunctionPoint = v;
            }

            public void SetSkipOfflineFile(bool v)
            {
                SkipOfflineFiles = v;
            }

            public override string ToString()
            {
                return Applicable.ToString();
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

            public RestoreDestination(int id, selected_shares selectedShares)
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

            public RestoreDestination(int id, selected_shares selectedShares, string destination)
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

            public share_mapping GetShareMapping()
            {
                return _shareMapping;
            }

            public void SetDestination(string destination)
            {
                _shareMapping.destination_path = destination;
                Destination = _shareMapping.destination_path;
            }

            public void SetTruncateLevel(int truncate)
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

    public class DSClientFSRestoreSession : DSClientRestoreSession
    {
        private FS_RestoreActivityInitiator _fsRestoreActivityInitiator;

        public DSClientFSRestoreSession(int restoreId,
            int backupSetId,
            string computer,
            Type setType,
            EBackupDataType dataType,
            DataSourceBrowser dataSourceBrowser,
            BackupSetRestoreView restoreView) : base (restoreId, backupSetId, computer, setType, dataType, dataSourceBrowser, restoreView)
        {
            
        }

        public override void SetSelectedItems()
        {
            if (_fsRestoreActivityInitiator != null)
                _fsRestoreActivityInitiator.Dispose();

            base.SetSelectedItems();

            _fsRestoreActivityInitiator = FS_RestoreActivityInitiator.from(GetRestoreActivityInitiator());

            _fsRestoreActivityInitiator.setFileOverwriteOption(StringToEnum<EFileOverwriteOption>(FileSystemOptions.FileOverwriteOption));
            _fsRestoreActivityInitiator.setRestoreMethod(StringToEnum<ERestoreMethod>(FileSystemOptions.RestoreMethod));
            _fsRestoreActivityInitiator.setRestorePermission(StringToEnum<ERestorePermission>(FileSystemOptions.RestorePermissions));

            UpdateReadyStatus();
        }
    }
}