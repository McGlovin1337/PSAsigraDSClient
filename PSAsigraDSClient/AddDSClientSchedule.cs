using System.Management.Automation;
using AsigraDSClientApi;

namespace PSAsigraDSClient
{
    [Cmdlet(VerbsCommon.Add, "DSClientSchedule")]

    public class AddDSClientSchedule: DSClientCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, HelpMessage = "The name of the Schedule")]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 1, HelpMessage = "Set a Short Name")]
        [ValidateNotNullOrEmpty]
        public string ShortName { get; set; }

        [Parameter(Position = 2, HelpMessage = "Set Backup CPU Throttle")]
        public int CPUThrottle { get; set; }

        [Parameter(Position = 3, HelpMessage = "Concurrent backup sets allowed")]
        public int ConcurrentBackups { get; set; }

        [Parameter(Position = 4, HelpMessage = "Specifies only Administrators can use this schedule")]
        public SwitchParameter AdminOnly { get; set; }

        [Parameter(Position = 5, HelpMessage = "Set the Schedule to Inactive")]
        public SwitchParameter Inactive { get; set; }

        [Parameter(Position = 6, HelpMessage = "Start only if DS-System Connection available")]
        public SwitchParameter UseNetworkDetection { get; set; }

        protected override void DSClientProcessRecord()
        {
            ScheduleManager DSClientScheduleMgr = DSClientSession.getScheduleManager();

            // Build a new Schedule
            WriteVerbose("Building new Schedule...");
            Schedule newSchedule = DSClientScheduleMgr.createSchedule();

            newSchedule.setName(Name);

            if (ShortName != null)
                newSchedule.setShortName(ShortName);

            if (CPUThrottle != null)
                newSchedule.setBackupCPUThrottle(CPUThrottle);

            if (ConcurrentBackups != null)
                newSchedule.setConcurrentBackupSets(ConcurrentBackups);

            if (MyInvocation.BoundParameters.ContainsKey("AdminOnly"))
                newSchedule.setAdministratorsOnly(AdminOnly);

            if (MyInvocation.BoundParameters.ContainsKey("Inactive"))
                newSchedule.setActive(!Inactive);

            if (MyInvocation.BoundParameters.ContainsKey("UseNetworkDetection"))
                newSchedule.setUsingNetworkDetection(UseNetworkDetection);

            // Apply the new Schedule
            WriteVerbose("Adding the new Schedule...");
            DSClientScheduleMgr.addSchedule(newSchedule);
            WriteObject("Added new schedule");

            DSClientScheduleMgr.Dispose();
        }
    }
}