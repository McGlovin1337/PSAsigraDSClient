using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public class DSClientRestoreSession
    {
        private BackupSetRestoreView _backupSetRestoreView;
        private RestoreActivityInitiator _restoreActivityInitiator;
        private readonly BackupSetCredentials _credentials;
        //private List<int> _destinationIds;
        private List<long> _selectedItemIds;
        //private readonly EBackupDataType _dataType;
        private readonly DataSourceBrowser _dataSourceBrowser;             
        private readonly List<DSClientBackupSetItemInfo> _browsedItems;
        private readonly Type _setType;
        private readonly Type _restoreOptionsType;

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
        public DSClientRestoreOption[] RestoreOptions { get; private set; }

        internal DSClientRestoreSession(int restoreId,
            int backupSetId,
            string computer,
            Type setType,
            EBackupDataType dataType,
            DataSourceBrowser dataSourceBrowser,
            BackupSetRestoreView restoreView)
        {
            _setType = setType;
            //_dataType = dataType;
            _backupSetRestoreView = restoreView;
            _restoreActivityInitiator = null;
            _dataSourceBrowser = dataSourceBrowser;
            _credentials = dataSourceBrowser.getCurrentCredentials();
            _browsedItems = new List<DSClientBackupSetItemInfo>();
            //_destinationIds = new List<int>();
            _selectedItemIds = new List<long>();

            RestoreId = restoreId;
            BackupSetId = backupSetId;
            DataType = EnumToString(dataType);
            Computer = computer;
            RestoreClassification = "Production";
            
            switch (dataType)
            {
                case EBackupDataType.EBackupDataType__FileSystem:
                    if (_setType == typeof(Win32FS_BackupSet))
                    {
                        _restoreOptionsType = typeof(RestoreOptions_Win32FileSystem);
                        RestoreOptions = new RestoreOptions_Win32FileSystem().GetRestoreOptions().ToArray();
                    }
                    else
                    {
                        _restoreOptionsType = typeof(RestoreOptions_FileSystem);
                        RestoreOptions = new RestoreOptions_FileSystem().GetRestoreOptions().ToArray();
                    }
                    //FileSystemOptions = (_setType == typeof(Win32FS_BackupSet)) ? new FileSystemRestoreOptions(true, true) : new FileSystemRestoreOptions(true, false);
                    break;
                default:
                    //FileSystemOptions = new FileSystemRestoreOptions(false);
                    break;
            }

            GetCredentials();
            UpdateReadyStatus();
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

            int readThreads = (int?)RestoreOptions.SingleOrDefault(v => v.Option == "DSSystemReadThreads").Value ?? 0;
            _restoreActivityInitiator.setDSSystemReadThreads(readThreads);

            int asyncIO = (int?)RestoreOptions.SingleOrDefault(v => v.Option == "MaxPendingAsyncIO").Value ?? 0;
            _restoreActivityInitiator.setMaxPendingAsyncIO(asyncIO);

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
        }

        internal void Dispose()
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

        internal Type GetRestoreOptionsType()
        {
            return _restoreOptionsType;
        }

        internal ref BackupSetRestoreView GetRestoreView()
        {
            return ref _backupSetRestoreView;
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

        internal void SetCredentials(PSCredential credential)
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

        internal void SetRestoreClassification(string classification)
        {
            if (_restoreActivityInitiator != null)
                _restoreActivityInitiator.setRestoreClassification(StringToEnum<ERestoreClassification>(classification));

            RestoreClassification = classification;
            UpdateReadyStatus();
        }

        internal void SetRestoreOptions(object restoreOptions)
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
            else
            {
                throw new Exception("Unsupported Option Type");
            }
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

                ApplyRestoreOptions();

                SelectedItems = new DSClientBackupSetItemInfo[items.Length];
                for (int i = 0; i < items.Length; i++)
                    SelectedItems[i] = _browsedItems.Single(item => item.ItemId == items[i]);

                selected_shares[] selectedShares = _restoreActivityInitiator.selected_shares();
                DestinationPaths = new RestoreDestination[selectedShares.Length];
                for (int i = 0; i < selectedShares.Length; i++)
                    DestinationPaths[i] = new RestoreDestination(i + 1, selectedShares[i]);
            }
            else
            {
                SelectedItems = null;
                DestinationPaths = null;
                _restoreActivityInitiator = null;
            }
        }

        internal void SetSudoCredentials(PSCredential credential)
        {
            if (_setType == typeof(UnixFS_Generic_BackupSet))
            {
                UnixFS_SSH_BackupSetCredentials creds = UnixFS_SSH_BackupSetCredentials.from(_credentials);
                creds.setSudoAs(credential.UserName, credential.GetNetworkCredential().Password);
            }
            else
                throw new Exception("This Restore Session does not support Sudo Credentials");
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

    public class RestoreOptions_Base
    {
        public bool UseDetailedLog { get; protected set; }
        public string LocalStorageMethod { get; protected set; }
        public int DSSystemReadThreads { get; protected set; }
        public int MaxPendingAsyncIO { get; protected set; }

        internal RestoreOptions_Base()
        {
            UseDetailedLog = false;
            LocalStorageMethod = "ContinueIfDisconnect";
            DSSystemReadThreads = 0;
            MaxPendingAsyncIO = 0;
        }

        internal DSClientRestoreOption[] GetRestoreOptions()
        {
            PropertyInfo[] props = this.GetType().GetProperties();
            DSClientRestoreOption[] restoreOptions = new DSClientRestoreOption[props.Length];
            for (int i = 0; i < props.Length; i++)
                restoreOptions[i] = new DSClientRestoreOption(this.GetType(), props[i].Name, props[i].GetValue(this, null));

            return restoreOptions;
        }

        internal void SetUseDetailedLog(bool v)
        {
            UseDetailedLog = v;
        }

        internal void SetLocalStorageMethod(string localStorageMethod)
        {
            LocalStorageMethod = localStorageMethod;
        }

        internal void SetDSSystemReadThreads(int readThreads)
        {
            DSSystemReadThreads = readThreads;
        }

        internal void SetMaxPendingIO(int maxIO)
        {
            MaxPendingAsyncIO = maxIO;
        }
    }

    public class RestoreOptions_FileSystem : RestoreOptions_Base
    {
        public string FileOverwriteOption { get; protected set; }
        public string RestoreMethod { get; protected set; }
        public string RestorePermissions { get; protected set; }

        internal RestoreOptions_FileSystem() : base()
        {
            FileOverwriteOption = "RestoreAll";
            RestoreMethod = "Fast";
            RestorePermissions = "Yes";
        }

        internal RestoreOptions_FileSystem(PSObject obj)
        {
            if (obj.ImmediateBaseObject.GetType() != typeof(RestoreOptions_FileSystem))
                throw new Exception("Invalid Object Type");

            PSMemberInfoCollection<PSPropertyInfo> objProps = obj.Properties;
            foreach (PSPropertyInfo objProp in objProps)
                foreach (PropertyInfo thisObj in this.GetType().GetProperties())
                    if (thisObj.Name == objProp.Name)
                        thisObj.SetValue(this, objProp.Value, null);
        }

        internal void SetOverwriteOption(string overwriteOption)
        {
            FileOverwriteOption = overwriteOption;
        }

        internal void SetRestoreMethod(string restoreMethod)
        {
            RestoreMethod = restoreMethod;
        }

        internal void SetRestorePermissions(string restorePermissions)
        {
            RestorePermissions = restorePermissions;
        }
    }

    public class RestoreOptions_Win32FileSystem : RestoreOptions_FileSystem
    {
        public bool AuthoritativeRestore { get; private set; }
        public bool OverwriteJunctionPoint { get; private set; }
        public bool SkipOfflineFiles { get; private set; }

        internal RestoreOptions_Win32FileSystem() : base()
        {
            AuthoritativeRestore = false;
            OverwriteJunctionPoint = false;
            SkipOfflineFiles = true;
        }

        internal RestoreOptions_Win32FileSystem(PSObject obj)
        {
            if (obj.ImmediateBaseObject.GetType() != typeof(RestoreOptions_Win32FileSystem))
                throw new Exception("Invalid Object Type");

            PSMemberInfoCollection<PSPropertyInfo> objProps = obj.Properties;
            foreach (PSPropertyInfo objProp in objProps)
                foreach (PropertyInfo thisObj in this.GetType().GetProperties())
                    if (thisObj.Name == objProp.Name)
                        thisObj.SetValue(this, objProp.Value, null);
        }

        internal static RestoreOptions_Win32FileSystem From(RestoreOptions_FileSystem restoreOptions_FileSystem)
        {
            RestoreOptions_Win32FileSystem restoreOptions = new RestoreOptions_Win32FileSystem();

            foreach (PropertyInfo parentProp in restoreOptions_FileSystem.GetType().GetProperties())
                foreach (PropertyInfo thisProp in restoreOptions.GetType().GetProperties())
                    if (thisProp.Name == parentProp.Name)
                        thisProp.SetValue(restoreOptions, parentProp.GetValue(restoreOptions_FileSystem), null);

            return restoreOptions;
        }

        internal void SetAuthoritative(bool v)
        {
            AuthoritativeRestore = v;
        }

        internal void SetOverwriteJunctionPoint(bool v)
        {
            OverwriteJunctionPoint = v;
        }

        internal void SetSkipOfflineFile(bool v)
        {
            SkipOfflineFiles = v;
        }
    }
}