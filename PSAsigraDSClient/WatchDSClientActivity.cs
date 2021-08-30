using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;
using System.Threading;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Watch, "DSClientActivity")]
    [OutputType(typeof(DSClientAcivityLog))]

    public class WatchDSClientActivity: BaseDSClientActivityLog
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the ActivityId to Watch")]
        [ValidateNotNullOrEmpty]
        public int ActivityId { get; set; }

        [Parameter(HelpMessage = "Specify the Refresh Interval in Seconds of the Activity")]
        public int Refresh { get; set; } = 5;

        protected override void DSClientProcessRecord()
        {
            GenericActivity activity = DSClientSession.activity(ActivityId);

            if (activity != null)
            {
                running_activity_info activityInfo = activity.getCurrentStatus();

                string activityDescription = (activityInfo.set_id > 0) ? $"Performing Task: {EnumToString(activityInfo.type)} on BackupSetId: {activityInfo.set_id}" : $"Performing Task: {EnumToString(activityInfo.type)}";

                ProgressRecord progressRecord = new ProgressRecord(activityInfo.activity_id, activityDescription, activityInfo.status_msg)
                {
                    CurrentOperation = $"Processing: {activityInfo.process_dir}",
                    RecordType = (activityInfo.finished) ? ProgressRecordType.Completed : ProgressRecordType.Processing,
                    PercentComplete = (int)Math.Round((double)((double)activityInfo.size_processed / (double)(activityInfo.size_left + activityInfo.size_processed) * 100))
                };

                while (!activityInfo.finished)
                {
                    WriteProgress(progressRecord);

                    Thread.Sleep(Refresh * 1000);

                    WriteDebug("Updating status");

                    activityInfo = activity.getCurrentStatus();
                    progressRecord.CurrentOperation = $"Processing: {activityInfo.process_dir}";
                    progressRecord.RecordType = (activityInfo.finished) ? ProgressRecordType.Completed : ProgressRecordType.Processing;
                    progressRecord.PercentComplete = (int)Math.Round((double)((double)activityInfo.size_processed / (double)(activityInfo.size_left + activityInfo.size_processed) * 100));
                    progressRecord.StatusDescription = activityInfo.status_msg ?? "Processing";
                }

                activity.Dispose();

                activity_log_info[] activityLog = (DSClientSession.activity_log(0, DateTimeToUnixEpoch(DateTime.Now)))
                                                                        .Where(act => act.id == ActivityId)
                                                                        .ToArray();

                List<DSClientAcivityLog> dSClientAcivityLogs = new List<DSClientAcivityLog>();
                foreach (activity_log_info log in activityLog)
                    dSClientAcivityLogs.Add(new DSClientAcivityLog(log));

                dSClientAcivityLogs.ForEach(WriteObject);
            }
        }
    }
}