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
            public int NotificationId { get; set; }
            public string Method { get; set; }
            public string[] Completion { get; set; }
            public string[] EmailOption { get; set; }
            public string Recipient { get; set; }

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

                List<string> completion = new List<string>();

                if ((notifyInfo.completion & (int)EBackupCompletion.EBackupCompletion__Incomplete) > 0)
                    completion.Add("Incomplete");
                if ((notifyInfo.completion & (int)EBackupCompletion.EBackupCompletion__CompletedWithErrors) > 0)
                    completion.Add("CompletedWithErrors");
                if ((notifyInfo.completion & (int)EBackupCompletion.EBackupCompletion__Successful) > 0)
                    completion.Add("Successful");
                if ((notifyInfo.completion & (int)EBackupCompletion.EBackupCompletion__CompletedWithWarnings) > 0)
                    completion.Add("CompletedWithWarnings");
                if ((notifyInfo.completion & (int)EBackupCompletion.EBackupCompletion__UNDEFINED) > 0)
                    completion.Add("Undefined");

                Completion = completion.ToArray();

                List<string> emailOpt = new List<string>();

                if ((notifyInfo.email_option & (int)ENotificationEmailOptions.ENotificationEmailOptions__DetailedInfo) > 0)
                    emailOpt.Add("DetailedInfo");
                if ((notifyInfo.email_option & (int)ENotificationEmailOptions.ENotificationEmailOptions__AttachDetailedLog) > 0)
                    emailOpt.Add("AttachDetailedLog");
                if ((notifyInfo.email_option & (int)ENotificationEmailOptions.ENotificationEmailOptions__CompressAttachment) > 0)
                    emailOpt.Add("CompressAttachment");
                if ((notifyInfo.email_option & (int)ENotificationEmailOptions.ENotificationEmailOptions__HtmlFormat) > 0)
                    emailOpt.Add("HtmlFormat");
                if ((notifyInfo.email_option & (int)ENotificationEmailOptions.ENotificationEmailOptions__UNDEFINED) > 0)
                    emailOpt.Add("Undefined");

                EmailOption = emailOpt.ToArray();

                Recipient = notifyInfo.recipient;
            }

            public override string ToString()
            {
                return Method;
            }
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

        public static int NotificationCompletionToInt(string[] notifyCompletion)
        {
            int notifyValue = 0;

            foreach (string notifyC in notifyCompletion)
            {
                switch(notifyC)
                {
                    case "Incomplete":
                        notifyValue += 1;
                        break;
                    case "CompletedWithErrors":
                        notifyValue += 2;
                        break;
                    case "Successful":
                        notifyValue += 4;
                        break;
                    case "CompletedWithWarnings":
                        notifyValue += 1024;
                        break;
                }
            }

            return notifyValue;
        }
    }
}