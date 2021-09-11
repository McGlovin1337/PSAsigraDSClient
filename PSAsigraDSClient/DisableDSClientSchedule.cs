using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Disable, "DSClientSchedule", SupportsShouldProcess = true)]
    [OutputType(typeof(void))]

    sealed public class DisableDSClientSchedule: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Schedule to Disable")]
        public int ScheduleId { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Schedule from DS-Client
            WriteVerbose($"Performing Action: Retrieve Schedule with ScheduleId: {ScheduleId}");
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
            Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            if (ShouldProcess($"Schedule '{schedule.getName()}'", "Disable"))
            {
                // Set the Schedule to InActive
                WriteVerbose($"Performing Action: Disable On Target: {ScheduleId}");
                schedule.setActive(false);
            }

            schedule.Dispose();
            DSClientScheduleMgr.Dispose();
        }
    }
}