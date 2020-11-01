using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSMTPNotification: DSClientCmdlet
    {
        protected abstract void ProcessSMTPConfig(smtp_email_notification_info smtpInfo);

        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            NotificationConfiguration DSClientNotifyConfigMgr = DSClientConfigMgr.getNotificationConfiguration();

            smtp_email_notification_info smtpConfig = DSClientNotifyConfigMgr.getSMTPEmailNotification();

            DSClientNotifyConfigMgr.Dispose();
            DSClientConfigMgr.Dispose();

            ProcessSMTPConfig(smtpConfig);
        }

        protected class DSClientSMTPConfig
        {
            public string SmtpServer { get; private set; }
            public int SmtpPort { get; private set; }
            public string SmtpUsername { get; private set; }
            public string SmtpPassword { get; private set; } = "****";
            public bool RequireSsl { get; private set; }
            public bool RequireTls { get; private set; }
            public string FromName { get; private set; }
            public string FromAddress { get; private set; }
            public string AdminEmail { get; private set; }
            public string PagerEmail { get; private set; }
            public bool SendDetail { get; private set; }
            public bool SendSummary { get; private set; }
            public bool SendHtmlSummary { get; private set; }
            public string SubjectAdmin { get; private set; }
            public string SubjectBackup { get; private set; }

            public DSClientSMTPConfig(smtp_email_notification_info smtpConfig)
            {
                SmtpServer = smtpConfig.smtp_server.address;
                SmtpPort = smtpConfig.smtp_server.port;
                SmtpUsername = smtpConfig.smtp_server.account_name;                
                RequireSsl = smtpConfig.smtp_server.require_ssl;
                RequireTls = smtpConfig.smtp_server.require_tls;
                FromName = smtpConfig.from_display_name;
                FromAddress = smtpConfig.from_email_address;
                AdminEmail = smtpConfig.notification_info.admin_email_addr;
                PagerEmail = smtpConfig.notification_info.pager_email_addr;
                SendDetail = smtpConfig.notification_info.send_backup_detail_with_summary;
                SendSummary = smtpConfig.notification_info.send_summary;
                SendHtmlSummary = smtpConfig.notification_info.send_summary_in_html_format;
                SubjectAdmin = smtpConfig.notification_info.subject_admin;
                SubjectBackup = smtpConfig.notification_info.subject_backup;
            }
        }
    }
}