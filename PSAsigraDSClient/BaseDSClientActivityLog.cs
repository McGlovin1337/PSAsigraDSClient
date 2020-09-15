using System;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using static PSAsigraDSClient.BaseDSClientRunningActivity;

namespace PSAsigraDSClient
{
    public class BaseDSClientActivityLog
    {
        public class DSClientAcivityLog
        {
            public int ActivityId { get; set; }
            public string ActivityType { get; set; }
            public int BackupSetId { get; set; }
            public string Description { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Status { get; set; }
            public long DataSize { get; set; }
            public int FileCount { get; set; }
            public int Errors { get; set; }
            public int Warnings { get; set; }
            public long Transmit { get; set; }
            public long NetTransmit { get; set; }
            public int ScheduleId { get; set; }
            public string User { get; set; }

            public DSClientAcivityLog(activity_log_info activityLogInfo)
            {
                ActivityId = activityLogInfo.id;
                ActivityType = EActivityTypeToString(activityLogInfo.type);
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

            switch(completionType)
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