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
            WriteVerbose("Retrieving DSClient Backup Sets...");
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
            public int BackupSetId { get; set; }
            public string Computer { get; set; }
            public string Name { get; set; }
            public bool Active { get; set; }
            public DateTime LastBackup { get; set; }
            public DateTime LastSuccess { get; set; }
            public string DataType { get; set; }
            public bool Synchronized { get; set; }
            public DateTime LastSynchronized { get; set; }
            public int ScheduleId { get; set; }
            public string ScheduleName { get; set; }
            public int RetentionId { get; set; }
            public string RetentionRuleName { get; set; }
            public long OnlineDataSize { get; set; }
            public int OnlineFileCount { get; set; }
            public long CompressedSize { get; set; }
            public long LocalStorageDataSize { get; set; }
            public int LocalStorageFileCount { get; set; }
            public string SetType { get; set; }
            public bool UseLocalStorage { get; set; }
            public bool HasItems { get; set; }
            public bool IsCDP { get; set; }
            public bool CreatedByPolicy { get; set; }
            public int OwnerId { get; set; }
            public string OwnerName { get; set; }

            public DSClientBackupSetInfo(backup_set_info backupSetInfo)
            {
                BackupSetId = backupSetInfo.id;
                Computer = backupSetInfo.computer;
                Name = backupSetInfo.name;
                Active = backupSetInfo.is_active;
                LastBackup = UnixEpochToDateTime(backupSetInfo.last_backup);
                LastSuccess = UnixEpochToDateTime(backupSetInfo.last_successful_backup);
                DataType = EBackupDataTypeToString(backupSetInfo.data_type);
                Synchronized = backupSetInfo.in_sync;
                LastSynchronized = UnixEpochToDateTime(backupSetInfo.last_sync_time);
                ScheduleId = backupSetInfo.schedule_id;
                ScheduleName = backupSetInfo.schedule_name;
                RetentionId = backupSetInfo.retention_rule_id;
                RetentionRuleName = backupSetInfo.retention_rule_name;
                OnlineDataSize = backupSetInfo.online_data_size;
                OnlineFileCount = backupSetInfo.online_file_count;
                CompressedSize = backupSetInfo.dssys_compressed_size;
                LocalStorageDataSize = backupSetInfo.local_storage_data_size;
                LocalStorageFileCount = backupSetInfo.local_storage_file_count;
                SetType = EBackupSetTypeToString(backupSetInfo.set_type);
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