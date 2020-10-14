using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Disable, "DSClientBackupSet")]

    public class DisableDSClientBackupSet: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to Disable")]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Backup Set from DS-Client
            WriteVerbose("Retrieving Backup Set from DS-Client...");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            // Set the Backup Set to InActive
            WriteVerbose("Performing action: Disable On Target: " + BackupSetId);
            backupSet.setActive(false);

            backupSet.Dispose();
        }
    }
}