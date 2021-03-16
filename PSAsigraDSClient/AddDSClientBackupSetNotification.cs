using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientNotification;
using System.Linq;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientBackupSetNotification", SupportsShouldProcess = true)]
    [OutputType(typeof(DSClientBackupSetNotification))]

    public class AddDSClientBackupSetNotification : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set Id")]
        public int BackupSetId { get; set; }

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

            notification_info[] existingNotifications = null;

            if (PassThru)
                existingNotifications = backupSetNotification.listNotification();

            notification_info newNotification = new notification_info
            {
                completion = ArrayToNotificationCompletionToInt(NotificationCompletion),
                email_option = (NotificationEmailOptions != null) ? ArrayToEmailOptionsInt(NotificationEmailOptions) : 0,
                id = 0,
                method = StringToEnum<ENotificationMethod>(NotificationMethod),
                recipient = NotificationRecipient
            };

            if (ShouldProcess($"Backup Set Id '{backupSet.getID()}'", "Add new Backup Set Notification"))
                backupSetNotification.addOrUpdateNotification(newNotification);

            if (PassThru)
            {
                notification_info addedNotification = backupSetNotification.listNotification()
                                                                            .Single(n => !existingNotifications.Any(e => e.id == n.id));
                WriteObject(new DSClientBackupSetNotification(addedNotification));
            }

            backupSetNotification.Dispose();
            backupSet.Dispose();
        }
    }
}