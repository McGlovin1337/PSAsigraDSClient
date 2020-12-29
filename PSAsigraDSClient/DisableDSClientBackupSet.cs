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
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            // Set the Backup Set to InActive
            WriteVerbose($"Performing Action: Disable On Target: {BackupSetId}");
            backupSet.setActive(false);

            backupSet.Dispose();
        }
    }
}