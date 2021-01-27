using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Enable, "DSClientBackupSet", SupportsShouldProcess = true)]

    public class EnableDSClientBackupSet: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to Enable")]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Backup Set from DS-Client
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            if (ShouldProcess($"Backup Set '{backupSet.getName()}'", "Enable"))
            {
                // Set the Backup Set to Active
                WriteVerbose($"Performing Action: Enable On Target: {BackupSetId}");
                backupSet.setActive(true);
            }

            backupSet.Dispose();
        }
    }
}