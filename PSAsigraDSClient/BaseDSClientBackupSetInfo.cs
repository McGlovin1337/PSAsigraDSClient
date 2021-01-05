using System;
using System.Collections.Generic;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientBackupSet;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSetInfo: DSClientCmdlet
    {
        protected abstract void ProcessBackupSet(IEnumerable<DSClientBackupSetInfo> dSClientBackupSetsInfo);

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve Backup Sets");
            backup_set_info[] backupSetsInfo = DSClientSession.backup_sets_info();

            List<DSClientBackupSetInfo> dSClientBackupSets = new List<DSClientBackupSetInfo>();

            foreach (backup_set_info backupSet in backupSetsInfo)
            {
                DSClientBackupSetInfo backupSetInfo = new DSClientBackupSetInfo(backupSet);
                dSClientBackupSets.Add(backupSetInfo);
            }

            ProcessBackupSet(dSClientBackupSets);
        }

        public class DSClientBackupSetInfo
        {
            public int BackupSetId { get; private set; }
            public string Computer { get; private set; }
            public string Name { get; private set; }
            public bool Enabled { get; private set; }
            public DateTime LastBackup { get; private set; }
            public DateTime LastSuccess { get; private set; }
            public string DataType { get; private set; }
            public bool Synchronized { get; private set; }
            public DateTime LastSynchronized { get; private set; }
            public int ScheduleId { get; private set; }
            public string ScheduleName { get; private set; }
            public int RetentionRuleId { get; private set; }
            public string RetentionRuleName { get; private set; }
            public long OnlineDataSize { get; private set; }
            public int OnlineFileCount { get; private set; }
            public long CompressedSize { get; private set; }
            public long LocalStorageDataSize { get; private set; }
            public int LocalStorageFileCount { get; private set; }
            public string SetType { get; private set; }
            public bool UseLocalStorage { get; private set; }
            public bool HasItems { get; private set; }
            public bool IsCDP { get; private set; }
            public bool CreatedByPolicy { get; private set; }
            public int OwnerId { get; private set; }
            public string OwnerName { get; private set; }

            public DSClientBackupSetInfo(backup_set_info backupSetInfo)
            {
                BackupSetId = backupSetInfo.id;
                Computer = backupSetInfo.computer;
                Name = backupSetInfo.name;
                Enabled = backupSetInfo.is_active;
                LastBackup = UnixEpochToDateTime(backupSetInfo.last_backup);
                LastSuccess = UnixEpochToDateTime(backupSetInfo.last_successful_backup);
                DataType = EBackupDataTypeToString(backupSetInfo.data_type);
                Synchronized = backupSetInfo.in_sync;
                LastSynchronized = UnixEpochToDateTime(backupSetInfo.last_sync_time);
                ScheduleId = backupSetInfo.schedule_id;
                ScheduleName = backupSetInfo.schedule_name;
                RetentionRuleId = backupSetInfo.retention_rule_id;
                RetentionRuleName = backupSetInfo.retention_rule_name;
                OnlineDataSize = backupSetInfo.online_data_size;
                OnlineFileCount = backupSetInfo.online_file_count;
                CompressedSize = backupSetInfo.dssys_compressed_size;
                LocalStorageDataSize = backupSetInfo.local_storage_data_size;
                LocalStorageFileCount = backupSetInfo.local_storage_file_count;
                SetType = EnumToString(backupSetInfo.set_type);
                UseLocalStorage = backupSetInfo.using_local_storage;
                HasItems = backupSetInfo.has_items;
                IsCDP = backupSetInfo.is_cdp;
                CreatedByPolicy = backupSetInfo.is_created_by_policy;
                OwnerId = backupSetInfo.owner_id;
                OwnerName = backupSetInfo.owner_name;
            }
        }
    }
}