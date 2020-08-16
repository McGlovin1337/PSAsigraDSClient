using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSet")]
    [OutputType(typeof(BackupSetInfo))]
    public class GetDSClientBackupSet: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Retrieving DSClient Backup Sets...");
            var BackupSetInfo = DSClientSession.backup_sets_info();

            List<BackupSetInfo> BackupSets = new List<BackupSetInfo>();

            foreach (var backupSet in BackupSetInfo)
            {
                BackupSets.Add(new BackupSetInfo
                {
                    computer = backupSet.computer,
                    data_type = backupSet.data_type,
                    dssys_compressed_size = backupSet.dssys_compressed_size,
                    has_items = backupSet.has_items,
                    id = backupSet.id,
                    in_sync = backupSet.in_sync,
                    is_active = backupSet.is_active,
                    is_cdp = backupSet.is_cdp,
                    is_created_by_policy = backupSet.is_created_by_policy,
                    last_backup = backupSet.last_backup,
                    last_successful_backup = backupSet.last_successful_backup,
                    last_sync_time = backupSet.last_sync_time,
                    local_storage_data_size = backupSet.local_storage_data_size,
                    local_storage_file_count = backupSet.local_storage_file_count,
                    name = backupSet.name,
                    online_data_size = backupSet.online_data_size,
                    online_file_count = backupSet.online_file_count,
                    owner_id = backupSet.owner_id,
                    owner_name = backupSet.owner_name,
                    retention_rule_id = backupSet.retention_rule_id,
                    retention_rule_name = backupSet.retention_rule_name,
                    schedule_id = backupSet.schedule_id,
                    schedule_name = backupSet.schedule_name,
                    set_type = backupSet.set_type,
                    using_local_storage = backupSet.using_local_storage
                });
            }

            WriteVerbose("Yielded " + BackupSets.Count() + " DSClient Backup Sets");

            BackupSets.ForEach(WriteObject);
        }

        protected class BackupSetInfo
        {
            public string computer { get; set; }
            public EBackupDataType data_type { get; set; }
            public long dssys_compressed_size { get; set; }
            public bool has_items { get; set; }
            public int id { get; set; }
            public bool in_sync { get; set; }
            public bool is_active { get; set; }
            public bool is_cdp { get; set; }
            public bool is_created_by_policy { get; set; }
            public int last_backup { get; set; }
            public int last_successful_backup { get; set; }
            public int last_sync_time { get; set; }
            public long local_storage_data_size { get; set; }
            public int local_storage_file_count { get; set; }
            public string name { get; set; }
            public long online_data_size { get; set; }
            public int online_file_count { get; set; }
            public int owner_id { get; set; }
            public string owner_name { get; set; }
            public int retention_rule_id { get; set; }
            public string retention_rule_name { get; set; }
            public int schedule_id { get; set; }
            public string schedule_name { get; set; }
            public EBackupSetType set_type { get; set; }
            public bool using_local_storage { get; set; }
        }
    }
}