using System;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientBackupSetDataBrowser: BaseDSClientInitializeBackupSetDataBrowser
    {
        [Parameter(Mandatory = true, ParameterSetName = "DeleteId", HelpMessage = "Specify an existing Delete Session Id")]
        public int DeleteId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "RestoreId", HelpMessage = "Specify an existing Restore Session Id")]
        public int RestoreId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "ValidationId", HelpMessage = "Specify and existing Validation Session Id")]
        public int ValidationId { get; set; }

        protected abstract void ProcessBackupSetData(BackedUpDataView DSClientBackedUpDataView);

        protected override void DSClientProcessRecord()
        {
            if (MyInvocation.BoundParameters.ContainsKey(nameof(ValidationId)))
            {
                // Get the Validation View from Validation Session
                DSClientValidationSession validationSession = DSClientSessionInfo.GetValidationSession(ValidationId);

                if (validationSession != null)
                    ProcessBackupSetData(validationSession.GetValidationView());
                else
                {
                    ErrorRecord errorRecord = new ErrorRecord(
                        new ArgumentException("Specified Validation Session not found"),
                        "PSArgumentException",
                        ErrorCategory.ObjectNotFound,
                        null);
                    WriteError(errorRecord);
                }
            }
            else if (MyInvocation.BoundParameters.ContainsKey(nameof(DeleteId)))
            {
                // Get the Delete View from Delete Session
                DSClientDeleteSession deleteSession = DSClientSessionInfo.GetDeleteSession(DeleteId);

                if (deleteSession != null)
                    ProcessBackupSetData(deleteSession.GetDeleteView());
                else
                {
                    ErrorRecord errorRecord = new ErrorRecord(
                        new ArgumentException("Specified Delete Session not found"),
                        "PSArgumentException",
                        ErrorCategory.ObjectNotFound,
                        null);
                    WriteError(errorRecord);
                }
            }
            else if (MyInvocation.BoundParameters.ContainsKey(nameof(RestoreId)))
            {
                // Get the Restore View from a Restore Session
                DSClientRestoreSession restoreSession = DSClientSessionInfo.GetRestoreSession(RestoreId);
                if (restoreSession != null)
                    ProcessBackupSetData(restoreSession.GetRestoreView());
                else
                {
                    ErrorRecord errorRecord = new ErrorRecord(
                        new ArgumentException("Specified Restore Session not found"),
                        "PSArgumentException",
                        ErrorCategory.ObjectNotFound,
                        null);
                    WriteError(errorRecord);
                }
            }
            else
            {
                BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

                if (backupSet.check_lock_status(EActivityType.EActivityType__Restore) == EBackupSetLockStatus.EBackupSetLockStatus__Locked)
                {
                    ErrorRecord errorRecord = new ErrorRecord(
                        new Exception("Backup Set is Currently Locked"),
                        "Exception",
                        ErrorCategory.ResourceBusy,
                        backupSet);
                    WriteError(errorRecord);
                }
                else
                {
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
}