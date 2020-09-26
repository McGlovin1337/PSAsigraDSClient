using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientBackupSetRetention")]

    public class StartDSClientBackupSetRetention: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set Id")]
        [ValidateNotNullOrEmpty]
        public int Id { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Retrieving Backup Set from DS-Client...");
            BackupSet backupSet = DSClientSession.backup_set(Id);

            WriteVerbose("Starting Backup Set Retention Activity...");
            GenericActivity retentionActivity = backupSet.enforceRetention();

            int retentionActivityId = retentionActivity.getID();

            WriteObject("Backup Set Retention Activity started with Id " + retentionActivityId);

            retentionActivity.Dispose();
            backupSet.Dispose();
        }
    }
}