using System;
using System.Collections.Generic;
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

        [Parameter(Mandatory = true, ParameterSetName = "RestoreId", HelpMessage = "Specify an existing Restore Session Id")]
        public int RestoreId { get; set; }

        protected virtual void ProcessRestoreSessionData(ref DSClientRestoreSession restoreSession)
        {
            throw new NotImplementedException("This Method Should be Overridden");
        }

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
            else if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreId)))
            {
                // Get the Restore View from a Restore Session
                List<DSClientRestoreSession> restoreSessions = SessionState.PSVariable.GetValue("RestoreSessions", null) as List<DSClientRestoreSession>;

                if (restoreSessions == null)
                    throw new Exception("No Restore Sessions found");

                // Get the Specified Restore Session by Id
                bool found = false;
                for (int i = 0; i < restoreSessions.Count; i++)
                {
                    if (restoreSessions[i].RestoreId == RestoreId)
                    {
                        DSClientRestoreSession restoreSession = restoreSessions[i];
                        ProcessRestoreSessionData(ref restoreSession);
                        restoreSessions[i] = restoreSession;
                        found = true;
                        break;
                    }
                }

                // Update the Session State if a Restore Session was found
                if (found)
                    SessionState.PSVariable.Set("RestoreSessions", restoreSessions);
                else
                    throw new Exception("Specified RestoreId not found");
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