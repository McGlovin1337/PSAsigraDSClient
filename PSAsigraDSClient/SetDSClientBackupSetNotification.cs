using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientNotification;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientBackupSetNotification", SupportsShouldProcess = true)]
    [OutputType(typeof(DSClientBackupSetNotification))]

    sealed public class SetDSClientBackupSetNotification : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set with the Notification to Modify")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Notification to Modify")]
        public int NotificationId { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Method")]
        [ValidateSet("Email", "Page", "Broadcast", "Event")]
        public string NotificationMethod { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Recipient")]
        public string NotificationRecipient { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Completion Status to Notify on")]
        [ValidateSet("Incomplete", "CompletedWithErrors", "Successful", "CompletedWithWarnings")]
        public string[] NotificationCompletion { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Email Notification Options")]
        [ValidateSet("DetailedInfo", "AttachDetailedLog", "CompressAttachment", "HtmlFormat")]
        public string[] NotificationEmailOptions { get; set; }

        [Parameter(HelpMessage = "Specify to Output Created Notification")]
        public SwitchParameter PassThru { get; set; }

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Performing Action: Retrieve Backup Set");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            BackupSetNotification backupSetNotification = backupSet.getNotification();

            WriteVerbose($"Performing Action: Retrieve Backup Set Notification with Id '{NotificationId}'");
            notification_info notification = backupSetNotification.listNotification()
                                                                        .Single(n => n.id == NotificationId);

            if (NotificationMethod != null)
            {
                if (ShouldProcess($"Notification Id '{notification.id}'", $"Set Notification Method to '{NotificationMethod}'"))
                    notification.method = StringToEnum<ENotificationMethod>(NotificationMethod);
            }

            if (NotificationRecipient != null)
            {
                if (ShouldProcess($"Notification Id '{notification.id}'", $"Set Notification Recipient to '{NotificationRecipient}'"))
                    notification.recipient = NotificationRecipient;
            }

            if (NotificationCompletion != null)
            {
                if (ShouldProcess($"Notification Id '{notification.id}'", "Update Notification on Completion Status"))
                    notification.completion = ArrayToNotificationCompletionToInt(NotificationCompletion);
            }

            if (NotificationEmailOptions != null)
            {
                if (ShouldProcess($"Notification Id '{notification.id}'", "Update Notification Email Options"))
                    notification.email_option = ArrayToEmailOptionsInt(NotificationEmailOptions);
            }

            if (ShouldProcess($"Notification Id '{notification.id}'", "Update Notification Configuration"))
                backupSetNotification.addOrUpdateNotification(notification);

            if (PassThru)
                WriteObject(new DSClientBackupSetNotification(notification));

            backupSetNotification.Dispose();
            backupSet.Dispose();
        }
    }
}