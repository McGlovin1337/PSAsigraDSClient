using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsData.Initialize, "DSClientDeleteSession")]
    [OutputType(typeof(DSClientDeleteSession))]

    sealed public class InitializeDSClientDeleteSession : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to Initialize a Delete Session for")]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Backup Set Details
            WriteVerbose("Performing Action: Retrieve Backup Set Details");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            // Check if Backup Set is in use
            if (backupSet.check_lock_status(EActivityType.EActivityType__Delete) == EBackupSetLockStatus.EBackupSetLockStatus__Locked)
            {
                backupSet.Dispose();
                throw new Exception("Backup Set is Currently Locked");
            }

            // Create the Delete Session
            WriteVerbose("Performing Action: Create Delete Session");
            DSClientDeleteSession deleteSession = new DSClientDeleteSession(DSClientSessionInfo.GenerateDeleteId(), backupSet);
            WriteVerbose($"Notice: Delete Session Id: {deleteSession.DeleteId}");

            DSClientSessionInfo.AddDeleteSession(deleteSession);

            WriteObject(deleteSession);
        }
    }
}