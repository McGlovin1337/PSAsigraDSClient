using System.Collections.Generic;
using AsigraDSClientApi;

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
                switch(notifyType)
                {
                    case ENotificationSendEmailThrough.ENotificationSendEmailThrough__None:
                        Type = "None";
                        break;
                    case ENotificationSendEmailThrough.ENotificationSendEmailThrough__MAPI:
                        Type = "MAPI";
                        break;
                    case ENotificationSendEmailThrough.ENotificationSendEmailThrough__SMTP:
                        Type = "SMTP";
                        break;
                    case ENotificationSendEmailThrough.ENotificationSendEmailThrough__UNDEFINED:
                        Type = "Undefined";
                        break;
                }
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

                switch (notifyInfo.method)
                {
                    case ENotificationMethod.ENotificationMethod__Email:
                        Method = "Email";
                        break;
                    case ENotificationMethod.ENotificationMethod__Page:
                        Method = "Pager";
                        break;
                    case ENotificationMethod.ENotificationMethod__Broadcast:
                        Method = "Broadcast";
                        break;
                    case ENotificationMethod.ENotificationMethod__Event:
                        Method = "Event";
                        break;
                    case ENotificationMethod.ENotificationMethod__UNDEFINED:
                        Method = "Undefined";
                        break;
                }

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
            ENotificationMethod eNotificationMethod = ENotificationMethod.ENotificationMethod__UNDEFINED;

            switch(notifyMethod)
            {
                case "Email":
                    eNotificationMethod = ENotificationMethod.ENotificationMethod__Email;
                    break;
                case "Pager":
                    eNotificationMethod = ENotificationMethod.ENotificationMethod__Page;
                    break;
                case "Broadcast":
                    eNotificationMethod = ENotificationMethod.ENotificationMethod__Broadcast;
                    break;
                case "Event":
                    eNotificationMethod = ENotificationMethod.ENotificationMethod__Event;
                    break;
                default:
                    eNotificationMethod = ENotificationMethod.ENotificationMethod__UNDEFINED;
                    break;
            }

            return eNotificationMethod;
        }

        public static int ArrayToNotificationCompletionToInt(string[] notifyCompletion)
        {
            int NotifyValue = 0;

            foreach (string notifyC in notifyCompletion)
            {
                switch(notifyC)
                {
                    case "Incomplete":
                        NotifyValue += 1;
                        break;
                    case "CompletedWithErrors":
                        NotifyValue += 2;
                        break;
                    case "Successful":
                        NotifyValue += 4;
                        break;
                    case "CompletedWithWarnings":
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