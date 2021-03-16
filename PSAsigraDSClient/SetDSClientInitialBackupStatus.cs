using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientInitialBackupStatus", SupportsShouldProcess = true)]

    public class SetDSClientInitialBackupStatus : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set Id")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Status of the Initial Backup")]
        public SwitchParameter Completed { get; set; }

        protected override void DSClientProcessRecord()
        {
            InitialBackupManager initialBackupManager = DSClientSession.getInitialBackupManager();

            if (ShouldProcess($"BackupSetId '{BackupSetId}'", $"Update Completed Status to '{Completed}'"))
            {
                EInitBackupStatus status = EInitBackupStatus.EInitBackupStatus__Completed;

                if (!Completed)
                    status = EInitBackupStatus.EInitBackupStatus__Incompleted;

                initialBackupManager.updateStatus(BackupSetId, status);
            }

            initialBackupManager.Dispose();
        }
    }
}