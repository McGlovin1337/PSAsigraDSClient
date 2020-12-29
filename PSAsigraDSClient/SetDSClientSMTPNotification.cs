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

        protected override void ProcessSMTPConfig(smtp_email_notification_info smtpInfo)
        {
            // Update SMTP Server Settings
            smtp_server_info smtpServer = smtpInfo.smtp_server;
            if (SmtpServer != null)
            {
                WriteVerbose("Performing Action: Validate SmtpServer is a valid hostname or IP Address");
                bool validateSmtpHostname = DSClientCommon.ValidateHostname.ValidateHost(SmtpServer);

                if (validateSmtpHostname == true)
                    smtpServer.address = SmtpServer;
                else
                    throw new Exception("SmtpServer is not a valid IP Address or Hostname");
            }

            if (MyInvocation.BoundParameters.ContainsKey("SmtpPort"))
                smtpServer.port = SmtpPort;

            if (MyInvocation.BoundParameters.ContainsKey("SmtpCredential"))
            {
                smtpServer.account_name = SmtpCredential.UserName;
                smtpServer.password = SmtpCredential.GetNetworkCredential().Password;
            }

            if (RequireSsl == true && RequireTls == true)
                throw new Exception("RequireSsl and RequireTls cannot both be enabled");

            if (MyInvocation.BoundParameters.ContainsKey("RequireSsl"))
            {
                smtpServer.require_ssl = RequireSsl;
                if (RequireSsl == true)
                    smtpServer.require_tls = false;
            }
            else if (MyInvocation.BoundParameters.ContainsKey("RequireTls"))
            {
                smtpServer.require_tls = RequireTls;
                if (RequireTls == true)
                    smtpServer.require_ssl = false;
            }
            smtpInfo.smtp_server = smtpServer;

            // Set the From Details
            if (FromName != null)
                smtpInfo.from_display_name = FromName;

            if (FromAddress != null)
                smtpInfo.from_email_address = FromAddress;

            // Set the Recipient Details
            email_notification_info recipients = smtpInfo.notification_info;
            if (AdminEmail != null)
                recipients.admin_email_addr = AdminEmail;

            if (PagerEmail != null)
                recipients.pager_email_addr = PagerEmail;

            if (MyInvocation.BoundParameters.ContainsKey("SendSummary"))
                recipients.send_summary = SendSummary;

            if (MyInvocation.BoundParameters.ContainsKey("SendDetail"))
            {
                if (SendDetail == true)
                {
                    recipients.send_summary = true;
                    recipients.send_backup_detail_with_summary = true;
                }
                else
                {
                    recipients.send_backup_detail_with_summary = false;
                }
            }

            if (MyInvocation.BoundParameters.ContainsKey("SendHtmlSummary"))
                recipients.send_summary_in_html_format = SendHtmlSummary;

            if (SubjectAdmin != null)
                recipients.subject_admin = SubjectAdmin;

            if (SubjectBackup != null)
                recipients.subject_backup = SubjectBackup;

            smtpInfo.notification_info = recipients;

            // Assign the new SMTP configuration
            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            NotificationConfiguration DSClientNotifyConfigMgr = DSClientConfigMgr.getNotificationConfiguration();

            WriteVerbose("Performing Action: Set DS-Client SMTP Notification Configuration");
            DSClientNotifyConfigMgr.setSMTPEmailNotification(smtpInfo);
            WriteObject("DS-Client SMTP Notification Configuration Updated");

            DSClientNotifyConfigMgr.Dispose();
            DSClientNotifyConfigMgr.Dispose();
        }
    }
}