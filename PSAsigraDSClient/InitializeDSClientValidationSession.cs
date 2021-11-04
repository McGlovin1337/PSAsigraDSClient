using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Initialize, "DSClientValidationSession")]
    [OutputType(typeof(DSClientValidationSession))]

    sealed public class InitializeDSClientValidationSession : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to Initialize a Validation Session for")]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Backup Set Details
            WriteVerbose("Performing Action: Retrieve Backup Set Details");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            // Check if Backup Set is in use
            if (backupSet.check_lock_status(EActivityType.EActivityType__Validation) == EBackupSetLockStatus.EBackupSetLockStatus__Locked)
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    new Exception("Backup Set is Currently Locked"),
                    "Exception",
                    ErrorCategory.ResourceBusy,
                    backupSet);
                WriteError(errorRecord);
                backupSet.Dispose();
            }
            else
            {
                // Create the Validation Session
                WriteVerbose("Performing Action: Create Validation Session");
                DSClientValidationSession validationSession = new DSClientValidationSession(DSClientSessionInfo.GenerateValidationId(), backupSet);
                WriteVerbose($"Notice: Validation Session Id: {validationSession.ValidationId}");

                DSClientSessionInfo.AddValidationSession(validationSession);

                WriteObject(validationSession);
            }
        }
    }
}