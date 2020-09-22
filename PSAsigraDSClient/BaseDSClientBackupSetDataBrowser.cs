using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSetDataBrowser: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Backup Set to Search")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Date to Search From")]
        public DateTime DateFrom { get; set; } = DateTime.Parse("1/1/1970");

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Date to Search To")]
        public DateTime DateTo { get; set; } = DateTime.Now;

        [Parameter(Position = 4, HelpMessage = "Specify to Hide Files Deleted from Source")]
        public SwitchParameter HideDeleted { get; set; }

        [Parameter(Position = 5, HelpMessage = "Specify to Only Show Deleted files from Source")]
        public SwitchParameter ShowOnlyDeleted { get; set; }

        [Parameter(Position = 1, HelpMessage = "Specify Date for Deleted Files from Source")]
        public DateTime DeletedDate { get; set; } = DateTime.Now.AddDays(-30);

        protected abstract void ProcessBackupSetData(BackupSetRestoreView DSClientBackupSetRestoreView);

        protected override void DSClientProcessRecord()
        {
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            WriteVerbose("Preparing Backup Set Data view...");
            WriteVerbose("From: " + DateFrom + " To: " + DateTo);
            BackupSetRestoreView backupSetRestoreView = backupSet.prepare_restore(DateTimeToUnixEpoch(DateFrom), DateTimeToUnixEpoch(DateTo), 0);

            if (HideDeleted == true)
                backupSetRestoreView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__HideDeleted, DateTimeToUnixEpoch(DeletedDate));
            else if (ShowOnlyDeleted == true)
                backupSetRestoreView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__ShowDeletedOnly, DateTimeToUnixEpoch(DeletedDate));
            else
                backupSetRestoreView.setDeletedFileFilterType(EDeleteFilterType.EDeleteFilterType__ShowAll, 0);

            ProcessBackupSetData(backupSetRestoreView);

            backupSetRestoreView.Dispose();
            backupSet.Dispose();
        }
    }
}