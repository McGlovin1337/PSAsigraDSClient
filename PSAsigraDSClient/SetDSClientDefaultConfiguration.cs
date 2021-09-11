using System;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientNotification;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientDefaultConfiguration", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class SetDSClientDefaultConfiguration: BaseDSClientDefaultConfiguration
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, HelpMessage = "Default Compression Level")]
        [ValidateSet("None", "ZLIB", "LZOP", "ZLIB_LO", "ZLIB_MED", "ZLIB_HI")]
        public string CompressionType { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "DS-Client Buffer Path")]
        public string DSClientBuffer { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Local Storage Path")]
        public string LocalStoragePath { get; set; }

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Method")]
        [ValidateSet("Email", "Page", "Broadcast", "Event")]
        public string NotificationMethod { get; set; }

        [Parameter(Position = 4, ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Recipient")]
        public string NotificationRecipient { get; set; }

        [Parameter(Position = 5, ValueFromPipelineByPropertyName = true, HelpMessage = "Completion Status to Notify on")]
        [ValidateSet("Incomplete", "CompletedWithErrors", "Successful", "CompletedWithWarnings")]
        public string[] NotificationCompletion { get; set; }

        [Parameter(Position = 6, ValueFromPipelineByPropertyName = true, HelpMessage = "Email Notification Options")]
        [ValidateSet("DetailedInfo", "AttachDetailedLog", "CompressAttachment", "HtmlFormat")]
        public string[] NotificationEmailOptions { get; set; }

        [Parameter(Position = 7, ValueFromPipelineByPropertyName = true, HelpMessage = "Number of Online Generations")]
        [ValidateRange(1, 9999)]
        public int OnlineGenerations { get; set; }

        [Parameter(Position = 8, ValueFromPipelineByPropertyName = true, HelpMessage = "Default Retention Rule by RetentionRuleId")]
        [ValidateNotNullOrEmpty]
        public int RetentionRuleId { get; set; }

        [Parameter(Position = 9, ValueFromPipelineByPropertyName = true, HelpMessage = "Default Retention Rule by Name")]
        public string RetentionRule { get; set; }

        [Parameter(Position = 10, ValueFromPipelineByPropertyName = true, HelpMessage = "Default Schedule by ScheduleId")]
        [ValidateNotNullOrEmpty]
        public int ScheduleId { get; set; }

        [Parameter(Position = 11, ValueFromPipelineByPropertyName = true, HelpMessage = "Default Schedule by Name")]
        public string Schedule { get; set; }

        [Parameter(Position = 12, ValueFromPipelineByPropertyName = true, HelpMessage = "Open File Operation/Method (Windows Only)")]
        [ValidateSet("TryDenyWrite", "DenyWrite", "PreventWrite", "AllowWrite")]
        public string OpenFileOperation { get; set; }

        [Parameter(Position = 13, ValueFromPipelineByPropertyName = true, HelpMessage = "Open File retry interval in seconds (Windows Only)")]
        public int OpenFileRetryInterval { get; set; }

        [Parameter(Position = 14, ValueFromPipelineByPropertyName = true, HelpMessage = "Number of times to retry Open File Operation (Windows Only)")]
        public int OpenFileRetryTimes { get; set; }

        [Parameter(Position = 15, HelpMessage = "Value for backing up File Permissions (Windows Only)")]
        public SwitchParameter BackupFilePermissions { get; set; }

        protected override void ProcessDefaultConfiguration(DSClientDefaultConfiguration dSClientDefaultConfiguration)
        {
            if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId") && MyInvocation.BoundParameters.ContainsKey("RetentionRule"))
                throw new Exception("Cannot specify both RetentionRuleId and RetentionRule");

            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId") && MyInvocation.BoundParameters.ContainsKey("Schedule"))
                throw new Exception("Cannot specify both ScheduleId and ScheduleName");

            ClientConfiguration DSClientConfigMgr = DSClientSession.getConfigurationManager();

            DefaultConfiguration defaultConfiguration = DSClientConfigMgr.getDefaultConfiguration();
            DefaultConfigurationWindowsClient defaultConfigurationWindows;

            BackupSetNotification backupSetNotification = defaultConfiguration.getDefaultNotification();
            notification_info[] notifyInfo = backupSetNotification.listNotification();
            bool notifyInfoUpdated = false;

            // Set Compression Type/Method
            if (CompressionType != null)
            {
                if (ShouldProcess("Default Compression Type", $"Set Value '{CompressionType}'"))
                {
                    WriteVerbose("Performing Action: Set Default Compression Type");
                    defaultConfiguration.setDefaultCompressionType(StringToEnum<ECompressionType>(CompressionType));
                }
            }

            // Set DS-Client Buffer Path
            if (DSClientBuffer != null)
            {
                if (ShouldProcess("DS-Client Buffer Path", $"Set Value '{DSClientBuffer}'"))
                {
                    WriteVerbose("Performing Action: Set DS-Client Buffer");
                    defaultConfiguration.setDefaultDSClientBuffer(DSClientBuffer);
                }
            }

            // Set DS-Client Local Storage Path
            if (LocalStoragePath != null)
            {
                if (ShouldProcess("DS-Client Local Storage Path", $"Set Value '{LocalStoragePath}'"))
                {
                    WriteVerbose("Performing Action: Set DS-Client Local Storage Path");
                    defaultConfiguration.setDefaultLocalStoragePath(LocalStoragePath);
                }
            }

            // Set DS-Client Default Notification Method
            if (NotificationMethod != null)
            {
                if (ShouldProcess("DS-Client Default Notification Method", $"Set Value '{NotificationMethod}'"))
                {
                    WriteVerbose("Performing Action: Set Default Notification Method");
                    notifyInfo[0].method = StringToEnum<ENotificationMethod>(NotificationMethod);
                    notifyInfoUpdated = true;
                }
            }

            // Set the Notification Recipient for the Default Notification Method
            if (NotificationRecipient != null)
            {
                if (ShouldProcess("DS-Client Default Notification Recipient", $"Set Value '{NotificationRecipient}'"))
                {
                    WriteVerbose("Performing Action: Set Notification Recipient");
                    notifyInfo[0].recipient = NotificationRecipient;
                    notifyInfoUpdated = true;
                }
            }

            // Set the Completion Status for Notifications
            if (NotificationCompletion != null)
            {
                if (ShouldProcess("DS-Client Default Notification Completion Options", $"Set Value '{NotificationCompletion}"))
                {
                    WriteVerbose("Performing Action: Set Notification Completion Status");
                    notifyInfo[0].completion = ArrayToNotificationCompletionToInt(NotificationCompletion);
                    notifyInfoUpdated = true;
                }
            }

            // Set the Email options
            if (notifyInfo[0].method == ENotificationMethod.ENotificationMethod__Email && NotificationEmailOptions != null )
            {
                if (ShouldProcess("DS-Client Default Email Notification Options", $"Set Value '{NotificationEmailOptions}'"))
                {
                    WriteVerbose("Performing Action: Set Email Notification options");
                    notifyInfo[0].email_option = ArrayToEmailOptionsInt(NotificationEmailOptions);

                    notifyInfoUpdated = true;
                }
            }

            // Apply the updated Notification Config
            if (notifyInfoUpdated == true)
            {
                if (ShouldProcess("DS-Client Default Notification Configuration", "Update Notification Configuration"))
                {
                    WriteVerbose("Performing Action: Apply updated default notification settings");
                    backupSetNotification.addOrUpdateNotification(notifyInfo[0]);
                    defaultConfiguration.setDefaultNotification(backupSetNotification);
                }
            }

            // Set Default Online Generations
            if (OnlineGenerations > 0)
            {
                if (ShouldProcess("Backup Set Default Online Generations", $"Set Value '{OnlineGenerations}'"))
                {
                    WriteVerbose("Performing Action: Set Online Generations");
                    defaultConfiguration.setDefaultOnlineGenerations(OnlineGenerations);
                }
            }

            // Set the Default Retention Rule
            if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId") || MyInvocation.BoundParameters.ContainsKey("RetentionRule"))
            {
                RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
                RetentionRule[] retentionRules = DSClientRetentionRuleMgr.definedRules();

                if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
                {                    
                    RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);
                    if (ShouldProcess("Default Retention Rule for Backup Sets", $"Set Retention Rule '{retentionRule.getName()}'"))
                    {
                        WriteVerbose("Performing Action: Set Default Retention Rule");
                        defaultConfiguration.setDefaultRetentionRule(retentionRule);
                    }
                }
                else if (MyInvocation.BoundParameters.ContainsKey("RetentionRule"))
                {                    
                    RetentionRule retentionRule = null;
                    if (!string.IsNullOrEmpty(RetentionRule))
                        retentionRule = retentionRules.Single(rule => rule.getName() == RetentionRule);
                    if (ShouldProcess("Default Retention Rule for Backup Sets", $"Set Retention Rule '{retentionRule.getName()}'"))
                    {
                        WriteVerbose("Performing Action: Set Default Retention Rule");
                        defaultConfiguration.setDefaultRetentionRule(retentionRule);
                    }
                }

                DSClientRetentionRuleMgr.Dispose();
            }

            // Set the Default Schedule
            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId") || MyInvocation.BoundParameters.ContainsKey("Schedule"))
            {
                ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
                                
                if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
                {
                    Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);
                    if (ShouldProcess("Default Schedule for Backup Sets", $"Set Schedule '{schedule.getName()}'"))
                    {
                        WriteVerbose("Performing Action: Set Default Schedule");
                        defaultConfiguration.setDefaultSchedule(schedule);
                    }
                }
                else if (MyInvocation.BoundParameters.ContainsKey("Schedule"))
                {
                    Schedule[] schedules = DSClientScheduleMgr.definedSchedules();
                    Schedule schedule = null;
                    if (!string.IsNullOrEmpty(Schedule))
                        schedule = schedules.Single(sched => sched.getName() == Schedule);
                    if (ShouldProcess("Default Schedule for Backup Sets", $"Set Schedule '{schedule.getName()}'"))
                    {
                        WriteVerbose("Performing Action: Set Default Schedule");
                        defaultConfiguration.setDefaultSchedule(schedule);
                    }
                }

                DSClientScheduleMgr.Dispose();
            }

            // If this is a Windows DS-Client, extend and set additional properties
            if (DSClientSessionInfo.OperatingSystem == "Windows")
            {
                defaultConfigurationWindows = DefaultConfigurationWindowsClient.from(defaultConfiguration);

                // Set Open File Method
                if (OpenFileOperation != null)
                {
                    if (ShouldProcess("Default Open File Operation", $"Set Value '{OpenFileOperation}'"))
                    {
                        WriteVerbose("Performing Action: Set Open File Method");
                        defaultConfigurationWindows.setDefaultOpenFilesOperation(StringToEnum<EOpenFileStrategy>(OpenFileOperation));
                    }
                }

                // Set the Open File Retry Interval
                if (MyInvocation.BoundParameters.ContainsKey("OpenFileRetryInterval"))
                {
                    if (ShouldProcess("Default Open File Retry Interval", $"Set Value '{OpenFileRetryInterval}'"))
                    {
                        WriteVerbose("Performing Action: Set Open File Retry Interval");
                        defaultConfigurationWindows.setDefaultOpenFilesRetryInterval(OpenFileRetryInterval);
                    }
                }

                // Set the number of times to retry Open File Method
                if (MyInvocation.BoundParameters.ContainsKey("OpenFileRetryTimes"))
                {
                    if (ShouldProcess("Default Open File Retry Attempts", $"Set Value '{OpenFileRetryTimes}'"))
                    {
                        WriteVerbose("Performing Action: Set the number of times to retry Open File Method");
                        defaultConfigurationWindows.setDefaultOpenFilesRetryTimes(OpenFileRetryTimes);
                    }
                }

                // Set the Backup File Permissions default
                if (MyInvocation.BoundParameters.ContainsKey("BackupFilePermissions"))
                {
                    if (ShouldProcess("Backup File Permissions", $"Set Value '{BackupFilePermissions}'"))
                    {
                        WriteVerbose("Performing Action: Set Backup File Permissions");
                        defaultConfigurationWindows.setDefaultToBackupPermissions(BackupFilePermissions);
                    }
                }

                defaultConfigurationWindows.Dispose();
            }

            WriteObject("DS-Client Default Configuration Updated");

            backupSetNotification.Dispose();
            defaultConfiguration.Dispose();
            DSClientConfigMgr.Dispose();
        }
    }
}