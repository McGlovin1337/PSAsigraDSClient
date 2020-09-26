using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Start, "DSClientBackupSetSync")]

    public class StartDSClientBackupSetSync: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set Id")]
        [ValidateNotNullOrEmpty]
        public int Id { get; set; }

        [Parameter(Position = 1, HelpMessage = "Specify Sync should be DS-System based")]
        public SwitchParameter DSSystemBased { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Retrieving Backup Set from DS-Client...");
            BackupSet backupSet = DSClientSession.backup_set(Id);
            GenericActivity syncActivity;

            WriteVerbose("Starting Backup Synchronization Activity...");
            if (MyInvocation.BoundParameters.ContainsKey("DSSystemBased"))
                syncActivity = backupSet.start_sync(DSSystemBased);
            else
                syncActivity = backupSet.start_sync(false);

            int syncActivityId = syncActivity.getID();

            WriteObject("Backup Set Sync started with Activity Id " + syncActivityId);

            syncActivity.Dispose();
            backupSet.Dispose();
        }
    }
}