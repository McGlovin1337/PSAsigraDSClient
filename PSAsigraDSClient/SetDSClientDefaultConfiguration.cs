﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientNotification;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientDefaultConfiguration")]

    public class SetDSClientDefaultConfiguration: BaseDSClientDefaultConfiguration
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, HelpMessage = "Default Compression Level")]
        [ValidateSet("None", "ZLIB", "LZOP", "ZLIB_LO", "ZLIB_MED", "ZLIB_HI")]
        public string CompressionType { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "DS-Client Buffer Path")]
        public string DSClientBuffer { get; set; }

        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true, HelpMessage = "Local Storage Path")]
        public string LocalStoragePath { get; set; }

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true, HelpMessage = "Notification Method")]
        [ValidateSet("Email", "Pager", "Broadcast", "Event")]
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
                WriteVerbose("Setting Default Compression Type...");
                ECompressionType eCompressionType = StringToECompressionType(CompressionType);
                defaultConfiguration.setDefaultCompressionType(eCompressionType);
            }

            // Set DS-Client Buffer Path
            if (DSClientBuffer != null)
            {
                WriteVerbose("Setting DS-Client Buffer...");
                defaultConfiguration.setDefaultDSClientBuffer(DSClientBuffer);
            }

            // Set DS-Client Local Storage Path
            if (LocalStoragePath != null)
            {
                WriteVerbose("Setting DS-Client Local Storage Path...");
                defaultConfiguration.setDefaultLocalStoragePath(LocalStoragePath);
            }

            // Set DS-Client Default Notification Method
            if (NotificationMethod != null)
            {
                WriteVerbose("Setting Default Notification Method...");
                ENotificationMethod eNotificationMethod = StringToENotificationMethod(NotificationMethod);
                notifyInfo[0].method = eNotificationMethod;
                notifyInfoUpdated = true;
            }

            // Set the Notification Recipient for the Default Notification Method
            if (NotificationRecipient != null)
            {
                WriteVerbose("Setting Notification Recipient...");
                notifyInfo[0].recipient = NotificationRecipient;
                notifyInfoUpdated = true;
            }

            // Set the Completion Status for Notifications
            if (NotificationCompletion != null)
            {
                WriteVerbose("Setting Notification Completion Status...");
                int notifyCompletion = ArrayToNotificationCompletionToInt(NotificationCompletion);
                notifyInfo[0].completion = notifyCompletion;
                notifyInfoUpdated = true;
            }

            // Set the Email options
            if (notifyInfo[0].method == ENotificationMethod.ENotificationMethod__Email && NotificationEmailOptions != null )
            {
                WriteVerbose("Setting Email Notification options...");
                notifyInfo[0].email_option = ArrayToEmailOptionsInt(NotificationEmailOptions);

                notifyInfoUpdated = true;
            }

            // Apply the updated Notification Config
            if (notifyInfoUpdated == true)
            {
                WriteVerbose("Applying updated default notification settings...");
                backupSetNotification.addOrUpdateNotification(notifyInfo[0]);
                defaultConfiguration.setDefaultNotification(backupSetNotification);
            }

            // Set Default Online Generations
            if (OnlineGenerations > 0)
            {
                WriteVerbose("Setting Online Generations...");
                defaultConfiguration.setDefaultOnlineGenerations(OnlineGenerations);
            }

            // Set the Default Retention Rule
            if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId") || MyInvocation.BoundParameters.ContainsKey("RetentionRule"))
            {
                RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
                RetentionRule[] retentionRules = DSClientRetentionRuleMgr.definedRules();

                WriteVerbose("Setting Default Retention Rule...");
                if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
                {
                    RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);
                    defaultConfiguration.setDefaultRetentionRule(retentionRule);
                }

                if (MyInvocation.BoundParameters.ContainsKey("RetentionRule"))
                {
                    RetentionRule retentionRule = null;
                    if (!string.IsNullOrEmpty(RetentionRule))
                        retentionRule = retentionRules.Single(rule => rule.getName() == RetentionRule);
                    defaultConfiguration.setDefaultRetentionRule(retentionRule);
                }

                DSClientRetentionRuleMgr.Dispose();
            }

            // Set the Default Schedule
            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId") || MyInvocation.BoundParameters.ContainsKey("Schedule"))
            {
                ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

                WriteVerbose("Setting Default Schedule...");
                if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
                {
                    Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);
                    defaultConfiguration.setDefaultSchedule(schedule);
                }

                if (MyInvocation.BoundParameters.ContainsKey("Schedule"))
                {
                    Schedule[] schedules = DSClientScheduleMgr.definedSchedules();
                    Schedule schedule = null;
                    if (!string.IsNullOrEmpty(Schedule))
                        schedule = schedules.Single(sched => sched.getName() == Schedule);
                    defaultConfiguration.setDefaultSchedule(schedule);
                }

                DSClientScheduleMgr.Dispose();
            }

            // If this is a Windows DS-Client, extend and set additional properties
            if (DSClientOSType.OsType == "Windows")
            {
                defaultConfigurationWindows = DefaultConfigurationWindowsClient.from(defaultConfiguration);

                // Set Open File Method
                if (OpenFileOperation != null)
                {
                    WriteVerbose("Setting Open File Method...");
                    EOpenFileStrategy eOpenFileStrategy = StringToEOpenFileStrategy(OpenFileOperation);

                    defaultConfigurationWindows.setDefaultOpenFilesOperation(eOpenFileStrategy);
                }

                // Set the Open File Retry Interval
                if (MyInvocation.BoundParameters.ContainsKey("OpenFileRetryInterval"))
                {
                    WriteVerbose("Setting Open File Retry Interval...");
                    defaultConfigurationWindows.setDefaultOpenFilesRetryInterval(OpenFileRetryInterval);
                }

                // Set the number of times to retry Open File Method
                if (MyInvocation.BoundParameters.ContainsKey("OpenFileRetryTimes"))
                {
                    WriteVerbose("Setting the number of times to retry Open File Method...");
                    defaultConfigurationWindows.setDefaultOpenFilesRetryTimes(OpenFileRetryTimes);
                }

                // Set the Backup File Permissions default
                if (MyInvocation.BoundParameters.ContainsKey("BackupFilePermissions"))
                {
                    WriteVerbose("Setting Backup File Permissions...");
                    defaultConfigurationWindows.setDefaultToBackupPermissions(BackupFilePermissions);
                }

                defaultConfigurationWindows.Dispose();
            }

            WriteObject("DS-Client Default Configuration Updated");

            backupSetNotification.Dispose();
            defaultConfiguration.Dispose();
            DSClientConfigMgr.Dispose();
        }

        public static EOpenFileStrategy StringToEOpenFileStrategy(string openFileMethod)
        {
            EOpenFileStrategy strategy = EOpenFileStrategy.EOpenFileStrategy__UNDEFINED;

            switch (openFileMethod)
            {
                case "TryDenyWrite":
                    strategy = EOpenFileStrategy.EOpenFileStrategy__TryDenyWrite;
                    break;
                case "DenyWrite":
                    strategy = EOpenFileStrategy.EOpenFileStrategy__DenyWrite;
                    break;
                case "PreventWrite":
                    strategy = EOpenFileStrategy.EOpenFileStrategy__PreventWrite;
                    break;
                case "AllowWrite":
                    strategy = EOpenFileStrategy.EOpenFileStrategy__AllowWrite;
                    break;
                default:
                    strategy = EOpenFileStrategy.EOpenFileStrategy__UNDEFINED;
                    break;
            }

            return strategy;
        }
    }
}