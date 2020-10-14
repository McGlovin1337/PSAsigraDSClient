﻿using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientWinFsBackupSet")]

    public class SetDSClientWinFsBackupSet: BaseDSClientWinFsBackupSetParams
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to Modify")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Set the Name of the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Backup Set Type")]
        [ValidateSet("Offsite", "Statistical", "SelfContained", "LocalOnly")]
        public new string SetType { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Compression Method to use")]
        [ValidateSet("None", "ZLIB", "LZOP", "ZLIB_LO", "ZLIB_MED", "ZLIB_HI")]
        public new string Compression { get; set; }

        protected override void ProcessWinFsBackupSet()
        {
            // Get the Backup Set from DS-Client
            WriteVerbose("Retrieving Backup Set from DS-Client...");
            BackupSet backupSet = DSClientSession.backup_set(BackupSetId);

            // Process the Common Backup Set Parameters
            backupSet = ProcessBaseBackupSetParams(MyInvocation.BoundParameters, backupSet);

            // Set the Schedule and Retention Rules
            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
            {
                ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
                Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);
                backupSet.setSchedule(schedule);
            }

            if (MyInvocation.BoundParameters.ContainsKey("RetentionRuleId"))
            {
                RetentionRuleManager DSClientRetentionRuleMgr = DSClientSession.getRetentionRuleManager();
                RetentionRule[] retentionRules = DSClientRetentionRuleMgr.definedRules();
                RetentionRule retentionRule = retentionRules.Single(rule => rule.getID() == RetentionRuleId);
                backupSet.setRetentionRule(retentionRule);
            }

            // Process this Cmdlets specific configuration
            Win32FS_BackupSet win32BackupSet = ProcessWinFsBackupSetParams(MyInvocation.BoundParameters, Win32FS_BackupSet.from(backupSet));            

            win32BackupSet.Dispose();
        }
    }
}