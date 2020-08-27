using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientSMTPNotification: DSClientCmdlet
    {
        protected abstract void ProcessSMTPConfig(DSClientSMTPConfig dSClientSMTPConfig);

        protected override void DSClientProcessRecord()
        {
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            NotificationConfiguration DSClientNotifyConfigMgr = DSClientConfigMgr.getNotificationConfiguration();

            smtp_email_notification_info smtpConfig = DSClientNotifyConfigMgr.getSMTPEmailNotification();

            DSClientSMTPConfig dSClientSMTPConfig = new DSClientSMTPConfig(smtpConfig);

            DSClientNotifyConfigMgr.Dispose();
            DSClientConfigMgr.Dispose();

            ProcessSMTPConfig(dSClientSMTPConfig);
        }

        protected class DSClientSMTPConfig
        {
            public string SmtpServer { get; set; }
            public int SmtpPort { get; set; }
            public string SmtpUsername { get; set; }
            public string SmtpPassword { get; set; } = "****";
            public bool RequireSsl { get; set; }
            public bool RequireTls { get; set; }
            public string FromName { get; set; }
            public string FromAddress { get; set; }
            public string AdminEmail { get; set; }
            public string PagerEmail { get; set; }
            public bool SendDetail { get; set; }
            public bool SendSummary { get; set; }
            public bool SendHtmlSummary { get; set; }
            public string SubjectAdmin { get; set; }
            public string SubjectBackup { get; set; }

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