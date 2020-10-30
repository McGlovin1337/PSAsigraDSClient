using System;
using System.Collections.Generic;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    public abstract class BaseDSClientRunningActivity: DSClientCmdlet
    {
        protected abstract void ProcessRunningActivity(IEnumerable<DSClientRunningActivity> dSClientRunningActivities);

        protected override void DSClientProcessRecord()
        {
            WriteVerbose("Retrieving current running activities...");
            running_activity_info[] runningActivities = DSClientSession.running_activities();

            List<DSClientRunningActivity> DSClientRunningActivities = new List<DSClientRunningActivity>();

            foreach (running_activity_info activity in runningActivities)
            {
                DSClientRunningActivity RunningActivity = new DSClientRunningActivity(activity);

                DSClientRunningActivities.Add(RunningActivity);
            }

            ProcessRunningActivity(DSClientRunningActivities);
        }

        public class DSClientRunningActivity
        {
            public int ActivityId { get; private set; }
            public string Description { get; private set; }
            public int FilesLeft { get; private set; }
            public int FilesProcessed { get; private set; }
            public bool Finished { get; private set; }
            public string ProcessDir { get; private set; }
            public int SetId { get; private set; }
            public long SizeLeft { get; private set; }
            public long SizeProcessed { get; private set; }
            public DateTime StartTime { get; private set; }
            public string StatusMsg { get; private set; }
            public string Type { get; private set; }
            public string User { get; private set; }

            public DSClientRunningActivity(running_activity_info activityInfo)
            {
                ActivityId = activityInfo.activity_id;
                Description = activityInfo.description;
                FilesLeft = activityInfo.files_left;
                FilesProcessed = activityInfo.files_processed;
                Finished = activityInfo.finished;
                ProcessDir = activityInfo.process_dir;
                SetId = activityInfo.set_id;
                SizeLeft = activityInfo.size_left;
                SizeProcessed = activityInfo.size_processed;
                StartTime = UnixEpochToDateTime(activityInfo.start_time);
                StatusMsg = activityInfo.status_msg;
                Type = EActivityTypeToString(activityInfo.type);
                User = activityInfo.user;
            }
        }

        public static string EActivityTypeToString(EActivityType activityType)
        {
            switch(activityType)
            {
                case EActivityType.EActivityType__Backup:
                    return "Backup";
                case EActivityType.EActivityType__CDP_Backup:
                    return "CDPBackup";
                case EActivityType.EActivityType__Restore:
                    return "Restore";
                case EActivityType.EActivityType__DailyAdmin:
                    return "DailyAdmin";
                case EActivityType.EActivityType__WeeklyAdmin:
                    return "WeeklyAdmin";
                case EActivityType.EActivityType__Delete:
                    return "Delete";
                case EActivityType.EActivityType__Recovery:
                    return "Recovery";
                case EActivityType.EActivityType__Synchronization:
                    return "Synchronization";
                case EActivityType.EActivityType__DiscTapeRequest:
                    return "DiscTapeRequest";
                case EActivityType.EActivityType__DiscTapeRestore:
                    return "DiscTapeRestore";
                case EActivityType.EActivityType__BLMRequest:
                    return "BLMRequest";
                case EActivityType.EActivityType__OnLineFileSummary:
                    return "OnlineFileSummary";
                case EActivityType.EActivityType__Registration:
                    return "Registration";
                case EActivityType.EActivityType__LANAnalyze:
                    return "LANAnalyze";
                case EActivityType.EActivityType__BLMRestore:
                    return "BLMRestore";
                case EActivityType.EActivityType__Validation:
                    return "Validation";
                case EActivityType.EActivityType__Retention:
                    return "Retention";
                case EActivityType.EActivityType__TapeConversion:
                    return "TapeConversion";
                case EActivityType.EActivityType__CacheCopy:
                    return "CacheCopy";
                case EActivityType.EActivityType__CacheMonitor:
                    return "CacheMonitor";
                case EActivityType.EActivityType__AppAutoUpgrade:
                    return "AppAutoUpgrade";
                case EActivityType.EActivityType__Convert:
                    return "Convert";
                case EActivityType.EActivityType__CancelConvert:
                    return "CancelConvert";
                case EActivityType.EActivityType__CleanLocalOnlyTrash:
                    return "CleanLocalOnlyTrash";
                case EActivityType.EActivityType__Connection:
                    return "Connection";
                case EActivityType.EActivityType__TestConnection:
                    return "TestConnection";
                case EActivityType.EActivityType__CloudDatabaseUpload:
                    return "CloudDatabaseUpload";
                case EActivityType.EActivityType__LANResourceDiscovery:
                    return "LANResourceDiscovery";
                case EActivityType.EActivityType__SnapshotRestore:
                    return "SnapshotRestore";
                case EActivityType.EActivityType__SnapshotTransfer:
                    return "SnapshotTransfer";
                case EActivityType.EActivityType__CancelSnapshotTransfer:
                    return "CancelSnapshotTransfer";
                default:
                    return null;
            }
        }
    }
}