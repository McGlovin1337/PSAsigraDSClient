﻿using System;
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
            string ActivityType = null;

            switch(activityType)
            {
                case EActivityType.EActivityType__Backup:
                    ActivityType = "Backup";
                    break;
                case EActivityType.EActivityType__CDP_Backup:
                    ActivityType = "CDPBackup";
                    break;
                case EActivityType.EActivityType__Restore:
                    ActivityType = "Restore";
                    break;
                case EActivityType.EActivityType__DailyAdmin:
                    ActivityType = "DailyAdmin";
                    break;
                case EActivityType.EActivityType__WeeklyAdmin:
                    ActivityType = "WeeklyAdmin";
                    break;
                case EActivityType.EActivityType__Delete:
                    ActivityType = "Delete";
                    break;
                case EActivityType.EActivityType__Recovery:
                    ActivityType = "Recovery";
                    break;
                case EActivityType.EActivityType__Synchronization:
                    ActivityType = "Synchronization";
                    break;
                case EActivityType.EActivityType__DiscTapeRequest:
                    ActivityType = "DiscTapeRequest";
                    break;
                case EActivityType.EActivityType__DiscTapeRestore:
                    ActivityType = "DiscTapeRestore";
                    break;
                case EActivityType.EActivityType__BLMRequest:
                    ActivityType = "BLMRequest";
                    break;
                case EActivityType.EActivityType__OnLineFileSummary:
                    ActivityType = "OnlineFileSummary";
                    break;
                case EActivityType.EActivityType__Registration:
                    ActivityType = "Registration";
                    break;
                case EActivityType.EActivityType__LANAnalyze:
                    ActivityType = "LANAnalyze";
                    break;
                case EActivityType.EActivityType__BLMRestore:
                    ActivityType = "BLMRestore";
                    break;
                case EActivityType.EActivityType__Validation:
                    ActivityType = "Validation";
                    break;
                case EActivityType.EActivityType__Retention:
                    ActivityType = "Retention";
                    break;
                case EActivityType.EActivityType__TapeConversion:
                    ActivityType = "TapeConversion";
                    break;
                case EActivityType.EActivityType__CacheCopy:
                    ActivityType = "CacheCopy";
                    break;
                case EActivityType.EActivityType__CacheMonitor:
                    ActivityType = "CacheMonitor";
                    break;
                case EActivityType.EActivityType__AppAutoUpgrade:
                    ActivityType = "AppAutoUpgrade";
                    break;
                case EActivityType.EActivityType__Convert:
                    ActivityType = "Convert";
                    break;
                case EActivityType.EActivityType__CancelConvert:
                    ActivityType = "CancelConvert";
                    break;
                case EActivityType.EActivityType__CleanLocalOnlyTrash:
                    ActivityType = "CleanLocalOnlyTrash";
                    break;
                case EActivityType.EActivityType__Connection:
                    ActivityType = "Connection";
                    break;
                case EActivityType.EActivityType__TestConnection:
                    ActivityType = "TestConnection";
                    break;
                case EActivityType.EActivityType__CloudDatabaseUpload:
                    ActivityType = "CloudDatabaseUpload";
                    break;
                case EActivityType.EActivityType__LANResourceDiscovery:
                    ActivityType = "LANResourceDiscovery";
                    break;
                case EActivityType.EActivityType__SnapshotRestore:
                    ActivityType = "SnapshotRestore";
                    break;
                case EActivityType.EActivityType__SnapshotTransfer:
                    ActivityType = "SnapshotTransfer";
                    break;
                case EActivityType.EActivityType__CancelSnapshotTransfer:
                    ActivityType = "CancelSnapshotTransfer";
                    break;
            }

            return ActivityType;
        }
    }
}