using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Remove, "DSClientSchedule")]

    public class RemoveDSClientSchedule: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The ScheduleId to remove")]
        [ValidateNotNullOrEmpty]
        public int ScheduleId { get; set; }

        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            WriteVerbose($"Performing Action: Retrieve Schedule with ScheduleId {ScheduleId}");
            Schedule removeSchedule = DSClientScheduleMgr.definedSchedule(ScheduleId);

            WriteVerbose($"Performing Action: Remove Schedule with ScheduleId {ScheduleId}");
            DSClientScheduleMgr.removeSchedule(removeSchedule);
            WriteObject("Removed ScheduleId " + ScheduleId);

            DSClientScheduleMgr.Dispose();
        }
    }
}