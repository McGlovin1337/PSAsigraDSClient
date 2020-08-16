using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientRunningActivity")]
    [OutputType(typeof(RunningActivityInfo))]    
    public class GetDSClientRunningActivity: DSClientCmdlet
    {
        protected override void DSClientProcessRecord()
        {
            var running_Activities = DSClientSession.running_activities();

            List<RunningActivityInfo> runningActivities = new List<RunningActivityInfo>();

            foreach (var activity in running_Activities)
            {
                // Convert the Unix Epoch Time to UTC DateTime
                DateTime startTime = DSClientCommon.UnixEpochToDateTime(activity.start_time);

                runningActivities.Add(new RunningActivityInfo
                {
                    ActivityId = activity.activity_id,
                    Description = activity.description,
                    FilesLeft = activity.files_left,
                    FilesProcessed = activity.files_processed,
                    Finished = activity.finished,
                    ProcessDir = activity.process_dir,
                    SetId = activity.set_id,
                    SizeLeft = activity.size_left,
                    SizeProcessed = activity.size_processed,
                    StartTime = startTime,
                    StatusMsg = activity.status_msg,
                    Type = activity.type,
                    User = activity.user
                });
            }

            runningActivities.ForEach(WriteObject);            
        }

        protected class RunningActivityInfo
        {
            public int ActivityId { get; set; }
            public string Description { get; set; }

            public int FilesLeft { get; set; }

            public int FilesProcessed { get; set; }

            public bool Finished { get; set; }            

            public string ProcessDir { get; set; }

            public int SetId { get; set; }

            public long SizeLeft { get; set; }

            public long SizeProcessed { get; set; }

            public DateTime StartTime { get; set; }

            public string StatusMsg { get; set; }

            public EActivityType Type { get; set; }

            public string User { get; set; }
        }
    }
}