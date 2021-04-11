using System;
using System.Collections.Generic;
using System.Management.Automation;
using AsigraDSClientApi;
using static PSAsigraDSClient.DSClientCommon;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientScheduleDetail", SupportsShouldProcess = true)]
    public class RemoveDSClientScheduleDetail: BaseDSClientSchedule
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, HelpMessage = "Specify the Schedule Id")]
        [ValidateNotNullOrEmpty]
        public int ScheduleId { get; set; }

        [Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true, HelpMessage = "Specify the Detail Id")]
        [ValidateNotNullOrEmpty]
        public int DetailId { get; set; }

        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            // Select the required Schedule
            WriteVerbose($"Performing Action: Retrieve Schedule with ScheduleId {ScheduleId}");
            Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            // Get Schedule Detail Hash Dictionary from SessionState
            Dictionary<string, int> detailHashes = SessionState.PSVariable.GetValue("ScheduleDetail", null) as Dictionary<string, int>;
            if (detailHashes == null)
                throw new Exception("No Schedule Details found in Session State, use Get-DSClientScheduleDetail Cmdlet");

            // Select the Schedule Detail
            (ScheduleDetail scheduleDetail, string detailHash) = SelectScheduleDetail(schedule, DetailId, detailHashes);

            if (scheduleDetail != null)
                if (ShouldProcess($"Schedule: '{schedule.getName()}'", $"Remove {EnumToString(scheduleDetail.getType())} Schedule Detail with Id '{DetailId}'"))
                    schedule.removeDetail(scheduleDetail);

            // Remove the Hash from SessionState Dictionary
            detailHashes.Remove(detailHash);
            SessionState.PSVariable.Set("ScheduleDetail", detailHashes);

            schedule.Dispose();
        }
    }
}