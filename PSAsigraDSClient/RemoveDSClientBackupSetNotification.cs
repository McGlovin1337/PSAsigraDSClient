using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientBackupSetNotification", SupportsShouldProcess = true)]

    public class RemoveDSClientBackupSetNotification : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set Id")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Id of the Notification to Remove")]
        public int NotificationId { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve Backup Set");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            BackupSetNotification backupSetNotification = backupSet.getNotification();

            WriteVerbose($"Performing Action: Retrieve Backup Set Notification with Id '{NotificationId}'");
            notification_info notification = backupSetNotification.listNotification()
                                                                        .Single(n => n.id == NotificationId);

            if (ShouldProcess($"Backup Set Id '{backupSet.getID()}'", $"Remove Notification with Id '{notification.id}'"))
                backupSetNotification.removeNotification(notification.id);

            backupSetNotification.Dispose();
            backupSet.Dispose();
        }
    }
}