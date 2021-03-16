using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientScheduleDetail")]
    [OutputType(typeof(DSClientScheduleDetail))]
    public class GetDSClientScheduleDetail: BaseDSClientSchedule
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Schedule Id")]
        [ValidateNotNullOrEmpty]
        public int ScheduleId { get; set; }

        [Parameter(Position = 1, HelpMessage = "Get the Schedule Details for the specified type: OneTime, Daily, Weekly, Monthly, Undefined")]
        [ValidateSet("OneTime", "Daily", "Weekly", "Monthly", "Undefined")]
        public string Type { get; set; }

        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            WriteVerbose($"Performing Action: Retrieve Schedule with ScheduleId: {ScheduleId}");
            Schedule Schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            WriteVerbose($"Performing Action: Retrieve Schedule Info for ScheduleId: {ScheduleId}");
            schedule_info scheduleInfo = DSClientScheduleMgr.definedScheduleInfo(ScheduleId);

            WriteVerbose($"Performing Action: Retrieve Schedule Details for ScheduleId: {ScheduleId}");
            ScheduleDetail[] ScheduleDetails = Schedule.getDetails();
            int detailCount = ScheduleDetails.Count();
            WriteVerbose($"Notice: Yielded {detailCount} Schedule Details");

            List<DSClientScheduleDetail> DSClientScheduleDetail = new List<DSClientScheduleDetail>();

            // Create a Base ID for each Detail in the Schedule
            int DetailId = 0;

            ProgressRecord progressRecord = new ProgressRecord(1, "Process Schedule Details", $"0 of {detailCount} processed, 0%")
            {
                RecordType = ProgressRecordType.Processing
            };

            Dictionary<int, int> scheduleHash = new Dictionary<int, int>();
            foreach (ScheduleDetail schedule in ScheduleDetails)
            {
                DetailId++;
                WriteVerbose($"Performing Action: Process DetailId: {DetailId}");
                progressRecord.CurrentOperation = $"Processing DetailId: {DetailId}";
                progressRecord.PercentComplete = (int)Math.Round((double)(((double)DetailId - 1) / (double)detailCount) * 100);
                progressRecord.StatusDescription = $"{DetailId - 1} of {detailCount} processed, {progressRecord.PercentComplete}%";
                WriteProgress(progressRecord);

                DSClientScheduleDetail scheduleDetail = new DSClientScheduleDetail(DetailId, scheduleInfo, schedule);
                int detailHash = scheduleDetail.GetHashCode();
                WriteDebug($"Hash Code for {DetailId}: {detailHash}");
                scheduleHash.Add(detailHash, DetailId);
                DSClientScheduleDetail.Add(scheduleDetail);
            }
            progressRecord.RecordType = ProgressRecordType.Completed;
            progressRecord.PercentComplete = (int)Math.Round((double)(((double)DetailId - 1) / (double)detailCount) * 100);
            WriteProgress(progressRecord);

            SessionState.PSVariable.Remove("ScheduleDetail");
            SessionState.PSVariable.Set("ScheduleDetail", scheduleHash);

            DSClientScheduleDetail.ForEach(WriteObject);

            Schedule.Dispose();
            DSClientScheduleMgr.Dispose();
        }
    }
}