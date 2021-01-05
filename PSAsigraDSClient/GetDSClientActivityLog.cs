using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientActivityLog")]
    [OutputType(typeof(DSClientAcivityLog))]

    public class GetDSClientActivityLog: BaseDSClientActivityLog
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Start Date & Time")]
        public DateTime StartTime { get; set; } = DateTime.Parse("01/01/1970");

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify End Date & Time")]
        public DateTime EndTime { get; set; } = DateTime.Now;

        [Parameter(Mandatory = true, ParameterSetName = "Id", ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify ActivityId")]
        public int ActivityId { get; set; }

        [Parameter(ParameterSetName = "OtherFilters", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Activity Type to Filter on")]
        [ValidateSet("Backup", "CDPBackup", "Restore", "DailyAdmin", "WeeklyAdmin", "Delete", "Recovery", "Synchronization", "DiscTapeRequest", "DiscTapeRestore", "BLMRequest", "OnlineFileSummary", "Registration", "LANAnalyze", "BLMRestore", "Validation", "Retention", "TapeConversion", "CacheCopy", "CacheMonitor", "AppAutoUpgrade", "Convert", "CancelConvert", "CleanLocalOnlyTrash", "Connection", "TestConnection", "CloudDatabaseUpload", "LANResourceDiscovery", "SnapshotRestore", "SnapshotTransfer", "CancelSnapshotTransfer")]
        public string[] ActivityType { get; set; }

        [Parameter(ParameterSetName = "OtherFilters", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Activity Completion Status to Filter on")]
        [ValidateSet("Succeeded", "NoConnection", "Disconnected", "TooManyErrors", "Exception", "PrePostFailure", "NoResource", "StorageLimitReached", "ShareUnavailable", "DSClientShutdown", "BackupOutOfSync", "TimeLimitReached", "UserStopped", "BackupSetLocked", "UpgradeTriggered", "ClientQuotaReached", "CustomerQuotaReached", "FatalError", "UnexpectedStop", "SynchronisationFailed", "DatabaseOutOfSpace", "NoLocalStoragePath", "SystemStopped", "OracleNotMounted", "OracleNotOpen", "NoCatalogRoot", "NoCatalog", "NoDiskSpace", "YieldOtherActivity", "NoInitialBuffer", "SysAdminDisabled", "SourceUnavailable", "MetadataUnavailable", "FailedVSSSnapshot", "SelfContainedLimitReached", "DSClientQuit")]
        public string[] Status { get; set; }

        [Parameter(ParameterSetName = "OtherFilters", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Activities related to specific Backup Set")]
        public int BackupSetId { get; set; }

        [Parameter(ParameterSetName = "OtherFilters", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Activities related to a specific Schedule")]
        public int ScheduleId { get; set; }

        [Parameter(ParameterSetName = "OtherFilters", ValueFromPipelineByPropertyName = true, HelpMessage = "Specify Activities related to a specific User")]
        [SupportsWildcards]
        public string User { get; set; }

        protected override void DSClientProcessRecord()
        {
            int epochStart = DateTimeToUnixEpoch(StartTime);
            int epochEnd = DateTimeToUnixEpoch(EndTime);

            WriteVerbose("Performing Action: Retrieve Activity Log Info");
            activity_log_info[] activityLogs = DSClientSession.activity_log(epochStart, epochEnd);

            if (MyInvocation.BoundParameters.ContainsKey("ActivityId"))
                activityLogs = activityLogs.Where(log => log.id == ActivityId).ToArray();

            if (MyInvocation.BoundParameters.ContainsKey("BackupSetId"))
                activityLogs = activityLogs.Where(log => log.set_id == BackupSetId).ToArray();

            if (MyInvocation.BoundParameters.ContainsKey("ScheduleId"))
                activityLogs = activityLogs.Where(log => log.schedule_id == ScheduleId).ToArray();

            if (User != null)
            {
                WildcardOptions wcOptions = WildcardOptions.IgnoreCase
                                            | WildcardOptions.Compiled;

                WildcardPattern wcPattern = new WildcardPattern(User, wcOptions);

                activityLogs = activityLogs.Where(log => wcPattern.IsMatch(log.user)).ToArray();
            }

            List<DSClientAcivityLog> ActivityLogs = new List<DSClientAcivityLog>();

            foreach(var activity in activityLogs)
            {
                DSClientAcivityLog activityLog = new DSClientAcivityLog(activity);
                ActivityLogs.Add(activityLog);
            }

            // Filter Activity Type and Status here so we don't have to convert the strings back to their equivilent enums
            if (ActivityType != null)
                ActivityLogs = ActivityLogs.Where(log => ActivityType.Contains(log.ActivityType)).ToList();

            if (Status != null)
                ActivityLogs = ActivityLogs.Where(log => Status.Contains(log.Status)).ToList();

            WriteVerbose($"Notice: Yielded {ActivityLogs.Count()} Activities");

            ActivityLogs.ForEach(WriteObject);
        }
    }
}