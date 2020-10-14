using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientNotification;
using System.Linq;

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

            baseParams.TryGetValue("Computer", out object Computer);
            if (Computer != null)
                backupSet.setComputerName(Computer as string);

            baseParams.TryGetValue("SetType", out object SetType);
            if (SetType != null)
                backupSet.setSetType(StringToEBackupSetType(SetType as string));

            baseParams.TryGetValue("Compression", out object Compression);
            if (Compression != null)
                backupSet.setCompressionType(StringToECompressionType(Compression as string));

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
                    method = StringToENotificationMethod(NotificationMethod as string),
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

        protected static IEnumerable<BackupSetFileItem> ProcessExclusionItems(DSClientOSType dSClientOSType, DataSourceBrowser dataSourceBrowser, string computer, IEnumerable<string> items)
        {
            List<BackupSetFileItem> fileItems = new List<BackupSetFileItem>();

            foreach(string item in items)
            {
                // Trim any whitespace from the end of the item
                string trimmedItem = item.Trim();

                // Set the item filter by extracting the chars after the last "\" (windows) or "/" (linux/unix)
                string filter = "*";
                int itemLength = 0;

                if (dSClientOSType.OsType == "Windows")
                {
                    filter = trimmedItem.Split('\\').Last();
                    itemLength = filter.Length;
                    if (string.IsNullOrEmpty(filter))
                        filter = "*";
                }
                else if (dSClientOSType.OsType == "Linux")
                {
                    filter = trimmedItem.Split('/').Last();
                    itemLength = filter.Length;
                    if (string.IsNullOrEmpty(filter))
                        filter = "*";
                }

                // Set the path by removing the specified filter from the end of the item
                string path = trimmedItem.Remove((trimmedItem.Length - itemLength), itemLength);

                BackupSetFileItem exclusion = dataSourceBrowser.createExclusionItem(computer, path);
                exclusion.setFilter(filter);
                fileItems.Add(exclusion);
            }

            return fileItems;
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

        protected static IEnumerable<Win32FS_BackupSetInclusionItem> ProcessWin32FSInclusionItems(DataSourceBrowser dataSourceBrowser, string computer, IEnumerable<string> items, int maxGens, bool excludeStreams, bool excludePerms)
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

                if (excludeStreams)
                    inclusionItem.setIncludingAlternateDataStreams(false);
                else
                    inclusionItem.setIncludingAlternateDataStreams(true);

                if (excludePerms)
                    inclusionItem.setIncludingPermissions(false);
                else
                    inclusionItem.setIncludingPermissions(true);

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

        protected class DSClientBackupSet
        {
            public int BackupSetId { get; set; }
            public string Computer { get; set; }
            public string Name { get; set; }
            public bool Enabled { get; set; }
            public DateTime LastSuccess { get; set; }
            public dynamic DataType { get; set; }
            public DSClientBackupSetItem[] BackupItems { get; set; }
            public bool Synchronized { get; set; }
            public DateTime LastSynchronized { get; set; }
            public int ScheduleId { get; set; }
            public string ScheduleName { get; set; }
            public int SchedulePriority { get; set; }
            public int RetentionRuleId { get; set; }
            public string RetentionRuleName { get; set; }
            public long OnlineDataSize { get; set; }
            public int OnlineFileCount { get; set; }
            public string CompressionType { get; set; }
            public long CompressedSize { get; set; }
            public long LocalStorageDataSize { get; set; }
            public int LocalStorageFileCount { get; set; }
            public string SetType { get; set; }
            public bool UseLocalStorage { get; set; }
            public bool ForceBackup { get; set; }
            public int ErrorLimit { get; set; }
            public int MaxPendingAsyncIO { get; set; }
            public bool PreScan { get; set; }
            public bool CreatedByPolicy { get; set; }
            public DSClientBackupSetNotification[] Notification { get; set; }
            public string[] SnmpTrapNotification { get; set; }
            public DSClientPrePost[] PrePost { get; set; }
            public int ReadBufferSize { get; set; }
            public bool UseTransmissionCache { get; set; }
            public bool DetailedLog { get; set; }
            public bool InfinateBLMGenerations { get; set; }
            public ShareInfo[] ShareInfo { get; set; }
            public int OwnerId { get; set; }
            public string OwnerName { get; set; }

            public DSClientBackupSet(BackupSet backupSet, DSClientOSType dSClientOSType)
            {
                backup_set_overview_info backupSetOverviewInfo = backupSet.getOverview();

                // Get Backup Set Notification Info
                BackupSetNotification backupSetNotification = backupSet.getNotification();
                notification_info[] notificationInfo = backupSetNotification.listNotification();
                List<DSClientBackupSetNotification> dSClientBackupSetNotifications = new List<DSClientBackupSetNotification>();
                foreach (var notification in notificationInfo)
                {
                    DSClientBackupSetNotification dSClientBackupSetNotification = new DSClientBackupSetNotification(notification);
                    dSClientBackupSetNotifications.Add(dSClientBackupSetNotification);
                }
                backupSetNotification.Dispose();

                // Get Backup Set Pre & Post Configuration
                PrePost prePost = backupSet.getPrePost();
                pre_post_info[] prePostInfo = prePost.listPrePost();
                List<DSClientPrePost> dSClientPrePosts = new List<DSClientPrePost>();
                foreach (var prepost in prePostInfo)
                {
                    DSClientPrePost dSClientPrePost = new DSClientPrePost(prepost);
                    dSClientPrePosts.Add(dSClientPrePost);
                }
                prePost.Dispose();

                // Get Backup Set Share Information
                shares_info[] sharesInfo = backupSet.getSharesInfo();
                List<ShareInfo> shareInfo = new List<ShareInfo>();
                foreach (var share in sharesInfo)
                {
                    ShareInfo info = new ShareInfo(share);
                    shareInfo.Add(info);
                }

                // Get the Backup Set Items
                BackupSetItem[] backupSetItems = backupSet.items();
                List<DSClientBackupSetItem> setItems = new List<DSClientBackupSetItem>();
                foreach (var item in backupSetItems)
                {
                    DSClientBackupSetItem backupSetItem = new DSClientBackupSetItem(item, backupSetOverviewInfo.data_type, dSClientOSType);
                    setItems.Add(backupSetItem);
                }

                // Set the DataType dynamic property based on the Backup Set Data type
                if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__FileSystem)
                {
                    if (dSClientOSType.OsType == "Windows")
                    {
                        Win32FSBackupSet win32FSBackupSet = new Win32FSBackupSet(backupSet);
                        DataType = win32FSBackupSet;
                    }

                    if (dSClientOSType.OsType == "Linux")
                    {
                        UnixFSBackupSet unixFSBackupSet = new UnixFSBackupSet(backupSet);
                        DataType = unixFSBackupSet;
                    }
                }
                else if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__SQLServer)
                {
                    MSSQLBackupSet mssqlBackupSet = new MSSQLBackupSet(backupSet);
                    DataType = mssqlBackupSet;
                }
                else if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__VSSSQLServer)
                {
                    VSSMSSQLBackupSet vssMSSQLBackupSet = new VSSMSSQLBackupSet(backupSet);
                    DataType = vssMSSQLBackupSet;
                }
                else if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__VSSExchange)
                {
                    VSSExchangeBackupSet vssExchangeBackupSet = new VSSExchangeBackupSet(backupSet);
                    DataType = vssExchangeBackupSet;
                }
                else if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__VMwareVADP)
                {
                    VMWareVADPBackupSet vmwareVADPBackupSet = new VMWareVADPBackupSet(backupSet);
                    DataType = vmwareVADPBackupSet;
                }
                else if (backupSetOverviewInfo.data_type == EBackupDataType.EBackupDataType__DB2)
                {
                    DB2BackupSet db2BackupSet = new DB2BackupSet(backupSet);
                    DataType = db2BackupSet;
                }
                else
                {
                    DataType = EBackupDataTypeToString(backupSetOverviewInfo.data_type);
                }

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
                CompressionType = ECompressionTypeToString(backupSet.getCompressionType());
                CompressedSize = backupSetOverviewInfo.status.dssystem_compressed_size;
                LocalStorageDataSize = backupSetOverviewInfo.status.local_storage_data_size;
                LocalStorageFileCount = backupSetOverviewInfo.status.local_storage_file_count;
                SetType = EBackupSetTypeToString(backupSet.getSetType());
                UseLocalStorage = backupSet.isUsingLocalStorage();
                ForceBackup = backupSet.isForceBackup();
                ErrorLimit = backupSet.getBackupErrorLimit();
                MaxPendingAsyncIO = backupSet.getMaxPendingAsyncIO();
                PreScan = backupSet.getPreScanByDefault();
                CreatedByPolicy = backupSet.isCreatedByBackupPolicy();
                Notification = dSClientBackupSetNotifications.ToArray();
                SnmpTrapNotification = IntEBackupCompletionToArray(backupSet.getSNMPTrapsConditions());
                PrePost = dSClientPrePosts.ToArray();
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
            public bool UseBuffer { get; set; }
            public string ArchiveLogPath { get; set; }
            public string DumpPath { get; set; }
            public bool OnlineBackup { get; set; }
            public bool PruneHistoryOrLogFile { get; set; }

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
            public bool UseBuffer { get; set; }
            public bool IncrementalP2VBackup { get; set; }
            public bool BackupVMMemory { get; set; }
            public bool SnapshotQuiesce { get; set; }
            public bool SameTimeSnapshot { get; set; }
            public bool UseCBT { get; set; }
            public bool UseFLR { get; set; }
            public bool UseLocalVDR { get; set; }
            public string VMLibraryVersion { get; set; }

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
                string Version = null;

                switch(libraryVersion)
                {
                    case EVMwareLibraryVersion.EVMwareLibraryVersion__Latest:
                        Version = "Latest";
                        break;
                    case EVMwareLibraryVersion.EVMwareLibraryVersion__VDDK5_5:
                        Version = "VDDK5.5";
                        break;
                    case EVMwareLibraryVersion.EVMwareLibraryVersion__VDDK6_0:
                        Version = "VDDK6.0";
                        break;
                    case EVMwareLibraryVersion.EVMwareLibraryVersion__VDDK6_5:
                        Version = "VDDK6.5";
                        break;
                }

                return Version;
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
            public string DumpMethod { get; set; }
            public string DumpPath { get; set; }

            public MSSQLBackupSet(BackupSet backupSet)
            {
                MSSQL_BackupSet mssqlBackupSet = MSSQL_BackupSet.from(backupSet);
                mssql_dump_parameters dumpParameters = mssqlBackupSet.getDumpParameters();
                incremental_policies incrementalPolicies = mssqlBackupSet.getIncrementalPolicies();

                DumpMethod = ESQLDumpMethodToString(dumpParameters.dump_method);
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

            private string ESQLDumpMethodToString(ESQLDumpMethod dumpMethod)
            {
                string DumpMethod = null;

                switch(dumpMethod)
                {
                    case ESQLDumpMethod.ESQLDumpMethod__DumpToSQLPath:
                        DumpMethod = "SQLPath";
                        break;
                    case ESQLDumpMethod.ESQLDumpMethod__DumpToClientBuffer:
                        DumpMethod = "ClientBuffer";
                        break;
                    case ESQLDumpMethod.ESQLDumpMethod__DumpToPipe:
                        DumpMethod = "Pipe";
                        break;
                }

                return DumpMethod;
            }
        }

        protected class UnixFSBackupSet
        {
            public bool IsCDP { get; set; }
            public DSClientCDPSettings CDPSettings { get; set; }
            public OldFileExclusionConfig OldFileExclusionOptions { get; set; }
            public bool CheckCommonFiles { get; set; }
            public bool UseBuffer { get; set; }
            public bool ForceBackup { get; set; }
            public bool FollowMountPoints { get; set; }
            public bool BackupHardLinks { get; set; }
            public bool IgnoreSnapshotFailures { get; set; }
            public bool UseSnapDiff { get; set; }

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
            public bool BackupRemoteStorage { get; set; }
            public bool BackupSingleInstanceStore { get; set; }
            public bool IsCDP { get; set; }
            public DSClientCDPSettings CDPSettings { get; set; }
            public bool UseVSS { get; set; }
            public VSSInfo[] VSSWriters { get; set; }
            public bool ExcludeVSSComponents { get; set; }
            public VSSInfo[] VSSComponentExclusions { get; set; }
            public bool IgnoreVSSComponents { get; set; }
            public bool IgnoreVSSWriters { get; set; }
            public bool FollowJunctionPoint { get; set; }
            public bool IgnoreAutoFileFilters { get; set; }
            public OldFileExclusionConfig OldFileExclusionOptions { get; set; }
            public bool CheckCommonFiles { get; set; }
            public bool UseBuffer { get; set; }

            public Win32FSBackupSet(BackupSet backupSet)
            {
                Win32FS_BackupSet win32FSBackupSet = Win32FS_BackupSet.from(backupSet);

                List<VSSInfo> VssWriters = new List<VSSInfo>();
                vss_exclusion_info[] vssWriters = win32FSBackupSet.getVSSWriters();
                foreach (var writer in vssWriters)
                {
                    VSSInfo vssInfo = new VSSInfo(writer);
                    VssWriters.Add(vssInfo);
                }

                List<VSSInfo> VssExclusions = new List<VSSInfo>();
                vss_exclusion_info[] vssExclusions = win32FSBackupSet.getVSSComponentExclusions();
                foreach (var exclusion in vssExclusions)
                {
                    VSSInfo vssInfo = new VSSInfo(exclusion);
                    VssExclusions.Add(vssInfo);
                }

                BackupRemoteStorage = win32FSBackupSet.getBackupRemoteStorage();
                BackupSingleInstanceStore = win32FSBackupSet.getBackupSingleInstanceStore();
                IsCDP = win32FSBackupSet.isContinuousDataProtection();
                CDPSettings = new DSClientCDPSettings(win32FSBackupSet.getCDPSettings());
                UseVSS = win32FSBackupSet.getUseVSS();
                VSSWriters = VssWriters.ToArray();
                ExcludeVSSComponents = win32FSBackupSet.getExcludeVSSComponents();
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
            public string Type { get; set; }
            public string Folder { get; set; }
            public string[] ItemOption { get; set; }
            public string Filter { get; set; }
            public bool SubDirDescend { get; set; }
            public string ItemType { get; set; }
            public int MaxGenerations { get; set; }
            public dynamic InclusionOptions { get; set; }
            public RegexItemExclusionOptions RegexExclusionOptions { get; set; } 

            public DSClientBackupSetItem(BackupSetItem backupSetItem, EBackupDataType backupDataType, DSClientOSType dSClientOSType)
            {
                EItemOption[] itemOptions = backupSetItem.getItemOptions();
                List<string> ItemOptions = new List<string>();
                foreach (var item in itemOptions)
                {
                    string Item = EItemOptionToString(item);
                    ItemOptions.Add(Item);
                }

                EBackupSetItemType itemType = backupSetItem.getType();                                

                Type = EBackupSetItemTypeToString(itemType);
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
                            if (dSClientOSType.OsType == "Windows")
                            {
                                Win32FS_BackupSetInclusionItem win32FSBackupSetInclusionItem = Win32FS_BackupSetInclusionItem.from(backupSetInclusionItem);
                                InclusionOptions = new Win32FSBackupSetInclusionOptions(win32FSBackupSetInclusionItem);
                            }

                            if (dSClientOSType.OsType == "Linux")
                            {
                                UnixFS_BackupSetInclusionItem unixFSBackupSetInclusionItem = UnixFS_BackupSetInclusionItem.from(backupSetInclusionItem);
                                InclusionOptions = new UnixFSBackupSetInclusionOptions(unixFSBackupSetInclusionItem);
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
                string ItemType = null;

                switch(itemType)
                {
                    case EBrowseItemType.EBrowseItemType__Drive:
                        ItemType = "Drive";
                        break;
                    case EBrowseItemType.EBrowseItemType__Share:
                        ItemType = "Share";
                        break;
                    case EBrowseItemType.EBrowseItemType__Directory:
                        ItemType = "Directory";
                        break;
                    case EBrowseItemType.EBrowseItemType__File:
                        ItemType = "File";
                        break;
                    case EBrowseItemType.EBrowseItemType__SystemState:
                        ItemType = "SystemState";
                        break;
                    case EBrowseItemType.EBrowseItemType__ServicesDB:
                        ItemType = "ServicesDatabase";
                        break;
                    case EBrowseItemType.EBrowseItemType__DatabaseInstance:
                        ItemType = "DatabaseInstance";
                        break;
                    case EBrowseItemType.EBrowseItemType__Database:
                        ItemType = "Database";
                        break;
                    case EBrowseItemType.EBrowseItemType__Tablespace:
                        ItemType = "OracleTablespace";
                        break;
                    case EBrowseItemType.EBrowseItemType__ControlFile:
                        ItemType = "OracleControlFile";
                        break;
                    case EBrowseItemType.EBrowseItemType__ArchiveLog:
                        ItemType = "OracleArchiveLog";
                        break;
                    case EBrowseItemType.EBrowseItemType__VirtualMachine:
                        ItemType = "VirtualMachine";
                        break;
                    case EBrowseItemType.EBrowseItemType__VssExchange:
                        ItemType = "VSSExchange";
                        break;
                    case EBrowseItemType.EBrowseItemType__VmDisk:
                        ItemType = "VirtualMachineDisk";
                        break;
                }

                return ItemType;
            }

            private string EItemOptionToString(EItemOption itemOption)
            {
                string ItemOption = null;

                switch(itemOption)
                {
                    case EItemOption.EItemOption__UnixFSBackupACL:
                        ItemOption = "UnixFSBackupACL";
                        break;
                    case EItemOption.EItemOption__UnixFSBackupPOSIX_ACL:
                        ItemOption = "UnixFSBackupPosixACL";
                        break;
                    case EItemOption.EItemOption__UnixFSBackupSElinux:
                        ItemOption = "UnixFSBackupSELinux";
                        break;
                    case EItemOption.EItemOption__WinFSBackupPermissions:
                        ItemOption = "WinFSBackupPermissions";
                        break;
                    case EItemOption.EItemOption__WinFSBackupStreams:
                        ItemOption = "WinFSBackupStreams";
                        break;
                    case EItemOption.EItemOption__WinExchangeDoNotTruncateTransactionLog:
                        ItemOption = "DoNotTruncateExchangeLog";
                        break;
                    case EItemOption.EItemOption__WinSQLRunDBCCBeforeBackup:
                        ItemOption = "SqlServerRunDBCC";
                        break;
                    case EItemOption.EItemOption__WinSQLBackupTransactionLog:
                        ItemOption = "SqlServerBackupTransactionLog";
                        break;
                    case EItemOption.EItemOption__WinSQLStopOnDBCCError:
                        ItemOption = "SqlServerStopOnDBCCError";
                        break;
                    case EItemOption.EItemOption__VSSExchangeBackupPasiveDatabase:
                        ItemOption = "VSSExchangeBackupPassiveDatabase";
                        break;
                    case EItemOption.EItemOption__VSSExchangeBackupActiveDatabase:
                        ItemOption = "VSSExchangeBackupActiveDatabase";
                        break;
                    case EItemOption.EItemOption__MacFSFileAttributes:
                        ItemOption = "MacFSFileAttributes";
                        break;
                    case EItemOption.EItemOption__MacFSResourceFork:
                        ItemOption = "MacFSResourceFork";
                        break;
                    case EItemOption.EItemOption__NovellBackupPermissions:
                        ItemOption = "NovellBackupPermissions";
                        break;
                    case EItemOption.EItemOption__NovellBackupAttributes:
                        ItemOption = "NovellBackupAttributes";
                        break;
                }

                return ItemOption;
            }

            private string EBackupSetItemTypeToString(EBackupSetItemType itemType)
            {
                string Type = null;

                switch(itemType)
                {
                    case EBackupSetItemType.EBackupSetItemType__Inclusion:
                        Type = "Inclusion";
                        break;
                    case EBackupSetItemType.EBackupSetItemType__Exclusion:
                        Type = "Exclusion";
                        break;
                    case EBackupSetItemType.EBackupSetItemType__RegExExclusion:
                        Type = "RegexExclusion";
                        break;
                }

                return Type;
            }
        }

        protected class UnixFSBackupSetInclusionOptions
        {
            public bool IncludeACLs { get; set; }
            public bool IncludePosixACLs { get; set; }

            public UnixFSBackupSetInclusionOptions(UnixFS_BackupSetInclusionItem inclusionItem)
            {
                UnixFS_LinuxLFS_BackupSetInclusionItem linuxFSBackupSetInclusionItem = UnixFS_LinuxLFS_BackupSetInclusionItem.from(inclusionItem);

                IncludeACLs = inclusionItem.isIncludingACL();
                IncludePosixACLs = linuxFSBackupSetInclusionItem.isIncludingPosixACL();
            }

            public override string ToString()
            {
                string Options = "False";

                if ((IncludeACLs || IncludePosixACLs) == true)
                    Options = "True";

                return Options;
            }
        }

        protected class Win32FSBackupSetInclusionOptions
        {
            public bool IncludeAltDatastreams { get; set; }
            public bool IncludePermissions { get; set; }

            public Win32FSBackupSetInclusionOptions(Win32FS_BackupSetInclusionItem inclusionItem)
            {
                IncludeAltDatastreams = inclusionItem.isIncludingAlternateDataStreams();
                IncludePermissions = inclusionItem.isIncludingPermissions();
            }

            public override string ToString()
            {
                string Options = "False";

                if ((IncludeAltDatastreams || IncludePermissions) == true)
                    Options = "True";

                return Options;
            }
        }

        protected class RegexItemExclusionOptions
        {
            public string Expression { get; set; }
            public bool CaseSensitive { get; set; }
            public bool Inclusion { get; set; }
            public bool MatchDirectories { get; set; }
            public bool NegateExpression { get; set; }

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
            public bool ForceFull { get; set; }
            public string Day { get; set; }
            public TimeInDay Time { get; set; }

            public ForceFullDayTime(bool forceFull, int day, time_in_day time)
            {
                ForceFull = forceFull;
                Day = day.ToString();
                Time = new TimeInDay(time);
            }

            public ForceFullDayTime(bool forceFull, EWeekDay day, time_in_day time)
            {
                ForceFull = forceFull;
                Day = EWeekDayToString(day);
                Time = new TimeInDay(time);
            }

            public override string ToString()
            {
                return ForceFull.ToString();
            }
        }

        protected class ForceFullPeriod
        {
            public bool ForceFull { get; set; }
            public string TimeUnit { get; set; }
            public int TimeValue { get; set; }

            public ForceFullPeriod(bool forceFull, ETimeUnit unit, int value)
            {
                ForceFull = forceFull;
                TimeUnit = ETimeUnitToString(unit);
                TimeValue = value;
            }

            public override string ToString()
            {
                return ForceFull.ToString();
            }
        }

        protected class SkipFullWeekDays
        {
            public bool SkipFull { get; set; }
            public string[] SkipWeekDays { get; set; }
            public TimeInDay SkipWeekDaysFrom { get; set; }
            public TimeInDay SkipWeekDaysTo { get; set; }

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
            public int GlobalId { get; set; }
            public int LocalId { get; set; }
            public int ServerId { get; set; }
            public string Path { get; set; }
            public string DirectoryType { get; set; }
            public string Info { get; set; }
            public string AdditionalInfo { get; set; }

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
                string ApplicationType = null;

                switch(applicationType)
                {
                    case EApplicationType.EApplicationType__File:
                        ApplicationType = "FileSystem";
                        break;
                    case EApplicationType.EApplicationType__Registry:
                        ApplicationType = "WindowsRegistry";
                        break;
                    case EApplicationType.EApplicationType__Bindery:
                        ApplicationType = "NovelBindery";
                        break;
                    case EApplicationType.EApplicationType__NDS:
                        ApplicationType = "NovelNDS";
                        break;
                    case EApplicationType.EApplicationType__Database:
                        ApplicationType = "Database";
                        break;
                    case EApplicationType.EApplicationType__Exchange:
                        ApplicationType = "MSExchange";
                        break;
                    case EApplicationType.EApplicationType__Service:
                        ApplicationType = "WindowsService";
                        break;
                    case EApplicationType.EApplicationType__ServiceDatabase:
                        ApplicationType = "WindowsServiceDatabase";
                        break;
                    case EApplicationType.EApplicationType__SystemState:
                        ApplicationType = "WindowsSystemState";
                        break;
                    case EApplicationType.EApplicationType__OracleDatabase:
                        ApplicationType = "OracleDatabase";
                        break;
                    case EApplicationType.EApplicationType__OracleTableSpace:
                        ApplicationType = "OracleTableSpace";
                        break;
                    case EApplicationType.EApplicationType__OracleArchiveLogs:
                        ApplicationType = "OracleArchiveLogs";
                        break;
                    case EApplicationType.EApplicationType__OracleControlFile:
                        ApplicationType = "OracleControlFile";
                        break;
                    case EApplicationType.EApplicationType__EmailLotusFolder:
                        ApplicationType = "EmailLotusFolder";
                        break;
                    case EApplicationType.EApplicationType__EmailExchangeFolder:
                        ApplicationType = "EmailExchangeFolder";
                        break;
                    case EApplicationType.EApplicationType__EmailOutlookFolder:
                        ApplicationType = "EmailOutlookFolder";
                        break;
                    case EApplicationType.EApplicationType__ISeries:
                        ApplicationType = "AS400iSeriesFolder";
                        break;
                    case EApplicationType.EApplicationType__EmailGroupwiseFolder:
                        ApplicationType = "EmailGroupwiseFolder";
                        break;
                    case EApplicationType.EApplicationType__MySql:
                        ApplicationType = "MySqlServer";
                        break;
                    case EApplicationType.EApplicationType__SharePoint:
                        ApplicationType = "SharePoint";
                        break;
                    case EApplicationType.EApplicationType__LotusDomino:
                        ApplicationType = "LotusDomino";
                        break;
                    case EApplicationType.EApplicationType__VssHyperV:
                        ApplicationType = "VssHyperV";
                        break;
                    case EApplicationType.EApplicationType__VssSqlServer:
                        ApplicationType = "VssSqlServer";
                        break;
                    case EApplicationType.EApplicationType__VssExchangeServer:
                        ApplicationType = "VssExchangeServer";
                        break;
                    case EApplicationType.EApplicationType__VssSharePointServer:
                        ApplicationType = "VssSharePointServer";
                        break;
                    case EApplicationType.EApplicationType__VmwareVadpVM:
                        ApplicationType = "VMWareVADPVirtualMachine";
                        break;
                    case EApplicationType.EApplicationType__VmwareVadpVD:
                        ApplicationType = "VMWareVADPVirtualDisk";
                        break;
                    case EApplicationType.EApplicationType__SalesForce:
                        ApplicationType = "SalesForce";
                        break;
                    case EApplicationType.EApplicationType__P2V:
                        ApplicationType = "P2V";
                        break;
                    case EApplicationType.EApplicationType__GoogleApps:
                        ApplicationType = "GoogleApps";
                        break;
                    case EApplicationType.EApplicationType__GoogleAppsDocument:
                        ApplicationType = "GoogleAppsDocument";
                        break;
                    case EApplicationType.EApplicationType__GoogleAppsCalendar:
                        ApplicationType = "GoogleAppsCalendar";
                        break;
                    case EApplicationType.EApplicationType__GoogleAppsContact:
                        ApplicationType = "GoogleAppsContact";
                        break;
                    case EApplicationType.EApplicationType__GoogleAppsEmail:
                        ApplicationType = "GoogleAppsEmail";
                        break;
                    case EApplicationType.EApplicationType__GoogleAppsSite:
                        ApplicationType = "GoogleAppsSite";
                        break;
                    case EApplicationType.EApplicationType__Office365:
                        ApplicationType = "Office365";
                        break;
                    case EApplicationType.EApplicationType__ExchangeOnline:
                        ApplicationType = "ExchangeOnline";
                        break;
                    case EApplicationType.EApplicationType__Office365Outlook:
                        ApplicationType = "Office365Outlook";
                        break;
                    case EApplicationType.EApplicationType__Office365OutlookTasks:
                        ApplicationType = "Office365OutlookTasks";
                        break;
                    case EApplicationType.EApplicationType__Office365OutlookNotes:
                        ApplicationType = "Office365OutlookNotes";
                        break;
                    case EApplicationType.EApplicationType__Office365Calendar:
                        ApplicationType = "Office365Calendar";
                        break;
                    case EApplicationType.EApplicationType__Office365People:
                        ApplicationType = "Office365People";
                        break;
                    case EApplicationType.EApplicationType__Office365Attachments:
                        ApplicationType = "Office365Attachments";
                        break;
                    case EApplicationType.EApplicationType__SharePointOnline:
                        ApplicationType = "SharePointOnline";
                        break;
                    case EApplicationType.EApplicationType__Office365Web:
                        ApplicationType = "Office365Web";
                        break;
                    case EApplicationType.EApplicationType__Office365List:
                        ApplicationType = "Office365List";
                        break;
                    case EApplicationType.EApplicationType__Office365Sites:
                        ApplicationType = "Office365Sites";
                        break;
                    case EApplicationType.EApplicationType__Office365ListItem:
                        ApplicationType = "Office365ListItem";
                        break;
                    case EApplicationType.EApplicationType__VmwareFLRDisk:
                        ApplicationType = "VMWareFLRDisk";
                        break;
                    case EApplicationType.EApplicationType__VmwareFLRVolume:
                        ApplicationType = "VMWareFLRVolume";
                        break;
                    case EApplicationType.EApplicationType__VmwareFLRDirUnexpanded:
                        ApplicationType = "VMWareFLRDirUnexpanded";
                        break;
                    case EApplicationType.EApplicationType__VmwareFLRDir:
                        ApplicationType = "VMWareFLRDir";
                        break;
                    case EApplicationType.EApplicationType__VmwareFLRMachine:
                        ApplicationType = "VMWareFLRMachine";
                        break;
                    case EApplicationType.EApplicationType__AmazonWebServices:
                        ApplicationType = "AmazonWebServices";
                        break;
                    case EApplicationType.EApplicationType__VmReplication:
                        ApplicationType = "VMReplication";
                        break;
                }

                return ApplicationType;
            }
        }

        protected class OldFileExclusionConfig
        {
            public string Type { get; set; }
            public string TimeUnit { get; set; }
            public int Value { get; set; }

            public OldFileExclusionConfig(old_file_exclusion_config exclusionConfig)
            {
                Type = EOldFileExclusionTypeToString(exclusionConfig.type);
                TimeUnit = ETimeUnitToString(exclusionConfig.unit);
                Value = exclusionConfig.value;
            }

            public override string ToString()
            {
                return Type;
            }

            private string EOldFileExclusionTypeToString(EOldFileExclusionType type)
            {
                string Type = null;

                switch(type)
                {
                    case EOldFileExclusionType.EOldFileExclusionType__None:
                        Type = "None";
                        break;
                    case EOldFileExclusionType.EOldFileExclusionType__Date:
                        Type = "Date";
                        break;
                    case EOldFileExclusionType.EOldFileExclusionType__TimeSpan:
                        Type = "TimeSpan";
                        break;
                }

                return Type;
            }
        }

        protected class VSSInfo
        {
            public string Name { get; set; }
            public string WriterGuid { get; set; }

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
            public int CheckInterval { get; set; }
            public string BackupStrategy { get; set; }
            public string ChangeDetection { get; set; }
            public string[] SuspendableActivity { get; set; }

            public DSClientCDPSettings(CDP_settings cdpSettings)
            {
                CheckInterval = cdpSettings.backup_check_interval;
                BackupStrategy = ECDPBackupStrategyToString(cdpSettings.backup_strategy);
                ChangeDetection = ECDPFileChangeDetectionTypeToString(cdpSettings.file_change_detection_type);
                SuspendableActivity = ECDPSuspendableScheduledActivityIntToString(cdpSettings.suspendable_activities);
            }

            public override string ToString()
            {
                return ChangeDetection;
            }

            private string ECDPBackupStrategyToString(ECDPBackupStrategy backupStrategy)
            {
                string BackupStrategy = null;

                switch(backupStrategy)
                {
                    case ECDPBackupStrategy.ECDPBackupStrategy__BackupNotOftenThan:
                        BackupStrategy = "NotMoreThanInterval";
                        break;
                    case ECDPBackupStrategy.ECDPBackupStrategy__BackupStopChangingFor:
                        BackupStrategy = "NoChangeWithinInterval";
                        break;
                }

                return BackupStrategy;
            }

            private string ECDPFileChangeDetectionTypeToString(ECDPFileChangeDetectionType changeDetectionType)
            {
                string ChangeDetection = null;

                switch(changeDetectionType)
                {
                    case ECDPFileChangeDetectionType.ECDPFileChangeDetectionType__WinBuiltInMonitor:
                        ChangeDetection = "WindowsBuiltInMonitor";
                        break;
                    case ECDPFileChangeDetectionType.ECDPFileChangeDetectionType__MLREmailMonitor:
                        ChangeDetection = "MLREmailMonitor";
                        break;
                    case ECDPFileChangeDetectionType.ECDPFileChangeDetectionType__GenericScanner:
                        ChangeDetection = "GenericScanner";
                        break;
                    case ECDPFileChangeDetectionType.ECDPFileChangeDetectionType__FileAlterationMonitor:
                        ChangeDetection = "FileAlterationMonitor";
                        break;
                }

                return ChangeDetection;
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
            public int Id { get; set; }
            public string Command { get; set; }
            public string ExecutionType { get; set; }
            public string BackupRestore { get; set; }
            public string PrePost { get; set; }
            public bool RemoteExecute { get; set; }
            public PreOptions PreOptions { get; set; }
            public PostOptions PostOptions { get; set; }

            public DSClientPrePost(pre_post_info prePostInfo)
            {
                Id = prePostInfo.id;
                Command = prePostInfo.command;
                ExecutionType = EPrePostExecutionTypeToString(prePostInfo.executionType);
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
            public bool SkipIncomplete { get; set; }
            public bool SkipSuccessful { get; set; }
            public bool SkipWithErrors { get; set; }

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
            public string Value { get; set; }
            public int DelaySeconds { get; set; }
            public bool EqualToValue { get; set; }
            public bool ExecutionFailure { get; set; }
            public string ResultCheck { get; set; }
            public bool SkipActivity { get; set; }
            public bool SkipPost { get; set; }

            public PreOptions(pre_options preOptions)
            {
                Value = preOptions.value;
                DelaySeconds = preOptions.delaySeconds;
                EqualToValue = preOptions.equalTo;
                ExecutionFailure = preOptions.orExecutionFailure;
                ResultCheck = EPreExecutionCheckTypeToString(preOptions.resultType);
                SkipActivity = preOptions.skipActivity;
                SkipPost = preOptions.skipPost;
            }

            public override string ToString()
            {
                return Value;
            }
        }

        private static string EPrePostExecutionTypeToString(EPrePostExecutionType executionType)
        {
            string ExecutionType = null;

            switch(executionType)
            {
                case EPrePostExecutionType.EPrePostExecutionType__RunCommand:
                    ExecutionType = "RunCommand";
                    break;
                case EPrePostExecutionType.EPrePostExecutionType__StartService:
                    ExecutionType = "StartService";
                    break;
                case EPrePostExecutionType.EPrePostExecutionType__StopService:
                    ExecutionType = "StopService";
                    break;
            }

            return ExecutionType;
        }

        private static string EPreExecutionCheckTypeToString(EPreExecutionCheckType checkType)
        {
            string CheckType = null;

            switch(checkType)
            {
                case EPreExecutionCheckType.EPreExecutionCheckType__OnExitCode:
                    CheckType = "OnExitCode";
                    break;
                case EPreExecutionCheckType.EPreExecutionCheckType__OnFileExistence:
                    CheckType = "OnFileExistence";
                    break;
                case EPreExecutionCheckType.EPreExecutionCheckType__OnOutputString:
                    CheckType = "OnOutputString";
                    break;
                case EPreExecutionCheckType.EPreExecutionCheckType__OnExecutionSuccess:
                    CheckType = "OnExecutionSuccess";
                    break;
                case EPreExecutionCheckType.EPreExecutionCheckType__OnExecutionFailure:
                    CheckType = "OnExecutionFailure";
                    break;
            }

            return CheckType;
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
            EBackupDataType DataType = EBackupDataType.EBackupDataType__UNDEFINED;

            switch(dataType)
            {
                case "FileSystem":
                    DataType = EBackupDataType.EBackupDataType__FileSystem;
                    break;
                case "MSSQLServer":
                    DataType = EBackupDataType.EBackupDataType__SQLServer;
                    break;
                case "MSExchange":
                    DataType = EBackupDataType.EBackupDataType__ExchangeServer;
                    break;
                case "Oracle":
                    DataType = EBackupDataType.EBackupDataType__Oracle;
                    break;
                case "PermissionsOnly":
                    DataType = EBackupDataType.EBackupDataType__Permissions;
                    break;
                case "MSExchangeItemLevel":
                    DataType = EBackupDataType.EBackupDataType__ExchangeEmail;
                    break;
                case "OutlookEmail":
                    DataType = EBackupDataType.EBackupDataType__OutlookEmail;
                    break;
                case "SystemI":
                    DataType = EBackupDataType.EBackupDataType__SystemI;
                    break;
                case "MySQL":
                    DataType = EBackupDataType.EBackupDataType__MySQL;
                    break;
                case "PostgreSQL":
                    DataType = EBackupDataType.EBackupDataType__PostgreSQL;
                    break;
                case "DB2":
                    DataType = EBackupDataType.EBackupDataType__DB2;
                    break;
                case "LotusNotesEmail":
                    DataType = EBackupDataType.EBackupDataType__LotusNotesEmail;
                    break;
                case "GroupWiseEmail":
                    DataType = EBackupDataType.EBackupDataType__GroupwiseEmail;
                    break;
                case "SharepointItemLevel":
                    DataType = EBackupDataType.EBackupDataType__Sharepoint;
                    break;
                case "VMWareVMDK":
                    DataType = EBackupDataType.EBackupDataType__VMWare;
                    break;
                case "XenServer":
                    DataType = EBackupDataType.EBackupDataType__XenServer;
                    break;
                case "Sybase":
                    DataType = EBackupDataType.EBackupDataType__Sybase;
                    break;
                case "HyperVServer":
                    DataType = EBackupDataType.EBackupDataType__HyperV;
                    break;
                case "VMWareVADP":
                    DataType = EBackupDataType.EBackupDataType__VMwareVADP;
                    break;
                case "VSSMSSQLServer":
                    DataType = EBackupDataType.EBackupDataType__VSSSQLServer;
                    break;
                case "VSSMSExchange":
                    DataType = EBackupDataType.EBackupDataType__VSSExchange;
                    break;
                case "VSSSharepoint":
                    DataType = EBackupDataType.EBackupDataType__VSSSharePoint;
                    break;
                case "SalesForce":
                    DataType = EBackupDataType.EBackupDataType__SalesForce;
                    break;
                case "GoogleApps":
                    DataType = EBackupDataType.EBackupDataType__GoogleApps;
                    break;
                case "Office365":
                    DataType = EBackupDataType.EBackupDataType__Office365;
                    break;
                case "OracleSBT":
                    DataType = EBackupDataType.EBackupDataType__OracleSBT;
                    break;
                case "LotusDomino":
                    DataType = EBackupDataType.EBackupDataType__LotusDomino;
                    break;
                case "HyperVCluster":
                    DataType = EBackupDataType.EBackupDataType__ClusteredHyperV;
                    break;
                case "P2V":
                    DataType = EBackupDataType.EBackupDataType__PToV;
                    break;
                case "MSExchangeEWS":
                    DataType = EBackupDataType.EBackupDataType__ExchangeEMailEWS;
                    break;
            }

            return DataType;
        }

        public static string EBackupSetTypeToString(EBackupSetType setType)
        {
            string SetType = null;

            switch (setType)
            {
                case EBackupSetType.EBackupSetType__OffSite:
                    SetType = "OffSite";
                    break;
                case EBackupSetType.EBackupSetType__Statistical:
                    SetType = "Statistical";
                    break;
                case EBackupSetType.EBackupSetType__SelfContained:
                    SetType = "SelfContained";
                    break;
                case EBackupSetType.EBackupSetType__LocalOnly:
                    SetType = "LocalOnly";
                    break;
            }

            return SetType;
        }

        public static EBackupSetType StringToEBackupSetType(string setType)
        {
            EBackupSetType SetType = EBackupSetType.EBackupSetType__UNDEFINED;

            switch (setType)
            {
                case "OffSite":
                    SetType = EBackupSetType.EBackupSetType__OffSite;
                    break;
                case "Statistical":
                    SetType = EBackupSetType.EBackupSetType__Statistical;
                    break;
                case "SelfContained":
                    SetType = EBackupSetType.EBackupSetType__SelfContained;
                    break;
                case "LocalOnly":
                    SetType = EBackupSetType.EBackupSetType__LocalOnly;
                    break;
            }

            return SetType;
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

        public static SSHAccesorType StringToSSHAccesorType(string accessType)
        {
            SSHAccesorType AccessType = SSHAccesorType.SSHAccesorType__UNDEFINED;

            switch(accessType)
            {
                case "Perl":
                    AccessType = SSHAccesorType.SSHAccesorType__Perl;
                    break;
                case "Python":
                    AccessType = SSHAccesorType.SSHAccesorType__Python;
                    break;
                case "Direct":
                    AccessType = SSHAccesorType.SSHAccesorType__Direct;
                    break;
            }

            return AccessType;
        }

        protected static int SwitchParamsToECDPSuspendableScheduledActivityInt(bool retention, bool blm, bool validation)
        {
            int Suspendable = 0;

            if (retention)
                Suspendable += 1;

            if (blm)
                Suspendable += 2;

            if (validation)
                Suspendable += 4;

            return Suspendable;
        }
    }
}