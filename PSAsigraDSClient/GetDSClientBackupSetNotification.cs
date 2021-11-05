using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.BaseDSClientNotification;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientBackupSetNotification")]
    [OutputType(typeof(DSClientBackupSetNotification))]

    sealed public class GetDSClientBackupSetNotification : DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to retrieve Notification Configuration")]
        public int BackupSetId { get; set; }

        protected override void DSClientProcessRecord()
        {
            BackupSet backupSet = null;

            // Retrieve the Backup Set
            try
            {
                WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
                backupSet = DSClientSession.backup_set(BackupSetId);
            }
            catch (APIException e)
            {
                ErrorRecord errorRecord = new ErrorRecord(
                    e,
                    "APIException",
                    ErrorCategory.ObjectNotFound,
                    null);
                WriteError(errorRecord);
            }

            if (backupSet != null)
            {
                BackupSetNotification backupSetNotification = backupSet.getNotification();

                WriteVerbose("Performing Action: Retrieve Backup Set Notification Configuration");
                notification_info[] notifications = backupSetNotification.listNotification();

                backupSetNotification.Dispose();
                backupSet.Dispose();

                List<DSClientBackupSetNotification> backupSetNotifications = new List<DSClientBackupSetNotification>();

                foreach (notification_info notification in notifications)
                    backupSetNotifications.Add(new DSClientBackupSetNotification(notification));

                backupSetNotifications.ForEach(WriteObject);
            }
        }
    }
}