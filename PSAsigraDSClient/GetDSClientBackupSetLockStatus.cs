using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSetLockStatus")]
    [OutputType(typeof(BackupSetLockStatus))]

    sealed public class GetDSClientBackupSetLockStatus : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set Id")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Activity Type to check Lock Status")]
        [ValidateSet("Backup", "Restore", "Delete", "Synchronization", "DiscTapeRestore", "BLMRequest", "BLMRestore", "Validation", "Retention", "SnapshotRestore", "SnapshotTransfer")]
        public string ActivityType { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Retrieve the Backup Set
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            // Activity Type
            EActivityType activityType = StringToEnum<EActivityType>(ActivityType);

            EBackupSetLockStatus setLockStatus = backupSet.check_lock_status(activityType);

            backupSet.Dispose();

            WriteObject(new BackupSetLockStatus(setLockStatus));
        }

        private class BackupSetLockStatus
        {
            public string LockStatus { get; private set; }

            public BackupSetLockStatus(EBackupSetLockStatus lockStatus)
            {
                LockStatus = EnumToString(lockStatus);
            }
        }
    }
}