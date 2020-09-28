using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSetDataBrowser: BaseDSClientInitializeBackupSetDataBrowser
    {
        [Parameter(Mandatory = true, ParameterSetName = "ValidationSession", HelpMessage = "Specify to use Validation View stored in SessionState")]
        public SwitchParameter UseValidationSession { get; set; }

        protected virtual void ProcessBackupSetData(BackedUpDataView DSClientBackedUpDataView)
        {
            throw new NotImplementedException("Method ProcessBackupSetData should be overridden");
        }

        protected override void DSClientProcessRecord()
        {
            if (UseValidationSession == true)
            {
                BackupSetValidationView validationView = SessionState.PSVariable.GetValue("ValidateView", null) as BackupSetValidationView;

                if (validationView != null)
                    ProcessBackupSetData(validationView);
            }
            else
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
}