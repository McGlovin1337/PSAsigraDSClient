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

        [Parameter(Mandatory = true, ParameterSetName = "DeleteSession", HelpMessage = "Specify to use Delete View stored in SessionState")]
        public SwitchParameter UseDeleteSession { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RestoreSession", HelpMessage = "Specify to use Restore View stored in SessionState")]
        public SwitchParameter UseRestoreSession { get; set; }

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
            else if (UseDeleteSession)
            {
                BackupSetDeleteView deleteView = SessionState.PSVariable.GetValue("DeleteView", null) as BackupSetDeleteView;

                if (deleteView != null)
                    ProcessBackupSetData(deleteView);
            }
            else if (UseRestoreSession)
            {
                BackupSetRestoreView restoreView = SessionState.PSVariable.GetValue("RestoreView", null) as BackupSetRestoreView;

                if (restoreView != null)
                    ProcessBackupSetData(restoreView);
            }
            else
            {
                BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

                WriteVerbose("Preparing Backup Set Data view...");
                WriteVerbose("From: " + DateFrom + " To: " + DateTo);
                BackupSetRestoreView backupSetRestoreView = backupSet.prepare_restore(DateTimeToUnixEpoch(DateFrom), DateTimeToUnixEpoch(DateTo), 0);

                ProcessBackupSetData(backupSetRestoreView);

                backupSetRestoreView.Dispose();
                backupSet.Dispose();
            }
        }
    }
}