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
            WriteVerbose("Performing Action: Retrieve Running Activities");
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
            public int BackupSetId { get; private set; }
            public DSClientStorageUnit SizeLeft { get; private set; }
            public DSClientStorageUnit SizeProcessed { get; private set; }
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
                BackupSetId = activityInfo.set_id;
                SizeLeft = new DSClientStorageUnit(activityInfo.size_left);
                SizeProcessed = new DSClientStorageUnit(activityInfo.size_processed);
                StartTime = UnixEpochToDateTime(activityInfo.start_time);
                StatusMsg = activityInfo.status_msg;
                Type = EnumToString(activityInfo.type);
                User = activityInfo.user;
            }
        }
    }
}