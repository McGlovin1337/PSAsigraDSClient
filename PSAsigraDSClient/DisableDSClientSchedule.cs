using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Disable, "DSClientSchedule")]

    public class DisableDSClientSchedule: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Schedule to Disable")]
        public int ScheduleId { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Schedule from DS-Client
            WriteVerbose("Retrieving Schedule from DS-Client...");
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
            Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            // Set the Schedule to InActive
            WriteVerbose("Performing action: Disable On Target: " + ScheduleId);
            schedule.setActive(false);

            schedule.Dispose();
            DSClientScheduleMgr.Dispose();
        }
    }
}