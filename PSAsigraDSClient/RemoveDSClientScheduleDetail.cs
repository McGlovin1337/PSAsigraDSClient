﻿using System.Linq;
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
        public int[] DetailId { get; set; }

        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            WriteVerbose($"Performing Action: Retrieve Schedule with ScheduleId {ScheduleId}");
            Schedule Schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            WriteVerbose($"Performing Action: Retrieve Schedule Details for ScheduleId {ScheduleId}");
            ScheduleDetail[] ScheduleDetails = Schedule.getDetails();
            WriteVerbose($"Notice: Yielded {ScheduleDetails.Count()} Schedule Details");

            // Create a Base ID for each Detail in the Schedule
            int DetailID = 0, RemoveCount = 0;

            foreach (ScheduleDetail schedule in ScheduleDetails)
            {
                DetailID += 1;

                /* Remove DetailId's that match the DetailID assigned to the current schedule
                 * IMPORTANT: The AsigraApi doesn't provide a unique id for each Schedule Detail, so we're trusting the DSClient/Api is providing the
                 * schedule details in the same order everytime.
                 * If the order is not the same, then the wrong Schedule Detail may be removed!!! */
                foreach (int targetId in DetailId)
                {
                    if (DetailID == targetId)
                    {
                        string scheduleType = EnumToString(schedule.getType());
                        string scheduleStart = new TimeInDay(schedule.getStartTime()).ToString();
                        if (ShouldProcess(
                            $"Performing Operation Remove Schedule Detail on 'Type: {scheduleType}, StartTime: {scheduleStart}'",
                            $"Are you sure you want to remove the {scheduleType} Schedule Detail (StartTime: {scheduleStart}) with Id {targetId}?",
                            "Remove Schedule Detail")
                        )
                        {
                            WriteVerbose("Performing Action: Remove Schedule Detail");
                            try
                            {
                                Schedule.removeDetail(schedule);
                                RemoveCount += 1;
                            }
                            catch
                            {
                                WriteWarning($"Failed to remove Schedule Detail with Id {targetId}");
                                continue;
                            }
                        }
                    }
                }
            }

            WriteObject("Removed " + RemoveCount + " Schedule Details");

            Schedule.Dispose();
        }
    }
}