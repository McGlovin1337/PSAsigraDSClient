using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

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
        public int[] DetailId { get; set; }

        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            WriteVerbose($"Performing Action: Retrieve Schedule with ScheduleId {ScheduleId}");
            Schedule Schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            WriteVerbose($"Performing Action: Retrieve Schedule Details for ScheduleId {ScheduleId}");
            ScheduleDetail[] ScheduleDetails = Schedule.getDetails();
            WriteVerbose($"Notice: Yielded {ScheduleDetails.Count()} Schedule Details");

            foreach (ScheduleDetail scheduleDetail in ScheduleDetails)
            {
                Dictionary<int, int> detailHash = SessionState.PSVariable.GetValue("ScheduleDetail", null) as Dictionary<int, int>;
                if (detailHash == null)
                    throw new Exception("No Schedule Details found in Session State, use Get-DSClientScheduleDetail Cmdlet");

                DSClientScheduleDetail detail = new DSClientScheduleDetail(DSClientScheduleMgr.definedScheduleInfo(ScheduleId), scheduleDetail);
                int detailHashCode = detail.GetHashCode();

                if (detailHash.TryGetValue(detailHashCode, out int detailId))
                    if (DetailId.Contains(detailId))
                        if (ShouldProcess(
                            $"Performing Operation Remove Schedule Detail on 'Type: {detail.Type}, StartTime: {detail.StartTime}'",
                            $"Are you sure you want to remove the {detail.Type} Schedule Detail (StartTime: {detail.StartTime}) with Id {detailId}?",
                            "Remove Schedule Detail")
                            )
                            Schedule.removeDetail(scheduleDetail);

                detailHash.Remove(detailHashCode);
                SessionState.PSVariable.Remove("ScheduleDetail");
                SessionState.PSVariable.Set("ScheduleDetail", detailHash);
            }

            Schedule.Dispose();
        }
    }
}