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

            // Get Hash Codes for Previously Identified Schedule Details
            Dictionary<string, int> detailHashes = SessionState.PSVariable.GetValue("ScheduleDetail", null) as Dictionary<string, int>;
            if (detailHashes == null)
                detailHashes = new Dictionary<string, int>();

            int startingId = 1;
            Dictionary<string, ScheduleDetail> unidentified = new Dictionary<string, ScheduleDetail>();

            int detailCounter = 0;
            WriteVerbose("Performing Action: Process Schedule Details");
            ProgressRecord progressRecord = new ProgressRecord(1, "Process Schedule Details", $"0 of {detailCount} processed, 0%")
            {
                RecordType = ProgressRecordType.Processing
            };
            foreach (ScheduleDetail detail in ScheduleDetails)
            {
                progressRecord.CurrentOperation = "Processing Schedule Detail";
                progressRecord.PercentComplete = (int)Math.Round((double)(((double)detailCounter) / (double)detailCount) * 100);
                progressRecord.StatusDescription = $"{detailCounter} of {detailCount} processed, {progressRecord.PercentComplete}%";
                WriteProgress(progressRecord);
                string detailHash = ScheduleDetailHash(Schedule, detail);
                WriteDebug($"Computed Hash: {detailHash}");

                detailHashes.TryGetValue(detailHash, out int id);
                if (id > 0)
                {
                    if (id >= startingId)
                        startingId = id + 1;
                    DSClientScheduleDetail.Add(new DSClientScheduleDetail(id, scheduleInfo, detail));
                    detail.Dispose();
                }
                else
                    unidentified.Add(detailHash, detail);

                detailCounter++;
            }

            progressRecord.RecordType = ProgressRecordType.Completed;
            progressRecord.PercentComplete = (int)Math.Round((double)(((double)detailCounter) / (double)detailCount) * 100);
            WriteProgress(progressRecord);

            foreach (KeyValuePair<string, ScheduleDetail> hash in unidentified)
            {
                DSClientScheduleDetail.Add(new DSClientScheduleDetail(startingId, scheduleInfo, hash.Value));
                hash.Value.Dispose();
                detailHashes.Add(hash.Key, startingId);
                startingId++;
            }

            SessionState.PSVariable.Set("ScheduleDetail", detailHashes);

            DSClientScheduleDetail.ForEach(WriteObject);

            Schedule.Dispose();
            DSClientScheduleMgr.Dispose();
        }
    }
}