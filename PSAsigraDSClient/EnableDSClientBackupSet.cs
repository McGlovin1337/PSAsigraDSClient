using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Enable, "DSClientBackupSet")]

    public class EnableDSClientBackupSet: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to Enable")]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Backup Set from DS-Client
            WriteVerbose("Retrieving Backup Set from DS-Client...");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            // Set the Backup Set to Active
            WriteVerbose("Performing action: Enable On Target: " + BackupSetId);
            backupSet.setActive(true);

            backupSet.Dispose();
        }
    }
}