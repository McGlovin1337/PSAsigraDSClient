using System.Collections.Generic;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientNotification: DSClientCmdlet
    {
        protected abstract void ProcessNotification(DSClientNotification dSClientNotification);

        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            NotificationConfiguration DSClientNotifyConfigMgr = DSClientConfigMgr.getNotificationConfiguration();

            ENotificationSendEmailThrough notifyType = DSClientNotifyConfigMgr.getSendingEmailThrough();

            DSClientNotification dSClientNotification = new DSClientNotification(notifyType);

            DSClientNotifyConfigMgr.Dispose();
            DSClientConfigMgr.Dispose();

            ProcessNotification(dSClientNotification);
        }

        protected class DSClientNotification
        {
            public string Type { get; set; }

            public DSClientNotification(ENotificationSendEmailThrough notifyType)
            {
                Type = EnumToString(notifyType);
            }
        }

        public class DSClientBackupSetNotification
        {
            public int NotificationId { get; private set; }
            public string Method { get; private set; }
            public string[] Completion { get; private set; }
            public string[] EmailOption { get; private set; }
            public string Recipient { get; private set; }

            public DSClientBackupSetNotification(notification_info notifyInfo)
            {
                NotificationId = notifyInfo.id;
                Method = EnumToString(notifyInfo.method);
                Completion = IntEBackupCompletionToArray(notifyInfo.completion);

                List<string> emailOpt = new List<string>();

                if ((notifyInfo.email_option & (int)ENotificationEmailOptions.ENotificationEmailOptions__DetailedInfo) > 0)
                    emailOpt.Add("DetailedInfo");
                if ((notifyInfo.email_option & (int)ENotificationEmailOptions.ENotificationEmailOptions__AttachDetailedLog) > 0)
                    emailOpt.Add("AttachDetailedLog");
                if ((notifyInfo.email_option & (int)ENotificationEmailOptions.ENotificationEmailOptions__CompressAttachment) > 0)
                    emailOpt.Add("CompressAttachment");
                if ((notifyInfo.email_option & (int)ENotificationEmailOptions.ENotificationEmailOptions__HtmlFormat) > 0)
                    emailOpt.Add("HtmlFormat");
                /*if ((notifyInfo.email_option & (int)ENotificationEmailOptions.ENotificationEmailOptions__UNDEFINED) > 0)
                    emailOpt.Add("Undefined");*/

                EmailOption = emailOpt.ToArray();
                Recipient = notifyInfo.recipient;
            }

            public override string ToString()
            {
                return Method;
            }
        }

        public static string[] IntEBackupCompletionToArray(int backupCompletion)
        {
            List<string> BackupCompletion = new List<string>();

            if ((backupCompletion & (int)EBackupCompletion.EBackupCompletion__Incomplete) > 0)
                BackupCompletion.Add("Incomplete");
            if ((backupCompletion & (int)EBackupCompletion.EBackupCompletion__CompletedWithErrors) > 0)
                BackupCompletion.Add("CompletedWithErrors");
            if ((backupCompletion & (int)EBackupCompletion.EBackupCompletion__Successful) > 0)
                BackupCompletion.Add("Successful");
            if ((backupCompletion & (int)EBackupCompletion.EBackupCompletion__CompletedWithWarnings) > 0)
                BackupCompletion.Add("CompletedWithWarnings");

            return BackupCompletion.ToArray();
        }

        public static ENotificationMethod StringToENotificationMethod(string notifyMethod)
        {
            switch(notifyMethod.ToLower())
            {
                case "email":
                    return ENotificationMethod.ENotificationMethod__Email;
                case "pager":
                    return ENotificationMethod.ENotificationMethod__Page;
                case "broadcast":
                    return ENotificationMethod.ENotificationMethod__Broadcast;
                case "event":
                    return ENotificationMethod.ENotificationMethod__Event;
                default:
                    return ENotificationMethod.ENotificationMethod__UNDEFINED;
            }
        }

        public static int ArrayToNotificationCompletionToInt(string[] notifyCompletion)
        {
            int NotifyValue = 0;

            foreach (string notifyC in notifyCompletion)
            {
                switch(notifyC.ToLower())
                {
                    case "incomplete":
                        NotifyValue += 1;
                        break;
                    case "completedwitherrors":
                        NotifyValue += 2;
                        break;
                    case "successful":
                        NotifyValue += 4;
                        break;
                    case "completedwithwarnings":
                        NotifyValue += 1024;
                        break;
                }
            }

            return NotifyValue;
        }

        public static int ArrayToEmailOptionsInt(string[] emailOptions)
        {
            int EmailOptions = 0;

            foreach (string option in emailOptions)
            {
                switch(option)
                {
                    case "DetailedInfo":
                        EmailOptions += 1;
                        break;
                    case "AttachDetailedLog":
                        EmailOptions += 16;
                        break;
                    case "CompressAttachment":
                        EmailOptions += 32;
                        break;
                    case "HtmlFormat":
                        EmailOptions += 128;
                        break;
                }
            }

            return EmailOptions;
        }
    }
}