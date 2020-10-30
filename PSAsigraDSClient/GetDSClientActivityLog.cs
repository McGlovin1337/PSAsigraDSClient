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

    public class GetDSClientActivityLog: DSClientCmdlet
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

            WriteVerbose("Retrieving Activity Log Info...");
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

            WriteVerbose("Yielded " + ActivityLogs.Count() + " Activities");

            ActivityLogs.ForEach(WriteObject);
        }

        private class DSClientAcivityLog
        {
            public int ActivityId { get; private set; }
            public string ActivityType { get; private set; }
            public int BackupSetId { get; private set; }
            public string Description { get; private set; }
            public DateTime StartTime { get; private set; }
            public DateTime EndTime { get; private set; }
            public string Status { get; private set; }
            public long DataSize { get; private set; }
            public int FileCount { get; private set; }
            public int Errors { get; private set; }
            public int Warnings { get; private set; }
            public long Transmit { get; private set; }
            public long NetTransmit { get; private set; }
            public int ScheduleId { get; private set; }
            public string User { get; private set; }

            public DSClientAcivityLog(activity_log_info activityLogInfo)
            {
                ActivityId = activityLogInfo.id;
                ActivityType = BaseDSClientRunningActivity.EActivityTypeToString(activityLogInfo.type);
                BackupSetId = activityLogInfo.set_id;
                Description = activityLogInfo.description;
                StartTime = UnixEpochToDateTime(activityLogInfo.start_time);
                EndTime = UnixEpochToDateTime(activityLogInfo.end_time);
                Status = ECompletionTypeToString(activityLogInfo.completion);
                DataSize = activityLogInfo.data_size;
                FileCount = activityLogInfo.file_count;
                Errors = activityLogInfo.errors;
                Warnings = activityLogInfo.warnings;
                Transmit = activityLogInfo.transmit_amt;
                NetTransmit = activityLogInfo.net_transmit_amt;
                ScheduleId = activityLogInfo.schedule_id;
                User = activityLogInfo.user;
            }
        }

        private static string ECompletionTypeToString(ECompletionType completionType)
        {
            string CompletionType = null;

            switch (completionType)
            {
                case ECompletionType.ECompletionType__Succeeded:
                    CompletionType = "Succeeded";
                    break;
                case ECompletionType.ECompletionType__NoConnection:
                    CompletionType = "NoConnection";
                    break;
                case ECompletionType.ECompletionType__Disconnected:
                    CompletionType = "Disconnected";
                    break;
                case ECompletionType.ECompletionType__TooManyErrors:
                    CompletionType = "TooManyErrors";
                    break;
                case ECompletionType.ECompletionType__Exception:
                    CompletionType = "Exception";
                    break;
                case ECompletionType.ECompletionType__PrePostFailure:
                    CompletionType = "PrePostFailure";
                    break;
                case ECompletionType.ECompletionType__NoResource:
                    CompletionType = "NoResource";
                    break;
                case ECompletionType.ECompletionType__StorageLimit:
                    CompletionType = "StorageLimitReached";
                    break;
                case ECompletionType.ECompletionType__ShareUnavailable:
                    CompletionType = "ShareUnavailable";
                    break;
                case ECompletionType.ECompletionType__Shutdown:
                    CompletionType = "DSClientShutdown";
                    break;
                case ECompletionType.ECompletionType__Synchronisation:
                    CompletionType = "BackupOutOfSync";
                    break;
                case ECompletionType.ECompletionType__TimeLimit:
                    CompletionType = "TimeLimitReached";
                    break;
                case ECompletionType.ECompletionType__UserStop:
                    CompletionType = "UserStopped";
                    break;
                case ECompletionType.ECompletionType__Locked:
                    CompletionType = "BackupSetLocked";
                    break;
                case ECompletionType.ECompletionType__UpgradeTriggered:
                    CompletionType = "UpgradeTriggered";
                    break;
                case ECompletionType.ECompletionType__ClientQuotaReached:
                    CompletionType = "ClientQuotaReached";
                    break;
                case ECompletionType.ECompletionType__CustomerQuotaReached:
                    CompletionType = "CustomerQuotaReached";
                    break;
                case ECompletionType.ECompletionType__FatalError:
                    CompletionType = "FatalError";
                    break;
                case ECompletionType.ECompletionType__UnexpectedStop:
                    CompletionType = "UnexpectedStop";
                    break;
                case ECompletionType.ECompletionType__SynchronisationFailed:
                    CompletionType = "SynchronisationFailed";
                    break;
                case ECompletionType.ECompletionType__NoDBSpace:
                    CompletionType = "DatabaseOutOfSpace";
                    break;
                case ECompletionType.ECompletionType__NoLocalPath:
                    CompletionType = "NoLocalStoragePath";
                    break;
                case ECompletionType.ECompletionType__SystemRequestStop:
                    CompletionType = "SystemStopped";
                    break;
                case ECompletionType.ECompletionType__OracleNotMounted:
                    CompletionType = "OracleNotMounted";
                    break;
                case ECompletionType.ECompletionType__OracleNotOpen:
                    CompletionType = "OracleNotOpen";
                    break;
                case ECompletionType.ECompletionType__NoCatalogRoot:
                    CompletionType = "NoCatalogRoot";
                    break;
                case ECompletionType.ECompletionType__NoCatalog:
                    CompletionType = "NoCatalog";
                    break;
                case ECompletionType.ECompletionType__NoSpace:
                    CompletionType = "NoDiskSpace";
                    break;
                case ECompletionType.ECompletionType__Yield:
                    CompletionType = "YieldOtherActivity";
                    break;
                case ECompletionType.ECompletionType__NoInitialBuffer:
                    CompletionType = "NoInitialBuffer";
                    break;
                case ECompletionType.ECompletionType__SysAdminDisabled:
                    CompletionType = "SysAdminDisabled";
                    break;
                case ECompletionType.ECompletionType__ConnectSetSource:
                    CompletionType = "SourceUnavailable";
                    break;
                case ECompletionType.ECompletionType__MetadataUnavailable:
                    CompletionType = "MetadataUnavailable";
                    break;
                case ECompletionType.ECompletionType__VSSStartSnapshot:
                    CompletionType = "FailedVSSSnapshot";
                    break;
                case ECompletionType.ECompletionType__SelfContainedLimit:
                    CompletionType = "SelfContainedLimitReached";
                    break;
                case ECompletionType.ECompletionType__DSClientQuit:
                    CompletionType = "DSClientQuit";
                    break;
            }

            return CompletionType;
        }
    }
}