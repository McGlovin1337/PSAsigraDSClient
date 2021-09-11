using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Get, "DSClientScheduleDetail")]
    [OutputType(typeof(DSClientScheduleDetail))]

    sealed public class GetDSClientScheduleDetail: BaseDSClientSchedule
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
            Dictionary<string, int> detailHashes = DSClientSessionInfo.GetScheduleOrRetentionDictionary(true);
            if (detailHashes == null)
                detailHashes = new Dictionary<string, int>();

            int startingId = 1;
            Dictionary<string, ScheduleDetail> unidentified = new Dictionary<string, ScheduleDetail>();

            int detailCounter = 0;
            int hashCounter = 0;
            WriteVerbose("Performing Action: Process Schedule Details");
            ProgressRecord progressRecord = new ProgressRecord(1, "Process Schedule Details", $"0 of {detailCount} processed, 0%")
            {
                RecordType = ProgressRecordType.Processing,
                CurrentOperation = "Processing Schedule Detail"
            };
            foreach (ScheduleDetail detail in ScheduleDetails)
            {
                progressRecord.PercentComplete = (int)Math.Round((double)(((double)detailCounter + (double)hashCounter) / ((double)detailCount * 2)) * 100);
                progressRecord.StatusDescription = $"{detailCounter} of {detailCount} processed, {progressRecord.PercentComplete}%";
                WriteProgress(progressRecord);

                string detailHash = ScheduleDetailHash(Schedule, detail);
                hashCounter++;
                WriteDebug($"Computed Hash: {detailHash}");
                progressRecord.PercentComplete = (int)Math.Round((double)(((double)detailCounter + (double)hashCounter) / ((double)detailCount * 2)) * 100);
                progressRecord.StatusDescription = $"{detailCounter} of {detailCount} processed, {progressRecord.PercentComplete}%";
                WriteProgress(progressRecord);

                // Check if the Hash is already in the Dictionary and if so fetch the associated Id
                detailHashes.TryGetValue(detailHash, out int id);
                if (id > 0)
                {
                    WriteDebug("Hash found in SessionState");
                    if (id >= startingId)
                        startingId = id + 1;
                    DSClientScheduleDetail.Add(new DSClientScheduleDetail(id, scheduleInfo, detail));
                    detail.Dispose();
                    detailCounter++;
                }
                else
                {
                    WriteDebug("Hash not found in SessionState");
                    unidentified.Add(detailHash, detail);
                }
            }

            // Process all the Schedule Details that were not already in the Dictionary, and add them
            foreach (KeyValuePair<string, ScheduleDetail> hash in unidentified)
            {
                progressRecord.PercentComplete = (int)Math.Round((double)(((double)detailCounter + hashCounter) / ((double)detailCount * 2)) * 100);
                progressRecord.StatusDescription = $"{detailCounter} of {detailCount} processed, {progressRecord.PercentComplete}%";
                WriteProgress(progressRecord);

                DSClientScheduleDetail.Add(new DSClientScheduleDetail(startingId, scheduleInfo, hash.Value));
                hash.Value.Dispose();
                detailHashes.Add(hash.Key, startingId);
                startingId++;
                detailCounter++;
            }

            progressRecord.RecordType = ProgressRecordType.Completed;
            progressRecord.PercentComplete = (int)Math.Round((double)(((double)detailCounter + hashCounter) / ((double)detailCount * 2)) * 100);
            WriteProgress(progressRecord);

            DSClientSessionInfo.SetScheduleOrRetentionDictonary(detailHashes, true);

            DSClientScheduleDetail.ForEach(WriteObject);

            Schedule.Dispose();
            DSClientScheduleMgr.Dispose();
        }
    }
}