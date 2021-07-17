using System;
using System.Linq;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientNotification;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSet: DSClientCmdlet
    {
        protected static void BaseBackupSetParamValidation(Dictionary<string, object> baseParams)
        {            
            // Method performs Parameter Validation for Base/Common Parameters
            baseParams.TryGetValue("RegexExcludeItem", out object value);
            if ((baseParams.ContainsKey("RegexExcludeDirectory") || baseParams.ContainsKey("RegexCaseInsensitive")) && value as string[] == null)
                throw new ParameterBindingException("RegexExcludeDirectory and RegexCaseInsensitive Parameters require RegexExcludeItem Parameter");

            if (baseParams.ContainsKey("ExcludeOldFilesByDate") && baseParams.ContainsKey("ExcludeOldFilesByTimeSpan"))
                throw new ParameterBindingException("ExcludeOldFilesByDate and ExcludeOldFilesByTimeSpan cannot both be specified");

            baseParams.TryGetValue("SetType", out object SetType);
            baseParams.TryGetValue("UseTransmissionCache", out object UseTransmissionCache);
            baseParams.TryGetValue("LocalStoragePath", out object LocalStoragePath);
            if ((SetType as string == "SelfContained" || SetType as string == "LocalOnly" || UseTransmissionCache as bool? == true) && LocalStoragePath as string == null)
                throw new ParameterBindingException("Local Backups and Transmission Cache require a Local Storage Path");
        }

        protected static BackupSet ProcessBaseBackupSetParams(Dictionary<string, object> baseParams, BackupSet backupSet)
        {
            baseParams.TryGetValue("Name", out object Name);
            if (Name != null)
                backupSet.setName(Name as string);

            baseParams.TryGetValue("SetType", out object SetType);
            if (SetType != null)
                backupSet.setSetType(StringToEnum<EBackupSetType>(SetType as string));

            baseParams.TryGetValue("Compression", out object Compression);
            if (Compression != null)
                backupSet.setCompressionType(StringToEnum<ECompressionType>(Compression as string));

            baseParams.TryGetValue("Disabled", out object Disabled);
            if (Disabled != null)
            {
                bool disabled = Convert.ToBoolean(Disabled.ToString());
                backupSet.setActive(!disabled);
            }

            baseParams.TryGetValue("LocalStoragePath", out object LocalStoragePath);
            if (LocalStoragePath != null)
                backupSet.setLocalStoragePath(LocalStoragePath as string);

            baseParams.TryGetValue("SchedulePriority", out object SchedulePriority);
            if (SchedulePriority != null)
                backupSet.setSchedulePriority((SchedulePriority as int?).GetValueOrDefault(1));

            baseParams.TryGetValue("ReadBufferSize", out object ReadBufferSize);
            if (ReadBufferSize != null)
                backupSet.setReadBufferSize((ReadBufferSize as int?).GetValueOrDefault(0));

            baseParams.TryGetValue("BackupErrorLimit", out object BackupErrorLimit);
            if (BackupErrorLimit != null)
                backupSet.setBackupErrorLimit((BackupErrorLimit as int?).GetValueOrDefault(0));

            baseParams.TryGetValue("ForceBackup", out object ForceBackup);
            if (ForceBackup != null)
                backupSet.setForceBackup(Convert.ToBoolean(ForceBackup.ToString()));

            baseParams.TryGetValue("PreScan", out object PreScan);
            if (PreScan != null)
                backupSet.setPreScanByDefault(Convert.ToBoolean(PreScan.ToString()));

            baseParams.TryGetValue("UseDetailedLog", out object UseDetailedLog);
            if (UseDetailedLog != null)
                backupSet.setUsingDetailedLog(Convert.ToBoolean(UseDetailedLog.ToString()));

            baseParams.TryGetValue("InfinateBLMGenerations", out object InfinateBLMGenerations);
            if (InfinateBLMGenerations != null)
                backupSet.setUsingInfBLMGen(Convert.ToBoolean(InfinateBLMGenerations.ToString()));

            baseParams.TryGetValue("UseLocalStorage", out object UseLocalStorage);
            if (UseLocalStorage != null)
                backupSet.setUsingLocalStorage(Convert.ToBoolean(UseLocalStorage.ToString()));

            baseParams.TryGetValue("UseTransmissionCache", out object UseTransmissionCache);
            if (UseTransmissionCache != null)
                backupSet.setUsingLocalTransmissionCache(Convert.ToBoolean(UseTransmissionCache.ToString()));

            baseParams.TryGetValue("NotificationMethod", out object NotificationMethod);
            if (NotificationMethod != null)
            {
                baseParams.TryGetValue("NotificationCompletion", out object NotificationCompletion);
                baseParams.TryGetValue("NotificationEmailOptions", out object NotificationEmailOptions);
                baseParams.TryGetValue("NotificationRecipient", out object NotificationRecipient);

                notification_info notificationInfo = new notification_info
                {
                    completion = ArrayToNotificationCompletionToInt(NotificationCompletion as string[]),
                    email_option = (NotificationEmailOptions != null) ? ArrayToEmailOptionsInt(NotificationEmailOptions as string[]) : 0,
                    id = 0,
                    method = StringToEnum<ENotificationMethod>(NotificationMethod as string),
                    recipient = NotificationRecipient as string
                };
                BackupSetNotification backupSetNotification = backupSet.getNotification();
                backupSetNotification.addOrUpdateNotification(notificationInfo);
                backupSetNotification.Dispose();
            }

            baseParams.TryGetValue("SnmpTrapNotifications", out object SnmpTrapNotifications);
            if (SnmpTrapNotifications != null)
                backupSet.setSNMPTrapsConditions(ArrayToNotificationCompletionToInt(SnmpTrapNotifications as string[]));

            return backupSet;
        }

        protected static IEnumerable<BackupSetFileItem> ProcessExclusionItems(string dSClientOSType, DataSourceBrowser dataSourceBrowser, string computer, IEnumerable<string> items, bool excludeSubDirs)
        {
            List<BackupSetFileItem> fileItems = new List<BackupSetFileItem>();

            foreach(string item in items)
            {
                // Trim any whitespace from the end of the item
                string trimmedItem = item.Trim();

                // Set the item filter by extracting the chars after the last "\" (windows) or "/" (linux/unix)
                string filter = "*";
                int itemLength = 0;

                if (dSClientOSType == "Windows")
                {
                    filter = trimmedItem.Split('\\').Last();
                    itemLength = filter.Length;
                    if (string.IsNullOrEmpty(filter))
                        filter = "*";
                }
                else if (dSClientOSType == "Linux")
                {
                    filter = trimmedItem.Split('/').Last();
                    filter = filter.Split('\\').Last();
                    itemLength = filter.Length;
                    if (string.IsNullOrEmpty(filter))
                        filter = "*";
                }

                // Set the path by removing the specified filter from the end of the item
                string path = trimmedItem.Remove((trimmedItem.Length - itemLength), itemLength);

                // Validate the path exists, otherwise an invalid path passed to createExclusionItem() can crash the DS-Client Service
                // getItemInfo() will throw an appropriate exception if the item doesn't exist
                dataSourceBrowser.getItemInfo(computer, path);

                BackupSetFileItem exclusion = dataSourceBrowser.createExclusionItem(computer, path);
                exclusion.setFilter(filter);
                fileItems.Add(exclusion);
                exclusion.setSubdirDescend(!excludeSubDirs);
            }

            return fileItems;
        }

        protected static IEnumerable<BackupSetFileItem> ProcessBasicExclusionItems(DataSourceBrowser dataSourceBrowser, string computer, IEnumerable<string> items, bool excludeSubDirs)
        {
            List<BackupSetFileItem> sqlItems = new List<BackupSetFileItem>();

            foreach (string item in items)
            {
                // Trim any whitespace from the end of the item
                string trimmedItem = item.Trim();

                BackupSetFileItem exclusion = dataSourceBrowser.createExclusionItem(computer, trimmedItem);
                exclusion.setSubdirDescend(!excludeSubDirs);
                sqlItems.Add(exclusion);
            }

            return sqlItems;
        }

        protected static IEnumerable<BackupSetRegexExclusion> ProcessRegexExclusionItems(DataSourceBrowser dataSourceBrowser, string computer, string exclusionPath, bool excludeDir, bool caseInsensitive, IEnumerable<string> items)
        {
            List<BackupSetRegexExclusion> regexItems = new List<BackupSetRegexExclusion>();

            foreach(string item in items)
            {
                BackupSetRegexExclusion regexExclusion = dataSourceBrowser.createRegexExclusion(computer, exclusionPath, item);

                if (excludeDir == true)
                    regexExclusion.setMatchDirectories(false);
                else
                    regexExclusion.setMatchDirectories(true);

                if (caseInsensitive == true)
                    regexExclusion.setCaseSensitive(false);
                else
                    regexExclusion.setCaseSensitive(true);

                regexItems.Add(regexExclusion);
            }

            return regexItems;
        }

        protected static IEnumerable<Win32FS_BackupSetInclusionItem> ProcessWin32FSInclusionItems(DataSourceBrowser dataSourceBrowser, string computer, IEnumerable<string> items, int maxGens, bool excludeStreams, bool excludePerms, bool excludeSubDirs)
        {
            List<Win32FS_BackupSetInclusionItem> inclusionItems = new List<Win32FS_BackupSetInclusionItem>();

            foreach (string item in items)
            {
                // Trim any whitespace from the end of the item
                string trimmedItem = item.Trim();

                // Set the item filter by extracting the chars after the last "\"
                string filter = trimmedItem.Split('\\').Last();
                int itemLength = filter.Length;
                if (string.IsNullOrEmpty(filter))
                    filter = "*";

                // Set the path by removing the specified filter from the end of the item
                string path = trimmedItem.Remove((trimmedItem.Length - itemLength), itemLength);

                Win32FS_BackupSetInclusionItem inclusionItem = Win32FS_BackupSetInclusionItem.from(dataSourceBrowser.createInclusionItem(computer, path, maxGens));

                inclusionItem.setFilter(filter);
                inclusionItem.setSubdirDescend(!excludeSubDirs);
                inclusionItem.setIncludingAlternateDataStreams(!excludeStreams);
                inclusionItem.setIncludingPermissions(!excludePerms);

                inclusionItems.Add(inclusionItem);
            }

            return inclusionItems;
        }

        protected static IEnumerable<MSSQL_BackupSetInclusionItem> ProcessMsSqlInclusionItems(DataSourceBrowser dataSourceBrowser, string computer, IEnumerable<string> items, int maxGens, bool logBackup, bool runDBCC, bool stopDBCC, bool excludeSubDirs)
        {
            List<MSSQL_BackupSetInclusionItem> sqlInclusionItems = new List<MSSQL_BackupSetInclusionItem>();

            foreach (string item in items)
            {
                // Trim any whitespace from the end of the item
                string trimmedItem = item.Trim();

                // Set the item filter by extracting the chars after the last "\"
                string filter = trimmedItem.Split('\\').Last();
                int itemLength = filter.Length;
                if (string.IsNullOrEmpty(filter))
                    filter = "*";

                MSSQL_BackupSetInclusionItem inclusionItem = MSSQL_BackupSetInclusionItem.from(dataSourceBrowser.createInclusionItem(computer, item, maxGens));

                inclusionItem.setFilter(filter);
                inclusionItem.setSubdirDescend(!excludeSubDirs);
                inclusionItem.setBackUpTransactionLog(logBackup);
                inclusionItem.setRunDBCC(runDBCC);
                inclusionItem.setStopOnDBCCErrors(stopDBCC);

                sqlInclusionItems.Add(inclusionItem);
            }

            return sqlInclusionItems;
        }

        protected static IEnumerable<BackupSetInclusionItem> ProcessVMwareVADPInclusionItem(DataSourceBrowser dataSourceBrowser, string computer, IEnumerable<string> items, int maxGens, bool excludeSubDirs)
        {
            List<BackupSetInclusionItem> inclusionItems = new List<BackupSetInclusionItem>();

            foreach (string item in items)
            {
                // Trim any whitespace from the end of the item
                string trimmedItem = item.Trim();

                // Set the item filter by extracting the chars after the last "\"
                string filter = trimmedItem.Split('\\').Last();
                int itemLength = filter.Length;
                if (string.IsNullOrEmpty(filter))
                    filter = "*";

                // Set the path by removing the specified filter from the end of the item
                string path = trimmedItem.Remove((trimmedItem.Length - itemLength), itemLength);

                BackupSetInclusionItem inclusionItem = dataSourceBrowser.createInclusionItem(computer, path, maxGens);
                
                inclusionItem.setFilter(filter);
                inclusionItem.setSubdirDescend(!excludeSubDirs);

                inclusionItems.Add(inclusionItem);
            }

            return inclusionItems;
        }

        protected class BackupSetItemComparer : IEqualityComparer<BackupSetItem>
        {
            public bool Equals(BackupSetItem itemA, BackupSetItem itemB)
            {
                EBackupSetItemType itemTypeA = itemA.getType();
                EBackupSetItemType itemTypeB = itemB.getType();

                string itemFolderA = itemA.getFolder();
                string itemFolderB = itemB.getFolder();

                if (itemTypeA != itemTypeB || itemFolderA != itemFolderB)
                    return false;
                else if (itemTypeA == EBackupSetItemType.EBackupSetItemType__RegExExclusion && itemTypeB == EBackupSetItemType.EBackupSetItemType__RegExExclusion)
                {
                    BackupSetRegexExclusion regexItemA = BackupSetRegexExclusion.from(itemA);
                    BackupSetRegexExclusion regexItemB = BackupSetRegexExclusion.from(itemB);

                    return regexItemA.getExpression() == regexItemB.getExpression();
                }
                else
                {
                    BackupSetFileItem fileItemA = BackupSetFileItem.from(itemA);
                    BackupSetFileItem fileItemB = BackupSetFileItem.from(itemB);

                    return fileItemA.getFilter() == fileItemB.getFilter();
                }
            }

            public int GetHashCode(BackupSetItem backupSetItem)
            {
                if (backupSetItem.getType() == EBackupSetItemType.EBackupSetItemType__RegExExclusion)
                {
                    BackupSetRegexExclusion regexItem = BackupSetRegexExclusion.from(backupSetItem);
                    return regexItem.getExpression().GetHashCode();
                }

                BackupSetFileItem fileItem = BackupSetFileItem.from(backupSetItem);
                return fileItem.getFilter().GetHashCode();
            }
        }

        protected class DSClientBackupSetBasicProps
        {
            public int BackupSetId { get; private set; }
            public string Computer { get; private set; }
            public string Name { get; private set; }
            public bool Enabled { get; private set; }

            public DSClientBackupSetBasicProps(BackupSet backupSet)
            {
                BackupSetId = backupSet.getID();
                Computer = backupSet.getComputerName();
                Name = backupSet.getName();
                Enabled = backupSet.isActive();
            }
        }

        protected class DSClientBackupSet
        {
            public int BackupSetId { get; private set; }
            public string Computer { get; private set; }
            public string Name { get; private set; }
            public bool Enabled { get; private set; }
            public DateTime LastSuccess { get; private set; }
            public dynamic DataType { get; private set; }
            public DSClientBackupSetItem[] BackupItems { get; private set; }
            public bool Synchronized { get; private set; }
            public DateTime LastSynchronized { get; private set; }
            public int ScheduleId { get; private set; }
            public string ScheduleName { get; private set; }
            public int SchedulePriority { get; private set; }
            public int RetentionRuleId { get; private set; }
            public string RetentionRuleName { get; private set; }
            public long OnlineDataSize { get; private set; }
            public int OnlineFileCount { get; private set; }
            public string CompressionType { get; private set; }
            public long CompressedSize { get; private set; }
            public long LocalStorageDataSize { get; private set; }
            public int LocalStorageFileCount { get; private set; }
            public string SetType { get; private set; }
            public bool UseLocalStorage { get; private set; }
            public bool ForceBackup { get; private set; }
            public int ErrorLimit { get; private set; }
            public int MaxPendingAsyncIO { get; private set; }
            public bool PreScan { get; private set; }
            public bool CreatedByPolicy { get; private set; }
            public DSClientBackupSetNotification[] Notification { get; private set; }
            public string[] SnmpTrapNotification { get; private set; }
            public DSClientPrePost[] PrePost { get; private set; }
            public int ReadBufferSize { get; private set; }
            public bool UseTransmissionCache { get; private set; }
            public bool DetailedLog { get; private set; }
            public bool InfinateBLMGenerations { get; private set; }
            public ShareInfo[] ShareInfo { get; private set; }
            public int OwnerId { get; private set; }
            public string OwnerName { get; private set; }

            public DSClientBackupSet(BackupSet backupSet, string dSClientOSType)
            {
                backup_set_overview_info backupSetOverviewInfo = backupSet.getOverview();

                // Get Backup Set Notification Info
                BackupSetNotification backupSetNotification = backupSet.getNotification();
                notification_info[] notificationInfo = backupSetNotification.listNotification();
                List<DSClientBackupSetNotification> dSClientBackupSetNotifications = new List<DSClientBackupSetNotification>();
                foreach (var notification in notificationInfo)
                    dSClientBackupSetNotifications.Add(new DSClientBackupSetNotification(notification));
                backupSetNotification.Dispose();

                // Get Backup Set Pre & Post Configuration
                PrePost prePost = backupSet.getPrePost();
                pre_post_info[] prePostInfo = prePost.listPrePost();
                List<DSClientPrePost> dSClientPrePosts = new List<DSClientPrePost>();
                foreach (var prepost in prePostInfo)
                    dSClientPrePosts.Add(new DSClientPrePost(prepost));
                prePost.Dispose();

                // Get Backup Set Share Information
                shares_info[] sharesInfo = backupSet.getSharesInfo();
                List<ShareInfo> shareInfo = new List<ShareInfo>();
                foreach (var share in sharesInfo)
                    shareInfo.Add(new ShareInfo(share));

                // Get the Backup Set Items
                BackupSetItem[] backupSetItems = backupSet.items();
                List<DSClientBackupSetItem> setItems = new List<DSClientBackupSetItem>();
                foreach (var item in backupSetItems)
                    setItems.Add(new DSClientBackupSetItem(item, backupSetOverviewInfo.data_type, dSClientOSType));

                // Set the DataType dynamic property based on the Backup Set Data type
                if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__FileSystem)
                {
                    if (dSClientOSType == "Windows")
                        DataType = new Win32FSBackupSet(backupSet);

                    if (dSClientOSType == "Linux")
                        DataType = new UnixFSBackupSet(backupSet);
                }
                else if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__SQLServer)
                    DataType = new MSSQLBackupSet(backupSet);
                else if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__VSSSQLServer)
                    DataType = new VSSMSSQLBackupSet(backupSet);
                else if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__VSSExchange)
                    DataType = new VSSExchangeBackupSet(backupSet);
                else if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__VMwareVADP)
                    /* There is a bug in the API where VADP Sets on Linux DS-Clients cannot be Cast to a
                     * VMwareVADP_BackupSet, therefore we only fetch the additional VMware Options
                     * for Sets created on Windows DS-Clients
                     */
                    if (dSClientOSType == "Windows")
                    {
                        DataType = new VMWareVADPBackupSet(backupSet);
                    }
                    else
                    {
                        DataType = "VMwareVADP";
                    }
                else if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__DB2)
                    DataType = new DB2BackupSet(backupSet);
                else
                    DataType = EBackupDataTypeToString(backupSetOverviewInfo.data_type);

                BackupSetId = backupSet.getID();
                Computer = backupSet.getComputerName();
                Name = backupSet.getName();
                Enabled = backupSet.isActive();
                LastSuccess = UnixEpochToDateTime(backupSetOverviewInfo.status.last_successful_backup);
                BackupItems = setItems.ToArray();
                Synchronized = backupSet.isInSync();
                LastSynchronized = UnixEpochToDateTime(backupSetOverviewInfo.status.last_sync_time);
                ScheduleId = backupSetOverviewInfo.schedule_id;
                ScheduleName = backupSetOverviewInfo.schedule_name;
                SchedulePriority = backupSet.getSchedulePriority();
                RetentionRuleId = backupSetOverviewInfo.retention_rule_id;
                RetentionRuleName = backupSetOverviewInfo.retention_rule_name;
                OnlineDataSize = backupSetOverviewInfo.status.on_line_data_size;
                OnlineFileCount = backupSetOverviewInfo.status.on_line_file_count;
                CompressionType = EnumToString(backupSet.getCompressionType());
                CompressedSize = backupSetOverviewInfo.status.dssystem_compressed_size;
                LocalStorageDataSize = backupSetOverviewInfo.status.local_storage_data_size;
                LocalStorageFileCount = backupSetOverviewInfo.status.local_storage_file_count;
                SetType = EnumToString(backupSet.getSetType());
                UseLocalStorage = backupSet.isUsingLocalStorage();
                ForceBackup = backupSet.isForceBackup();
                ErrorLimit = backupSet.getBackupErrorLimit();
                if (dSClientOSType == "Windows")
                    MaxPendingAsyncIO = backupSet.getMaxPendingAsyncIO();
                PreScan = backupSet.getPreScanByDefault();
                CreatedByPolicy = backupSet.isCreatedByBackupPolicy();
                Notification = dSClientBackupSetNotifications.ToArray();
                SnmpTrapNotification = IntEBackupCompletionToArray(backupSet.getSNMPTrapsConditions());
                PrePost = dSClientPrePosts.ToArray();
                if (dSClientOSType == "Windows")
                    ReadBufferSize = backupSet.getReadBufferSize();
                UseTransmissionCache = backupSet.isUsingLocalTransmissionCache();
                DetailedLog = backupSet.isUsingDetailedLog();
                InfinateBLMGenerations = backupSet.isUsingInfBLMGen();
                ShareInfo = shareInfo.ToArray();
                OwnerId = backupSet.getOwnerID();
                OwnerName = backupSet.getOwner();

                backupSet.Dispose();
            }
        }

        protected class DB2BackupSet
        {
            public bool UseBuffer { get; private set; }
            public string ArchiveLogPath { get; private set; }
            public string DumpPath { get; private set; }
            public bool OnlineBackup { get; private set; }
            public bool PruneHistoryOrLogFile { get; private set; }

            public DB2BackupSet(BackupSet backupSet)
            {
                DB2_BackupSet db2BackupSet = DB2_BackupSet.from(backupSet);

                UseBuffer = db2BackupSet.isUsingBuffer();
                ArchiveLogPath = db2BackupSet.getArchiveLogPath();
                DumpPath = db2BackupSet.getDumpPath();
                OnlineBackup = db2BackupSet.isOnlineBackup();
                PruneHistoryOrLogFile = db2BackupSet.isPruningHistoryOrLogfile();
            }

            public override string ToString()
            {
                return "DB2";
            }
        }

        protected class VMWareVADPBackupSet
        {
            public bool UseBuffer { get; private set; }
            public bool IncrementalP2VBackup { get; private set; }
            public bool BackupVMMemory { get; private set; }
            public bool SnapshotQuiesce { get; private set; }
            public bool SameTimeSnapshot { get; private set; }
            public bool UseCBT { get; private set; }
            public bool UseFLR { get; private set; }
            public bool UseLocalVDR { get; private set; }
            public string VMLibraryVersion { get; private set; }

            public VMWareVADPBackupSet(BackupSet backupSet)
            {
                VMwareVADP_BackupSet vmwareVADPBackupSet = VMwareVADP_BackupSet.from(backupSet);
                vmware_additional_options additionalOptions = vmwareVADPBackupSet.getAdditionalVMwareOptions();

                UseBuffer = vmwareVADPBackupSet.isUsingBuffer();
                IncrementalP2VBackup = additionalOptions.valBackupP2VIncrementally;
                BackupVMMemory = additionalOptions.valBackupVMmemory;
                SnapshotQuiesce = additionalOptions.valQuiesceIOBeforeSnap;
                SameTimeSnapshot = additionalOptions.valSnapAllVMSameTime;
                UseCBT = additionalOptions.valUsingCBT;
                UseFLR = additionalOptions.valUsingFLR;
                UseLocalVDR = additionalOptions.valUsingLocalVDR;
                VMLibraryVersion = EVMwareLibraryVersionToString(additionalOptions.valVMLibVersion);
            }

            public override string ToString()
            {
                return "VMWareVADP";
            }

            private string EVMwareLibraryVersionToString(EVMwareLibraryVersion libraryVersion)
            {
                switch(libraryVersion)
                {
                    case EVMwareLibraryVersion.EVMwareLibraryVersion__Latest:
                        return "Latest";
                    case EVMwareLibraryVersion.EVMwareLibraryVersion__VDDK5_5:
                        return "VDDK5.5";
                    case EVMwareLibraryVersion.EVMwareLibraryVersion__VDDK6_0:
                        return "VDDK6.0";
                    case EVMwareLibraryVersion.EVMwareLibraryVersion__VDDK6_5:
                        return "VDDK6.5";
                    default:
                        return null;
                }
            }
        }

        protected class IncrementalPolicy
        {
            public string BackupPolicy { get; set; }
            public ForceFullDayTime ForceFullMonthly { get; set; }
            public ForceFullDayTime ForceFullWeekly { get; set; }
            public ForceFullPeriod ForceFullPeriodically { get; set; }
            public SkipFullWeekDays SkipFullWeekDays { get; set; }

            public IncrementalPolicy()
            {
                // Empty default constructor
            }

            public IncrementalPolicy(incremental_policies policies)
            {
                BackupPolicy = EBackupPolicyToString(policies.backup_policy);
                ForceFullMonthly = new ForceFullDayTime(policies.is_force_full_monthly, policies.force_full_monthly_day, policies.force_full_monthly_time);
                ForceFullWeekly = new ForceFullDayTime(policies.is_force_full_weekly, policies.force_full_weekly_day, policies.force_full_weekly_time);
                ForceFullPeriodically = new ForceFullPeriod(policies.is_force_full_periodically, policies.unit_type, policies.unit_value);
                SkipFullWeekDays = new SkipFullWeekDays(policies.is_skip_full_on_weekdays, policies.skip_full_on_weekdays, policies.skip_full_on_weekdays_from, policies.skip_full_on_weekdays_to);
            }
        }

        protected class VSSExchangeBackupSet: IncrementalPolicy
        {
            public VSSExchangeBackupSet(BackupSet backupSet)
            {
                VSSEXCH_BackupSet vssEXCHBackupSet = VSSEXCH_BackupSet.from(backupSet);
                incremental_policies incrementalPolicies = vssEXCHBackupSet.getIncrementalPolicies();

                BackupPolicy = EBackupPolicyToString(incrementalPolicies.backup_policy);
                ForceFullMonthly = new ForceFullDayTime(incrementalPolicies.is_force_full_monthly, incrementalPolicies.force_full_monthly_day, incrementalPolicies.force_full_monthly_time);
                ForceFullWeekly = new ForceFullDayTime(incrementalPolicies.is_force_full_weekly, incrementalPolicies.force_full_weekly_day, incrementalPolicies.force_full_weekly_time);
                ForceFullPeriodically = new ForceFullPeriod(incrementalPolicies.is_force_full_periodically, incrementalPolicies.unit_type, incrementalPolicies.unit_value);
                SkipFullWeekDays = new SkipFullWeekDays(incrementalPolicies.is_skip_full_on_weekdays, incrementalPolicies.skip_full_on_weekdays, incrementalPolicies.skip_full_on_weekdays_from, incrementalPolicies.skip_full_on_weekdays_to);
            }

            public override string ToString()
            {
                return "VSSMSExchange";
            }
        }

        protected class VSSMSSQLBackupSet: IncrementalPolicy
        {
            public VSSMSSQLBackupSet(BackupSet backupSet)
            {
                VSSMSSQL_BackupSet vssMSSQLBackupSet = VSSMSSQL_BackupSet.from(backupSet);
                incremental_policies incrementalPolicies = vssMSSQLBackupSet.getIncrementalPolicies();

                BackupPolicy = EBackupPolicyToString(incrementalPolicies.backup_policy);
                ForceFullMonthly = new ForceFullDayTime(incrementalPolicies.is_force_full_monthly, incrementalPolicies.force_full_monthly_day, incrementalPolicies.force_full_monthly_time);
                ForceFullWeekly = new ForceFullDayTime(incrementalPolicies.is_force_full_weekly, incrementalPolicies.force_full_weekly_day, incrementalPolicies.force_full_weekly_time);
                ForceFullPeriodically = new ForceFullPeriod(incrementalPolicies.is_force_full_periodically, incrementalPolicies.unit_type, incrementalPolicies.unit_value);
                SkipFullWeekDays = new SkipFullWeekDays(incrementalPolicies.is_skip_full_on_weekdays, incrementalPolicies.skip_full_on_weekdays, incrementalPolicies.skip_full_on_weekdays_from, incrementalPolicies.skip_full_on_weekdays_to);
            }

            public override string ToString()
            {
                return "VSSMSSQLServer";
            }
        }

        protected class MSSQLBackupSet: IncrementalPolicy
        {
            public string DumpMethod { get; private set; }
            public string DumpPath { get; private set; }

            public MSSQLBackupSet(BackupSet backupSet)
            {
                MSSQL_BackupSet mssqlBackupSet = MSSQL_BackupSet.from(backupSet);
                mssql_dump_parameters dumpParameters = mssqlBackupSet.getDumpParameters();
                incremental_policies incrementalPolicies = mssqlBackupSet.getIncrementalPolicies();

                DumpMethod = EnumToString(dumpParameters.dump_method);
                DumpPath = dumpParameters.path;
                BackupPolicy = EBackupPolicyToString(incrementalPolicies.backup_policy);
                ForceFullMonthly = new ForceFullDayTime(incrementalPolicies.is_force_full_monthly, incrementalPolicies.force_full_monthly_day, incrementalPolicies.force_full_monthly_time);
                ForceFullWeekly = new ForceFullDayTime(incrementalPolicies.is_force_full_weekly, incrementalPolicies.force_full_weekly_day, incrementalPolicies.force_full_weekly_time);
                ForceFullPeriodically = new ForceFullPeriod(incrementalPolicies.is_force_full_periodically, incrementalPolicies.unit_type, incrementalPolicies.unit_value);
                SkipFullWeekDays = new SkipFullWeekDays(incrementalPolicies.is_skip_full_on_weekdays, incrementalPolicies.skip_full_on_weekdays, incrementalPolicies.skip_full_on_weekdays_from, incrementalPolicies.skip_full_on_weekdays_to);
            }

            public override string ToString()
            {
                return "MSSqlServer";
            }            
        }

        protected class UnixFSBackupSet
        {
            public bool IsCDP { get; private set; }
            public DSClientCDPSettings CDPSettings { get; private set; }
            public OldFileExclusionConfig OldFileExclusionOptions { get; private set; }
            public bool CheckCommonFiles { get; private set; }
            public bool UseBuffer { get; private set; }
            public bool ForceBackup { get; private set; }
            public bool FollowMountPoints { get; private set; }
            public bool BackupHardLinks { get; private set; }
            public bool IgnoreSnapshotFailures { get; private set; }
            public bool UseSnapDiff { get; private set; }

            public UnixFSBackupSet(BackupSet backupSet)
            {
                UnixFS_Generic_BackupSet unixFSBackupSet = UnixFS_Generic_BackupSet.from(backupSet);

                IsCDP = unixFSBackupSet.isContinuousDataProtection();
                CDPSettings = new DSClientCDPSettings(unixFSBackupSet.getCDPSettings());
                OldFileExclusionOptions = new OldFileExclusionConfig(unixFSBackupSet.getOldFileExclusionOption());
                CheckCommonFiles = unixFSBackupSet.isCheckingCommonFiles();
                UseBuffer = unixFSBackupSet.isUsingBuffer();
                ForceBackup = unixFSBackupSet.isOption(EBackupSetOption.EBackupSetOption__ForceBackup);
                FollowMountPoints = unixFSBackupSet.isOption(EBackupSetOption.EBackupSetOption__SSHFollowLink);
                BackupHardLinks = unixFSBackupSet.isOption(EBackupSetOption.EBackupSetOption__HardLink);
                IgnoreSnapshotFailures = unixFSBackupSet.isOption(EBackupSetOption.EBackupSetOption__SnapshotFailure);
                UseSnapDiff = unixFSBackupSet.isOption(EBackupSetOption.EBackupSetOption__UseSnapDiff);
            }

            public override string ToString()
            {
                return "FileSystem";
            }
        }

        protected class Win32FSBackupSet
        {
            public bool BackupRemoteStorage { get; private set; }
            public bool BackupSingleInstanceStore { get; private set; }
            public bool IsCDP { get; private set; }
            public DSClientCDPSettings CDPSettings { get; private set; }
            public bool UseVSS { get; private set; }
            public VSSInfo[] VSSWriters { get; private set; }
            public bool ExcludeVSSComponents { get; private set; }
            public VSSInfo[] VSSComponentExclusions { get; private set; }
            public bool IgnoreVSSComponents { get; private set; }
            public bool IgnoreVSSWriters { get; private set; }
            public bool FollowJunctionPoint { get; private set; }
            public bool IgnoreAutoFileFilters { get; private set; }
            public OldFileExclusionConfig OldFileExclusionOptions { get; private set; }
            public bool CheckCommonFiles { get; private set; }
            public bool UseBuffer { get; private set; }

            public Win32FSBackupSet(BackupSet backupSet)
            {
                Win32FS_BackupSet win32FSBackupSet = Win32FS_BackupSet.from(backupSet);

                bool useVss = win32FSBackupSet.getUseVSS();
                bool excludeVss = win32FSBackupSet.getExcludeVSSComponents();

                List<VSSInfo> VssWriters = new List<VSSInfo>();
                if (useVss)
                {
                    try
                    {
                        vss_exclusion_info[] vssWriters = win32FSBackupSet.getVSSWriters();
                        foreach (var writer in vssWriters)
                            VssWriters.Add(new VSSInfo(writer));
                    }
                    catch
                    {
                        // Do nothing, just continue
                    }
                }

                List<VSSInfo> VssExclusions = new List<VSSInfo>();
                if (excludeVss)
                {
                    vss_exclusion_info[] vssExclusions = win32FSBackupSet.getVSSComponentExclusions();
                    foreach (var exclusion in vssExclusions)
                        VssExclusions.Add(new VSSInfo(exclusion));
                }

                BackupRemoteStorage = win32FSBackupSet.getBackupRemoteStorage();
                BackupSingleInstanceStore = win32FSBackupSet.getBackupSingleInstanceStore();
                IsCDP = win32FSBackupSet.isContinuousDataProtection();
                CDPSettings = new DSClientCDPSettings(win32FSBackupSet.getCDPSettings());
                UseVSS = useVss;
                VSSWriters = VssWriters.ToArray();
                ExcludeVSSComponents = excludeVss;
                VSSComponentExclusions = VssExclusions.ToArray();
                IgnoreVSSComponents = win32FSBackupSet.getIgnoreVSSComponents();
                IgnoreVSSWriters = win32FSBackupSet.getIgnoreVSSWriters();
                FollowJunctionPoint = win32FSBackupSet.getFollowJunctionPoint();
                IgnoreAutoFileFilters = win32FSBackupSet.getNoAutomaticFileFilter();
                OldFileExclusionOptions = new OldFileExclusionConfig(win32FSBackupSet.getOldFileExclusionOption());
                CheckCommonFiles = win32FSBackupSet.isCheckingCommonFiles();
                UseBuffer = win32FSBackupSet.isUsingBuffer();

                win32FSBackupSet.Dispose();
            }

            public override string ToString()
            {
                return "FileSystem";
            }
        }

        protected class DSClientBackupSetItem
        {
            public string Type { get; private set; }
            public string Folder { get; private set; }
            public string[] ItemOption { get; private set; }
            public string Filter { get; private set; }
            public bool SubDirDescend { get; private set; }
            public string ItemType { get; private set; }
            public int MaxGenerations { get; private set; }
            public BackupSetInclusionOptions InclusionOptions { get; private set; }
            public RegexItemExclusionOptions RegexExclusionOptions { get; private set; } 

            public DSClientBackupSetItem(BackupSetItem backupSetItem, EBackupDataType backupDataType, string dSClientOSType)
            {
                EItemOption[] itemOptions = backupSetItem.getItemOptions();
                List<string> ItemOptions = new List<string>();
                foreach (var item in itemOptions)
                    ItemOptions.Add(EItemOptionToString(item));

                EBackupSetItemType itemType = backupSetItem.getType();

                Type = EnumToString(itemType);
                Folder = backupSetItem.getFolder();
                ItemOption = ItemOptions.ToArray();

                if (itemType == EBackupSetItemType.EBackupSetItemType__Inclusion || itemType == EBackupSetItemType.EBackupSetItemType__Exclusion)
                {
                    BackupSetFileItem backupSetFileItem = BackupSetFileItem.from(backupSetItem);
                    Filter = backupSetFileItem.getFilter();
                    SubDirDescend = backupSetFileItem.getSubdirDescend();

                    if (itemType == EBackupSetItemType.EBackupSetItemType__Inclusion)
                    {
                        BackupSetInclusionItem backupSetInclusionItem = BackupSetInclusionItem.from(backupSetFileItem);
                        ItemType = EBrowseItemTypeToString(backupSetInclusionItem.getDataType());
                        MaxGenerations = backupSetInclusionItem.getGenerationCount();

                        if (backupDataType == EBackupDataType.EBackupDataType__FileSystem)
                        {
                            if (dSClientOSType == "Windows")
                            {
                                Win32FS_BackupSetInclusionItem win32FSBackupSetInclusionItem = Win32FS_BackupSetInclusionItem.from(backupSetInclusionItem);
                                InclusionOptions = new BackupSetInclusionOptions(win32FSBackupSetInclusionItem);
                            }
                            else if (dSClientOSType == "Linux")
                            {
                                UnixFS_BackupSetInclusionItem unixFSBackupSetInclusionItem = UnixFS_BackupSetInclusionItem.from(backupSetInclusionItem);
                                InclusionOptions = new BackupSetInclusionOptions(unixFSBackupSetInclusionItem);
                            }
                        }
                    }
                }

                if (itemType == EBackupSetItemType.EBackupSetItemType__RegExExclusion)
                {
                    BackupSetRegexExclusion backupSetRegexExclusion = BackupSetRegexExclusion.from(backupSetItem);
                    RegexExclusionOptions = new RegexItemExclusionOptions(backupSetRegexExclusion);
                }
            }

            public override string ToString()
            {
                return ItemType;
            }

            private string EBrowseItemTypeToString(EBrowseItemType itemType)
            {
                switch(itemType)
                {
                    case EBrowseItemType.EBrowseItemType__Drive:
                        return "Drive";
                    case EBrowseItemType.EBrowseItemType__Share:
                        return "Share";
                    case EBrowseItemType.EBrowseItemType__Directory:
                        return "Directory";
                    case EBrowseItemType.EBrowseItemType__File:
                        return "File";
                    case EBrowseItemType.EBrowseItemType__SystemState:
                        return "SystemState";
                    case EBrowseItemType.EBrowseItemType__ServicesDB:
                        return "ServicesDatabase";
                    case EBrowseItemType.EBrowseItemType__DatabaseInstance:
                        return "DatabaseInstance";
                    case EBrowseItemType.EBrowseItemType__Database:
                        return "Database";
                    case EBrowseItemType.EBrowseItemType__Tablespace:
                        return "OracleTablespace";
                    case EBrowseItemType.EBrowseItemType__ControlFile:
                        return "OracleControlFile";
                    case EBrowseItemType.EBrowseItemType__ArchiveLog:
                        return "OracleArchiveLog";
                    case EBrowseItemType.EBrowseItemType__VirtualMachine:
                        return "VirtualMachine";
                    case EBrowseItemType.EBrowseItemType__VssExchange:
                        return "VSSExchange";
                    case EBrowseItemType.EBrowseItemType__VmDisk:
                        return "VirtualMachineDisk";
                    default:
                        return null;
                }
            }

            private string EItemOptionToString(EItemOption itemOption)
            {
                switch(itemOption)
                {
                    case EItemOption.EItemOption__UnixFSBackupACL:
                        return "UnixFSBackupACL";
                    case EItemOption.EItemOption__UnixFSBackupPOSIX_ACL:
                        return "UnixFSBackupPosixACL";
                    case EItemOption.EItemOption__UnixFSBackupSElinux:
                        return "UnixFSBackupSELinux";
                    case EItemOption.EItemOption__WinFSBackupPermissions:
                        return "WinFSBackupPermissions";
                    case EItemOption.EItemOption__WinFSBackupStreams:
                        return "WinFSBackupStreams";
                    case EItemOption.EItemOption__WinExchangeDoNotTruncateTransactionLog:
                        return "DoNotTruncateExchangeLog";
                    case EItemOption.EItemOption__WinSQLRunDBCCBeforeBackup:
                        return "SqlServerRunDBCC";
                    case EItemOption.EItemOption__WinSQLBackupTransactionLog:
                        return "SqlServerBackupTransactionLog";
                    case EItemOption.EItemOption__WinSQLStopOnDBCCError:
                        return "SqlServerStopOnDBCCError";
                    case EItemOption.EItemOption__VSSExchangeBackupPasiveDatabase:
                        return "VSSExchangeBackupPassiveDatabase";
                    case EItemOption.EItemOption__VSSExchangeBackupActiveDatabase:
                        return "VSSExchangeBackupActiveDatabase";
                    case EItemOption.EItemOption__MacFSFileAttributes:
                        return "MacFSFileAttributes";
                    case EItemOption.EItemOption__MacFSResourceFork:
                        return "MacFSResourceFork";
                    case EItemOption.EItemOption__NovellBackupPermissions:
                        return "NovellBackupPermissions";
                    case EItemOption.EItemOption__NovellBackupAttributes:
                        return "NovellBackupAttributes";
                    default:
                        return null;
                }
            }            
        }

        protected class BackupSetInclusionOptions
        {
            private readonly bool _isUnixItem;

            public bool IncludeACLs { get; private set; }
            public bool IncludePosixACLs { get; private set; }
            public bool IncludeAltDatastreams { get; private set; }
            public bool IncludePermissions { get; private set; }

            public BackupSetInclusionOptions(UnixFS_BackupSetInclusionItem unixItem)
            {
                _isUnixItem = true;

                IncludeACLs = unixItem.isIncludingACL();

                try
                {
                    UnixFS_LinuxLFS_BackupSetInclusionItem lfsUnixItem = UnixFS_LinuxLFS_BackupSetInclusionItem.from(unixItem);

                    IncludePosixACLs = lfsUnixItem.isIncludingPosixACL();
                }
                catch
                {
                    IncludePosixACLs = false;
                }
            }

            public BackupSetInclusionOptions(Win32FS_BackupSetInclusionItem win32Item)
            {
                _isUnixItem = false;

                IncludeAltDatastreams = win32Item.isIncludingAlternateDataStreams();
                IncludePermissions = win32Item.isIncludingPermissions();
            }

            public override string ToString()
            {
                string options = null;

                if (_isUnixItem)
                {
                    if (IncludeACLs)
                        options += (options == null) ? nameof(IncludeACLs) : $", {nameof(IncludeACLs)}";

                    if (IncludePosixACLs)
                        options += (options == null) ? nameof(IncludePosixACLs) : $", {nameof(IncludePosixACLs)}";
                }
                else
                {
                    if (IncludeAltDatastreams)
                        options += (options == null) ? nameof(IncludeAltDatastreams) : $", {nameof(IncludeAltDatastreams)}";

                    if (IncludePermissions)
                        options += (options == null) ? nameof(IncludePermissions) : $", {nameof(IncludePermissions)}";
                }

                return options;
            }
        }

        protected class RegexItemExclusionOptions
        {
            public string Expression { get; private set; }
            public bool CaseSensitive { get; private set; }
            public bool Inclusion { get; private set; }
            public bool MatchDirectories { get; private set; }
            public bool NegateExpression { get; private set; }

            public RegexItemExclusionOptions(BackupSetRegexExclusion regexExclusion)
            {
                Expression = regexExclusion.getExpression();
                CaseSensitive = regexExclusion.getCaseSensitive();
                Inclusion = regexExclusion.getInclusion();
                MatchDirectories = regexExclusion.getMatchDirectories();
                NegateExpression = regexExclusion.getNegate();
            }

            public override string ToString()
            {
                return Expression;
            }
        }

        protected class ForceFullDayTime
        {
            public bool ForceFull { get; private set; }
            public string Day { get; private set; }
            public TimeInDay Time { get; private set; }

            public ForceFullDayTime(bool forceFull, int day, time_in_day time)
            {
                ForceFull = forceFull;
                Day = day.ToString();
                Time = new TimeInDay(time);
            }

            public ForceFullDayTime(bool forceFull, EWeekDay day, time_in_day time)
            {
                ForceFull = forceFull;
                Day = EnumToString(day);
                Time = new TimeInDay(time);
            }

            public override string ToString()
            {
                return ForceFull.ToString();
            }
        }

        protected class ForceFullPeriod
        {
            public bool ForceFull { get; private set; }
            public string TimeUnit { get; private set; }
            public int TimeValue { get; private set; }

            public ForceFullPeriod(bool forceFull, ETimeUnit unit, int value)
            {
                ForceFull = forceFull;
                TimeUnit = EnumToString(unit);
                TimeValue = value;
            }

            public override string ToString()
            {
                return ForceFull.ToString();
            }
        }

        protected class SkipFullWeekDays
        {
            public bool SkipFull { get; private set; }
            public string[] SkipWeekDays { get; private set; }
            public TimeInDay SkipWeekDaysFrom { get; private set; }
            public TimeInDay SkipWeekDaysTo { get; private set; }

            public SkipFullWeekDays(bool skipFull, int weekDays, time_in_day from, time_in_day to)
            {
                SkipFull = skipFull;
                SkipWeekDays = BaseDSClientSchedule.EScheduleWeekDaysIntToArray(weekDays);
                SkipWeekDaysFrom = new TimeInDay(from);
                SkipWeekDaysTo = new TimeInDay(to);
            }

            public override string ToString()
            {
                return SkipFull.ToString();
            }
        }

        protected class ShareInfo
        {
            public int GlobalId { get; private set; }
            public int LocalId { get; private set; }
            public int ServerId { get; private set; }
            public string Path { get; private set; }
            public string DirectoryType { get; private set; }
            public string Info { get; private set; }
            public string AdditionalInfo { get; private set; }

            public ShareInfo(shares_info shareInfo)
            {
                GlobalId = shareInfo.global_share_id;
                LocalId = shareInfo.local_share_id;
                ServerId = shareInfo.server_id;
                Path = shareInfo.path;
                DirectoryType = EApplicationTypeToString(shareInfo.dir_type);
                Info = shareInfo.info;
                AdditionalInfo = shareInfo.info2;
            }

            public override string ToString()
            {
                return DirectoryType;
            }

            private string EApplicationTypeToString(EApplicationType applicationType)
            {
                switch(applicationType)
                {
                    case EApplicationType.EApplicationType__File:
                        return "FileSystem";
                    case EApplicationType.EApplicationType__Registry:
                        return "WindowsRegistry";
                    case EApplicationType.EApplicationType__Bindery:
                        return "NovelBindery";
                    case EApplicationType.EApplicationType__NDS:
                        return "NovelNDS";
                    case EApplicationType.EApplicationType__Database:
                        return "Database";
                    case EApplicationType.EApplicationType__Exchange:
                        return "MSExchange";
                    case EApplicationType.EApplicationType__Service:
                        return "WindowsService";
                    case EApplicationType.EApplicationType__ServiceDatabase:
                        return "WindowsServiceDatabase";
                    case EApplicationType.EApplicationType__SystemState:
                        return "WindowsSystemState";
                    case EApplicationType.EApplicationType__OracleDatabase:
                        return "OracleDatabase";
                    case EApplicationType.EApplicationType__OracleTableSpace:
                        return "OracleTableSpace";
                    case EApplicationType.EApplicationType__OracleArchiveLogs:
                        return "OracleArchiveLogs";
                    case EApplicationType.EApplicationType__OracleControlFile:
                        return "OracleControlFile";
                    case EApplicationType.EApplicationType__EmailLotusFolder:
                        return "EmailLotusFolder";
                    case EApplicationType.EApplicationType__EmailExchangeFolder:
                        return "EmailExchangeFolder";
                    case EApplicationType.EApplicationType__EmailOutlookFolder:
                        return "EmailOutlookFolder";
                    case EApplicationType.EApplicationType__ISeries:
                        return "AS400iSeriesFolder";
                    case EApplicationType.EApplicationType__EmailGroupwiseFolder:
                        return "EmailGroupwiseFolder";
                    case EApplicationType.EApplicationType__MySql:
                        return "MySqlServer";
                    case EApplicationType.EApplicationType__SharePoint:
                        return "SharePoint";
                    case EApplicationType.EApplicationType__LotusDomino:
                        return "LotusDomino";
                    case EApplicationType.EApplicationType__VssHyperV:
                        return "VssHyperV";
                    case EApplicationType.EApplicationType__VssSqlServer:
                        return "VssSqlServer";
                    case EApplicationType.EApplicationType__VssExchangeServer:
                        return "VssExchangeServer";
                    case EApplicationType.EApplicationType__VssSharePointServer:
                        return "VssSharePointServer";
                    case EApplicationType.EApplicationType__VmwareVadpVM:
                        return "VMWareVADPVirtualMachine";
                    case EApplicationType.EApplicationType__VmwareVadpVD:
                        return "VMWareVADPVirtualDisk";
                    case EApplicationType.EApplicationType__SalesForce:
                        return "SalesForce";
                    case EApplicationType.EApplicationType__P2V:
                        return "P2V";
                    case EApplicationType.EApplicationType__GoogleApps:
                        return "GoogleApps";
                    case EApplicationType.EApplicationType__GoogleAppsDocument:
                        return "GoogleAppsDocument";
                    case EApplicationType.EApplicationType__GoogleAppsCalendar:
                        return "GoogleAppsCalendar";
                    case EApplicationType.EApplicationType__GoogleAppsContact:
                        return "GoogleAppsContact";
                    case EApplicationType.EApplicationType__GoogleAppsEmail:
                        return "GoogleAppsEmail";
                    case EApplicationType.EApplicationType__GoogleAppsSite:
                        return "GoogleAppsSite";
                    case EApplicationType.EApplicationType__Office365:
                        return "Office365";
                    case EApplicationType.EApplicationType__ExchangeOnline:
                        return "ExchangeOnline";
                    case EApplicationType.EApplicationType__Office365Outlook:
                        return "Office365Outlook";
                    case EApplicationType.EApplicationType__Office365OutlookTasks:
                        return "Office365OutlookTasks";
                    case EApplicationType.EApplicationType__Office365OutlookNotes:
                        return "Office365OutlookNotes";
                    case EApplicationType.EApplicationType__Office365Calendar:
                        return "Office365Calendar";
                    case EApplicationType.EApplicationType__Office365People:
                        return "Office365People";
                    case EApplicationType.EApplicationType__Office365Attachments:
                        return "Office365Attachments";
                    case EApplicationType.EApplicationType__SharePointOnline:
                        return "SharePointOnline";
                    case EApplicationType.EApplicationType__Office365Web:
                        return "Office365Web";
                    case EApplicationType.EApplicationType__Office365List:
                        return "Office365List";
                    case EApplicationType.EApplicationType__Office365Sites:
                        return "Office365Sites";
                    case EApplicationType.EApplicationType__Office365ListItem:
                        return "Office365ListItem";
                    case EApplicationType.EApplicationType__VmwareFLRDisk:
                        return "VMWareFLRDisk";
                    case EApplicationType.EApplicationType__VmwareFLRVolume:
                        return "VMWareFLRVolume";
                    case EApplicationType.EApplicationType__VmwareFLRDirUnexpanded:
                        return "VMWareFLRDirUnexpanded";
                    case EApplicationType.EApplicationType__VmwareFLRDir:
                        return "VMWareFLRDir";
                    case EApplicationType.EApplicationType__VmwareFLRMachine:
                        return "VMWareFLRMachine";
                    case EApplicationType.EApplicationType__AmazonWebServices:
                        return "AmazonWebServices";
                    case EApplicationType.EApplicationType__VmReplication:
                        return "VMReplication";
                    default:
                        return null;
                }
            }
        }

        protected class OldFileExclusionConfig
        {
            public string Type { get; private set; }
            public string TimeUnit { get; private set; }
            public int Value { get; private set; }

            public OldFileExclusionConfig(old_file_exclusion_config exclusionConfig)
            {
                Type = EnumToString(exclusionConfig.type);
                TimeUnit = EnumToString(exclusionConfig.unit);
                Value = exclusionConfig.value;
            }

            public override string ToString()
            {
                return Type;
            }
        }

        protected class VSSInfo
        {
            public string Name { get; private set; }
            public string WriterGuid { get; private set; }

            public VSSInfo(vss_exclusion_info vssInfo)
            {
                Name = vssInfo.name;
                WriterGuid = vssInfo.writerGUID;
            }

            public override string ToString()
            {
                return WriterGuid;
            }
        }

        protected class DSClientCDPSettings
        {
            public int CheckInterval { get; private set; }
            public string BackupStrategy { get; private set; }
            public string ChangeDetection { get; private set; }
            public string[] SuspendableActivity { get; private set; }

            public DSClientCDPSettings(CDP_settings cdpSettings)
            {
                CheckInterval = cdpSettings.backup_check_interval;
                BackupStrategy = ECDPBackupStrategyToString(cdpSettings.backup_strategy);
                ChangeDetection = EnumToString(cdpSettings.file_change_detection_type);
                SuspendableActivity = ECDPSuspendableScheduledActivityIntToString(cdpSettings.suspendable_activities);
            }

            public override string ToString()
            {
                return ChangeDetection;
            }

            private string ECDPBackupStrategyToString(ECDPBackupStrategy backupStrategy)
            {
                switch(backupStrategy)
                {
                    case ECDPBackupStrategy.ECDPBackupStrategy__BackupNotOftenThan:
                        return "NotMoreThanInterval";
                    case ECDPBackupStrategy.ECDPBackupStrategy__BackupStopChangingFor:
                        return "NoChangeWithinInterval";
                    default:
                        return null;
                }
            }            

            private string[] ECDPSuspendableScheduledActivityIntToString(int suspendableActivities)
            {
                List<string> SuspendableActivities = new List<string>();

                if ((suspendableActivities & (int)ECDPSuspendableScheduledActivity.ECDPSuspendableScheduledActivity__Retention) > 0)
                    SuspendableActivities.Add("Retention");
                if ((suspendableActivities & (int)ECDPSuspendableScheduledActivity.ECDPSuspendableScheduledActivity__BLM) > 0)
                    SuspendableActivities.Add("BLM");
                if ((suspendableActivities & (int)ECDPSuspendableScheduledActivity.ECDPSuspendableScheduledActivity__Validation) > 0)
                    SuspendableActivities.Add("Validation");

                return SuspendableActivities.ToArray();
            }
        }

        protected class DSClientPrePost
        {
            public int Id { get; private set; }
            public string Command { get; private set; }
            public string ExecutionType { get; private set; }
            public string BackupRestore { get; private set; }
            public string PrePost { get; private set; }
            public bool RemoteExecute { get; private set; }
            public PreOptions PreOptions { get; private set; }
            public PostOptions PostOptions { get; private set; }

            public DSClientPrePost(pre_post_info prePostInfo)
            {
                Id = prePostInfo.id;
                Command = prePostInfo.command;
                ExecutionType = EnumToString(prePostInfo.executionType);
                BackupRestore = (prePostInfo.isForBackup == true) ? "Backup" : "Restore";
                PrePost = (prePostInfo.isForPre == true) ? "Pre" : "Post";
                RemoteExecute = prePostInfo.isRemote;
                PreOptions = new PreOptions(prePostInfo.preOptions);
                PostOptions = new PostOptions(prePostInfo.postOptions);
            }

            public override string ToString()
            {
                return Command;
            }
        }

        protected class PostOptions
        {
            public bool SkipIncomplete { get; private set; }
            public bool SkipSuccessful { get; private set; }
            public bool SkipWithErrors { get; private set; }

            public PostOptions(post_options postOptions)
            {
                SkipIncomplete = postOptions.skipPostIfActivityIncomplete;
                SkipSuccessful = postOptions.skipPostIfActivitySuccessful;
                SkipWithErrors = postOptions.skipPostIfActivityWithErrors;
            }

            public override string ToString()
            {
                string Skips = "False";

                if ((SkipIncomplete || SkipSuccessful || SkipWithErrors) == true)
                    Skips = "True";

                return Skips;
            }
        }

        protected class PreOptions
        {
            public string Value { get; private set; }
            public int DelaySeconds { get; private set; }
            public bool EqualToValue { get; private set; }
            public bool ExecutionFailure { get; private set; }
            public string ResultCheck { get; private set; }
            public bool SkipActivity { get; private set; }
            public bool SkipPost { get; private set; }

            public PreOptions(pre_options preOptions)
            {
                Value = preOptions.value;
                DelaySeconds = preOptions.delaySeconds;
                EqualToValue = preOptions.equalTo;
                ExecutionFailure = preOptions.orExecutionFailure;
                ResultCheck = EnumToString(preOptions.resultType);
                SkipActivity = preOptions.skipActivity;
                SkipPost = preOptions.skipPost;
            }

            public override string ToString()
            {
                return Value;
            }
        }

        public static string EBackupDataTypeToString(EBackupDataType dataType)
        {
            string DataType = null;

            switch (dataType)
            {
                case EBackupDataType.EBackupDataType__FileSystem:
                    DataType = "FileSystem";
                    break;
                case EBackupDataType.EBackupDataType__SQLServer:
                    DataType = "MSSQLServer";
                    break;
                case EBackupDataType.EBackupDataType__ExchangeServer:
                    DataType = "MSExchange";
                    break;
                case EBackupDataType.EBackupDataType__Oracle:
                    DataType = "Oracle";
                    break;
                case EBackupDataType.EBackupDataType__Permissions:
                    DataType = "PermissionsOnly";
                    break;
                case EBackupDataType.EBackupDataType__ExchangeEmail:
                    DataType = "MSExchangeItemLevel";
                    break;
                case EBackupDataType.EBackupDataType__OutlookEmail:
                    DataType = "OutlookEmail";
                    break;
                case EBackupDataType.EBackupDataType__SystemI:
                    DataType = "SystemI";
                    break;
                case EBackupDataType.EBackupDataType__MySQL:
                    DataType = "MySQL";
                    break;
                case EBackupDataType.EBackupDataType__PostgreSQL:
                    DataType = "PostgreSQL";
                    break;
                case EBackupDataType.EBackupDataType__DB2:
                    DataType = "DB2";
                    break;
                case EBackupDataType.EBackupDataType__LotusNotesEmail:
                    DataType = "LotusNotesEmail";
                    break;
                case EBackupDataType.EBackupDataType__GroupwiseEmail:
                    DataType = "GroupWiseEmail";
                    break;
                case EBackupDataType.EBackupDataType__Sharepoint:
                    DataType = "SharepointItemLevel";
                    break;
                case EBackupDataType.EBackupDataType__VMWare:
                    DataType = "VMWareVMDK";
                    break;
                case EBackupDataType.EBackupDataType__XenServer:
                    DataType = "XenServer";
                    break;
                case EBackupDataType.EBackupDataType__Sybase:
                    DataType = "Sybase";
                    break;
                case EBackupDataType.EBackupDataType__HyperV:
                    DataType = "HyperVServer";
                    break;
                case EBackupDataType.EBackupDataType__VMwareVADP:
                    DataType = "VMWareVADP";
                    break;
                case EBackupDataType.EBackupDataType__VSSSQLServer:
                    DataType = "VSSMSSQLServer";
                    break;
                case EBackupDataType.EBackupDataType__VSSExchange:
                    DataType = "VSSMSExchange";
                    break;
                case EBackupDataType.EBackupDataType__VSSSharePoint:
                    DataType = "VSSSharepoint";
                    break;
                case EBackupDataType.EBackupDataType__SalesForce:
                    DataType = "SalesForce";
                    break;
                case EBackupDataType.EBackupDataType__GoogleApps:
                    DataType = "GoogleApps";
                    break;
                case EBackupDataType.EBackupDataType__Office365:
                    DataType = "Office365";
                    break;
                case EBackupDataType.EBackupDataType__OracleSBT:
                    DataType = "OracleSBT";
                    break;
                case EBackupDataType.EBackupDataType__LotusDomino:
                    DataType = "LotusDomino";
                    break;
                case EBackupDataType.EBackupDataType__ClusteredHyperV:
                    DataType = "HyperVCluster";
                    break;
                case EBackupDataType.EBackupDataType__PToV:
                    DataType = "P2V";
                    break;
                case EBackupDataType.EBackupDataType__ExchangeEMailEWS:
                    DataType = "MSExchangeEWS";
                    break;
            }

            return DataType;
        }

        public static EBackupDataType StringToEBackupDataType(string dataType)
        {
            switch(dataType.ToLower())
            {
                case "filesystem":
                    return EBackupDataType.EBackupDataType__FileSystem;
                case "mssqlserver":
                    return EBackupDataType.EBackupDataType__SQLServer;
                case "msexchange":
                    return EBackupDataType.EBackupDataType__ExchangeServer;
                case "oracle":
                    return EBackupDataType.EBackupDataType__Oracle;
                case "permissionsonly":
                    return EBackupDataType.EBackupDataType__Permissions;
                case "msexchangeitemlevel":
                    return EBackupDataType.EBackupDataType__ExchangeEmail;
                case "outlookemail":
                    return EBackupDataType.EBackupDataType__OutlookEmail;
                case "systemi":
                    return EBackupDataType.EBackupDataType__SystemI;
                case "mysql":
                    return EBackupDataType.EBackupDataType__MySQL;
                case "postgresql":
                    return EBackupDataType.EBackupDataType__PostgreSQL;
                case "db2":
                    return EBackupDataType.EBackupDataType__DB2;
                case "lotusnotesemail":
                    return EBackupDataType.EBackupDataType__LotusNotesEmail;
                case "groupwiseemail":
                    return EBackupDataType.EBackupDataType__GroupwiseEmail;
                case "sharepointitemlevel":
                    return EBackupDataType.EBackupDataType__Sharepoint;
                case "vmwarevmdk":
                    return EBackupDataType.EBackupDataType__VMWare;
                case "xenserver":
                    return EBackupDataType.EBackupDataType__XenServer;
                case "sybase":
                    return EBackupDataType.EBackupDataType__Sybase;
                case "hypervserver":
                    return EBackupDataType.EBackupDataType__HyperV;
                case "vmwarevadp":
                    return EBackupDataType.EBackupDataType__VMwareVADP;
                case "vssmssqlserver":
                    return EBackupDataType.EBackupDataType__VSSSQLServer;
                case "vssmsexchange":
                    return EBackupDataType.EBackupDataType__VSSExchange;
                case "vsssharepoint":
                    return EBackupDataType.EBackupDataType__VSSSharePoint;
                case "salesforce":
                    return EBackupDataType.EBackupDataType__SalesForce;
                case "googleapps":
                    return EBackupDataType.EBackupDataType__GoogleApps;
                case "office365":
                    return EBackupDataType.EBackupDataType__Office365;
                case "oraclesbt":
                    return EBackupDataType.EBackupDataType__OracleSBT;
                case "lotusdomino":
                    return EBackupDataType.EBackupDataType__LotusDomino;
                case "hypervcluster":
                    return EBackupDataType.EBackupDataType__ClusteredHyperV;
                case "p2v":
                    return EBackupDataType.EBackupDataType__PToV;
                case "msexchangeews":
                    return EBackupDataType.EBackupDataType__ExchangeEMailEWS;
                default:
                    return EBackupDataType.EBackupDataType__UNDEFINED;
            }
        }

        private static string EBackupPolicyToString(EBackupPolicy backupPolicy)
        {
            string BackupPolicy = null;

            switch (backupPolicy)
            {
                case EBackupPolicy.EBackupPolicy__FullBackup:
                    BackupPolicy = "Full";
                    break;
                case EBackupPolicy.EBackupPolicy__DiffBackup:
                    BackupPolicy = "Differential";
                    break;
                case EBackupPolicy.EBackupPolicy__IncBackup:
                    BackupPolicy = "Incremental";
                    break;
            }

            return BackupPolicy;
        }

        protected static int SwitchParamsToECDPSuspendableScheduledActivityInt(bool retention, bool blm, bool validation)
        {
            int Suspendable = 0;

            if (retention)
                Suspendable += (int)ECDPSuspendableScheduledActivity.ECDPSuspendableScheduledActivity__Retention;

            if (blm)
                Suspendable += (int)ECDPSuspendableScheduledActivity.ECDPSuspendableScheduledActivity__BLM;

            if (validation)
                Suspendable += (int)ECDPSuspendableScheduledActivity.ECDPSuspendableScheduledActivity__Validation;

            return Suspendable;
        }
    }
}