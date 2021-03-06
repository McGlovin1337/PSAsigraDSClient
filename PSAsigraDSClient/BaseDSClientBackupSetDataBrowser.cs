﻿using System.Management.Automation;
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

        protected abstract void ProcessBackupSetData(BackedUpDataView DSClientBackedUpDataView);

        protected override void DSClientProcessRecord()
        {
            if (UseValidationSession == true)
            {
                // Get the Validation View from SessionState
                BackupSetValidationView validationView = SessionState.PSVariable.GetValue("ValidateView", null) as BackupSetValidationView;

                if (validationView != null)
                    ProcessBackupSetData(validationView);

                // Update the Validation View in SessionState
                SessionState.PSVariable.Set("ValidateView", validationView);
            }
            else if (UseDeleteSession)
            {
                // Get the Delete View from SessionState
                BackupSetDeleteView deleteView = SessionState.PSVariable.GetValue("DeleteView", null) as BackupSetDeleteView;

                if (deleteView != null)
                    ProcessBackupSetData(deleteView);

                // Update the Delete View in SessionState
                SessionState.PSVariable.Set("DeleteView", deleteView);
            }
            else if (UseRestoreSession)
            {
                // Get the Restore View from SessionState
                BackupSetRestoreView restoreView = SessionState.PSVariable.GetValue("RestoreView", null) as BackupSetRestoreView;

                if (restoreView != null)
                    ProcessBackupSetData(restoreView);

                // Update the Restore View in SessionState
                SessionState.PSVariable.Set("RestoreView", restoreView);
            }
            else
            {
                BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

                int deletedDate = 0;

                if (MyInvocation.BoundParameters.ContainsKey("DeletedDate"))
                    deletedDate = DateTimeToUnixEpoch(DeletedDate);

                WriteVerbose("Performing Action: Prepare Backup Set Data view");
                WriteVerbose("Notice: From: " + DateFrom + " To: " + DateTo);
                BackupSetRestoreView backupSetRestoreView = backupSet.prepare_restore(DateTimeToUnixEpoch(DateFrom), DateTimeToUnixEpoch(DateTo), deletedDate);

                ProcessBackupSetData(backupSetRestoreView);

                backupSetRestoreView.Dispose();
                backupSet.Dispose();
            }
        }
    }
}