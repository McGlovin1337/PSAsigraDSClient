using System;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientSMTPNotification")]

    public class SetDSClientSMTPNotification: BaseDSClientSMTPNotification
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, HelpMessage = "SMTP Server Hostname or IP Address")]
        public string SmtpServer { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "SMTP Server TCP Port")]
        [ValidateRange(1, 65535)]
        public int SmtpPort { get; set; } = 25;

        [Parameter(Position = 2, HelpMessage = "SMTP Server Authentication Credentials")]
        public PSCredential SmtpCredential { get; set; }

        [Parameter(Position = 3, HelpMessage = "SMTP Server Requires SSL?")]
        public SwitchParameter RequireSsl { get; set; }

        [Parameter(Position = 4, HelpMessage = "SMTP Server Requires TLS?")]
        public SwitchParameter RequireTls { get; set; }

        [Parameter(Position = 5, ValueFromPipelineByPropertyName = true, HelpMessage = "Email From Display Name")]
        public string FromName { get; set; }

        [Parameter(Position = 6, ValueFromPipelineByPropertyName = true, HelpMessage = "Email From Address")]
        public string FromAddress { get; set; }

        [Parameter(Position = 7, ValueFromPipelineByPropertyName = true, HelpMessage = "Administrator Email Address(es)")]
        public string AdminEmail { get; set; }

        [Parameter(Position = 8, ValueFromPipelineByPropertyName = true, HelpMessage = "Pager Email Address(es)")]
        public string PagerEmail { get; set; }

        [Parameter(Position = 9, HelpMessage = "Send Summary Email")]
        public SwitchParameter SendSummary { get; set; }

        [Parameter(Position = 10, HelpMessage = "Send Backup Detail with Summary")]
        public SwitchParameter SendDetail { get; set; }

        [Parameter(Position = 11, HelpMessage = "Send Summary in HTML Format")]
        public SwitchParameter SendHtmlSummary { get; set; }

        [Parameter(Position = 12, ValueFromPipelineByPropertyName = true, HelpMessage = "Subject line for Admin Activity notifcations (256 character limit)")]
        [ValidateLength(0, 256)]
        public string SubjectAdmin { get; set; }

        [Parameter(Position = 13, ValueFromPipelineByPropertyName = true, HelpMessage = "Subject line for Backup Activity notifications (256 character limit)")]
        [ValidateLength(0, 256)]
        public string SubjectBackup { get; set; }

        protected override void ProcessSMTPConfig(DSClientSMTPConfig dSClientSMTPConfig)
        {
            // Copy the current Config and update all specified properties
            DSClientSMTPConfig newDSClientSMTPConfig = dSClientSMTPConfig;

            if (MyInvocation.BoundParameters.ContainsKey("SmtpServer"))
            {
                WriteVerbose("Validating SmtpServer is a valid hostname or IP Address...");
                bool validateSmtpHostname = DSClientCommon.ValidateHostname.ValidateHost(SmtpServer);

                if (validateSmtpHostname == true)
                    newDSClientSMTPConfig.SmtpServer = SmtpServer;
                else
                    throw new Exception("SmtpServer is not a valid IP Address or Hostname");
            }

            if (MyInvocation.BoundParameters.ContainsKey("SmtpPort"))
                newDSClientSMTPConfig.SmtpPort = SmtpPort;

            if (MyInvocation.BoundParameters.ContainsKey("SmtpCredential"))
            {
                newDSClientSMTPConfig.SmtpUsername = SmtpCredential.UserName;
                newDSClientSMTPConfig.SmtpPassword = SmtpCredential.GetNetworkCredential().Password;
            }

            if (RequireSsl == true && RequireTls == true)
                throw new Exception("RequireSsl and RequireTls cannot both be enabled");

            if (MyInvocation.BoundParameters.ContainsKey("RequireSsl"))
            {
                newDSClientSMTPConfig.RequireSsl = RequireSsl;
                if (RequireSsl == true)
                    newDSClientSMTPConfig.RequireTls = false;
            }

            if (MyInvocation.BoundParameters.ContainsKey("RequireTls"))
            {
                newDSClientSMTPConfig.RequireTls = RequireTls;
                if (RequireTls == true)
                    newDSClientSMTPConfig.RequireSsl = false;
            }

            if (MyInvocation.BoundParameters.ContainsKey("FromName"))
                newDSClientSMTPConfig.FromName = FromName;

            if (MyInvocation.BoundParameters.ContainsKey("FromAddress"))
                newDSClientSMTPConfig.FromAddress = FromAddress;

            if (MyInvocation.BoundParameters.ContainsKey("AdminEmail"))
                newDSClientSMTPConfig.AdminEmail = AdminEmail;

            if (MyInvocation.BoundParameters.ContainsKey("PagerEmail"))
                newDSClientSMTPConfig.PagerEmail = PagerEmail;

            if (MyInvocation.BoundParameters.ContainsKey("SendSummary"))
                newDSClientSMTPConfig.SendSummary = SendSummary;

            if (MyInvocation.BoundParameters.ContainsKey("SendDetail"))
            {
                if (SendDetail == true)
                {
                    newDSClientSMTPConfig.SendSummary = true;
                    newDSClientSMTPConfig.SendDetail = true;
                }
                else
                {
                    newDSClientSMTPConfig.SendDetail = false;
                }
            }

            if (MyInvocation.BoundParameters.ContainsKey("SendHtmlSummary"))
                newDSClientSMTPConfig.SendHtmlSummary = SendHtmlSummary;

            if (MyInvocation.BoundParameters.ContainsKey("SubjectAdmin"))
                newDSClientSMTPConfig.SubjectAdmin = SubjectAdmin;

            if (MyInvocation.BoundParameters.ContainsKey("SubjectBackup"))
                newDSClientSMTPConfig.SubjectBackup = SubjectBackup;


            // Set the Asigra Api classes from the DSClientSMTPConfig class properties
            smtp_server_info smtpServerInfo = new smtp_server_info {
                account_name = newDSClientSMTPConfig.SmtpUsername,
                address = newDSClientSMTPConfig.SmtpServer,
                password = newDSClientSMTPConfig.SmtpPassword,
                port = newDSClientSMTPConfig.SmtpPort,
                require_ssl = newDSClientSMTPConfig.RequireSsl,
                require_tls = newDSClientSMTPConfig.RequireTls
            };

            email_notification_info emailNotificationInfo = new email_notification_info {
                admin_email_addr = newDSClientSMTPConfig.AdminEmail,
                pager_email_addr = newDSClientSMTPConfig.PagerEmail,
                send_backup_detail_with_summary = newDSClientSMTPConfig.SendDetail,
                send_summary = newDSClientSMTPConfig.SendSummary,
                send_summary_in_html_format = newDSClientSMTPConfig.SendHtmlSummary,
                subject_admin = newDSClientSMTPConfig.SubjectAdmin,
                subject_backup = newDSClientSMTPConfig.SubjectBackup
            };

            smtp_email_notification_info smtpEmailNotifyInfo = new smtp_email_notification_info
            {
                from_display_name = newDSClientSMTPConfig.FromName,
                from_email_address = newDSClientSMTPConfig.FromAddress,
                notification_info = emailNotificationInfo,
                smtp_server = smtpServerInfo
            };

            // Assign the new SMTP configuration
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            NotificationConfiguration DSClientNotifyConfigMgr = DSClientConfigMgr.getNotificationConfiguration();

            WriteVerbose("Setting DS-Client SMTP Notification Configuration...");
            DSClientNotifyConfigMgr.setSMTPEmailNotification(smtpEmailNotifyInfo);
            WriteObject("DS-Client SMTP Notification Configuration Updated");

            DSClientNotifyConfigMgr.Dispose();
            DSClientNotifyConfigMgr.Dispose();
        }
    }
}