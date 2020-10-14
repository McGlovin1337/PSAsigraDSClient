using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Enable, "DSClientSchedule")]

    public class EnableDSClientSchedule: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Schedule to Enable")]
        public int ScheduleId { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Schedule from DS-Client
            WriteVerbose("Retrieving Schedule from DS-Client...");
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
            Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            // Set the Schedule to Active
            WriteVerbose("Performing action: Enable On Target: " + ScheduleId);
            schedule.setActive(true);

            schedule.Dispose();
            DSClientScheduleMgr.Dispose();
        }
    }
}