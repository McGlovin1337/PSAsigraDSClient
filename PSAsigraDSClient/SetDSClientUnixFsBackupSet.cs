using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Set, "DSClientUnixFsBackupSet")]
    [OutputType(typeof(void))]

    sealed public class SetDSClientUnixFsBackupSet: BaseDSClientUnixFsBackupSet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Backup Set to Modify")]
        public int BackupSetId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Set the Name of the Backup Set")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(ValueFromPipelineByPropertyName = true, HelpMessage = "Set the Compression Method to use")]
        [ValidateSet("None", "ZLIB", "LZOP", "ZLIB_LO", "ZLIB_MED", "ZLIB_HI")]
        public new string Compression { get; set; }

        protected override void ProcessUnixFsBackupSet()
        {
            // Get the Backup Set from DS-Client
            WriteVerbose($"Performing Action: Retrieve Backup Set with BackupSetId: {BackupSetId}");
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
            UnixFS_Generic_BackupSet unixFsBackupSet = ProcessUnixFsBackupSetParams(MyInvocation.BoundParameters, UnixFS_Generic_BackupSet.from(backupSet));

            unixFsBackupSet.Dispose();
        }
    }
}