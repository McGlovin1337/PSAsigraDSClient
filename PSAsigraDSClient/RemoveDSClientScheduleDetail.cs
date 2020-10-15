using System.Linq;
using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientScheduleDetail")]
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

            WriteVerbose("Looking up DSClient for ScheduleId " + ScheduleId + "...");
            Schedule Schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            WriteVerbose("Getting Schedule Details for ScheduleId " + ScheduleId + "...");
            ScheduleDetail[] ScheduleDetails = Schedule.getDetails();
            WriteVerbose("Yielded " + ScheduleDetails.Count() + " Schedule Details");

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
                        WriteVerbose("Removing Schedule Detail...");
                        try
                        {
                            Schedule.removeDetail(schedule);
                            RemoveCount += 1;
                        }
                        catch
                        {
                            WriteVerbose("Failed to remove Schedule Detail with Id " + targetId);
                            continue;
                        }
                    }
                }
            }

            WriteObject("Removed " + RemoveCount + " Schedule Details");

            Schedule.Dispose();
        }
    }
}