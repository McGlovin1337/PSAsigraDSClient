using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsLifecycle.Enable, "DSClientSchedule", SupportsShouldProcess = true)]

    public class EnableDSClientSchedule: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Specify the Schedule to Enable")]
        public int ScheduleId { get; set; }

        protected override void DSClientProcessRecord()
        {
            // Get the Schedule from DS-Client
            WriteVerbose($"Performing Action: Retrieve Schedule with ScheduleId: {ScheduleId}");
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();
            Schedule schedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            if (ShouldProcess($"Schedule '{schedule.getName()}'", "Enable"))
            {
                // Set the Schedule to Active
                WriteVerbose($"Performing Action: Enable On Target: {ScheduleId}");
                schedule.setActive(true);
            }

            schedule.Dispose();
            DSClientScheduleMgr.Dispose();
        }
    }
}